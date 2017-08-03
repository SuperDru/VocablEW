using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocablEW
{
    public enum ListOfWords
    {
        Studying,
        Studied
    }
    interface IDataHandler
    {      
        string getEngWord(int id, ListOfWords list);
        string getRusWord(int id, ListOfWords list);
        int incCount(int id);
        int decCount(int id);
        void removeWord(int id, ListOfWords list);
        void addWord(string engWord, string rusWord, ListOfWords list);
    }
}
