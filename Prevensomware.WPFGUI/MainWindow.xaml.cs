using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Prevensomware.WPFGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void tabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void MainFrm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnEnd_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var numericRegex = new Regex("[^0-9]+");
            e.Handled = numericRegex.IsMatch(e.Text);
        }

        private void TxtAddExtension_OnPreviewTextInputValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var extensionRegex = new Regex("");
            e.Handled = extensionRegex.IsMatch(e.Text);
        }
    }
}
