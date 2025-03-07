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
using System.Text.RegularExpressions;

namespace Lab8
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

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog()
            {
                Description = "Select directory to open"
            };
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if(dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                fileTreeView.Items.Clear();
                CreateTreeView(folderBrowserDialog.SelectedPath);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CreateTreeView(String rootPath)
        {
            var root = new TreeViewItem();
            root.Tag = rootPath;
            root.Header = System.IO.Path.GetFileName(rootPath);
            root.ContextMenu = CreateDictionaryContextMenu();
            
            fileTreeView.Items.Add(root);

            AddFiles(rootPath, root);
            AddDirectories(rootPath, root);
        }

        private void AddFiles(String path, TreeViewItem root)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach(var file in  directory.GetFiles())
            {
                var item = new TreeViewItem();
                item.Tag = file.FullName;
                item.Header = file.Name;
                item.ContextMenu = CreateFileContextMenu();
                root.Items.Add(item);
            }
        }

        private void AddDirectories(String path, TreeViewItem root)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach(var dir in directory.GetDirectories())
            {
                var item = new TreeViewItem();
                item.Tag = dir.FullName;
                item.Header = dir.Name;
                item.ContextMenu = CreateDictionaryContextMenu();
                root.Items.Add(item);
                AddFiles(dir.FullName, item);
                AddDirectories(dir.FullName, item);
            }
        }

        private System.Windows.Controls.ContextMenu CreateFileContextMenu()
        {
            System.Windows.Controls.ContextMenu contextMenu = new System.Windows.Controls.ContextMenu();
            System.Windows.Controls.MenuItem deleteItem = new System.Windows.Controls.MenuItem();
            deleteItem.Header = "Delete";
            deleteItem.Click += DeleteItem_Click;

            System.Windows.Controls.MenuItem openItem = new System.Windows.Controls.MenuItem();
            openItem.Header = "Open";
            openItem.Click += OpenItem_Click;

            contextMenu.Items.Add(openItem);
            contextMenu.Items.Add(deleteItem);
            
            return contextMenu;
        }

        private void OpenItem_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = fileTreeView.SelectedItem as TreeViewItem; 
            if (selectedItem != null)
            {
                string pathToFile = selectedItem.Tag.ToString();
                if (File.Exists(pathToFile))
                {
                    fileTextBlock.Text = File.ReadAllText(pathToFile);
                }
            }
        }

        private System.Windows.Controls.ContextMenu CreateDictionaryContextMenu()
        {
            System.Windows.Controls.ContextMenu contextMenu = new System.Windows.Controls.ContextMenu();
            System.Windows.Controls.MenuItem deleteItem = new System.Windows.Controls.MenuItem();
            deleteItem.Header = "Delete";
            deleteItem.Click += DeleteItem_Click;

            System.Windows.Controls.MenuItem createItem = new System.Windows.Controls.MenuItem();
            createItem.Header = "Create";
            createItem.Click += CreateItem_Click;

            contextMenu.Items.Add(createItem);
            contextMenu.Items.Add(deleteItem);
            
            return contextMenu;
        }

        private void CreateItem_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = fileTreeView.SelectedItem as TreeViewItem;
            if(selectedItem != null)
            {
                var createDlg = new CreateFileWindow();
                if(createDlg.ShowDialog() == true)
                {
                    string name = createDlg.txtName.Text;
                    bool isFileChecked = createDlg.rbFile.IsChecked ?? false;
                    bool isDirChecked = createDlg.rbDirectory.IsChecked ?? false;
                    bool isReadOnly = createDlg.boxRO.IsChecked ?? false;
                    bool isArchive = createDlg.boxA.IsChecked ?? false;
                    bool isHidden = createDlg.boxH.IsChecked ?? false;
                    bool isSystem = createDlg.boxS.IsChecked ?? false;
                    string path = System.IO.Path.Combine(selectedItem.Tag.ToString(), name);
                    if (isFileChecked)
                    {
                        if(Regex.IsMatch(name, @"^[\w~\-]{1,8}\.(txt|php|html)$"))
                        {
                            CreateFile(name, path, selectedItem);
                            setDosArguments(path, isReadOnly, isArchive, isHidden, isSystem);
                        }
                        else
                        {
                            var errorDlg = new ErrorNameWindow();
                            errorDlg.ShowDialog();
                        }
                    }
                    if(isDirChecked)
                    {
                        if(name.Length > 0)
                        {
                            CreateDirectory(name, path, selectedItem);
                            setDosArguments(path, isReadOnly, isArchive, isHidden, isSystem);
                        }
                        else
                        {
                            var errorDlg = new ErrorNameWindow();
                            errorDlg.ShowDialog();
                        }
                        
                    }
                }
            }
        }

        private void setDosArguments(string path, bool isReadOnly, bool isArchive, bool isHidden, bool isSystem)
        {
            FileAttributes fileAttributes = FileAttributes.Normal;
            if (isReadOnly) fileAttributes |= FileAttributes.ReadOnly;
            if (isHidden) fileAttributes |= FileAttributes.Hidden;
            if (isArchive) fileAttributes |= FileAttributes.Archive;
            if (isSystem) fileAttributes |= FileAttributes.System;
            File.SetAttributes(path, fileAttributes);
        }

        private void CreateFile(string name, string path, TreeViewItem parentItem)
        {
            File.Create(path);
            TreeViewItem newItem = new TreeViewItem();
            newItem.Tag = path;
            newItem.Header = name;
            newItem.ContextMenu = CreateFileContextMenu();
            parentItem.Items.Add(newItem);
        }

        private void CreateDirectory(string name, string path, TreeViewItem parentItem)
        {
            Directory.CreateDirectory(path);
            TreeViewItem newItem = new TreeViewItem();
            newItem.Tag = path;
            newItem.Header = name;
            newItem.ContextMenu = CreateDictionaryContextMenu();
            parentItem.Items.Add(newItem);
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = fileTreeView.SelectedItem as TreeViewItem;
            if(selectedItem != null)
            {
                string pathToFile = selectedItem.Tag.ToString();
                if (File.Exists(pathToFile))
                {
                    SetAttributesBeforeDelete(pathToFile);
                    File.Delete(pathToFile);
                }
                else
                {
                    if (Directory.Exists(pathToFile))
                    {
                        DeleteRecursive(pathToFile);
                        SetAttributesBeforeDelete(pathToFile);
                        Directory.Delete(pathToFile);
                    }
                }
                TreeViewItem parentItem = selectedItem.Parent as TreeViewItem;
                parentItem.Items.Remove(selectedItem);
            }
        }

        private void DeleteRecursive(string pathToFile)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(pathToFile);
            foreach(var file in directoryInfo.GetFiles())
            {
                SetAttributesBeforeDelete(file.FullName);
                File.Delete(file.FullName);
            }
            foreach(var dir in directoryInfo.GetDirectories())
            {
                DeleteRecursive(dir.FullName);
                SetAttributesBeforeDelete(dir.FullName);
                Directory.Delete(dir.FullName);
            }
        }

        private void SetAttributesBeforeDelete(string pathToFile)
        {
            FileAttributes attributes = File.GetAttributes(pathToFile);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                attributes &= ~FileAttributes.ReadOnly;
                File.SetAttributes(pathToFile, attributes);
            }
            if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                attributes &= ~FileAttributes.Hidden;
                File.SetAttributes(pathToFile, attributes);
            }
        }

        private void fileTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem selectedItem = fileTreeView.SelectedItem as TreeViewItem;
            if(selectedItem != null)
            {
                string pathToFile = selectedItem.Tag.ToString();
                dosTextBlock.Text = getDosAttributes(pathToFile);
            }
        }

        private string getDosAttributes(string path)
        {
            string attributes = "";
            FileAttributes fileAttributes = File.GetAttributes(path);
            if((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                attributes += "r";
            }
            else
            {
                attributes += "-";
            }
            if ((fileAttributes & FileAttributes.Archive) == FileAttributes.Archive)
            {
                attributes += "a";
            }
            else
            {
                attributes += "-";
            }
            if ((fileAttributes & FileAttributes.System) == FileAttributes.System)
            {
                attributes += "s";
            }
            else
            {
                attributes += "-";
            }
            if ((fileAttributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                attributes += "h";
            }
            else
            {
                attributes += "-";
            }
            return attributes;
        }

      
    }
}
