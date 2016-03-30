using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using KMeans.Calc;
using KMeans.Calc.Models;
using KMeans.Gui.ViewModels;
using Newtonsoft.Json;
using Path = System.IO.Path;
using Point = KMeans.Calc.Models.Point;

namespace KMeans.Gui.Windows
{
  /// <summary>
  ///   Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    private static readonly string SaveFilePath =
      $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}settings.json";

    public MainWindow()
    {
      LoadSaves();
      InitializeFields();
      InitializeComponent();
      InitializeBinding();
      FlushMenuRecents();
    }

    public StatusViewModel StatusViewModel { get; set; }

    public CursorPositionViewModel CursorPositionViewModel { get; set; }

    public Calc.KMeans KMeans { get; set; }

    public SaveViewModel SaveViewModel { get; set; }

    private CancellationTokenSource CancellationTokenSource { get; set; }

    private void LoadSaves()
    {
      SaveViewModel temp = null;
      if (File.Exists(SaveFilePath))
      {
        try
        {
          using (var fileStream = new FileStream(SaveFilePath, FileMode.Open))
          using (var streamReader = new StreamReader(fileStream))
          using (var jsonTextReader = new JsonTextReader(streamReader))
          {
            var jsonSerializer = new JsonSerializer();
            temp = jsonSerializer.Deserialize<SaveViewModel>(jsonTextReader);
          }
        }
        catch (Exception)
        {
          //ignore...
        }
      }

      if (temp?.RandomPointNum == null || temp.RandomClusterNum == null || temp.AlgorithmMaxStepNum == null)
      {
        SaveViewModel = new SaveViewModel
        {
          RandomClusterNum = new Stack<int>(5),
          RandomPointNum = new Stack<int>(5),
          AlgorithmMaxStepNum = new Stack<int>(5)
        };
      }
      else
      {
        SaveViewModel = temp;
      }
    }

    private void FlushMenuRecents()
    {
      AddRandomPointsMenu.Items.Clear();
      AddRandomClustersMenu.Items.Clear();
      RunAlgorithmMenu.Items.Clear();

      AddRandomPointsMenu.Items.Add(new MenuItem
      {
        Header = "_Enter custom number..."
      });
      if (SaveViewModel.RandomPointNum.Any())
      {
        AddRandomPointsMenu.Items.Add(new Separator());

        foreach (var point in SaveViewModel.RandomPointNum)
        {
          var addRandomPointMenu = new MenuItem
          {
            Header = $"_{point}"
          };

          addRandomPointMenu.Click += (sender, args) => { AddRandomPoints(point); };

          AddRandomPointsMenu.Items.Add(addRandomPointMenu);
        }
      }

      AddRandomClustersMenu.Items.Add(new MenuItem
      {
        Header = "_Enter custom number..."
      });

      if (SaveViewModel.RandomClusterNum.Any())
      {
        AddRandomClustersMenu.Items.Add(new Separator());

        foreach (var cluster in SaveViewModel.RandomClusterNum)
        {
          var randomClusterMenu = new MenuItem
          {
            Header = $"_{cluster}"
          };

          randomClusterMenu.Click += (sender, args) => { AddRandomClusters(cluster); };

          AddRandomClustersMenu.Items.Add(randomClusterMenu);
        }
      }

      RunAlgorithmMenu.Items.Add(new MenuItem
      {
        Header = "_Enter custom number..."
      });

      if (SaveViewModel.AlgorithmMaxStepNum.Any())
      {
        RunAlgorithmMenu.Items.Add(new Separator());

        foreach (var steps in SaveViewModel.AlgorithmMaxStepNum)
        {
          var runAlgorithmMenu = new MenuItem
          {
            Header = $"_{steps}"
          };

          runAlgorithmMenu.Click += async (sender, args) =>
          {

            StatusViewModel.StatusText = $"Algorithm is running...";

            bool finished =  await RunAlgorithm(steps);
            //EnsureDrawingCanvasChildren();

            StatusViewModel.StatusText = $"Finished last step, algorithm has {(finished ? string.Empty : "not yet ")}finished.";
          };

          RunAlgorithmMenu.Items.Add(runAlgorithmMenu);
        }
      }
    }

    private void InitializeFields()
    {
      StatusViewModel = new StatusViewModel();
      CursorPositionViewModel = new CursorPositionViewModel();
      KMeans = new Calc.KMeans();
      KMeans.UpdatedData += KMeansUpdatedData;
      KMeans.DoneStep += KMeansDoneStep;
    }

    private void KMeansDoneStep(object sender, DoneStepEventArgs doneStepEventArgs)
    {
      StatusViewModel.StatusText = $"Algorithm is running... {doneStepEventArgs.StepNo}/{doneStepEventArgs.StepsNum}";
    }

    private void KMeansUpdatedData(object sender, UpdatedDataEventArgs e)
    {
      DrawingCanvas.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(EnsureDrawingCanvasChildren));
    }

    private void AddRandomPoints(int numPoints)
    {
      var r = new Random();

      var randomPoints =
        Enumerable.Range(0, numPoints)
          .Select(i => new Point(r.NextDouble() * DrawingCanvas.ActualWidth, r.NextDouble() * DrawingCanvas.ActualHeight));

      KMeans.Points.AddRange(randomPoints);
      EnsureDrawingCanvasChildren();
    }

    private void AddRandomClusters(int numClusters)
    {
      var r = new Random();

      var randomClusters =
        Enumerable.Range(0, numClusters)
          .Select(i => new Cluster(r.NextDouble() * DrawingCanvas.ActualWidth, r.NextDouble() * DrawingCanvas.ActualHeight));

      KMeans.Clusters.AddRange(randomClusters);
      EnsureDrawingCanvasChildren();
    }

    private async Task<bool> RunAlgorithm(int maxSteps = 100)
    {
      CancellationTokenSource = new CancellationTokenSource();
      var finished = await KMeans.FindClusters(CancellationTokenSource.Token, maxSteps);

      return finished;
    }

    private void InitializeBinding()
    {
      StatusInfo.DataContext = StatusViewModel;
      CursorPosition.DataContext = CursorPositionViewModel;
    }

    private void ExitMenuItemClick(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void DrawingCanvasOnMouseMove(object sender, MouseEventArgs e)
    {
      if (!(sender is IInputElement)) return;

      CursorPositionViewModel.Position.Point = Mouse.GetPosition((IInputElement)sender);
    }

    private void DrawingCanvasOnMouseEnter(object sender, MouseEventArgs e)
    {
      //StatusViewModel.StatusText = "Mouse enter...";
      CursorPositionViewModel.CursorInsideCanvas = true;
    }

    private void DrawingCanvasOnMouseLeave(object sender, MouseEventArgs e)
    {
      //StatusViewModel.StatusText = "Mouse leave...";
      CursorPositionViewModel.CursorInsideCanvas = false;
    }

    private void DrawingCanvasOnMouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton == MouseButton.Right)
      {
        CancellationTokenSource?.Cancel();
        CancellationTokenSource = null;
      }
    }

    private void DrawingCanvasOnMouseUp(object sender, MouseButtonEventArgs e)
    {
      EnsureDrawingCanvasChildren();
    }

    private void EnsureDrawingCanvasChildren()
    {
      //TODO: reuse existing children
      DrawingCanvas.Children.Clear();

      foreach (var point in KMeans.Points)
      {
        var circle = new Ellipse
        {
          Height = 5d,
          Width = 5d,
          Fill = new SolidColorBrush(Colors.Black)
        };

        Canvas.SetLeft(circle, point.Values[0] - 2);
        Canvas.SetTop(circle, point.Values[1] - 2);

        DrawingCanvas.Children.Add(circle);

        if (point.Cluster != null)
        {
          var line = new Line
          {
            StrokeThickness = 0.5d,
            Stroke = new SolidColorBrush(Colors.Gray),
            X1 = point.Values[0],
            Y1 = point.Values[1],
            X2 = point.Cluster.Values[0],
            Y2 = point.Cluster.Values[1]
          };

          DrawingCanvas.Children.Add(line);
        }
      }

      foreach (var cluster in KMeans.Clusters)
      {
        var rectangle = new Rectangle
        {
          Height = 9d,
          Width = 9d,
          AllowDrop = true,
          Stroke = new SolidColorBrush(Colors.Black),
          StrokeThickness = 2d,
          Fill = new SolidColorBrush(Colors.Gray)
        };

        Canvas.SetLeft(rectangle, cluster.Values[0] - 4);
        Canvas.SetTop(rectangle, cluster.Values[1] - 4);

        DrawingCanvas.Children.Add(rectangle);
      }
    }

    protected override void OnClosed(EventArgs e)
    {
      try
      {
        using (var fileStream = new FileStream(SaveFilePath, FileMode.Create))
        using (var streamWriter = new StreamWriter(fileStream))
        using (var jsonTextWriter = new JsonTextWriter(streamWriter))
        {
          var jsonSerializer = new JsonSerializer
          {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
          };
          jsonSerializer.Serialize(jsonTextWriter, SaveViewModel);
        }
      }
      catch (Exception)
      {
        //ignore....
      }
      base.OnClosed(e);
    }

    private async void AlgorithmDoStepMenuClick(object sender, RoutedEventArgs e)
    {
      StatusViewModel.StatusText = "Performing one algorithm step...";

      CancellationTokenSource = new CancellationTokenSource();

      bool finished = await KMeans.FindClustersStep(CancellationTokenSource.Token);

      EnsureDrawingCanvasChildren();
      StatusViewModel.StatusText = $"Done one step, algorithm has {(finished ? string.Empty : "not yet ")}finished";
    }

    private void ClearMenuItemClick(object sender, RoutedEventArgs e)
    {
      KMeans.Clusters.Clear();
      KMeans.Points.Clear();
      EnsureDrawingCanvasChildren();
    }
  }
}