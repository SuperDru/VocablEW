using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для CheckingWindow.xaml
    /// </summary>
    public partial class CheckingWindow : Window
    {
        CheckingWords chw = new CheckingWords();
        string issueWord, correctWord;
        int count;
        public CheckingWindow()
        {
            InitializeComponent();
            handleWords();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StartButtons.FlagOfPressChecking = false;
        }

        private void btnOpenWeb_Click(object sender, RoutedEventArgs e)
        {
            chw.openWeb(ListOfWords.Studied);
        }

        //Проверяем слово на корректность. Если верно, то увеличиваем счётчик слова, иначе выводим верное слово, 
        //Уменьшаем счётчик слова и через 5 сек продолжаем
        //Если счётчик будет равен 20, то считаем слово полностью выученным и удаляем его из словаря
        //Если счётчик будет равен -10, то считаем слово невыученным и перемещаем его в список "слов на изучении"
        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            check();
        }

        private void entryField_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                check();
        }

        private void handleWords()
        {
            if (count++ > 20) Close();
            issueWord = chw.getWord();
            correctWord = chw.getCorrectWord();
            entryField.Text = string.Empty;
            lblCorrect.Content = string.Empty;
            lblIssue.Content = issueWord;
        }

        //Получаем новое слово и выводим его на экран
        //Увеличиваем счётчик слов на 1 и если он превысил 20, то закрываем окно
        private async void check()
        {
            int countRight;
            if (entryField.Text == correctWord)
                countRight = chw.incCount();
            else
            {
                btnCheck.Background = new SolidColorBrush(Color.FromRgb(193, 3, 3));
                countRight = chw.decCount();
                lblCorrect.Content = correctWord;
                await Task.Factory.StartNew(() => Thread.Sleep(5 * 1000));
                btnCheck.Background = new SolidColorBrush(Color.FromRgb(53, 64, 93));
            }
            if (countRight >= 20)
                chw.removeWord();
            if (countRight <= -10)
                chw.transferWord();
            handleWords();
        }
    }
}
