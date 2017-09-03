using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp
{
    public class CSSStyleManager
    {
        private Dictionary<string, CSSStyling> stylesPool;
        
        public CSSStyleManager()
        {
            stylesPool = new Dictionary<string, CSSStyling>();
        }

        public void PutStyle(CSSStyling style)
        {
            stylesPool.Add(style.toStyleTag,style);
        }

        public bool HasStyle(string checkTag)
        {
            return stylesPool.ContainsKey(checkTag);
        }

        public CSSStyling GetStyle(string cssTag)
        {
            if (stylesPool.ContainsKey(cssTag))
            {
                return stylesPool[cssTag];
            }
            return null;
        }

    }
}
