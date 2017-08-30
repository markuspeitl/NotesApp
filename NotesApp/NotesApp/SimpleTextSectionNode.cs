using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class SimpleTextSectionNode
    {
        public SimpleHtmlNode thisHtmlTextNode;
        public int textStart;
        public int textEnd;

        public SimpleTextSectionNode(SimpleHtmlNode thisHtmlTextNode, int textStart, int textEnd)
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
            this.thisHtmlTextNode.Content.Insert(position, "" + insertCharacter);
            textEnd += 1;
        }
        public void InsertString(int position, string insertString)
        {
            this.thisHtmlTextNode.Content.Insert(position, insertString);
            textEnd += insertString.Length;
        }
        public void RemoveCharacter(int position)
        {
            this.thisHtmlTextNode.Content.Remove(position);
            textEnd -= 1;
        }
        public void RemoveString(int position, int lenght)
        {
            this.thisHtmlTextNode.Content.Remove(position, lenght);
            textEnd -= lenght;
        }

        public void UpdateTextPosition(int difference)
        {
            textStart += difference;
            textEnd += difference;
        }

        public string GetText()
        {
            return this.thisHtmlTextNode.GetInnerString();
        }


        public void SetPartialRightNewMasterNode(String style, int startPoint, int endPoint)
        {
            SimpleHtmlNode newMaster;
            newMaster = new SimpleHtmlNode(style, null);

            SimpleHtmlNode newText;
            newText = new SimpleHtmlNode("text", null);

            string firstText = this.thisHtmlTextNode.Content.Substring(endPoint - this.textStart);
            string secondText = this.thisHtmlTextNode.Content.Substring(0, endPoint - this.textStart);
            this.thisHtmlTextNode.Content = firstText;
            newText.Content = secondText;

            if (this.thisHtmlTextNode.Parent == null)
            {
                //Should nor happen because text always needs a parent
                this.thisHtmlTextNode.Parent = newMaster;
            }
            if (this.thisHtmlTextNode.Parent != null)
            {
                if (!this.thisHtmlTextNode.Parent.ChildNodes.Contains(newMaster))
                {
                    //Insert Master node at the place of the first node
                    this.thisHtmlTextNode.Parent.ChildNodes.Insert(
                        this.thisHtmlTextNode.Parent.ChildNodes.IndexOf(this.thisHtmlTextNode), newMaster);
                    newMaster.Parent = this.thisHtmlTextNode.Parent;
                }

                if (newText.Parent != newMaster)
                {
                    //Swap parent to newmaster
                    newMaster.ChildNodes.Add(newText);
                    newText.Parent = newMaster;
                }
            }
        }
        public void SetPartialLeftNewMasterNode(String style, int startPoint, int endPoint)
        {
            SimpleHtmlNode newMaster;
            newMaster = new SimpleHtmlNode(style, null);

            SimpleHtmlNode newText;
            newText = new SimpleHtmlNode("text", null);

            string firstText = this.thisHtmlTextNode.Content.Substring(0, startPoint - this.textStart);
            string secondText = this.thisHtmlTextNode.Content.Substring(startPoint - this.textStart);
            this.thisHtmlTextNode.Content = firstText;
            newText.Content = secondText;

            if (this.thisHtmlTextNode.Parent == null)
            {
                //Should nor happen because text always needs a parent
                this.thisHtmlTextNode.Parent = newMaster;
            }
            if (this.thisHtmlTextNode.Parent != null)
            {
                if (!this.thisHtmlTextNode.Parent.ChildNodes.Contains(newMaster))
                {
                    //Insert Master node at the place of the first node
                    this.thisHtmlTextNode.Parent.ChildNodes.Add(newMaster);
                    newMaster.Parent = this.thisHtmlTextNode.Parent;
                }

                if (newText.Parent != newMaster)
                {
                    //Swap parent to newmaster
                    newMaster.ChildNodes.Add(newText);
                    newText.Parent = newMaster;
                }
            }
        }

        public void SetNewMasterNode(String style)
        {
            SimpleHtmlNode newMaster;
            newMaster = new SimpleHtmlNode(style, null);

            if (this.thisHtmlTextNode.Parent == null)
            {
                //Should nor happen because text always needs a parent
                this.thisHtmlTextNode.Parent = newMaster;
            }
            if (this.thisHtmlTextNode.Parent != null)
            {
                if (!this.thisHtmlTextNode.Parent.ChildNodes.Contains(newMaster))
                {
                    //Insert Master node at the place of the first node
                    this.thisHtmlTextNode.Parent.ChildNodes.Insert(
                        this.thisHtmlTextNode.Parent.ChildNodes.IndexOf(this.thisHtmlTextNode), newMaster);
                    newMaster.Parent = this.thisHtmlTextNode.Parent;
                }

                if (this.thisHtmlTextNode.Parent != newMaster)
                {
                    //Swap parent to newmaster
                    newMaster.ChildNodes.Add(this.thisHtmlTextNode);
                    this.thisHtmlTextNode.Parent.ChildNodes.Remove(this.thisHtmlTextNode);
                    this.thisHtmlTextNode.Parent = newMaster;
                }
            }
        }

        /*public void SetNewMasterNode(SimpleHtmlNode newMaster)
        {
            if (this.thisHtmlTextNode.Parent != null)
            {

                if (!this.thisHtmlTextNode.Parent.Parent.ChildNodes.Contains(newMaster))
                {
                    //this.thisHtmlTextNode.Parent.InsertAfter(newMaster, this.thisHtmlTextNode);
                }


                if (this.thisHtmlTextNode.Parent != newMaster)
                {
                    if (!newMaster.ChildNodes.Contains(this.thisHtmlTextNode))
                    {
                        //this.thisHtmlTextNode.Parent.RemoveChild(thisHtmlTextNode);
                        //newMaster.AppendChild(this.thisHtmlTextNode);
                    }
                }
            }
        }*/

    }
}
