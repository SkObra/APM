using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace APM
{
    class Connection
    {
        public string connection;
        public string connectionState;

        public Connection(string connected, string connectionStated)
        {
            connection = connected;
            connectionState = connectionStated;
        }

        public string connected
        {
            get { return connection;}
        }

    }
}
