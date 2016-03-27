using System.Windows;
using System.Windows.Input;
using KMeans.Gui.ViewModels;

namespace KMeans.Gui.Windows
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    public StatusViewModel StatusViewModel { get; set; }

    public CursorPositionViewModel CursorPositionViewModel { get; set; }  

    public MainWindow()
    {
      InitializeFields();
      InitializeComponent();
      InitializeBinding();
    }

    private void InitializeBinding()
    {
      StatusInfo.DataContext = StatusViewModel;
      CursorPosition.DataContext = CursorPositionViewModel;
    }

    private void InitializeFields()
    {
      StatusViewModel = new StatusViewModel();
      CursorPositionViewModel = new CursorPositionViewModel();
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
      StatusViewModel.StatusText = "Mouse enter...";
      CursorPositionViewModel.CursorInsideCanvas = true;
    }

    private void DrawingCanvasOnMouseLeave(object sender, MouseEventArgs e)
    {
      StatusViewModel.StatusText = "Mouse leave...";
      CursorPositionViewModel.CursorInsideCanvas = false;
    }

    private void DrawingCanvasOnMouseDown(object sender, MouseButtonEventArgs e)
    {
      

    }

    private void DrawingCanvasOnMouseUp(object sender, MouseButtonEventArgs e)
    {
      

    }
  }
}
