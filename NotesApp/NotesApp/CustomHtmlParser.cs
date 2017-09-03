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
        private char[][] tagList;
        private bool[] tagActives;

        CharTreeElement[] openerHirarchies;

        //Doctype only first line

        public CustomHtmlParser()
        {
            openerHirarchies = new CharTreeElement[2];

            //Create a char tree of html literal hirarchy - for '<' opener
            CharTreeElement openers = new CharTreeElement() { character = '<', children = new CharTreeElement[2] , valid = true, id = 1 };
            openers.children[0] = new CharTreeElement() { character = '/', children = null, valid = true, id = 2 };
            openers.children[1] = new CharTreeElement() { character = '!', valid = false, children = new CharTreeElement[1] };
            openers.children[1].children[0] = new CharTreeElement() { character = '-', valid = false, children = new CharTreeElement[1] };
            openers.children[1].children[0].children[0] = new CharTreeElement() { character = '-', children = null, valid = true, id = 3 };

            openerHirarchies[0] = openers;

            CharTreeElement closer = new CharTreeElement() { character = '>', children = null , valid = true };
            openerHirarchies[1] = closer;

        }

        private class CharTreeElement
        {
            public short id;
            public bool valid;
            public char character;
            public CharTreeElement[] children;
        }

        /*private char[] commopen = new char[3] {'<','-','-'};
        private bool commOpenActive = false;
        private char[] commclose = new char[3] {'-','-','>'};
        private bool commCloseActive = false;
        private char[] tagopen = new char[1] { '<' };
        private bool tagOpenActive = false;
        private char[] tagclose = new char[2] { '<', '/' };
        private bool tagCloseActive = false;

        private int activeCount;

        private string[] immediateClose = new string[2] { "br", "meta" };*/

        private CharTreeElement currentTree = null;
        private CharTreeElement lastValidNode = null;
        private CharTreeElement CheckTree(char readChar)
        {
            if (currentTree == null)
            {
                foreach (CharTreeElement el in openerHirarchies)
                {
                    if (el.character == readChar)
                    {
                        currentTree = el;
                        if (currentTree.valid)
                        {
                            lastValidNode = currentTree;
                        }
                    }
                }
            }
            else
            {
                bool found = false;
                if (currentTree.children != null)
                {
                    foreach (CharTreeElement child in currentTree.children)
                    {
                        if (child.character == readChar)
                        {
                            currentTree = child;
                            if (currentTree.valid)
                            {
                                lastValidNode = currentTree;
                            }
                            found = true;
                        }
                    }
                }

                if (!found)
                {
                    if (currentTree.children == null)
                    {
                        //leaf found
                        //return leaf
                        return currentTree;
                    }
                    //no child found return parent
                    //return currentTree;
                    return lastValidNode;
                }
            }

            return null;
        }

        private string ParseTag(BackTrackReader lineReader)
        {
            int readCharInt = -1;
            char readChar;
            string tag = "";
            while ((readCharInt = lineReader.Read()) != -1)
            {
                readChar = (char)readCharInt;

                if (readChar == '>')
                {
                    return tag;
                }
                else if(readChar == ' ')
                {
                    ParseAttributes(lineReader);
                }
                else if (readChar != '<' && readChar != '/')
                {
                    tag += readChar;
                }
            }
            return "";
        }

        private Dictionary<string, SimpleHtmlAttribute> delayedAttributes = null;
        private CSSStyling inlineStyling = null;
        private string ParseAttributes(BackTrackReader lineReader)
        {
            delayedAttributes = new Dictionary<string, SimpleHtmlAttribute>();

            int readCharInt = -1;
            char readChar;
            bool namebuilding = true;
            bool parenthesisopened = false;
            string name = "";
            string value = "";
            while ((readCharInt = lineReader.Read()) != -1)
            {
                readChar = (char)readCharInt;

                if (readChar == '>')
                {
                    if (!name.Equals("") && !value.Equals(""))
                    {
                        delayedAttributes.Add(name, new SimpleHtmlAttribute(name, value));
                        if (name.Equals("style"))
                        {
                            inlineStyling = this.ReadCSSStyleValue(value);
                        }
                    }

                    lineReader.ReturnToPreviousChar(1);
                    return "";
                }
                else if(readChar == '=')
                {
                    namebuilding = !namebuilding;
                }
                else if (readChar == '\"')
                {
                    parenthesisopened = !parenthesisopened;
                }
                else if (readChar == ' ' && !namebuilding && !parenthesisopened)
                {
                    delayedAttributes.Add(name, new SimpleHtmlAttribute(name, value));
                    if (name.Equals("style"))
                    {
                        inlineStyling = this.ReadCSSStyleValue(value);
                    }
                    name = "";
                    value = "";
                    namebuilding = !namebuilding;
                }
                else if(readChar != ' ')
                {
                    if (namebuilding)
                    {
                        name += readChar;
                    }
                    else
                    {
                        value += readChar;
                    }
                }
            }
            return "";
        }

        private string IgnoreComment(BackTrackReader lineReader)
        {
            int readCharInt = -1;
            char readChar;
            bool ok = false;
            while ((readCharInt = lineReader.Read()) != -1)
            {
                readChar = (char)readCharInt;
                
                if(readChar == '-')
                {
                    ok = true;
                }
                else if(readChar == '>' && ok)
                {
                    return "";
                }
                else
                {
                    ok = false;
                }
            }
            return "";
        }

        private bool DoImmediateClose(string tagName)
        {
            return tagName.Equals("meta") || tagName.Equals("br");
        }

        private SimpleHtmlNode CloseNode(SimpleHtmlNode toClose, string closeTag)
        {
            if (!DoImmediateClose(closeTag))
            {
                if (toClose.tag.Equals(closeTag))
                {
                    toClose.wasClosed = true;
                    toClose = toClose.Parent;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Html malformed wrong close tag:" + toClose.tag + "/" + "expected " + closeTag);
                }
            }
            return toClose;
        }

        private SimpleHtmlNode OpenNode(SimpleHtmlNode parentNode, string openTag)
        {
            SimpleHtmlNode newNode = new SimpleHtmlNode(openTag, null, null);
            parentNode.AddChild(newNode);
            parentNode = newNode;

            if (DoImmediateClose(openTag))
            {
                parentNode.wasClosed = true;
                parentNode = parentNode.Parent;
            }

            return parentNode;
        }

        private SimpleHtmlNode AddTextNode(SimpleHtmlNode parentNode, string text)
        {
            //Add Text Node befor closing
            if (text != "")
            {
                SimpleHtmlNode newNode = new SimpleHtmlNode("text", null, null);
                //Set content to node BEFORE adding as child, to calculate spans correctly
                newNode.Content = text;
                parentNode.AddChild(newNode);
            }

            return parentNode;
        }

        public SimpleHtmlNode ParseXML2(Stream fileReader, CSSStyleManager styleManager)
        {

            SimpleHtmlNode rootNode;

            BackTrackReader lineReader = new BackTrackReader(fileReader,10);

            int readCharInt = -1;
            char readChar;

            SimpleHtmlNode currentNode = new SimpleHtmlNode("root", null, null);
            rootNode = currentNode;

            int readCnt = 0; ;
            string text = "";
            while ((readCharInt = lineReader.Read()) != -1)
            {
                readChar = (char)readCharInt;

                if (readChar != '\n')
                {

                    CharTreeElement element = CheckTree(readChar);
                    if(currentTree != null)
                    {
                        readCnt++;
                    }

                    //special found
                    if (element != null)
                    {
                        currentTree = null;
                        lastValidNode = null;
                        lineReader.ReturnToPreviousChar(readCnt);
                        readCnt = 0;

                        switch (element.id)
                        {
                            case 1:

                                //Adding Text Node if text exists before opening
                                currentNode = AddTextNode(currentNode, text);
                                text = "";

                                //Opening Tag
                                string opentag = ParseTag(lineReader);
                                currentNode = OpenNode(currentNode, opentag);

                                if(delayedAttributes != null)
                                {
                                    if(delayedAttributes.Count > 0)
                                    {
                                        currentNode.attributes = delayedAttributes;
                                        delayedAttributes = null;
                                    }
                                }
                                if (inlineStyling != null)
                                {
                                    currentNode.htmlCssStyle = inlineStyling;
                                    inlineStyling = null;
                                }

                                break;
                            case 2:
                                
                                //Adding Text Node if text exists before closing
                                currentNode = AddTextNode(currentNode, text);
                                text = "";

                                //Closing Tag
                                string closetag2 = ParseTag(lineReader);
                                currentNode = CloseNode(currentNode, closetag2);
                                break;
                            case 3:
                                //Comment
                                IgnoreComment(lineReader);

                                break;
                            default:

                                break;
                        }

                        text = "";

                    }
                    else if (currentTree == null)
                    {
                        text += readChar;
                    }
                }
            }
            
            return rootNode;
        }

        /*int charCnt = 0;
                while ((readCharInt = lineReader.Read()) != -1)
                {
                    readChar = (char)readCharInt;
                    for(int elemPos = 0; elemPos < tagList.Length; elemPos++)
                    {
                        if(tagList[elemPos].Length > charCnt)
                        {
                            if(tagList[elemPos][charCnt] == readChar)
                            {
                                tagActives[elemPos] = true;
                                activeCount++;
                            }
                            else
                            {
                                if(tagActives[elemPos])
                                {
                                    activeCount--;
                                    tagActives[elemPos] = false;
                                }
                            }
                        }
                    }


                }*/

        public SimpleHtmlNode ParseXML(Stream fileReader, CSSStyleManager styleManager)
        {

            SimpleHtmlNode rootNode;

            System.IO.StreamReader lineReader = new StreamReader(fileReader);

            int readCharInt = -1;
            char readChar;

            char lastChar = ' ';

            Dictionary<string, SimpleHtmlAttribute> delayedAttributes = new Dictionary<string, SimpleHtmlAttribute>();
            
            bool readTag = false;
            bool closeTag = false;
            bool readComment = false;
            bool readAttributes = false;

            bool readHead = false;

            bool fixAttReadinQuotes = false;

            string attribute = "";
            string tagName = "";
            string text = "";

            SimpleHtmlNode currentNode = new SimpleHtmlNode("root",null,null);

            CSSStyling inlineStyling = null;
            rootNode = currentNode;

            while ((readCharInt = lineReader.Read()) != -1){
                readChar = (char)readCharInt;

                //Ignore Line breaks and tabs
                if (readChar != '\r' && readChar != '\n' && readChar != '\t')
                {

                    if (readTag && readChar != '>')
                    {
                        if (readAttributes)
                        {
                            attribute += readChar;
                            if(readChar == '\"')
                            {
                                fixAttReadinQuotes = !fixAttReadinQuotes;
                            }
                        }

                        if (readChar == ' ' && !fixAttReadinQuotes)
                        {
                            if (attribute != "")
                            {
                                System.Diagnostics.Debug.WriteLine("Add Att normal:" + tagName);
                                SimpleHtmlAttribute att = new SimpleHtmlAttribute(attribute);
                                if (att.Name.Equals("style"))
                                {
                                    inlineStyling = this.ReadCSSStyleValue(att.Value);
                                }
                                delayedAttributes.Add(att.Name, att);
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

                            if (text != "")
                            {
                                System.Diagnostics.Debug.WriteLine("Add Text Node:" + text);
                                SimpleHtmlNode newNode = new SimpleHtmlNode("text", null, styleManager);
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
                                System.Diagnostics.Debug.WriteLine("Add Att on Close Tag:" + tagName);
                                SimpleHtmlAttribute att = new SimpleHtmlAttribute(attribute);
                                if (att.Name.Equals("style"))
                                {
                                    inlineStyling = this.ReadCSSStyleValue(att.Value);
                                }
                                delayedAttributes.Add(att.Name, att);
                                attribute = "";
                            }
                            if (tagName != "")
                            {
                                if (!closeTag)
                                {
                                    System.Diagnostics.Debug.WriteLine("Create Node:" + tagName);
                                    SimpleHtmlNode newNode = new SimpleHtmlNode(tagName, inlineStyling, styleManager);
                                    currentNode.AddChild(newNode);
                                    if (delayedAttributes.Count > 0)
                                    {
                                        newNode.attributes = delayedAttributes;
                                        delayedAttributes = new Dictionary<string, SimpleHtmlAttribute>();
                                    }
                                    inlineStyling = null;
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
                                    System.Diagnostics.Debug.WriteLine("Close Tag:" + tagName);
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
                            if (lastChar == '<')
                            {
                                readTag = false;
                                readComment = true;
                                tagName = "";
                            }
                            break;
                        default:
                            if (!readTag && !closeTag && !readComment)
                            {
                                text += readChar;
                            }
                            break;
                    }

                    lastChar = readChar;

                }
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

                //Ignore Line breaks and tabs
                if (readChar != '\r' && readChar != '\n' && readChar != '\t')
                {

                    switch (readChar)
                    {
                        case '{':
                            readTag = false;
                            readpropertyname = true;
                            //Clean up tag name
                            while (tagName[tagName.Length - 1] == ' ')
                            {
                                tagName = tagName.Substring(0, tagName.Length - 1);
                            }

                            break;
                        case '}':
                            readpropertyvalue = false;
                            readpropertyname = false;

                            currentStyle = new CSSStyling(tagName, styleAttributes, isClass, isID);
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
            }

            lineReader.Dispose();
            fileReader.Dispose();

            System.Diagnostics.Debug.WriteLine("" + styleManager.ToString());

            return styleManager;
        }


        private CSSStyling ReadCSSStyleValue(string value)
        {
            StringReader reader = new StringReader(value);

            Dictionary<string, string> tagStyles = new Dictionary<string, string>();

            int readCharInt;
            char readChar;
            bool readpropertyname = true;
            bool readpropertyvalue = false;
            string propertyname = "";
            string propertyvalue = "";

            while ((readCharInt = reader.Read()) != -1)
            {
                readChar = (char)readCharInt;
                //Ignore Line breaks and tabs
                if (readChar != '\r' && readChar != '\n' && readChar != '\t')
                {

                    switch (readChar)
                    {
                        case ':':
                            readpropertyvalue = true;
                            readpropertyname = false;
                            break;
                        case ';':
                            tagStyles.Add(propertyname, propertyvalue);
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
                            }
                            break;
                    }

                }
            }

            if(!propertyname.Equals("") && !propertyvalue.Equals(""))
            {
                tagStyles.Add(propertyname, propertyvalue);
            }

            reader.Dispose();

            return new CSSStyling("", tagStyles, false, false);
        } 

    }
}
