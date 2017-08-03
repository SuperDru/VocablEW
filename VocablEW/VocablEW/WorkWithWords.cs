using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace VocablEW
{
    public abstract class WorkWithWords : IWorkWithWords
    {
        protected static Random rand = new Random();
        protected DataHandler data;
        protected string issueWord, correctWord;
        protected int id;

        public WorkWithWords()
        {
            try
            {
                data = new DataHandler("Data.xml");
            }
            catch
            {
                MessageBox.Show("Data.xml absent in this folder.");
                ((IDisposable)Application.Current.MainWindow).Dispose();
                Application.Current.MainWindow.Close();
            }
        }

        //Получает рандомное слово из xml документа
        public abstract string getWord();

        //Удаляет текущее слова из xml документа
        public abstract void removeWord();

        //Перемещает текущее слово из одного типа слов к другому
        public abstract void transferWord();

        //Возвращает true, если ответ верный, иначе false
        public bool check(string answer)
        {
            if (answer.ToLower() == correctWord)
                return true;
            else
                return false;
        }

        //Возвращает верный ответ
        public string getCorrectWord()
        {
            if (string.IsNullOrEmpty(correctWord))
                throw new Exception();
            return correctWord;
        }

        //Открыват веб сайт с информацией о слове
        public void openWeb(ListOfWords list)
        {
            Process.Start(
                string.Format("http://wooordhunt.ru/word/{0}", data.getEngWord(id, list)));
        }
    }
}