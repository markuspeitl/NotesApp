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

            rootNode = new SimpleHtmlNode("html", null, null);

            currentNode = new SimpleHtmlNode("head", null, null);
            rootNode.AddChild(currentNode);

            currentNode = new SimpleHtmlNode("body", null, null);
            rootNode.AddChild(currentNode);

            currentNode = new SimpleHtmlNode("p", null, null);
            rootNode.AddChild(currentNode);
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
