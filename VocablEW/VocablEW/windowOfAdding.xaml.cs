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
    /// Логика взаимодействия для windowOfAdding.xaml
    /// </summary>
    public partial class WindowOfAdding : Window
    {
        DataHandler data = new DataHandler("Data.xml");
        public WindowOfAdding()
        {
            InitializeComponent();
        }

        private void engWord_GotFocus(object sender, RoutedEventArgs e)
        {
            engWord.Text = string.Empty;
        }

        private void engWord_LostFocus(object sender, RoutedEventArgs e)
        {
            if (engWord.Text == "")
                engWord.Text = "Eng word";
        }

        private void rusWord_LostFocus(object sender, RoutedEventArgs e)
        {
            if (rusWord.Text == "")
                rusWord.Text = "Rus word";
        }

        private void rusWord_GotFocus(object sender, RoutedEventArgs e)
        {
            rusWord.Text = string.Empty;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            data.addWord(engWord.Text, rusWord.Text, ListOfWords.Studying);
            Close();
        }

        private void Window_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            double x = System.Windows.Forms.Control.MousePosition.X;
            double y = System.Windows.Forms.Control.MousePosition.Y;
            if (x < 1191 || y < 606 || y > 728)
                Close();
        }
    }
}
