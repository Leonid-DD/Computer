using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ООО_Компутир.Classes
{
    public class PCExtended
    {

        public Model.PC PC { get; set; }

        public List<Model.Program> programs;

        public string PCImagePath
        {
            get
            {
                string temp;
                if (this.PC.PCImagePath != null)
                {
                    temp = Directory.GetCurrentDirectory() + "/images/" + this.PC.PCImagePath.ToString(); 
                }
                else
                {
                    temp = "/Resources/blank.png";
                }
                return temp;
            }
        }

        public double PCResultCost
        {
            get
            {
                double resultCost = (double)this.PC.PCCost;
                foreach (Model.Program program in programs)
                {
                    if (this.PC.Program.Contains(program))
                    {
                        resultCost += (double)program.ProgramCost;
                    }
                }
                return resultCost;
            }
        }

        public double PCDiscountCost
        {
            get
            {
                return Math.Round((double)this.PCResultCost * (100.0 - this.PC.PCDiscount) / 100.0 , 2);
            }
        }

    }
}
