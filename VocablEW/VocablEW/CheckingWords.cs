using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocablEW
{
    class CheckingWords : WorkWithWords
    {
        //Комменатрии к перезаписанным методам даны в классе-предке
        List<int> randCheck = new List<int>();
        public override string getWord()
        {
            int k = rand.Next(0, 2);
      loot: id = rand.Next(data.MaxIdStudied + 1);
            foreach (int i in randCheck)
                if (i == id) goto loot;
            randCheck.Add(id);
            correctWord = k == 0 ? data.getRusWord(id, ListOfWords.Studied) : data.getEngWord(id, ListOfWords.Studied);
            issueWord = k == 1 ? data.getRusWord(id, ListOfWords.Studied) : data.getEngWord(id, ListOfWords.Studied);
            return issueWord;
        }

        public override void removeWord()
        {
            data.removeWord(id, ListOfWords.Studied);
        }

        //Увеличиваем счётчик текущего слова
        public int incCount()
        {
            return data.incCount(id);
        }

        //Уменьшаем счётчик текущего слова
        public int decCount()
        {
            return data.decCount(id);
        }

        public override void transferWord()
        {
            string engWord = data.getEngWord(id, ListOfWords.Studied);
            string rusWord = data.getRusWord(id, ListOfWords.Studied);
            removeWord();
            data.addWord(engWord, rusWord, ListOfWords.Studying);
        }
    }
}
