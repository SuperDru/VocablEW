using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VocablEW
{
    /// <summary>
    /// Логика взаимодействия для StudyingWindow.xaml
    /// </summary>
    public partial class StudyingWindow : Window
    {
        StudyingWords stw;
        string issueWord, correctWord;
        int count;//счётчик
        public StudyingWindow()
        {
            try
            {
                stw = new StudyingWords();
            } catch
            {
                return;
            }
            InitializeComponent();
            imgRemove.Source = BitmapToImageSource(Properties.Resources.Remove);
            handleWords();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StartButtons.FlagOfPressStudy = false;
        }

        //Спрашиваем у пользователя, действительно ли он хочет удалить это слово и если да, то удаляем
        private void imgRemove_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            borderRemove.BorderThickness = new Thickness(2);
            MessageBoxResult result = MessageBox.Show("Are you confident that you want to remove this word?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                stw.removeWord();
                handleWords();
            }
        }

        //Проверяем слово на корректность. Если верно, то продолжаем, иначе выводим верное слово 
        //И через 5 сек продолжаем
        private async void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (entryField.Text != correctWord)
            {
                btnCheck.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(193,3,3));
                lblCorrect.Content = correctWord;
                await Task.Factory.StartNew(() => Thread.Sleep(5 * 1000));
                btnCheck.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(53, 64, 93));
            }
            handleWords();
        }

        //При нажатии на Learned если слово введено верно, то добавляем к списку "слов на проверке",
        //Иначе вызываем handleWords 
        private async void btnLearned_Click(object sender, RoutedEventArgs e)
        {
            if (entryField.Text == correctWord)
            {
                stw.transferWord();
                handleWords();
            }
            else
            {
                lblCorrect.Content = correctWord;
                await Task.Factory.StartNew(() => Thread.Sleep(5 * 1000));
                handleWords();
            }
        }

        private void btnOpenWeb_Click(object sender, RoutedEventArgs e)
        {
            stw.openWeb(ListOfWords.Studying);
        }

        private void imgRemove_MouseEnter(object sender, MouseEventArgs e)
        {
            borderRemove.BorderThickness = new Thickness(2);
        }

        private void imgRemove_MouseLeave(object sender, MouseEventArgs e)
        {
            borderRemove.BorderThickness = new Thickness(0);
        }

        private void imgRemove_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            borderRemove.BorderThickness = new Thickness(4);
        }

        private async void entryField_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (entryField.Text != correctWord)
                {
                    btnCheck.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(193, 3, 3));
                    lblCorrect.Content = correctWord;
                    await Task.Factory.StartNew(() => Thread.Sleep(5 * 1000));
                    btnCheck.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(53, 64, 93));
                }
                handleWords();
            }
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            MemoryStream memory = new MemoryStream();
            bitmap.Save(memory, ImageFormat.Png);
            memory.Position = 0;
            BitmapImage bitmapimage = new BitmapImage();
            bitmapimage.BeginInit();
            bitmapimage.StreamSource = memory;
            bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapimage.EndInit();

            return bitmapimage;
        }

        //Получаем новое слово и выводим его на экран
        //Увеличиваем счётчик слов на 1 и если он превысил 5, то закрываем окно
        private void handleWords()
        {
            if (count++ > 5) Close();
            issueWord = stw.getWord();
            correctWord = stw.getCorrectWord();
            entryField.Text = string.Empty;
            lblCorrect.Content = string.Empty;
            lblIssue.Content = issueWord;
        }
    }
}
