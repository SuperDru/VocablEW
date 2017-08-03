using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;

namespace VocablEW
{
    public class DataHandler : IDataHandler
    {
        XmlDocument doc = new XmlDocument();
        XmlElement studyingWords, studiedWords;
        int maxIdStudying, maxIdStudied;
        string path;

        public int MaxIdStudying
        {
            get
            {
                return maxIdStudying;
            }
        }
        public int MaxIdStudied
        {
            get
            {
                return maxIdStudied;
            }
        }

        public DataHandler(string path)
        {
            this.path = path;
            try
            {
                doc.Load(path);
            }
            catch
            {
                MessageBox.Show("Data.xml absent in this folder.");
                Environment.Exit(0);
            }
            studyingWords = (XmlElement)doc.DocumentElement.ChildNodes[0];
            studiedWords = (XmlElement)doc.DocumentElement.ChildNodes[1];
            maxIdStudying = studyingWords.ChildNodes.Count - 1;
            maxIdStudied = studiedWords.ChildNodes.Count - 1;
        }

        public string getEngWord(int id, ListOfWords list)
        {
            return list == ListOfWords.Studying ? getWord(id, 0, studyingWords) : getWord(id, 0, studiedWords);
        }

        public string getRusWord(int id, ListOfWords list)
        {
            return list == ListOfWords.Studying ? getWord(id, 1, studyingWords) : getWord(id, 1, studiedWords);
        }

        public int incCount(int id)
        {
            return changeCount(id, true);
        }

        public int decCount(int id)
        {
            return changeCount(id, false);
        }

        public void addWord(string engWord, string rusWord, ListOfWords list)
        {
            addXmlWord(list == ListOfWords.Studying ? studyingWords : studiedWords, engWord, rusWord);
        }

        public void removeWord(int id, ListOfWords list)
        {
            XmlElement nodes = list == ListOfWords.Studied ? studiedWords : studyingWords;
            removeElement(id, nodes);
            if (list == ListOfWords.Studied)
            {
                maxIdStudied--;
            }
            else
                maxIdStudying--;
            doc.Save(path);
        }



        private int changeCount(int id, bool f)
        {
            XmlNodeList list = studiedWords.ChildNodes;
            foreach (XmlElement node in list)
            {
                if (node.GetAttribute("id") == id.ToString())
                {
                    int count = int.Parse(node.GetAttribute("count"));
                    if (f)
                        count++;
                    else
                        count--;
                    node.RemoveAttribute("count");
                    node.SetAttribute("count", count.ToString());
                    doc.Save(path);
                    return count;
                }
            }
            throw new Exception();
        }

        private string getWord(int id, int i, XmlElement nodes)
        {
            XmlNodeList listWords = nodes.ChildNodes;
            foreach (XmlElement node in listWords)
            {
                if (node.GetAttribute("id") == id.ToString())
                    return ((XmlElement)node.ChildNodes[i]).GetAttribute("word");
            }
            throw new Exception();
        }
        
        private void addXmlWord(XmlElement nodes, string engWord, string rusWord)
        {
            XmlElement word = doc.CreateElement("Word");
            word.SetAttribute("id", nodes.ChildNodes.Count.ToString());

            var engWordXml = doc.CreateElement("engWord");
            engWordXml.SetAttribute("word", engWord);
            word.AppendChild(engWordXml);

            var rusWordXml = doc.CreateElement("rusWord");
            rusWordXml.SetAttribute("word", rusWord);
            word.AppendChild(rusWordXml);

            if (nodes.Equals(studiedWords))
            {
                word.SetAttribute("count", "0");
                maxIdStudied++;
            }
            else
                maxIdStudying++;

            nodes.AppendChild(word);
            doc.Save(path);
        }

        private void removeElement(int id, XmlElement list)
        {
            bool b = false;
            XmlNodeList nodeList = list.ChildNodes;
            for (int i = 0; i < nodeList.Count;)
            {
                XmlElement node = (XmlElement)nodeList[i];
                if (b)
                {
                    node.RemoveAttribute("id");
                    node.SetAttribute("id", i.ToString());
                }
                if (node.GetAttribute("id") == id.ToString() && !b)
                {
                    list.RemoveChild(node);
                    b = true;
                }
                else i++;
            }
        }

    }
}
