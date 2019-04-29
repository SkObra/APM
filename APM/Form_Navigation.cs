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
using System.Net.Mail;
using System.Net;

namespace APM
{
    public partial class Form_Navigation : Form
    {
        /*string ConStr = Properties.Settings.Default.APM_DatabaseConnectionString;
        SqlConnection APM_C = new SqlConnection(ConStr);*/
        public static string connectionString = (@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Samsung\Desktop\test5Working\APM-lilicovileac-test\APM\APM_Database.mdf;Integrated Security=True");
        //public static string connectionString = ("Data Source='172.18.39.133, 1433';Initial Catalog=APM_DB;Persist Security Info=True;User ID=sa;Password=Intelcorei21*");
        public static SqlConnection APM_C = new SqlConnection(connectionString);
        DataTable tableInformation = new DataTable();
        int selectedRow;

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
            if (APM_C.State == ConnectionState.Closed)
            {
                APM_C.Open();
            }
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
            try
            {
                if (APM_C.State == ConnectionState.Closed)
                {
                    APM_C.Open();
                }

                SqlCommand commandLoad = APM_C.CreateCommand();
                commandLoad.CommandType = CommandType.Text;
                commandLoad.CommandText = "Select * From CustomerP, CustomerV, Service, Invoice where CustomerP.CustomerID = CustomerV.fkCustomerID AND CustomerV.CarID = Service.CarID AND Service.ServiceID = Invoice.ServiceID";
                commandLoad.ExecuteNonQuery();
                DataTable tableInformation = new DataTable();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(commandLoad);
                dataAdapter.Fill(tableInformation);
                dataGridView1.DataSource = tableInformation;
                APM_C.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            if (APM_C.State == ConnectionState.Closed)
            {
                APM_C.Open();
            }
            string qr = "select Service.ServiceID, Service.DateS, CustomerV.CarID as CarID, CustomerV.Manufacturer as CarType, CustomerV.Model as CarModel, CustomerV.fkCustomerID as CustomerID, CustomerP.Name, CustomerP.Email as Email from Service, CustomerV, CustomerP where" +
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
                cust.CarType = reader["CarType"].ToString();
                cust.CarModel = reader["CarModel"].ToString();
                cust.lastService = Convert.ToDateTime(reader["DateS"].ToString());
                cust.Email = reader["Email"].ToString();
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

            //SmtpClient details
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Port = 587;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("appointmentmanager2@gmail.com", "apm.team1");

            // email generation and sending

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("appointmentmanager2@gmail.com");
            mailMessage.To.Add(selectedCustomer.Email);
            mailMessage.Subject = "New appointment to confirm!";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body ="<h4> Hello there dear " + selectedCustomer.FullName + " </h2> <p> We are happy to bring your car " +selectedCustomer.CarType + " to our service</p>" ;

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught When attempting to send the message: {0}",
                            ex.ToString());
            }

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
           
            if(Appointment_Info.appointment_set == true)
            {
                appointments.Clear();
                Extract_Appointments();
            }
            
        }

        private void button_Refresh_Click(object sender, EventArgs e)
        {
            string message = "Are you sure?";
            string title = "Do you want to refresh?";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                this.Controls.Clear();
                this.InitializeComponent();
            }
        }

        private void button_Find_Click(object sender, EventArgs e)
        {
            string findQuery = "select * from CustomerP, CustomerV, Service, Invoice where CustomerP.Name='" + textBox_Find.Text + "' AND CustomerP.CustomerID = CustomerV.fkCustomerID AND CustomerV.CarID = Service.CarID AND Service.ServiceID = Invoice.ServiceID";
            APM_C.Open();
            SqlCommand findCommand = new SqlCommand(findQuery, APM_C);
            SqlDataAdapter findAdapter = new SqlDataAdapter(findCommand);
            DataTable dataFind = new DataTable();
            findAdapter.Fill(dataFind);
            dataGridView1.DataSource = dataFind;
            APM_C.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (APM_C.State == ConnectionState.Closed)
                {
                    APM_C.Open();
                }
                SqlCommand delCommand =APM_C.CreateCommand();
                delCommand.CommandType = CommandType.Text;
                delCommand.CommandText = "delete From Invoice where ServiceID = '" + textBox_Sv.Text + "'";
                delCommand.ExecuteNonQuery();
                APM_C.Close();
                MessageBox.Show("Associated details with the typed ServiceID8 have been successfully deleted");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            DataGridViewRow newDataRow = dataGridView1.Rows[selectedRow];
            newDataRow.Cells[0].Value = textBox_CustID.Text;
            newDataRow.Cells[1].Value = textBox_Name.Text;
            newDataRow.Cells[2].Value = comboBox_Title.Text;
            newDataRow.Cells[3].Value = textBox_PC.Text;
            newDataRow.Cells[4].Value = textBox_MN.Text;
            newDataRow.Cells[5].Value = textBox_Email.Text;
            newDataRow.Cells[6].Value = comboBox_CP.Text;

            newDataRow.Cells[7].Value = textBox_CI.Text;
            newDataRow.Cells[8].Value = textBox_Man.Text;
            newDataRow.Cells[9].Value = textBox_Mod.Text;
            newDataRow.Cells[10].Value = comboBox_Tran.Text;
            newDataRow.Cells[11].Value = textBox_LP.Text;
            newDataRow.Cells[12].Value = textBox_VC.Text;
            newDataRow.Cells[13].Value = textBox_CustID.Text;

            newDataRow.Cells[14].Value = textBox_Sv.Text;
            newDataRow.Cells[15].Value = textBox_DP.Text;
            newDataRow.Cells[16].Value = textBox_CI.Text;
            newDataRow.Cells[17].Value = textBox_FL.Text;
            newDataRow.Cells[18].Value = textBox_MA.Text;


            newDataRow.Cells[19].Value = textBox_InD.Text;
            newDataRow.Cells[20].Value = textBox_Sv.Text;
            newDataRow.Cells[21].Value = textBox_TC.Text;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[selectedRow];

            textBox_CustID.Text = row.Cells[0].Value.ToString();
            textBox_Name.Text = row.Cells[1].Value.ToString();
            comboBox_Title.Text = row.Cells[2].Value.ToString();
            textBox_PC.Text = row.Cells[3].Value.ToString();
            textBox_MN.Text = row.Cells[4].Value.ToString();
            textBox_Email.Text = row.Cells[5].Value.ToString();
            comboBox_CP.Text = row.Cells[6].Value.ToString();

            textBox_CI.Text = row.Cells[7].Value.ToString();
            textBox_Man.Text = row.Cells[8].Value.ToString();
            textBox_Mod.Text = row.Cells[9].Value.ToString();
            comboBox_Tran.Text = row.Cells[10].Value.ToString();
            textBox_LP.Text = row.Cells[11].Value.ToString();
            textBox_VC.Text = row.Cells[12].Value.ToString();
            textBox_CustID.Text = row.Cells[13].Value.ToString();

            textBox_Sv.Text = row.Cells[14].Value.ToString();
            textBox_DP.Text = row.Cells[15].Value.ToString();
            textBox_CI.Text = row.Cells[16].Value.ToString();
            textBox_FL.Text = row.Cells[17].Value.ToString();
            textBox_MA.Text = row.Cells[18].Value.ToString();

            textBox_InD.Text = row.Cells[19].Value.ToString();
            textBox_Sv.Text = row.Cells[20].Value.ToString();
            textBox_TC.Text = row.Cells[21].Value.ToString();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (this.listView1.SelectedItems.Count == 0)
                return;

            string[] appointment = this.listView1.SelectedItems[0].Text.Split(' ');
            Appointment app = new Appointment();
            app.CustomerID = appointment[0];
            app.CustomerName = appointment[1] + " " + appointment[2];

            // Create the sql statement to retrieve details for the user
            string sql = string.Format("select Details, appointmentDate from Appointments where fkCustomerID = '{0}'", app.CustomerID);
            if (APM_C.State == ConnectionState.Closed)
            {
                APM_C.Open();
            }
           
            SqlCommand DateCommand = new SqlCommand(sql, APM_C);
            SqlDataReader reader = DateCommand.ExecuteReader();
            while (reader.Read())
            {
                app.details = reader["Details"].ToString();
                app.Date = Convert.ToDateTime(reader["AppointmentDate"].ToString());

            }
            APM_C.Close();

            MessageBox.Show(" Appointment on "+ app.Date +" for "+ app.CustomerName + ", CustomerID: " + app.CustomerID + ". Details: " +app.details);

        }

        private void tabPage_Maintenance_Click(object sender, EventArgs e)
        {

        }
    }
}
