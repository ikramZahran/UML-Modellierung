using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHI_UML
{
    [Serializable]
    class Relation
    {
        private string nameUC1; //panel 1
        private string nameUC2; //panel 2
        private string nameUO1; //multiplicity de table 1
        private string nameUO2; //multiplicity de table 2
        private Point point1 = new Point();
        private Point point2 = new Point();
        private Color color = new Color();
        private bool dashed = false;
       

        public string NameUC1 { get => nameUC1; set => nameUC1 = value; }
        public string NameUC2 { get => nameUC2; set => nameUC2 = value; }
        public string NameUO1 { get => nameUO1; set => nameUO1 = value; }
        public string NameUO2 { get => nameUO2; set => nameUO2 = value; }
        public Point Point1 { get => point1; set => point1 = value; }
        public Point Point2 { get => point2; set => point2 = value; }
        public Color ColorC { get => color; set => color = value; }
        public bool Dashed { get => dashed; set => dashed = value; }
        
    }
}
