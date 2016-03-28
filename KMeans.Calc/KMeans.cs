using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KMeans.Calc.Models;

namespace KMeans.Calc
{
  public class KMeans
  {
    public List<Point> Points { get; }
    public List<Cluster> Clusters { get; }

    public int NumDimenstions { get; }

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
        Enumerable.Range(0, numPoints)
          .Select(i => new Point(dimBounds.Select(bound => r.NextDouble() * bound).ToArray()))
          .ToList();

      Clusters =
        Enumerable.Range(0, numClusters)
          .Select(i => new Cluster(dimBounds.Select(bound => r.NextDouble() * bound).ToArray()))
          .ToList();
    }

    public KMeans(int numDimensions = 2)
    {
      Points = new List<Point>();
      Clusters = new List<Cluster>();
      NumDimenstions = numDimensions;
    }

    public static double CalcDistance(Point point, Cluster cluster)
    {
      return Math.Sqrt(point.Values.Zip(cluster.Values, (p, c) => Math.Pow(p - c, 2)).Sum());
    }

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
      var clusterPoints = points.Where(point => point.Cluster == cluster).ToList();

      if (!clusterPoints.Any())
        return;

      for (int dimension = 0; dimension < numDimensions; dimension++)
      {
        cluster.Values[dimension] = points.Average(point => point.Values[dimension]);
      }
    }

    public async Task<bool> FindClusters(int maxIterations = 100)
    {
      return await FindClusters(CancellationToken.None, maxIterations);
    }

    public async Task<bool> FindClusters(CancellationToken cancellationToken, int maxIterations = 100)
    {
      int counter = 0;

      while (counter++ < maxIterations || !await FindClustersStep(cancellationToken))
      {
      }

      return FindClustersFinished();
    }

    public async Task<bool> FindClustersStep()
    {
      return await FindClustersStep(CancellationToken.None);
    }

    public async Task<bool> FindClustersStep(CancellationToken cancellationToken)
    {
      await
        Task.Factory.StartNew(() => Parallel.ForEach(Points, point => { FindNewCluster(point, Clusters); }),
          cancellationToken);

      await
        Task.Factory.StartNew(
          () => Parallel.ForEach(Clusters, cluster => { MoveCluster(cluster, Points, NumDimenstions); }),
          cancellationToken);

      return FindClustersFinished();
    }

    public bool FindClustersFinished()
    {
      return Points.AsParallel().All(point => point.Cluster != null && point.Cluster == point.PreviousCluster);
    }
  }
}
