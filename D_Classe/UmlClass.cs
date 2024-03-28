using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHI_UML
{
    [Serializable]
    class UmlClass
    {
        private string name;
        private string text;
        private List<string> attributs = new List<string>();
        private List<string> methodes = new List<string>();
        private int x, y;
        private bool hideAtt = false, hideMeth = false;
        public string Name { get => name; set => name = value; } 
        public string Text { get => text; set => text = value; }
        public List<string> Attributs { get => attributs; set => attributs = value; }
        public List<string> Methodes { get => methodes; set => methodes = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public bool HideAtt { get => hideAtt; set => hideAtt = value; }
        public bool HideMeth { get => hideMeth; set => hideMeth = value; }
    }
}
