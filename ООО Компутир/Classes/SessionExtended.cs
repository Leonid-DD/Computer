using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ООО_Компутир.Classes
{
    public class SessionExtended
    {

        public Model.Session Session { get; set; }

        public PCExtended PCExt;


        public double SessionLength 
        {
            get
            {
                return Math.Round((this.Session.SessionEndDateTime - this.Session.SessionStartDateTime).TotalHours,2);
            }
        }

        public double TotalCost
        {
            get
            {
                return this.PCExt.PCDiscountCost * this.SessionLength;
            }
        }

    }
}
