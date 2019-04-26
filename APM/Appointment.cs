using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM
{
    class Appointment
    {
        public string CustomerID;
        public string CustomerName;
        public DateTime Date;
        public string details;

        public override string ToString()
        {
            return CustomerID + " " + CustomerName + " " + Date.ToString();
        }
    }
}
