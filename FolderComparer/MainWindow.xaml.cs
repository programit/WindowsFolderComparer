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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;

namespace FolderComparer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> diffList = new List<string>();
        private string base1;
        private string base2;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnRightClick(object sender, RoutedEventArgs e)
        {
            string path = this.ShowDialog();
            this.UpdateLabelPath(this.tbRight, path);
        }

        private void btnLeftClick(object sender, RoutedEventArgs e)
        {
            string path = this.ShowDialog();
            this.UpdateLabelPath(this.tbLeft, path);            
        }

        private void UpdateLabelPath(System.Windows.Controls.TextBox l, string path)
        {
            l.Visibility = System.Windows.Visibility.Visible;
            l.Text = path;            
        }

        private void Compare(string path1, string path2)
        {                       
            string[] files1 = Directory.GetFiles(path1);
            string[] files2 = Directory.GetFiles(path2);
            this.CompareItems(files1, files2);
            
            string[] dirs1 = Directory.GetDirectories(path1);
            string[] dirs2 = Directory.GetDirectories(path2);
            this.CompareItems(dirs1, dirs2, true);
        }        

        private void CompareItems(string[] items1, string[] items2, bool dirs = false)
        {
            List<string> diffs = new List<string>();
            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();
            foreach(string file in items1) {
                list1.Add(file.Replace(base1, ""));
            }
            
            foreach (string file in items2)
            {
                list2.Add(file.Replace(base2, ""));
            }

            foreach (var item in list1)
            {
                if(!list2.Contains(item))  {
                    Console.WriteLine("Different Left: " + item);
                    var image = new System.Windows.Controls.Image();
                    BitmapImage bitmap = new BitmapImage(new Uri(@"C:\Users\John\Documents\GitHub\WindowsFolderComparer\FolderComparer\Media-Different.png", UriKind.Relative));
                    var label = new System.Windows.Controls.Label();
                    label.Content = item;
                    image.Source = bitmap;
                    Grid.SetColumn(image, 0);
                    Grid.SetColumn(label, 1);
                    var rowDef = new RowDefinition();
                    this.gridLeft.RowDefinitions.Add(rowDef);
                    Grid.SetRow(image, this.gridLeft.RowDefinitions.Count - 1);
                    Grid.SetRow(label, this.gridLeft.RowDefinitions.Count - 1);
                    this.gridLeft.Children.Add(image);
                    this.gridLeft.Children.Add(label);
                    diffList.Add(item);
                }
                else
                {
                    if (dirs)
                    {
                        Compare(base1 + item, base2 + item);
                        Console.WriteLine("Same: " + item);
                    }
                }
            }
            
            foreach (var item in list2)
            {
                if (!list1.Contains(item))
                {
                    Console.WriteLine("Different Right: " + item);
                    diffList.Add(item);
                }
            }
            
        }

        private string ShowDialog()
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedPath.ToString();
            }
            else
            {
                return null;
            }
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            base1 = this.tbLeft.Text;
            base2 = this.tbRight.Text;
            Compare(this.tbLeft.Text, this.tbRight.Text);
        }
    }
}
