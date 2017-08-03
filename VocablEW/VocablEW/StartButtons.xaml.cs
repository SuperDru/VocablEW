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
using System.Windows.Shapes;
using System.Threading;
using System.Media;
using System.Runtime.InteropServices;
using System.IO;

namespace VocablEW
{
    /// <summary>
    /// Логика взаимодействия для StartButtons.xaml
    /// </summary>
    public partial class StartButtons : Window
    {
        static bool flagOfPressStudy, flagOfPressChecking;
        StudyingWindow windowStudy;
        CheckingWindow windowChecking;
        WindowOfAdding windowAdding;
        WindowListWords windowWords;
        SoundPlayer sp = new SoundPlayer(Properties.Resources.Ting_sound_effect);
        System.Windows.Forms.NotifyIcon icon;

        //Properties
        public static bool FlagOfPressStudy
        {
            get
            {
                return flagOfPressStudy;
            }

            set
            {
                flagOfPressStudy = value;
            }
        }
        public static bool FlagOfPressChecking
        {
            get
            {
                return flagOfPressChecking;
            }

            set
            {
                flagOfPressChecking = value;
            }
        }

        //При запуске программы вызываем асинхронные методы "слов на изучении" и "слов на проверке"
        public StartButtons()
        {
            InitializeComponent();
            Application.Current.MainWindow.Top = SystemParameters.WorkArea.Height - Application.Current.MainWindow.Height;
            Application.Current.MainWindow.Left = SystemParameters.WorkArea.Width - Application.Current.MainWindow.Width;
            Application.Current.MainWindow.Height += 6;
            Application.Current.MainWindow.Width += 6;
            initIcon();
            startStudy();
            startChecking();
        }

        private void btnStudy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            windowStudy = new StudyingWindow();
            windowStudy.Top = SystemParameters.WorkArea.Height - windowStudy.Height;
            windowStudy.Left = SystemParameters.WorkArea.Width - windowStudy.Width;
            windowStudy.Height += 6;
            windowStudy.Width += 6;
            windowStudy.ShowInTaskbar = false;
            btnStudy.Visibility = Visibility.Hidden;
            FlagOfPressStudy = true;
            windowStudy.Show();
        }

        private void btnCheck_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            windowChecking = new CheckingWindow();
            windowChecking.Top = SystemParameters.WorkArea.Height - windowChecking.Height;
            windowChecking.Left = SystemParameters.WorkArea.Width - windowChecking.Width;
            windowChecking.Height += 6;
            windowChecking.Width += 6;
            windowChecking.ShowInTaskbar = false;
            btnCheck.Visibility = Visibility.Hidden;
            FlagOfPressChecking = true;
            windowChecking.Show();
        }

        //Появляется синяя кнопка, если не запущено окно "слов на проверке", иначе запускается через 5 минут
        //При нажатии на кнопку открывается окно StudyingWindow, а кнопка скрывается и появляется снова через 15-20 мин
        //И дальше повторение.
        private async void startStudy()
        {
            while (true)
            {
                sp.Play();
                if (FlagOfPressChecking || btnCheck.Visibility == Visibility.Visible)
                    await Task.Factory.StartNew(() => Thread.Sleep(5 * 60 * 1000));
                btnStudy.Visibility = Visibility.Visible;
                await Task.Factory.StartNew(() => Thread.Sleep(20 * 1000));
                btnStudy.Visibility = Visibility.Hidden;
                await Task.Factory.StartNew(() => 
                {
                    Random rand = new Random();
                    Thread.Sleep(rand.Next(15 * 60 * 1000, 20 * 60 * 1000));
                });
            }
        }

        private void btn_MouseEnter(object sender, MouseEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            ellipse.Stroke = new SolidColorBrush(Colors.DarkBlue);
            ellipse.StrokeThickness = 4;
        }

        private void btn_MouseLeave(object sender, MouseEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            ellipse.Stroke = new SolidColorBrush(Colors.Black);
            ellipse.StrokeThickness = 1;
        }

        private void btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            ellipse.Stroke = new SolidColorBrush(Colors.DarkBlue);
            ellipse.StrokeThickness = 5;
        }

        //Появляется красная кнопка, если не запущено окно "слов на изучении", иначе запускается через 5 минут
        //При нажатии на кнопку открывается окно CheckingWindow, а кнопка скрывается и появляется снова через 2-3 часа
        //И дальше повторение.
        private async void startChecking()
        {
            while (true)
            {
                await Task.Factory.StartNew(() =>
                {
                    Random rand = new Random();
                    Thread.Sleep(rand.Next(2 * 60 * 60 * 1000, 3 * 60 * 60 * 1000));
                    //Thread.Sleep(rand.Next(40 * 1000, 45 * 1000));
                });
                sp.Play();
                if (FlagOfPressStudy || btnStudy.Visibility == Visibility.Visible)
                    await Task.Factory.StartNew(() => Thread.Sleep(5 * 60 * 1000));
                btnCheck.Visibility = Visibility.Visible;
                await Task.Factory.StartNew(() => Thread.Sleep(20 * 1000));
                btnCheck.Visibility = Visibility.Hidden;
            }
        }

        private void initIcon()
        {
            icon = new System.Windows.Forms.NotifyIcon();
            icon.Icon = Properties.Resources.icoEW;
            System.Windows.Forms.MenuItem[] menuItems = new System.Windows.Forms.MenuItem[3];
            menuItems[0] = new System.Windows.Forms.MenuItem("Add word", new EventHandler(addWord));
            menuItems[1] = new System.Windows.Forms.MenuItem("Show words", new EventHandler(showWords));
            menuItems[2] = new System.Windows.Forms.MenuItem("Exit", new EventHandler(exit));
            icon.ContextMenu = new System.Windows.Forms.ContextMenu(menuItems);
            icon.Visible = true;         
        }

        //При нажатии на "Show words" открывается WindowListWords для показа всех имеющихся слов
        private void showWords(object sender, EventArgs e)
        {
            windowWords = new WindowListWords();
            windowWords.Top = SystemParameters.WorkArea.Height - windowWords.Height;
            windowWords.Left = SystemParameters.WorkArea.Width - windowWords.Width;
            windowWords.Topmost = true;
            windowWords.ShowInTaskbar = false;
            windowWords.Show();
        }

        //При нажатии на "Add word" открывается WindowOfAdding для добавления нового слова в словарь
        private void addWord(object sender, EventArgs e)
        {
            windowAdding = new WindowOfAdding();
            windowAdding.Top = SystemParameters.WorkArea.Height - windowAdding.Height;
            windowAdding.Left = SystemParameters.WorkArea.Width - windowAdding.Width;
            windowAdding.Topmost = true;
            windowAdding.ShowInTaskbar = false;
            windowAdding.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            icon.Dispose();
        }

        private void exit(object sender, EventArgs e)
        {
            if (windowStudy != null)
                windowStudy.Close();
            if (windowChecking != null)
                windowChecking.Close();
            if (windowAdding != null)
                windowAdding.Close();
            if (windowWords != null)
                windowWords.Close();
            Application.Current.Shutdown();
        }
    }
}
