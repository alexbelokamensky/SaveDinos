using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace lucruIndividual2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class Meteorit
    {
        public Image? ImgMeteorit { get; set; }
        public double Speed { get; set; }
        public bool _isExpoding { get; set; } = false;
        public int _ExplodingFramesRemaining { get; set; } = 0;
    }
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private bool _IsGameStopped = true;

        private bool _IsMoveRight = false;
        private bool _IsMoveLeft = false;

        private const int _playerInitial_Speed = 10;
        private const int _playerInitial_xPosition= 125;
        private const int _playerAcceleration = 1;
        private int _xPosition = _playerInitial_xPosition;
        private int _playerSpeed = _playerInitial_Speed;

        private double _meteoritAcceleration = 0.5;
        private List<Meteorit> Meteorits = new List<Meteorit>();

        private const int _framesInitial_SpawntMeteorit = 25;
        private int _framesSpawnMeteorit = _framesInitial_SpawntMeteorit;
        private int _framesCounter = 0;

        private int _level = 0; 
        private int _score = 0;
        private int _recordScore = 0;

        private const int _livesInitial = 3;
        private int _lives = _livesInitial;
        private readonly Random _random = new Random();
        public int angle = 0;
        public int _rotateSpeed = 2;
        public MainWindow()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(30);
            _timer.Tick += OnTimerTick;
            Canvas.SetLeft(Player, _playerInitial_xPosition);
            HideGamePart();
            CreateAnimations();
            LoadRecord();
        }

        //управление видимостью объектов
        public void ShowGamePart()
        {
            imgStart.Visibility = Visibility.Hidden;
            tbScore.Visibility = Visibility.Visible;
            heart1.Visibility = Visibility.Visible;
            heart2.Visibility = Visibility.Visible;
            heart3.Visibility = Visibility.Visible;
            bRestart.Visibility = Visibility.Visible;
            bPause.Visibility = Visibility.Visible;
            Player.Visibility = Visibility.Visible;
        }
        public void HideGamePart()
        {
            tbScore.Visibility = Visibility.Hidden;
            heart1.Visibility = Visibility.Hidden;
            heart2.Visibility = Visibility.Hidden;
            heart3.Visibility = Visibility.Hidden;
            bRestart.Visibility = Visibility.Hidden;
            bPause.Visibility = Visibility.Hidden;
            Player.Visibility = Visibility.Hidden;
        }
        public void ShowMenuPart()
        {
            if (_lives == 0) { imgGameOver.Visibility = Visibility.Visible; imgGameLogo.Visibility = Visibility.Hidden; }
            else { imgGameLogo.Visibility = Visibility.Visible; imgGameOver.Visibility = Visibility.Hidden; }
            lbSore.Visibility = Visibility.Visible;
            lbRecordScore.Visibility = Visibility.Visible;
            imgCup.Visibility = Visibility.Visible;
            imgStart.Visibility = Visibility.Visible;
            lbRecordScore.Visibility = Visibility.Visible;
            imgCup.Visibility = Visibility.Visible;
        }
        public void HideMenuPart()
        {
            imgGameOver.Visibility = Visibility.Hidden;
            imgGameLogo.Visibility = Visibility.Hidden;
            lbSore.Visibility = Visibility.Hidden;
            lbRecordScore.Visibility = Visibility.Hidden;
            imgCup.Visibility = Visibility.Hidden;
            imgStart.Visibility = Visibility.Hidden;
            lbRecordScore.Visibility = Visibility.Hidden;
            imgCup.Visibility = Visibility.Hidden;
        }

        //игровой процесс
        private void OnTimerTick(object? sender, EventArgs e)
        {
            //движение игрока
            if (_IsMoveRight && _xPosition + Player.Width + _playerSpeed < this.Width && !_IsMoveLeft)
            {
                _xPosition += _playerSpeed;
                _playerSpeed += _playerAcceleration;
            }
            if(_IsMoveLeft && _xPosition > 0 && !_IsMoveRight)
            {
                _xPosition -= _playerSpeed;
                _playerSpeed += _playerAcceleration;
            }
            Canvas.SetLeft(Player, _xPosition);
            //движение метеоритов
            angle += _rotateSpeed;
            for (int i = 0; i < Meteorits.Count; i++)
            {
                RotateTransform rotateTransform = new RotateTransform(angle) { CenterX = Meteorits[i].ImgMeteorit.ActualHeight / 2, CenterY = Meteorits[i].ImgMeteorit.ActualWidth / 2 };
                Meteorits[i].ImgMeteorit.RenderTransform = rotateTransform;
                if (Meteorits[i]._isExpoding)
                {
                    Meteorits[i]._ExplodingFramesRemaining--;
                    if (Meteorits[i]._ExplodingFramesRemaining == 0)
                    {
                        GameArea.Children.Remove(Meteorits[i].ImgMeteorit);
                        Meteorits.RemoveAt(i);
                    }
                }
                else
                {
                    Canvas.SetTop(Meteorits[i].ImgMeteorit, Canvas.GetTop(Meteorits[i].ImgMeteorit) + Meteorits[i].Speed);
                    Meteorits[i].Speed += _meteoritAcceleration;
                }
                //проверка столкновения игрока и метеорита
                if (Canvas.GetTop(Meteorits[i].ImgMeteorit) > 500-Player.Height && Canvas.GetTop(Meteorits[i].ImgMeteorit) < 500 &&
                    Canvas.GetLeft(Meteorits[i].ImgMeteorit) <= Canvas.GetLeft(Player) + Player.Width &&
                    Canvas.GetLeft(Meteorits[i].ImgMeteorit) + Meteorits[i].ImgMeteorit.Width >= Canvas.GetLeft(Player))
                {   

                        Meteorits[i].ImgMeteorit.Source = new BitmapImage(new Uri("pack://application:,,,/images/explode.png"));
                        Meteorits[i]._ExplodingFramesRemaining = 1;
                        Meteorits[i]._isExpoding = true;
                        ScoreChange();
                    
                }
                if (Canvas.GetTop(Meteorits[i].ImgMeteorit) > 710)
                {
                    GameArea.Children.Remove(Meteorits[i].ImgMeteorit);
                    Meteorits.RemoveAt(i);
                    LivesChange();
                }
            }
            _framesCounter++;
            if (_framesCounter > _framesSpawnMeteorit) { 
                CreateMeteorit();
                _framesCounter = 0;
            }
        }
        private void ScoreChange()
        {
            _score += 20;
            tbScore.Text = $"Score: {_score:D5}";
            lbSore.Content = $"Score: {_score:D5}";
            if (_score >= 100 && _score < 1000) { _framesSpawnMeteorit = 15; _level = 1; _rotateSpeed = 4;  }
            else if (_score >= 1000) { _framesSpawnMeteorit = 12; _level = 2; _rotateSpeed = 8; }
        }
        private void LivesChange()
        {
            _lives--;
            if (_lives == 0)
            {
                GameOver();
            }
            if (_lives == 1)
            {
                heart2.Source = new BitmapImage(new Uri("pack://application:,,,/images/emptyheart.png"));
                Player.Source = new BitmapImage(new Uri("pack://application:,,,/images/lowhpplayer.png"));
            }
            if (_lives == 2)
            {
                heart3.Source = new BitmapImage(new Uri("pack://application:,,,/images/emptyheart.png"));
                Player.Source = new BitmapImage(new Uri("pack://application:,,,/images/middlehpplayer.png"));
            }
        }
        public void CreateMeteorit()
        {
            Image imgMeteorit = new Image
            {
                Width = 75,
                Height = 75
            };
            int imagenr = _random.Next(0, 3);
            switch (imagenr)
            {
                case 0: imgMeteorit.Source = new BitmapImage(new Uri("pack://application:,,,/images/meteorit1.png")); break;
                case 1: imgMeteorit.Source = new BitmapImage(new Uri("pack://application:,,,/images/meteorit2.png")); break;
                case 2: imgMeteorit.Source = new BitmapImage(new Uri("pack://application:,,,/images/meteorit3.png")); break;
                default: imgMeteorit.Source = new BitmapImage(new Uri("pack://application:,,,/images/meteorit1.png")); break;
            }
            GameArea.Children.Add(imgMeteorit);


            Meteorit meteorit = new Meteorit
            {
                ImgMeteorit = imgMeteorit,
            };
            switch (_level)
            {
                case 0: meteorit.Speed = 7; break;
                case 1: meteorit.Speed = 12; break;
                case 2: meteorit.Speed = 15; break;
            }
            Meteorits.Add(meteorit);
            Canvas.SetLeft(imgMeteorit, _random.Next(0, 325));
            Canvas.SetTop(imgMeteorit, -110);
        }

        //отслеживание кнопок
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D: _IsMoveRight = true; break;
                case Key.Right: _IsMoveRight = true; break;
                case Key.A: _IsMoveLeft = true; break;
                case Key.Left: _IsMoveLeft = true; break;
                case Key.P: PauseGame(); break;
                case Key.Escape: PauseGame(); break;
                case Key.R: RestartGame(); break;
            }
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D: _IsMoveRight = false; _playerSpeed = _playerInitial_Speed; break;
                case Key.Right: _IsMoveRight = false; _playerSpeed = _playerInitial_Speed; break;
                case Key.A: _IsMoveLeft = false; _playerSpeed = _playerInitial_Speed; break;
                case Key.Left: _IsMoveLeft = false; _playerSpeed = _playerInitial_Speed; break;
            }
        }

        //кнопки меню
        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btRestart_Click(object sender, RoutedEventArgs e)
        {
            RestartGame();
        }
        private void btPause_Click(object sender, RoutedEventArgs e)
        {
            PauseGame();
        }
        private void imgStart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PauseGame();
            HideMenuPart();
            ShowGamePart();
            _IsGameStopped = false;
        }

        //логика кнопок меню
        private void RestartGame()
        {
            HideGamePart();
            ShowMenuPart();
            tbScore.Text = "Score: 00000";
            _score = 0;
            _xPosition = _playerInitial_xPosition;
            Canvas.SetLeft(Player, _xPosition);
            for (int i = 0; i < Meteorits.Count; i++)
            {
                GameArea.Children.Remove(Meteorits[i].ImgMeteorit);
            }
            _framesCounter = 0;
            Meteorits.Clear();
            _lives = _livesInitial;
            heart1.Source = new BitmapImage(new Uri("pack://application:,,,/images/fullheart.png"));
            heart2.Source = new BitmapImage(new Uri("pack://application:,,,/images/fullheart.png"));
            heart3.Source = new BitmapImage(new Uri("pack://application:,,,/images/fullheart.png"));
            Player.Source = new BitmapImage(new Uri("pack://application:,,,/images/fullhpplayer.png"));
            if (!_IsGameStopped)
            {
                _timer.Stop();
                _IsGameStopped = true;
            }
        }
        private void PauseGame()
        {
            if (!_IsGameStopped)
            {
                _timer.Stop();
                _IsGameStopped = true;
                ShowMenuPart();
            }
            else
            {
                _timer.Start();
                _IsGameStopped = false;
                HideMenuPart();
                ShowGamePart();
            }
            if(_lives == 0)
            {
                RestartGame();
            }
        }
        private void GameOver()
        {
            lbSore.Content = $"Score: {_score:D5}";

            if(_recordScore < _score)
            {
                SaveRecord();
                LoadRecord();
            }
            PauseGame();
        }

        //анимации
        private void CreateAnimations()
        {
            //анимация игрока
            DoubleAnimation playerOpacityAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.8,
                Duration = TimeSpan.FromMilliseconds(200),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            //анимация кнопки старта
            ScaleTransform scaleTransform = new ScaleTransform();
            imgStart.RenderTransform = scaleTransform;
            imgStart.RenderTransformOrigin = new Point(0.5, 0.5);

            DoubleAnimation scaleXAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 1.1,
                Duration = TimeSpan.FromMilliseconds(400),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            DoubleAnimation scaleYAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 1.1,
                Duration = TimeSpan.FromMilliseconds(400),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };


            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(playerOpacityAnimation);
            storyboard.Children.Add(scaleXAnimation);
            storyboard.Children.Add(scaleYAnimation);

            Storyboard.SetTarget(playerOpacityAnimation, Player);
            Storyboard.SetTarget(scaleXAnimation, imgStart);
            Storyboard.SetTarget(scaleYAnimation, imgStart);

            Storyboard.SetTargetProperty(playerOpacityAnimation, new PropertyPath("Opacity"));
            Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("RenderTransform.ScaleY"));


            storyboard.Begin();
        }
        private void btClose_MouseLeave(object sender, MouseEventArgs e)
        {
            bClose.Background = Brushes.Red;
            btClose.Foreground = Brushes.White;
        }
        private void btClose_MouseEnter(object sender, MouseEventArgs e)
        {
            bClose.Background = Brushes.DarkRed;
            btClose.Foreground = Brushes.LightGray;
        }
        private void btRestart_MouseEnter(object sender, MouseEventArgs e)
        {
            bRestart.Background = Brushes.DarkGoldenrod;
            btRestart.Foreground = Brushes.LightGray;
        }
        private void btRestart_MouseLeave(object sender, MouseEventArgs e)
        {
            bRestart.Background = Brushes.Gold;
            btRestart.Foreground = Brushes.White;
        }
        private void btPause_MouseEnter(object sender, MouseEventArgs e)
        {
            bPause.Background = Brushes.DarkGreen;
            btPause.Foreground = Brushes.LightGray;
        }
        private void btPause_MouseLeave(object sender, MouseEventArgs e)
        {
            bPause.Background = Brushes.Green;
            btPause.Foreground = Brushes.White;
        }

        //сохранение и чтение рекорда
        private void SaveRecord()
        {
            string fileName = "../../../Record.json";
            File.WriteAllText(fileName, JsonSerializer.Serialize(_score));
        }
        private void LoadRecord()
        {
            string fileName = "../../../Record.json";
            _recordScore = Convert.ToInt32(File.ReadAllText(fileName));
            lbRecordScore.Content = $"{_recordScore:D5}";
        }
    }
}