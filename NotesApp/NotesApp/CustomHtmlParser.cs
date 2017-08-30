using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class CustomHtmlParser
    {
        public CustomHtmlParser()
        {
        }

        public SimpleHtmlNode ParseXML(Stream fileReader)
        {

            SimpleHtmlNode rootNode;

            System.IO.StreamReader lineReader = new StreamReader(fileReader);

            int readCharInt = -1;
            char readChar;

            List<SimpleHtmlAttribute> delayedAttributes = new List<SimpleHtmlAttribute>();

            bool readTag = false;
            bool closeTag = false;
            bool readComment = false;
            bool readAttributes = false;

            string attribute = "";
            string tagName = "";
            string text = "";

            SimpleHtmlNode currentNode = new SimpleHtmlNode("root",null);
            rootNode = currentNode;

            while ((readCharInt = lineReader.Read()) != -1){
                readChar = (char)readCharInt;

                if (readTag && readChar != '>')
                {
                    if (readAttributes)
                    {
                        attribute += readChar;
                    }

                    if (readChar == ' ')
                    {
                        if (attribute != "")
                        {
                            delayedAttributes.Add(new SimpleHtmlAttribute(attribute));
                            attribute = "";
                        }
                        readAttributes = true;
                    }

                    if (!readAttributes)
                    {
                        tagName += readChar;
                    }
                     
                }

                switch (readChar)
                {
                    case '<':
                        readTag = true;

                        if(text != "")
                        {
                            SimpleHtmlNode newNode = new SimpleHtmlNode("text", currentNode);
                            currentNode.ChildNodes.Add(newNode);
                            newNode.Parent = currentNode;
                            newNode.Content = text;
                            text = "";
                        }

                        break;
                    case '>':
                        readTag = false;
                        readComment = false;
                        readAttributes = false;

                        if (attribute != "")
                        {
                            delayedAttributes.Add(new SimpleHtmlAttribute(attribute));
                            attribute = "";
                        }
                        if (tagName != "")
                        {
                            if (!closeTag)
                            {
                                SimpleHtmlNode newNode = new SimpleHtmlNode(tagName, currentNode);
                                currentNode.ChildNodes.Add(newNode);
                                newNode.Parent = currentNode;
                                if (delayedAttributes.Count > 0)
                                {
                                    newNode.attributes.AddRange(delayedAttributes);
                                    delayedAttributes = new List<SimpleHtmlAttribute>();
                                }
                                currentNode = newNode;
                            }
                            else
                            {
                                if (tagName.Equals(currentNode.tag))
                                {
                                    currentNode.wasClosed = true;
                                    currentNode = currentNode.Parent;
                                    closeTag = false;
                                }
                            }
                            tagName = "";
                        }
                        
                        break;
                    case '/':
                        closeTag = true;
                        tagName = "";
                        break;
                    case '!':
                        readTag = false;
                        readComment = true;
                        tagName = "";
                        break;
                    default:
                        if (!readTag && !closeTag&& !readComment) {
                            text += readChar;
                        }
                        break;
                }
            }

            lineReader.Dispose();
            fileReader.Dispose();

            System.Diagnostics.Debug.WriteLine("" + rootNode.ToString());

            return rootNode;
        }


    }
}
