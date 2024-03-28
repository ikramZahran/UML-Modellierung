using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHI_UML
{
    [Serializable]
    class DiagClass
    {
        private string name;
        private List<UmlClass> umlClasses = new List<UmlClass>();
        private List<UmlClassObjects> umlObjects = new List<UmlClassObjects>();
        private List<Relation> relations = new List<Relation>();

        public string Name { get => name; set => name = value; }
        internal List<UmlClass> UmlClasses { get => umlClasses; set => umlClasses = value; }
        internal List<UmlClassObjects> UmlObjects { get => umlObjects; set => umlObjects = value; }
        internal List<Relation> Relations { get => relations; set => relations = value; }

        public void ChangeNameID()
        {
            for (int i = 0; i < umlClasses.Count; i++)
            {
                umlClasses[i].Name += "ID" + 1000;
            }

            for (int i = 0; i < umlObjects.Count; i++)
            {
                umlObjects[i].Name += "ID" + 1000;
            }

            for (int i = 0; i < relations.Count; i++)
            {
                relations[i].NameUC1 += "ID" + 1000;
                relations[i].NameUC2 += "ID" + 1000;
                relations[i].NameUO1 += "ID" + 1000;
                relations[i].NameUO2 += "ID" + 1000;
            }
        }
    }
}
