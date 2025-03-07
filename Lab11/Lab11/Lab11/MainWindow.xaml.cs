using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab11
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

        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            if(NtextBox.Text != "" && KTextBox.Text != "")
            {
                int N = int.Parse(NtextBox.Text);
                int K = int.Parse(KTextBox.Text);
                long result = Exercises.Tasks(N, K);
                TasksResultTextBox.Text = result.ToString();
            }
            else
            {
                System.Windows.MessageBox.Show("Input N and K");
            }
        }

        private void DelegatesButton_Click(object sender, RoutedEventArgs e)
        {
            if (NtextBox.Text != "" && KTextBox.Text != "")
            {
                int N = int.Parse(NtextBox.Text);
                int K = int.Parse(KTextBox.Text);
                long result = Exercises.Delegates(N, K);
                DelegatesResultTextBox.Text = result.ToString();
            }
            else
            {
                System.Windows.MessageBox.Show("Input N and K");
            }
        }

        private async void AsyncButton_Click(object sender, RoutedEventArgs e)
        {
            if (NtextBox.Text != "" && KTextBox.Text != "")
            {
                int N = int.Parse(NtextBox.Text);
                int K = int.Parse(KTextBox.Text);
                long result = await Exercises.awaitMethod(N, K);
                AsyncResultTextBox.Text = result.ToString();
            }
            else
            {
                System.Windows.MessageBox.Show("Input N and K");
            }
        }

        private void FibonacciButton_Click(object sender, RoutedEventArgs e)
        {
            if (ITextBox.Text != "")
            {
                FibonacciTextBox.Text = "None";
                int i = int.Parse(ITextBox.Text);
                Exercises.Fibonacci(i, FibonacciProgressBar, (result) => { FibonacciTextBox.Text = result.ToString(); });
            }
            else
            {
                System.Windows.MessageBox.Show("Input i and then count fibonacci");
            }
        }

        private void CompressButton_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog()
            {
                Description = "Select directory to open"
            };
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if(dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;
                Exercises.compressFiles(path);
                System.Windows.MessageBox.Show("All files compressed");
            }
        }
    }
}
