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

namespace VocablEW
{
    /// <summary>
    /// Логика взаимодействия для WindowListWords.xaml
    /// </summary>
    public partial class WindowListWords : Window
    {
        DataHandler data = new DataHandler("Data.xml");

        //При запуске окна считываем все слова из xml документа и выводим их на экран
        public WindowListWords()
        {
            InitializeComponent();
            studying.Text = "                   Studying words";
            studied.Text = "                   Studied words";
            for (int i = 0; i <= data.MaxIdStudying; i++)
            {
                studying.Text += "\n" + data.getEngWord(i, ListOfWords.Studying) + " - " + data.getRusWord(i, ListOfWords.Studying);
            }
            for (int i = 0; i <= data.MaxIdStudied; i++)
            {
                studied.Text += "\n" + data.getEngWord(i, ListOfWords.Studied) + " - " + data.getRusWord(i, ListOfWords.Studied);
            }
        }

        private void Window_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            double x = System.Windows.Forms.Control.MousePosition.X;
            double y = System.Windows.Forms.Control.MousePosition.Y;
            if (x < 880 || y < 438 || y > 728)
                Close();
        }
    }
}
