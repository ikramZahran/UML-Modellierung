using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHI_UML
{
    class UR_Diagram
    {
        public string Name;
        public int valDC = 0;
        public List<DiagClass> diagClasses = new List<DiagClass>();
        public int valDS = 0;
        public List<DiagSequence> diagSequences = new List<DiagSequence>();
        public int valDU = 0;
        public List<DiagUseCase> diagUseCases = new List<DiagUseCase>();

        public UR_Diagram(string name)
        {
            this.Name = name;
        }
    }
}
