using MyApp.MEP.ExternalCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyApp.MEP
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        private readonly PickPointViewModel _viewModel;

        public TestWindow(PickPointViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = viewModel;
            viewModel.RequestClose += () => this.Close();

            InitializeComponent();
        }
    }
}
