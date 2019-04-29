using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace APM
{
    public partial class Appointment_Info : Form
    {
        public static bool appointment_set = false;
        // SqlConnection APM_C = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Samsung\Desktop\APM-Miscellaneous\APM\APM_Database.mdf;Integrated Security=True");
        SqlConnection APM_C = Form_Navigation.APM_C;
        private Customer selectedCustomer = Form_Navigation.SelectedCustomer;
        public Appointment_Info()
        {
            InitializeComponent();
            label3.Text = "New appointment for " + selectedCustomer.FullName;
            textBox1.Text = selectedCustomer.CustomerID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (APM_C.State == ConnectionState.Closed)
            {
                APM_C.Open();
            }
            
            //DateTime appointmentDate = DateTime.Now.AddDays(4);
            string pickedDate = dateTimePicker1.Value.ToShortDateString();
            string appointmentDetails = textBox3.Text;

            SqlCommand appointmentCommand = new SqlCommand();
            appointmentCommand.Connection = APM_C;
            appointmentCommand.CommandType = CommandType.Text;
            appointmentCommand.CommandText = "insert into [Appointments] ( appointmentDate, fkCustomerID, CustomerName, Details) values ('" + pickedDate + "', '" + selectedCustomer.CustomerID + "', '"+ selectedCustomer.FullName +"', '" + appointmentDetails + "')";
            appointmentCommand.ExecuteNonQuery();
            appointmentCommand.Parameters.Clear();
            APM_C.Close();
            appointment_set = true;


            string message = "Appointment added on the " + pickedDate;
            string title = "Appointment! ";
            MessageBoxButtons button = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(message, title, button);
            if (result == DialogResult.OK)
            {
                Close();
            }
        }
    }
}
