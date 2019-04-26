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
using System.Configuration;

namespace APM
{
    public partial class Form_Navigation : Form
    {
        /*string ConStr = Properties.Settings.Default.APM_DatabaseConnectionString;
        SqlConnection APM_C = new SqlConnection(ConStr);*/
     
        SqlConnection APM_C = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\University\Year 2\Team Project\APM-Miscellaneous\APM\APM_Database.mdf;Integrated Security=True");
        
        public Form_Navigation()
        {
            InitializeComponent();
            Extract_Appointments();
        }
        
  
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void Extract_Appointments()
        {
            APM_C.Open();
            string qr = "select * from Appointments";
            SqlCommand DateCommand = new SqlCommand(qr, APM_C);
            SqlDataReader reader = DateCommand.ExecuteReader();
            while (reader.Read())
            {
                Appointment app = new Appointment();
                app.CustomerID = reader["fkCustomerID"].ToString();
                app.CustomerName = reader["CustomerName"].ToString();
                app.Date = Convert.ToDateTime(reader["appointmentDate"].ToString());
                app.details = reader["Details"].ToString();
                appointments.Add(app);
            }
            APM_C.Close();
        }
        private void button_Save_Click(object sender, EventArgs e)
        {         
            try
            {
                if(APM_C.State == ConnectionState.Closed)
                {
                    APM_C.Open();
                }
               
                SqlCommand Command1 = APM_C.CreateCommand();
                Command1.CommandType = CommandType.Text;
                Command1.CommandText = "insert into [CustomerP] (CustomerID, Name, Title, PostCode, MobileNumber, Email, ContactPreference) values ('" + textBox_CustID.Text + "','" + textBox_Name.Text + "','" + comboBox_Title.Text + "','" + textBox_PC.Text + "','" + textBox_MN.Text + "','" + textBox_Email.Text + "','" + comboBox_CP.Text + "')";
                Command1.ExecuteNonQuery();
                Command1.Parameters.Clear();

                //textBox_CustID.Text = "";
                textBox_Name.Text = "";
                comboBox_Title.Text = "";
                textBox_PC.Text = "";
                textBox_MN.Text = "";
                textBox_Email.Text = "";
                comboBox_CP.Text = "";

                SqlCommand Command2 = new SqlCommand();
                Command2.Connection = APM_C;
                Command2.CommandType = CommandType.Text;
                Command2.CommandText = "insert into [CustomerV] (CarID, Manufacturer, Model, Transmission, LicensePlateID, Color, fkCustomerID) values ('" + textBox_CI.Text + "', '" + textBox_Man.Text + "', '" + textBox_Mod.Text + "', '" + comboBox_Tran.Text + "', '" + textBox_LP.Text + "', '" + textBox_VC.Text + "', '" + textBox_CustID.Text + "')";
                Command2.ExecuteNonQuery();
                Command2.Parameters.Clear();

                // -- textBox_CI.Text = "";
                textBox_Man.Text = "";
                textBox_Mod.Text = "";
                comboBox_Tran.Text = "";
                textBox_LP.Text = "";
                textBox_VC.Text = "";
                textBox_CustID.Text = "";

                SqlCommand Command3 = new SqlCommand();
                Command3.Connection = APM_C;
                Command3.CommandType = CommandType.Text;
                string pickedDate = textBox_DP.Value.ToShortDateString()  ;
                Command3.CommandText = "insert into [Service] (ServiceID, DateS, Fault, Mechanic, CarID) values ('" + textBox_Sv.Text + "', '" + pickedDate + "','" + textBox_FL.Text + "', '" + textBox_MA.Text + "', '" + textBox_CI.Text + "')";
                Command3.ExecuteNonQuery();
                Command3.Parameters.Clear();
                
                // -- textBox_Sv.Text = "";
                textBox_DP.Text = "";
                textBox_FL.Text = "";
                textBox_MA.Text = "";
                textBox_CI.Text = "";

                SqlCommand Command4 = new SqlCommand();
                Command4.Connection = APM_C;
                Command4.CommandType = CommandType.Text;
                Command4.CommandText = "insert into [Invoice] (InvoiceID, ServiceID, TotalCost) values ('" + textBox_InD.Text + "','" + textBox_Sv.Text + "', '" + textBox_TC.Text + "')";
                Command4.ExecuteNonQuery();
                Command4.Parameters.Clear();
                APM_C.Close();

                textBox_InD.Text = "";
                textBox_Sv.Text = "";
                textBox_TC.Text = "";
                MessageBox.Show("Data Saved Successfully");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message,"Error! Please review the form");
            }
        }

        public void dataLoader()
        {
            APM_C.Open();
            SqlCommand commandLoad = APM_C.CreateCommand();
            commandLoad.CommandType = CommandType.Text;
            commandLoad.CommandText = "Select * from CustomerP, CustomerV, Service, Invoice";
            commandLoad.ExecuteNonQuery();
            DataTable tableInformation = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(commandLoad);
            dataAdapter.Fill(tableInformation);
            dataGridView1.DataSource = tableInformation;
            APM_C.Close();
        }

        private void button_Logout_Click(object sender, EventArgs e)
        {
            string message = "Are you sure?";
            string title = "Caution!";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button_StoredData_Click(object sender, EventArgs e)
        {
            dataLoader();
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void LoadMiscellaneousListBox()
        {
            APM_C.Open();
            string qr = "select Service.ServiceID, Service.DateS, CustomerV.CarID, CustomerV.fkCustomerID as CustomerID, CustomerP.Name from Service, CustomerV, CustomerP where" +
                "(Service.DateS < (SELECT CONVERT(VARCHAR(10), (SELECT DATEADD(DD,-30,GETDATE())),101)))  AND  (Service.CarID = CustomerV.CarID) " +
                "AND (CustomerV.fkCustomerID = CustomerP.CustomerID) ";
            SqlCommand DateCommand = new SqlCommand(qr, APM_C);
            SqlDataReader reader = DateCommand.ExecuteReader();
            while (reader.Read())
            {
                Customer cust = new Customer();
                cust.CustomerID = reader["CustomerID"].ToString();
                cust.FullName = reader["Name"].ToString();
                cust.CarID = reader["CarID"].ToString();
                cust.lastService = Convert.ToDateTime(reader["DateS"].ToString());
                //cust.CarPlate = reader["LicensePlateID"].ToString();
                listBox1.Items.Add(cust); 
               // Customers.Add(cust);

            }
            APM_C.Close();
            
        }

        private List<Appointment> appointments = new List<Appointment>();

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTime date = monthCalendar1.SelectionStart;
            label20.Text = date.ToShortDateString();
            listView1.Items.Clear();
            
            for (int i = 0; i < appointments.Count; i++)
            {
                if (date.ToShortDateString() == appointments[i].Date.ToShortDateString())
                {
                    listView1.BeginUpdate();
                    listView1.Items.Add(appointments[i].CustomerID + " " + appointments[i].CustomerName + " , Last Service: " + appointments[i].Date.ToShortDateString());
                    listView1.EndUpdate();
                }
            }
            
                                   
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadMiscellaneousListBox();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Customer selectedCustomer = (Customer)listBox1.SelectedItem;
            string message = "Letter for " + selectedCustomer.FullName + ", CustomerID: " + selectedCustomer.CustomerID;
            string title = "Letter for Customer ";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(message, title, buttons);
            
        }

        private static Customer selectedCustomer = new Customer();

        internal static Customer SelectedCustomer { get => selectedCustomer; set => selectedCustomer = value; }
       

        private void add_appointmentBtn_Click(object sender, EventArgs e)
        {
          
            selectedCustomer = (Customer)listBox1.SelectedItem;
            Appointment_Info new_appointment = new Appointment_Info();
            new_appointment.Show();
            
        }
    }
}
