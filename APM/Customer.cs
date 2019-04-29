using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM
{
    class Customer
    {
        public string CustomerID;
        public string FullName;
        public string Title;
        public string CarType;
        public string CarModel;
        public string CarID;
        public string CarPlate;
        public string Email;

        public DateTime lastService;
        public DateTime AppointmentDate;


        public Customer()
        {

        }
        public override string ToString()
        {
            return FullName +" and their last service is :" + lastService.ToShortDateString() ;
        }
    }
}
