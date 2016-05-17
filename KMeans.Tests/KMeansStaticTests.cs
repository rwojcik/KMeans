using System;
using System.Collections.Generic;
using System.Linq;
using KMeans.Calc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KMeans.Tests
{
  [TestClass]
  public class KMeansOfflineTests
  {
    [TestMethod]
    public void CalcDistance_1D()
    {
      var cluster = new Cluster(0d);
      var point = new Point(2d);

      Assert.AreEqual(2d, Calc.KMeans.CalcDistance(point, cluster));
    }

    [TestMethod]
    public void CalcDistance_2D()
    {
      var cluster = new Cluster(0d, 0d);
      var point = new Point(1d, 1d);

      Assert.AreEqual(Math.Sqrt(2), Calc.KMeans.CalcDistance(point, cluster));
    }

    [TestMethod]
    public void CalcDistance_3D()
    {
      var cluster = new Cluster(0d, 0d, 0d);
      var point = new Point(1d, 1d, 1d);

      Assert.AreEqual(Math.Sqrt(3), Calc.KMeans.CalcDistance(point, cluster));
    }


    [TestMethod]
    public void CalcDistance_4D()
    {
      var cluster = new Cluster(0d, 0d, 0d, 0d);
      var point = new Point(1d, 1d, 1d, 1d);

      Assert.AreEqual(Math.Sqrt(4), Calc.KMeans.CalcDistance(point, cluster));
    }

    [TestMethod]
    public void FindNewCluster_1D()
    {
      var clusters = new List<Cluster>
      {
        new Cluster(0d),
        new Cluster(5d)
      };

      var points = new List<Point>
      {
        new Point(-1d),
        new Point(0d),
        new Point(1d),
        new Point(10d)
      };

      var calc = new Calc.KMeans(points, clusters, 1);

      var finished = calc.FindClustersStep().Result;

      Assert.IsTrue(finished);
      Assert.Equals(clusters[0], points[0].Cluster);
      Assert.Equals(clusters[0], points[1].Cluster);
      Assert.Equals(clusters[0], points[2].Cluster);
      Assert.Equals(clusters[1], points[3].Cluster);
    }

    [TestMethod]
    public void FindNewCluster_2D()
    {
      var clusters = new List<Cluster>
      {
        new Cluster(0d, 0d),
        new Cluster(5d, 5d)
      };

      var points = new List<Point>
      {
        new Point(-1d, -10d),
        new Point(0d, 0d),
        new Point(1d, 1d),
        new Point(100d, 100d)
      };

      var calc = new Calc.KMeans(points, clusters, 1);

      var finished = calc.FindNewCluster().Result;

      Assert.IsTrue(finished);
      Assert.AreSame(calc.Clusters[0], calc.Points[0].Cluster);
      Assert.AreSame(calc.Clusters[0], calc.Points[1].Cluster);
      Assert.AreSame(calc.Clusters[0], calc.Points[2].Cluster);
      Assert.AreSame(calc.Clusters[1], calc.Points[3].Cluster);
    }

    [TestMethod]
    public void MoveCluster_1D()
    {

      var clusters = new List<Cluster>
      {
        new Cluster(0d)
      };
      var points = new List<Point>
      {
        new Point(5d) {Cluster = clusters[0]},
        new Point(6d) {Cluster = clusters[0]}
      };

      var calc = new Calc.KMeans(points, clusters, 1);

      var finished = calc.FindClustersStep().Result;

      Assert.IsTrue(finished);
      Assert.AreEqual(5.5d, clusters[0].Values[0]);
    }

    [TestMethod]
    public void MoveCluster_2D()
    {
      var clusters = new List<Cluster>
      {
        new Cluster(0d, 0d)
      };

      var points = new List<Point>
      {
        new Point(5d, 5d) {Cluster = clusters[0]},
        new Point(6d, 6d) {Cluster = clusters[0]}
      };

      var calc = new Calc.KMeans(points, clusters, 1);

      Assert.AreEqual(5.5d, calc.Clusters.Values[0]);
      Assert.AreEqual(5.5d, calc.Clusters.Values[1]);
    }
  }
}