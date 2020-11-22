using GameLive.Properties;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace GameLive
{
    /// <summary>
    /// Логика взаимодействия для WindowLog.xaml
    /// </summary>
    public partial class WindowLog : Window
    {

        ConnectionSQL sql = new ConnectionSQL();
        bool isChecked;

        DateTime selectedDate;
        public WindowLog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sql.ConnectLogsTable(logsGrid, "", "", 2);
            calendar.SelectedDate = DateTime.Now;
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

        }

        private void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDate = (DateTime)calendar.SelectedDate;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            time1.IsEnabled = true;
            time2.IsEnabled = true;
            isChecked = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            time1.IsEnabled = false;
            time2.IsEnabled = false;
            isChecked = false;
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            string dateTime1;
            string dateTime2;
            if (isChecked)
            {
                var selectTime1 = (DateTime)time1.SelectedTime;
                var selectTime2 = (DateTime)time2.SelectedTime;
                dateTime1 = selectedDate.ToString("yyyyMMdd") + " " + selectTime1.ToString("HH:mm:ss");
                dateTime2 = selectedDate.ToString("yyyyMMdd") + " " + selectTime2.ToString("HH:mm:ss");
            }
            else
            {
                dateTime1 = selectedDate.ToString("yyyyMMdd") + " 00:00:00";
                dateTime2 = selectedDate.ToString("yyyyMMdd") + " 23:59:59";
            }
            sql.ConnectLogsTable(logsGrid, dateTime1, dateTime2, 1);
        }

        private void ButtonReloadTable_Click(object sender, RoutedEventArgs e)
        {
            sql.ConnectLogsTable(logsGrid, "", "", 2);
        }

        public int LaodGameId
        {
            get { return LoadGameID(); }
        }

        int LoadGameID()
        {
            if (logsGrid.SelectedItems != null)
            {
                for (int i = 0; i < logsGrid.SelectedItems.Count; i++)
                {
                    DataRowView datarowView = logsGrid.SelectedItems[i] as DataRowView;
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
