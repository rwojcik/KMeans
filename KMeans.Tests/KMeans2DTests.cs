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
        new Cluster(0,0)
      });

      _kMeans.Points.AddRange(new List<Point>
      {
        new Point(0,0),
        new Point(1,1)
      });

      Assert.IsFalse(_kMeans.FindClustersFinished());
    }

    [TestMethod]
    public void TestFinished_2()
    {

      _kMeans.Clusters.AddRange(new List<Cluster>
      {
        new Cluster(0,0)
      });

      _kMeans.Points.AddRange(new List<Point>
      {
        new Point(0,0),
        new Point(1,1)
      });

      bool finished = _kMeans.FindClusters().Result;

      Assert.IsTrue(finished);
    }
  }
}
