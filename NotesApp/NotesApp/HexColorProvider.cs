using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class HexColorProvider
    {

        private Dictionary<string, string> colorLookUpDictionary;

        public HexColorProvider()
        {
            colorLookUpDictionary = new Dictionary<string, string>();
            colorLookUpDictionary.Add("red",    "#ffff0000");
            colorLookUpDictionary.Add("green",  "#ff00ff00");
            colorLookUpDictionary.Add("blue",   "#ff0000ff");
            colorLookUpDictionary.Add("cyan",   "#ff00ffff");
            colorLookUpDictionary.Add("pink",   "#ffff33cc");
            colorLookUpDictionary.Add("orange", "#ffff9900");
            colorLookUpDictionary.Add("yellow", "#ffffff00");
        }

        public void AddToDic(string name, string hex)
        {
            if (!colorLookUpDictionary.ContainsKey(name))
            {
                colorLookUpDictionary.Add(name, hex);
            }
            else
            {
                colorLookUpDictionary[name] = hex;
            }
        }

        public string ConvertToArgb(string color)
        {
            if (color[0] == '#')
            {
                if (color.Length == 7)
                {
                    return "#ff" + color.Substring(1);
                }
            }
            return color;
        }

        public bool CanParse(string color)
        {
            if (color != null)
            {
                if (!color.Equals(""))
                {
                    if (color[0] == '#')
                    {
                        return true;
                    }
                    else if (colorLookUpDictionary.ContainsKey(color))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public byte[] ParseColorStringToBytes(string colorRep)
        {
            if (this.CanParse(colorRep))
            {
                colorRep = this.GetHexColorFromString(colorRep, true);
                byte a = (byte)(Convert.ToUInt32(colorRep.Substring(1, 2), 16));
                byte r = (byte)(Convert.ToUInt32(colorRep.Substring(3, 2), 16));
                byte g = (byte)(Convert.ToUInt32(colorRep.Substring(5, 2), 16));
                byte b = (byte)(Convert.ToUInt32(colorRep.Substring(7, 2), 16));
                return new byte[4] { a, r, g, b };
            }
            else
            {
                return new byte[4] {0,0,0,0};
            }
        }

        public string GetHexColorFromString(string colorRepresentation, bool alpha)
        {
            if(colorRepresentation != null)
            {
                if(!colorRepresentation.Equals(""))
                {
                    if(colorRepresentation[0] == '#')
                    {
                        if(alpha && colorRepresentation.Length == 7)
                        {
                            return "#ff" + colorRepresentation.Substring(1);
                        }
                        return colorRepresentation;
                    }
                    else
                    {
                        if (colorLookUpDictionary.ContainsKey(colorRepresentation))
                        {
                            return colorLookUpDictionary[colorRepresentation];
                        }
                    }
                }
            }

            return "";
        }

    }
}
