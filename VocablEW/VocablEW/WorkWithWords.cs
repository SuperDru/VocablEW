using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VocablEW
{
    public abstract class WorkWithWords : IWorkWithWords
    {
        protected static Random rand = new Random();
        protected DataHandler data = new DataHandler("Data.xml");
        protected string issueWord, correctWord;
        protected int id;

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
