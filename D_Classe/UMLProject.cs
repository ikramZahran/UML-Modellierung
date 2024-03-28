using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHI_UML
{
    [Serializable]
    class UMLProject
    {
        private string name;
        private string pathP;
        private List<string> diagramsNames = new List<string>();

        public string Name { get => name; set => name = value; }
        public string PathP { get => pathP; set => pathP = value; }
        public List<string> DiagramsNames { get => diagramsNames; set => diagramsNames = value; }
    }
}
