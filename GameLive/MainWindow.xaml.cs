using GameLive.Properties;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameLive
{ 
    public partial class MainWindow : Window
    {
        public const int FIELD_X = 30;
        public const int FIELD_Y = 30;

        int SumAlive;

        double W;
        double H;

        int[,] MasAlive = new int[FIELD_X, FIELD_Y];
        int[,] MasAliveSide = new int[FIELD_X, FIELD_Y];

        bool IsEqual = true;
        bool IsGo = false;

        SolidColorBrush Live = new SolidColorBrush(Color.FromRgb(255, 235, 160));
        SolidColorBrush Dead = new SolidColorBrush(Color.FromRgb(0, 188, 212));
        Ellipse[,] Fielder = new Ellipse[FIELD_X, FIELD_Y];
        DispatcherTimer Timer = new DispatcherTimer();
        ConnectionSQL Sql = new ConnectionSQL();
        Random Rnd = new Random();

        public bool Swicthed { get => Swicth.Toggled1; }
        public MainWindow()
        {
            InitializeComponent();
            Timer.Tick += Timer_Tick;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowField(MasAlive);
        }

        //Кнопка ЗАНОВО
        private void ButtonShow_Click(object sender, RoutedEventArgs e)
        {
            ShowField(MasAlive);
        }

        //Кнопка СТАРТ/ПАУЗА
        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {

            if (IsGo)
            {
                Timer.Stop();
                IsGo = false;
                buttonGo.Content = "СТАРТ";
            }
            else
            {
                new Log().LogGame(FielderToMas(Fielder), "Нажата кнопка СТАРТ");
                Timer.Start();
                IsGo = true;
                buttonGo.Content = "ПАУЗА";
            }

        }

        //Кнопка СОХРАНИТЬ
        private void ButtonAddGame_Click(object sender, RoutedEventArgs e)
        {
            Compare c = new Compare();
            SaveGameDialog saveGameDialog = new SaveGameDialog();

            IsEqual = true;
            string date = "";

            int[] masIdGames = Sql.SelectIdGames();
            int[,] mas = FielderToMas(Fielder);
            for (int i = 0; i < masIdGames.Length; i++)
            {
                int[,] mas2 = Sql.SelectMap(masIdGames[i], 1);
                if (!(c.IsNotEqualMas(mas, mas2)))
                {
                    date = Sql.GetDateGame(masIdGames[i]).ToString();
                    IsEqual = false;
                }
            }


            if (IsEqual)
            {
                if (saveGameDialog.ShowDialog() == true)
                {
                    //сохранение текущей расстановки
                    int idGame = Sql.AddGame(saveGameDialog.NameSave, 1);
                    for (int i = 0; i < FIELD_X; i++)
                    {
                        for (int j = 0; j < FIELD_Y; j++)
                        {
                            Sql.AddMap(idGame, i, j, mas[i, j], 1);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Не сохранил, есть совпадение. \nДата и время сохранения: " + date);
            }

        }

        //Кнопка ЗАГРУЗИТЬ
        private void ButtonGetGame_Click(object sender, RoutedEventArgs e)
        {
            WindowLoad windowLoad = new WindowLoad();
            if (windowLoad.ShowDialog() == true)
            {

                ShowField(Sql.SelectMap(windowLoad.LaodGameId, 1));
            }
        }

        //Кнопка ЛОГИ
        private void ButtonLogsShow_Click(object sender, RoutedEventArgs e)
        {
            WindowLog windowLog = new WindowLog();
            if (windowLog.ShowDialog() == true)
            {
                ShowField(Sql.SelectMap(windowLog.LaodGameId, 2));
            }

        }
        
        //Кнопка АВТО1
        private void ButtonRandom1_Click(object sender, RoutedEventArgs e)
        {

            int[,] randMas = new int[FIELD_X, FIELD_Y];
            for (int i = 0; i < FIELD_X; i++)
            {
                for (int j = 0; j < FIELD_Y; j++)
                {
                    randMas[i, j] = Rnd.Next(0, 2);
                }
            }
            ShowField(randMas);
        }

        //Кнопка АВТО2
        private void ButtonRandom2_Click(object sender, RoutedEventArgs e)
        {
            int[] masIdGames = Sql.SelectIdGames();
            ShowField(Sql.SelectMap(masIdGames[Rnd.Next(0, masIdGames.Length)], 1));
        }

        //Автоотрисовка следующего поколения
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGen();
            if (SumAlive == 0)
            {
                new Log().LogGame(FielderToMas(Fielder), "Игра завершена");
                Timer.Stop();
                IsGo = false;
                buttonGo.Content = "СТАРТ";
            }
        }

        //Отображение живих клеток
        private void ShowField(int[,] masAlive)
        {
            W = CanvasField.ActualWidth / FIELD_X;
            H = CanvasField.ActualHeight / FIELD_Y;
            for (int i = 0; i < FIELD_X; i++)
            {
                for (int j = 0; j < FIELD_Y; j++)
                {
                    Ellipse dot = new Ellipse();
                    dot.Width = W;
                    dot.Height = H;

                    if (masAlive[i, j] == 1)
                    {
                        dot.Fill = Live;
                    }
                    else
                    {
                        dot.Fill = Dead;
                    }

                    Canvas.SetLeft(dot, j * W);
                    Canvas.SetTop(dot, i * H);
                    dot.Cursor = Cursors.Hand;
                    dot.MouseDown += Dot_MouseDown;
                    dot.MouseEnter += Dot_MouseEnter;
                    CanvasField.Children.Add(dot);
                    Fielder[i, j] = dot;
                }
            }
        }

        //Логика появления живих клеток
        private void NextGen()
        {
            SumAlive = 0;
            if (SwicthWorld.Toggled1)
            {
                // поиск живых огранниченное поле
                for (int row = 0; row < FIELD_X; row++)
                {
                    for (int column = 0; column < FIELD_Y; column++)
                    {
                        int alive = 0;
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                int checkI = row + i;
                                int checkJ = column + j;
                                if ((checkI >= 0 && checkI < FIELD_X) &&
                                    (checkJ >= 0 && checkJ < FIELD_Y) &&
                                    (!(checkI == row && checkJ == column)) &&
                                    Fielder[checkI, checkJ].Fill == Live)
                                {
                                    alive++;
                                }
                            }
                        }
                        MasAliveSide[row, column] = alive;
                    }
                }
            }
            else
            {
                // поиск живых замкнутое поле
                for (int row = 0; row < FIELD_X; row++)
                {
                    for (int column = 0; column < FIELD_Y; column++)
                    {
                        int alive = 0;
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                int checkI = row + i;
                                int checkJ = column + j;
                                if (checkI < 0)
                                {
                                    checkI = FIELD_X - 1;
                                }
                                else
                                {
                                    if (checkI >= FIELD_X)
                                    {
                                        checkI = 0;
                                    }
                                }
                                if (checkJ < 0)
                                {
                                    checkJ = FIELD_Y - 1;
                                }
                                else
                                {
                                    if (checkJ >= FIELD_Y)
                                    {
                                        checkJ = 0;
                                    }
                                }

                                if ((!(checkI == row && checkJ == column)) &&
                                    Fielder[checkI, checkJ].Fill == Live)
                                {
                                    alive++;
                                }
                            }
                        }
                        MasAliveSide[row, column] = alive;
                    }
                }

            }
            //отрисовка нового поколения
            for (int row = 0; row < FIELD_X; row++)
            {
                for (int column = 0; column < FIELD_Y; column++)
                {
                    if (Fielder[row, column].Fill == Dead) //мертвая клетка
                    {
                        if (MasAliveSide[row, column] == 3)
                        {
                            Fielder[row, column].Fill = Live;
                            SumAlive++;
                        }
                        else
                        {
                            Fielder[row, column].Fill = Dead;
                        }
                    }
                    else //живая клетка
                    {
                        if (MasAliveSide[row, column] == 2 || MasAliveSide[row, column] == 3)
                        {
                            Fielder[row, column].Fill = Live;
                        }
                        else
                        {
                            Fielder[row, column].Fill = Dead;
                        }
                    }
                }
            }
        }

        //Формирование массва текущих живых клеток
        int[,] FielderToMas(Ellipse[,] CanvasField)
        {
            int[,] res = new int[FIELD_X, FIELD_Y];
            for (int i = 0; i < FIELD_X; i++)
            {
                for (int j = 0; j < FIELD_Y; j++)
                {
                    if (CanvasField[i, j].Fill == Live)
                    {
                        res[i, j] = 1;
                    }
                    else
                    {
                        res[i, j] = 0;
                    }
                }
            }
            return res;
        }

        //Переключатель рисования
        private void Swicth_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Swicth.Toggled1)
            {
                dot.Foreground = Dead;
                paint.Foreground = Live;
            }
            else
            {
                dot.Foreground = Live;
                paint.Foreground = Dead;
            }
        }

        //Переключатель замкнутого мира
        private void SwicthWorld_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SwicthWorld.Toggled1)
            {
                closed.Foreground = Dead;
                limit.Foreground = Live;
            }
            else
            {
                closed.Foreground = Live;
                limit.Foreground = Dead;
            }
        }

        //Слайдер для задержки отрисовки нового поколения
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double a = e.NewValue / 100;
            Timer.Interval = TimeSpan.FromSeconds(a);
        }

        private void Dot_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && Swicth.Toggled1)
            {
                ((Ellipse)sender).Fill = (((Ellipse)sender).Fill == Dead) ? Live : Dead;
            }
        }

        private void Dot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((Ellipse)sender).Fill = (((Ellipse)sender).Fill == Dead) ? Live : Dead;
        }
    }
}