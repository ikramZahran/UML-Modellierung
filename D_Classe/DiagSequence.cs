using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHI_UML
{
    [Serializable]
    class DiagSequence
    {
        private string name;
        private string text;
        private List<UmlClassObjects> sequences = new List<UmlClassObjects>();
        private List<UmlClassObjects> actors = new List<UmlClassObjects>();
        private List<UmlClassObjects> messages = new List<UmlClassObjects>();
        private List<Relation> relations = new List<Relation>();
        private List<Relation> rects = new List<Relation>();

        public string Name { get => name; set => name = value; }
        public string Text { get => text; set => text = value; }
        internal List<UmlClassObjects> Sequences { get => sequences; set => sequences = value; }
        internal List<UmlClassObjects> Actors { get => actors; set => actors = value; }
        internal List<Relation> Relations { get => relations; set => relations = value; }
        internal List<UmlClassObjects> Messages { get => messages; set => messages = value; }
        internal List<Relation> Rects { get => rects; set => rects = value; }
    }
}
