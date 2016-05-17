using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KMeans.Calc.Models;

namespace KMeans.Calc
{
  public delegate void UpdatedData(object sender, UpdatedDataEventArgs e);

  public class UpdatedDataEventArgs
  {
    public UpdatedDataEventArgs(List<Cluster> clusters, List<Point> points)
    {
      Clusters = clusters;
      Points = points;
    }

    public List<Point> Points { get; }
    public List<Cluster> Clusters { get; }
  }

  public delegate void DoneStep(object sender, DoneStepEventArgs e);

  public class DoneStepEventArgs
  {
    public DoneStepEventArgs(List<Point> points, List<Cluster> clusters, int stepNo, int stepsNum, bool finished)
    {
      Points = points;
      Clusters = clusters;
      StepNo = stepNo;
      StepsNum = stepsNum;
      Finished = finished;
    }

    public List<Point> Points { get; }
    public List<Cluster> Clusters { get; }

    public int StepNo { get; }

    public int StepsNum { get; }

    public bool Finished { get; }
  }
  
  public class KMeans
  {
    public event UpdatedData UpdatedData;
    public event DoneStep DoneStep;

    public KMeans(List<Point> points, List<Cluster> clusters, int numDimensions)
    {
      Points = points;
      Clusters = clusters;
      NumDimenstions = numDimensions;
    }

    public KMeans(int numPoints, int numClusters, double[] dimBounds)
    {
      NumDimenstions = dimBounds.Length;
      var r = new Random();

      Points =
        GenerateRandomPoints(numPoints, dimBounds, r);

      Clusters =
        GenerateRandomClusters(numClusters, dimBounds, r);
    }

    public static List<Cluster> GenerateRandomClusters(int numClusters, double[] dimBounds, Random r = null)
    {
      if (r == null) r = new Random();
      return Enumerable.Range(0, numClusters)
        .Select(i => new Cluster(dimBounds.Select(bound => r.NextDouble() * bound).ToArray()))
        .ToList();
    }

    public static List<Point> GenerateRandomPoints(int numPoints, double[] dimBounds, Random r = null)
    {
      if (r == null) r = new Random();
      return Enumerable.Range(0, numPoints)
        .Select(i => new Point(dimBounds.Select(bound => r.NextDouble() * bound).ToArray()))
        .ToList();
    }

    public KMeans(int numDimensions = 2)
    {
      Points = new List<Point>();
      Clusters = new List<Cluster>();
      NumDimenstions = numDimensions;
    }

    public List<Point> Points { get; }
    public List<Cluster> Clusters { get; }

    public int NumDimenstions { get; }

    public static double CalcDistance(Point point, Cluster cluster) => Math.Sqrt(point.Values.Zip(cluster.Values, (p, c) => Math.Pow(p - c, 2)).Sum());

    public static void FindNewCluster(Point point, List<Cluster> clusters)
    {
      //var distances = clusters.ToDictionary(cluster => CalcDistance(point, cluster));

      var distances = new Dictionary<double, Cluster>(clusters.Count);

      foreach (var cluster in clusters)
      {
        var distance = CalcDistance(point, cluster);

        if (!distances.ContainsKey(distance))
        {
          distances.Add(distance, cluster);
        }
      }

      var newCluster = distances[distances.Keys.Min()];

      point.PreviousCluster = point.Cluster;
      point.Cluster = newCluster;

    }

    public static void MoveCluster(Cluster cluster, List<Point> points)
    {
      MoveCluster(cluster, points, cluster.Values.Length);
    }

    public static void MoveCluster(Cluster cluster, List<Point> points, int numDimensions)
    {
      var clusterPoints = points.Where(point => ReferenceEquals(point.Cluster, cluster)).ToList();

      if (!clusterPoints.Any())
        return;

      for (var dimension = 0; dimension < numDimensions; dimension++)
      {
        cluster.Values[dimension] = clusterPoints.Average(point => point.Values[dimension]);
      }
    }

    public async Task<bool> FindClusters(int maxIterations = 100)
    {
      return await FindClusters(CancellationToken.None, maxIterations);
    }

    public async Task<bool> FindClusters(CancellationToken cancellationToken, int maxIterations = 100)
    {
      var counter = 0;

      var finished = false;

      try
      {
        while (counter++ < maxIterations && !cancellationToken.IsCancellationRequested && !(finished = await FindClustersStep(cancellationToken)))
        {
          DoneStep?.Invoke(this, new DoneStepEventArgs(Points, Clusters, counter, maxIterations, finished));
        }
      }
      catch (OperationCanceledException)
      {
        //eat the fuken exception you cancellation token
        return false;
      }
      return finished;
    }

    public async Task<bool> FindClustersStep()
    {
      return await FindClustersStep(CancellationToken.None);
    }
    
    public async Task<bool> FindClustersStep(CancellationToken cancellationToken)
    {
      var parallelOptions = new ParallelOptions
      {
        MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount / 2),
        CancellationToken = cancellationToken,
      };

      try
      {
        await
          Task.Factory.StartNew(() => Parallel.ForEach(Points, parallelOptions, point => { FindNewCluster(point, Clusters); }),
            cancellationToken);

        await
          Task.Factory.StartNew(
            () => Parallel.ForEach(Clusters, parallelOptions, cluster => { MoveCluster(cluster, Points, NumDimenstions); }),
            cancellationToken);

        UpdatedData?.Invoke(this, new UpdatedDataEventArgs(Clusters, Points));

      }
      catch (OperationCanceledException)
      {
        return false;
      }
      return await FindClustersFinished();
    }

    public async Task<bool> FindClustersFinished()
    {
      return await Task.Factory.StartNew(() => Points.All(point => point.Cluster != null && ReferenceEquals(point.Cluster, point.PreviousCluster)));
    }

    public int GetProperlyClassifiedNum()
    {
      if (Points.Any(p => string.IsNullOrEmpty(p.Class)))
        return -1;

      var ret = 0;

      foreach (var cluster in Clusters)
      {
        var className =
          Points.Where(point => ReferenceEquals(point.Cluster, cluster))
            .GroupBy(point => new { ClassName = point.Class })
            .Select(g => new { g.Key.ClassName, Count = g.Count() })
            .OrderBy(g => g.Count)
            .FirstOrDefault()?.ClassName;

        ret += Points.Count(point => ReferenceEquals(point.Cluster, cluster) && point.Class == className);
      }
      return ret;
    }
  }
}