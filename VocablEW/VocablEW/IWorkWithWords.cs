using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocablEW
{
    interface IWorkWithWords
    {
        string getWord();
        void removeWord();
        void transferWord();
        string getCorrectWord();
        bool check(string answer);
        void openWeb(ListOfWords list);
    }
}
