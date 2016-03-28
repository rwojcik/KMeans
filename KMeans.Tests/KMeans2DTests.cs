using System.Collections.Generic;
using KMeans.Calc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KMeans.Tests
{
  [TestClass]
  public class KMeans2DTests
  {
    private Calc.KMeans _kMeans;

    [TestInitialize]
    public void SetUp()
    {
      _kMeans = new Calc.KMeans(new List<Point>(), new List<Cluster>(), 2);
    }

    [TestMethod]
    public void TestFinished_1()
    {
      _kMeans.Clusters.AddRange(new List<Cluster>
      {
        new Cluster(0, 0)
      });

      _kMeans.Points.AddRange(new List<Point>
      {
        new Point(0, 0),
        new Point(1, 1)
      });

      Assert.IsFalse(_kMeans.FindClustersFinished());
    }

    [TestMethod]
    public void TestFinished_2()
    {
      _kMeans.Clusters.AddRange(new List<Cluster>
      {
        new Cluster(0, 0)
      });

      _kMeans.Points.AddRange(new List<Point>
      {
        new Point(0, 0),
        new Point(1, 1)
      });

      var finished = _kMeans.FindClusters().Result;

      Assert.IsTrue(finished);
    }


    [TestMethod]
    public void TestClusterMove()
    {
      var clusters = new List<Cluster>
      {
        new Cluster(-1, 0),
        new Cluster(10, 0)
      };
      _kMeans.Clusters.AddRange(clusters);

      var points = new List<Point>
      {
        new Point(0, -1),
        new Point(0, 1)
      };
      _kMeans.Points.AddRange(points);

      //bool finished = _kMeans.FindClustersStep().Result;

      //Assert.IsFalse(finished);

      //finished = _kMeans.FindClustersStep().Result;

      //Assert.IsTrue(finished);

      var finished = _kMeans.FindClusters().Result;

      Assert.AreEqual(new Cluster(0, 0), clusters[0]);
      Assert.AreEqual(new Cluster(10, 0), clusters[1]);
    }
  }
}