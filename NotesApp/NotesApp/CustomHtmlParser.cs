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

        public SimpleHtmlNode ParseXML(Stream fileReader, CSSStyleManager styleManager)
        {

            SimpleHtmlNode rootNode;

            System.IO.StreamReader lineReader = new StreamReader(fileReader);

            int readCharInt = -1;
            char readChar;

            char lastChar = ' ';

            List<SimpleHtmlAttribute> delayedAttributes = new List<SimpleHtmlAttribute>();
            
            bool readTag = false;
            bool closeTag = false;
            bool readComment = false;
            bool readAttributes = false;

            bool readHead = false;

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
                            SimpleHtmlNode newNode = new SimpleHtmlNode("text", styleManager);
                            //Set content to node BEFORE adding as child, to calculate spans correctly
                            newNode.Content = text;
                            currentNode.AddChild(newNode);
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
                                SimpleHtmlNode newNode = new SimpleHtmlNode(tagName, styleManager);
                                currentNode.AddChild(newNode);
                                if (delayedAttributes.Count > 0)
                                {
                                    newNode.attributes.AddRange(delayedAttributes);
                                    delayedAttributes = new List<SimpleHtmlAttribute>();
                                }
                                currentNode = newNode;

                                /*if (readHead)
                                {
                                    currentNode.wasClosed = true;
                                    currentNode = currentNode.Parent;
                                    closeTag = false;
                                }*/
                            }
                            else
                            {
                                /*if (tagName.Equals("head"))
                                {
                                    readHead = false;
                                }*/

                                if (tagName.Equals(currentNode.tag))
                                {
                                    currentNode.wasClosed = true;
                                    currentNode = currentNode.Parent;
                                    closeTag = false;
                                }
                            }

                            /*if (tagName.Equals("head"))
                            {
                                readHead = false;
                            }*/

                            tagName = "";
                        }
                        
                        break;
                    case '/':
                        if (lastChar == '<')
                        {
                            closeTag = true;
                            tagName = "";
                        }
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

                lastChar = readChar;
            }

            lineReader.Dispose();
            fileReader.Dispose();

            System.Diagnostics.Debug.WriteLine("" + rootNode.ToString());

            return rootNode;
        }

        public CSSStyleManager ParseCSS(Stream fileReader)
        {
            CSSStyleManager styleManager = new CSSStyleManager();

            System.IO.StreamReader lineReader = new StreamReader(fileReader);

            int readCharInt = -1;
            char readChar;

            CSSStyling currentStyle;
            Dictionary<string, string> styleAttributes = new Dictionary<string, string>();

            bool readTag = false;
            bool readpropertyname = false;
            bool readpropertyvalue = false;

            bool isID = false;
            bool isClass = false;

            string propertyname = "";
            string propertyvalue = "";

            string tagName = "";
            

            while ((readCharInt = lineReader.Read()) != -1)
            {
                readChar = (char)readCharInt;
                
                switch (readChar)
                {
                    case '{':
                        readTag = false;
                        readpropertyname = true;
                        //Clean up tag name
                        while (tagName[tagName.Length-1] == ' ')
                        {
                            tagName = tagName.Substring(0, tagName.Length - 1);
                        }

                        break;
                    case '}':
                        readpropertyvalue = false;
                        readpropertyname = false;

                        currentStyle = new CSSStyling(tagName, styleAttributes,isClass,isID);
                        styleManager.PutStyle(currentStyle);
                        tagName = "";

                        styleAttributes = new Dictionary<string, string>();
                        isClass = false;
                        isID = false;

                        break;
                    case ':':
                        readpropertyvalue = true;
                        readpropertyname = false;
                        break;
                    case ';':
                        styleAttributes.Add(propertyname, propertyvalue);
                        propertyname = "";
                        propertyvalue = "";
                        readpropertyvalue = false;
                        readpropertyname = true;
                        break;
                    default:
                        if (readChar != ' ')
                        {
                            if (readpropertyname)
                            {
                                propertyname += readChar;
                            }
                            else if (readpropertyvalue)
                            {
                                propertyvalue += readChar;
                            }
                            else if (!readTag)
                            {
                                readTag = true;
                            }
                        }
                        if (readTag)
                        {
                            if (!readpropertyname && !readpropertyvalue)
                            {
                                tagName += readChar;
                                if (readChar == '.')
                                    isClass = true;
                                if (readChar == '#')
                                    isID = true;
                            }
                        }

                        break;
                }
            }

            lineReader.Dispose();
            fileReader.Dispose();

            System.Diagnostics.Debug.WriteLine("" + styleManager.ToString());

            return styleManager;
        }

    }
}
