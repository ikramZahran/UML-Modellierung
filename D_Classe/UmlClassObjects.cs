using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHI_UML
{
    [Serializable]
    class UmlClassObjects
    {
        private string name;
        private string text;
        private int x, y;

        public string Name { get => name; set => name = value; }
        public string Text { get => text; set => text = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
    }
}
