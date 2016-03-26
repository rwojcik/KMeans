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

    public MainWindow()
    {
      InitializeFields();
      InitializeComponent();
      InitializeBinding();
    }

    private void InitializeBinding()
    {
      StatusBar.DataContext = StatusViewModel;
    }

    private void InitializeFields()
    {
      StatusViewModel = new StatusViewModel();
    }

    private void ExitMenuItemClick(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void DrawingCanvasOnMouseMove(object sender, MouseEventArgs e)
    {
      if (!(sender is IInputElement)) return;

      StatusViewModel.CursorPositionViewModel.Position.Point = Mouse.GetPosition((IInputElement)sender);
    }

    private void DrawingCanvasOnMouseEnter(object sender, MouseEventArgs e)
    {
      StatusViewModel.StatusText = "Mouse enter...";
    }

    private void DrawingCanvasOnMouseLeave(object sender, MouseEventArgs e)
    {
      StatusViewModel.StatusText = "Mouse leave...";
    }
  }
}
