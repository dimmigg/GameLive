using GameLive.Properties;
using System;
using System.Data;
using System.Windows;

namespace GameLive
{
    /// <summary>
    /// Логика взаимодействия для WindowLoad.xaml
    /// </summary>
    public partial class WindowLoad : Window
    {
        public WindowLoad()
        {
            InitializeComponent();
        }

        ConnectionSQL sql = new ConnectionSQL();
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (gamesGrid.SelectedItems != null)
            {
                for (int i = 0; i < gamesGrid.SelectedItems.Count; i++)
                {
                    DataRowView datarowView = gamesGrid.SelectedItems[i] as DataRowView;

                    if (datarowView != null)
                    {
                        DataRow dataRow = (DataRow)datarowView.Row;
                        dataRow.Delete();
                    }
                }
            }
            sql.UpdateDB();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sql.ConnectDataTable(gamesGrid);
        }

        public int LaodGameId
        {
            get { return LoadGameID(); }
        }

        int LoadGameID()
        {
            if (gamesGrid.SelectedItems != null)
            {
                for (int i = 0; i < gamesGrid.SelectedItems.Count; i++)
                {
                    DataRowView datarowView = gamesGrid.SelectedItems[i] as DataRowView;
                    if (datarowView != null)
                    {
                        DataRow dataRow = (DataRow)datarowView.Row;
                        return (Int32)dataRow.ItemArray[0];
                    }
                }
            }
            return 0;
        }
    }
}
