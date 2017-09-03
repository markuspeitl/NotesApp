using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class CSSStyle
    {
        public ValueType valueType;
        public string propertyType;
        public string propertyValue;

        public CSSStyle(string propertyType, string propertyValue)
        {
            this.propertyType = propertyType;
            this.propertyValue = propertyValue;
        }

        public string GetStringValue()
        {
            return propertyValue;
        }

        public enum ValueType
        {
            STRING,
            INTEGER,
            PERCENT,
            EM
        }

    }
}
