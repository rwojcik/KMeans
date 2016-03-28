using System;
using System.Collections.Generic;
using KMeans.Calc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KMeans.Tests
{
  [TestClass]
  public class KMeansStaticTests
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

      foreach (var point in points)
      {
        Calc.KMeans.FindNewCluster(point, clusters);
      }

      Assert.AreSame(clusters[0], points[0].Cluster);
      Assert.AreSame(clusters[0], points[1].Cluster);
      Assert.AreSame(clusters[0], points[2].Cluster);
      Assert.AreSame(clusters[1], points[3].Cluster);
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

      foreach (var point in points)
      {
        Calc.KMeans.FindNewCluster(point, clusters);
      }

      Assert.AreSame(clusters[0], points[0].Cluster);
      Assert.AreSame(clusters[0], points[1].Cluster);
      Assert.AreSame(clusters[0], points[2].Cluster);
      Assert.AreSame(clusters[1], points[3].Cluster);
    }

    [TestMethod]
    public void MoveCluster_1D()
    {
      var cluser = new Cluster(0d);

      var points = new List<Point>
      {
        new Point(5d),
        new Point(6d)
      };

      foreach (var point in points)
      {
        point.Cluster = cluser;
      }

      Calc.KMeans.MoveCluster(cluser, points);

      Assert.AreEqual(5.5d, cluser.Values[0]);
    }

    [TestMethod]
    public void MoveCluster_2D()
    {
      var cluser = new Cluster(0d, 0d);

      var points = new List<Point>
      {
        new Point(5d, 5d),
        new Point(6d, 6d)
      };

      foreach (var point in points)
      {
        point.Cluster = cluser;
      }

      Calc.KMeans.MoveCluster(cluser, points);

      Assert.AreEqual(5.5d, cluser.Values[0]);
      Assert.AreEqual(5.5d, cluser.Values[1]);
    }
  }
}