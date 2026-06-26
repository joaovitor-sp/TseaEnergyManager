using System.Windows.Controls;
using Tsea.Desktop.ViewModels;

namespace Tsea.Desktop.Views
{
    public partial class StatsControl : UserControl
    {
        public StatsControl()
        {
            InitializeComponent();
            this.DataContext = new StatsViewModel();
        }
    }
}
