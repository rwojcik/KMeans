using KMeans.Gui.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KMeans.Tests
{
  [TestClass]
  public class GuiViewModelTests
  {
    [TestMethod]
    public void CursosPosPropertyChangedTest()
    {
      var cursorPosVm = new CursorPositionViewModel
      {
        Position = new System.Windows.Point(1d, 2d)
      };

      var localPosX = cursorPosVm.X;
      var localPosY = cursorPosVm.Y;

      cursorPosVm.PropertyChanged += (sender, args) =>
      {
        if (args.PropertyName == nameof(CursorPositionViewModel.Position))
        {
          localPosX = cursorPosVm.Position.X;
          localPosY = cursorPosVm.Position.Y;
        }
      };

      cursorPosVm.Position = new System.Windows.Point(3d, 4d);

      Assert.AreEqual(cursorPosVm.Position.X, localPosX);
      Assert.AreEqual(cursorPosVm.Position.Y, localPosY);
    }

    [TestMethod]
    public void CursosPosNestedPropertyChangedTest()
    {
      var cursorPosVm = new CursorPositionViewModel
      {
        Position = new System.Windows.Point(1d, 2d)
      };

      var localPosX = cursorPosVm.X;
      var localPosY = cursorPosVm.Y;

      cursorPosVm.PropertyChanged += (sender, args) =>
      {
        if (args.PropertyName == nameof(CursorPositionViewModel.PositionText))
        {
          localPosX = cursorPosVm.Position.X;
          localPosY = cursorPosVm.Position.Y;
        }
      };

      cursorPosVm.Position = new System.Windows.Point(3d, 4d);

      Assert.AreEqual(cursorPosVm.Position.X, localPosX);
      Assert.AreEqual(cursorPosVm.Position.Y, localPosY);
    }

    [TestMethod]
    public void AlgRunningPropertyChangedTest()
    {
      var statusVm = new StatusViewModel
      {
        AlgorithmRunning = false,
      };
      
      var algRunningLocal = statusVm.AlgorithmRunning;

      statusVm.PropertyChanged += (sender, args) =>
      {
        if (args.PropertyName == nameof(StatusViewModel.AlgorithmRunning))
        {
          algRunningLocal = statusVm.AlgorithmRunning;
        }
      };

      statusVm.AlgorithmRunning = true;

      Assert.AreEqual(statusVm.AlgorithmRunning, algRunningLocal);
    }
  }
}
