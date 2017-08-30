using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class TextExtendedNode
    {
        public HtmlNode thisHtmlTextNode;
        public int textStart;
        public int textEnd;

        public TextExtendedNode(HtmlNode thisHtmlTextNode, int textStart, int textEnd)
        {
            this.thisHtmlTextNode = thisHtmlTextNode;
            this.textStart = textStart;
            this.textEnd = textEnd;
        }

        public void SetNewText(string newText)
        {

        }
        public void InsertCharacter(int position, char insertCharacter)
        {
            this.thisHtmlTextNode.InnerHtml.Insert(position, ""+ insertCharacter);
            textEnd += 1;
        }
        public void InsertString(int position, string insertString)
        {
            this.thisHtmlTextNode.InnerHtml.Insert(position, insertString);
            textEnd+= insertString.Length;
        }
        public void RemoveCharacter(int position)
        {
            this.thisHtmlTextNode.InnerHtml.Remove(position);
            textEnd -= 1;
        }
        public void RemoveString(int position, int lenght)
        {
            this.thisHtmlTextNode.InnerHtml.Remove(position,lenght);
            textEnd -= lenght;
        }

        public void UpdateTextPosition(int difference)
        {
            textStart += difference;
            textEnd += difference;
        }

        public string GetText()
        {
            return this.thisHtmlTextNode.InnerHtml;
        }

        public void SetNewMasterNode(HtmlNode newMaster)
        {
            if (this.thisHtmlTextNode.ParentNode != null)
            {

                if (!this.thisHtmlTextNode.ParentNode.ParentNode.ChildNodes.Contains(newMaster))
                {
                    this.thisHtmlTextNode.ParentNode.InsertAfter(newMaster, this.thisHtmlTextNode);
                }


                if (this.thisHtmlTextNode.ParentNode != newMaster)
                {
                    if (!newMaster.ChildNodes.Contains(this.thisHtmlTextNode))
                    {
                        this.thisHtmlTextNode.ParentNode.RemoveChild(thisHtmlTextNode);
                        newMaster.AppendChild(this.thisHtmlTextNode);
                    }
                }
            }
        }
    }
}
