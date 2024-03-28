using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHI_UML
{
    [Serializable]
    class DiagUseCase
    {
        private string name;// nom de diag
        private string text; //titre  de diag ===== sys
        private List<UmlClassObjects> useCases = new List<UmlClassObjects>();
        private List<UmlClassObjects> actors = new List<UmlClassObjects>();
        private List<UmlClassObjects> objects = new List<UmlClassObjects>();
        private List<Relation> relations = new List<Relation>();

        public string Name { get => name; set => name = value; }
        public string Text { get => text; set => text = value; }
        internal List<UmlClassObjects> UseCases { get => useCases; set => useCases = value; }
        internal List<UmlClassObjects> Actors { get => actors; set => actors = value; }
        internal List<UmlClassObjects> Objects { get => objects; set => objects = value; }
        internal List<Relation> Relations { get => relations; set => relations = value; }
        public void ChangeNameID()
        {
            for (int i = 0; i < useCases.Count; i++)
            {
                useCases[i].Name += "ID" + 1000;
            }

            for (int i = 0; i < actors.Count; i++)
            {
                actors[i].Name += "ID" + 1000;
            }

            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].Name += "ID" + 1000;
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
