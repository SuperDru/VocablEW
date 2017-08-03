using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VocablEW
{
    class StudyingWords : WorkWithWords
    {
        //Комменатрии к перезаписанным методам даны в классе-предке
        List<int> randCheck = new List<int>();

        public StudyingWords()
        {
            if (data.MaxIdStudying < 4)
            {
                MessageBox.Show("There are less than five words in the dictionary, therefore add some words.", "Warning");
                throw new Exception();
            }
        }

        public override string getWord()
        {
            int k = rand.Next(0, 2);
      loot: id = rand.Next(data.MaxIdStudying + 1);
            foreach (int i in randCheck)
                if (i == id) goto loot;
            randCheck.Add(id);
            correctWord = k == 0 ? data.getRusWord(id, ListOfWords.Studying) : data.getEngWord(id, ListOfWords.Studying);
            issueWord = k == 1 ? data.getRusWord(id, ListOfWords.Studying) : data.getEngWord(id, ListOfWords.Studying);
            return issueWord;
        }

        public override void removeWord()
        {
            data.removeWord(id, ListOfWords.Studying);
        }

        public override void transferWord()
        {
            string engWord = data.getEngWord(id, ListOfWords.Studying);
            string rusWord = data.getRusWord(id, ListOfWords.Studying);
            removeWord();
            data.addWord(engWord, rusWord, ListOfWords.Studied);
        }
    }
}
