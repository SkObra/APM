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
     
        SqlConnection APM_C = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\rpear\Documents\sK_oBrA\UOD\UnderGraduate\Year 2\Semester2\Team Project\Appointment Manager\Version0.3\APM\APM\APM_Database.mdf;Integrated Security=True");
        
        public Form_Navigation()
        {
            InitializeComponent();          
        }
        
  
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

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
                Command3.CommandText = "insert into [Service] (ServiceID, DateS, Fault, Mechanic, CarID) values ('" + textBox_Sv.Text + "', '" + textBox_DP.Text + "','" + textBox_FL.Text + "', '" + textBox_MA.Text + "', '" + textBox_CI.Text + "')";
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
    }
}
