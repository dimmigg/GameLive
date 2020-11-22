using System.Windows;

namespace GameLive
{
    /// <summary>
    /// Логика взаимодействия для SaveGameDialog.xaml
    /// </summary>
    public partial class SaveGameDialog : Window
    {
        public SaveGameDialog()
        {
            InitializeComponent();
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string NameSave
        {
            get { return saveGame.Text; }
        }


    }
}
