using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.TextSection
{
    public class TextSectionHtmlGenerator
    {
        SimpleHtmlNode rootNode;
        SimpleHtmlNode currentNode;
        LinkedList<TextSectionObject> sortedTextSections;

        public TextSectionHtmlGenerator()
        {
            sortedTextSections = new LinkedList<TextSectionObject>();

            rootNode = new SimpleHtmlNode("html", null);

            currentNode = new SimpleHtmlNode("head", rootNode);
            rootNode.ChildNodes.Add(currentNode);

            currentNode = new SimpleHtmlNode("body", rootNode);
            rootNode.ChildNodes.Add(currentNode);

            currentNode = new SimpleHtmlNode("p", rootNode);
            rootNode.ChildNodes.Add(currentNode);
        }

        public SimpleHtmlNode GenerateHtmlFromTextSections(Dictionary<Type, List<TextSectionObject>> existingTextSection)
        {
            return rootNode;
        }

        //private LinkedList<TextSectionObject> DictionaryToSortedList()
        //{

        //}

    }
}
