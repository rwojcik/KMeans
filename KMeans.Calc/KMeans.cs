using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KMeans.Calc.Models;
using PostSharp.Patterns.Model;

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
          .Select(i => new Point(dimBounds.Select(bound => r.NextDouble()*bound).ToArray()))
          .ToList();

      Clusters =
        Enumerable.Range(0, numClusters)
          .Select(i => new Cluster(dimBounds.Select(bound => r.NextDouble()*bound).ToArray()))
          .ToList();
    }

    private static double CalcDistance(Point point, Cluster cluster)
    {
      return point.Values.Zip(cluster.Values, (p, c) => Math.Pow(p - c, 2)).Sum();
    }

    private static void FindNewCluster(Point point, List<Cluster> clusters)
    {
      var distances = clusters.ToDictionary(cluster => CalcDistance(point, cluster));

      var newCluster = distances[distances.Keys.Min()];

      point.PreviousCluster = point.Cluster;
      point.Cluster = newCluster;
    }

    private static void MoveCluster(Cluster cluster, List<Point> points, int numDimensions)
    {
      var clusterPoints = points.Where(point => point.Cluster == cluster).ToList();

      if(!clusterPoints.Any())
        return;

      for (int dimension = 0; dimension < numDimensions; dimension++)
      {
        cluster.Values[dimension] = points.Average(point => point.Values[dimension]);
      }
    }

    public async Task<bool> FindClusters(CancellationToken cancellationToken)
    {
      await Task.Factory.StartNew(() => Parallel.ForEach(Points, point =>
      {
        FindNewCluster(point, Clusters);
      }), cancellationToken);

      await Task.Factory.StartNew(() => Parallel.ForEach(Clusters, cluster =>
      {
        MoveCluster(cluster, Points, NumDimenstions);
      }), cancellationToken);

      return FindClustersFinished();
    }

    public bool FindClustersFinished()
    {
      return Points.AsParallel().All(point => point.Cluster != null && point.Cluster == point.PreviousCluster);
    }
  }
}
