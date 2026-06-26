using System.Windows;
using Tsea.Desktop.ViewModels;

namespace Tsea.Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}