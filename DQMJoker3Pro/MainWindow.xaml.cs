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
using Microsoft.Win32;

namespace DQMJoker3Pro
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

		private void Window_PreviewDragOver(object sender, DragEventArgs e)
		{
			e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
		}

		private void Window_Drop(object sender, DragEventArgs e)
		{
			String[] files = e.Data.GetData(DataFormats.FileDrop) as String[];
			if (files == null) return;

			FileOpen(files[0], false);
		}

		private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e)
		{
			FileOpen(false);
		}

		private void MenuItemFileForceOpen_Click(object sender, RoutedEventArgs e)
		{
			FileOpen(true);
		}

		private void MenuItemFileSave_Click(object sender, RoutedEventArgs e)
		{
			SaveData.Instance().Save();
		}

		private void MenuItemFileImport_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Import(dlg.FileName);
		}

		private void MenuItemFileExport_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Instance().Export(dlg.FileName);
		}

		private void MenuItemFileExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void ButtonChoiceMonsterType_Click(object sender, RoutedEventArgs e)
		{
			var monster = (sender as Button)?.DataContext as Monster;
			if (monster == null) return;

			monster.Type = Choice(ChoiceWindow.eType.eType, monster.Type);
		}

		private void ButtonChoiceMonsterProperty1_Click(object sender, RoutedEventArgs e)
		{
			var monster = (sender as Button)?.DataContext as Monster;
			if (monster == null) return;

			monster.Property1 = Choice(ChoiceWindow.eType.eProperty, monster.Property1);
		}

		private void ButtonChoiceMonsterProperty2_Click(object sender, RoutedEventArgs e)
		{
			var monster = (sender as Button)?.DataContext as Monster;
			if (monster == null) return;

			monster.Property2 = Choice(ChoiceWindow.eType.eProperty, monster.Property2);
		}

		private void ButtonChoiceMonsterProperty3_Click(object sender, RoutedEventArgs e)
		{
			var monster = (sender as Button)?.DataContext as Monster;
			if (monster == null) return;

			monster.Property3 = Choice(ChoiceWindow.eType.eProperty, monster.Property3);
		}

		private void FileOpen(bool force)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;
			FileOpen(dlg.FileName, force);
		}

		private void FileOpen(String filename, bool force)
		{
			if (SaveData.Instance().Open(filename, force) == false)
			{
				MessageBox.Show("CheckSum Error", "File Open", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			DataContext = new ViewModel();
		}

		private uint Choice(ChoiceWindow.eType type, uint id)
		{
			var dlg = new ChoiceWindow();
			dlg.ID = id;
			dlg.Type = type;
			dlg.ShowDialog();
			return dlg.ID;
		}
	}
}
