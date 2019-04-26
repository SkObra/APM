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
using Dapper;

namespace APM
{
    public partial class Form_Navigation : Form
    {
        
        public static string connectionString = ("Data Source = JESUSCHRIST\\ALORASQL; Initial Catalog = APM_DB; Integrated Security = True");
        SqlConnection connectionLink = new SqlConnection(connectionString);
        DataTable tableInformation = new DataTable();
        
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
                if(connectionLink.State == ConnectionState.Closed)
                {
                    connectionLink.Open();
                }

                SqlCommand Command1 = connectionLink.CreateCommand();
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
                Command2.Connection = connectionLink;
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
                Command3.Connection = connectionLink;
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
                Command4.Connection = connectionLink;
                Command4.CommandType = CommandType.Text;
                Command4.CommandText = "insert into [Invoice] (InvoiceID, ServiceID, TotalCost) values ('" + textBox_InD.Text + "','" + textBox_Sv.Text + "', '" + textBox_TC.Text + "')";
                Command4.ExecuteNonQuery();
                Command4.Parameters.Clear();
                connectionLink.Close();

                textBox_InD.Text = "";
                textBox_Sv.Text = "";
                textBox_TC.Text = "";
                MessageBox.Show("Data Saved Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connectionLink.Close();
            }
        }

        public void dataLoader()
        {
            try
            {
                if (connectionLink.State == ConnectionState.Closed)
                {
                    connectionLink.Open();
                }

                SqlCommand commandLoad = connectionLink.CreateCommand();
                commandLoad.CommandType = CommandType.Text;
                commandLoad.CommandText = "Select * From CustomerP, CustomerV, Service, Invoice where CustomerP.CustomerID = CustomerV.fkCustomerID AND CustomerV.CarID = Service.CarID AND Service.ServiceID = Invoice.ServiceID"; 
                commandLoad.ExecuteNonQuery();
                DataTable tableInformation = new DataTable();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(commandLoad);
                dataAdapter.Fill(tableInformation);
                dataGridView1.DataSource = tableInformation;
                connectionLink.Close();
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

        private void button1_Click(object sender, EventArgs e)
        {
            /*DataTable tableInformation = new DataTable();
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows.RemoveAt(rowIndex);*/

            try
            {
                if (connectionLink.State == ConnectionState.Closed)
                {
                    connectionLink.Open();
                }            
                string Query = "Delete from Invoice WHERE ServiceID = '" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "'";
                SqlCommand cmd = new SqlCommand(Query, connectionLink);
                cmd.ExecuteNonQuery();
                connectionLink.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }       
        }

        private void textBox_Find_TextChanged(object sender, EventArgs e)
        {   
        }

        private void button2_Click(object sender, EventArgs e)
        {            
            string findQuery = "select * from CustomerP, CustomerV, Service, Invoice where CustomerP.Name='"+ textBox_Find.Text+ "' AND CustomerP.CustomerID = CustomerV.fkCustomerID AND CustomerV.CarID = Service.CarID AND Service.ServiceID = Invoice.ServiceID";
            connectionLink.Open();
            SqlCommand findCommand = new SqlCommand(findQuery, connectionLink);
            SqlDataAdapter findAdapter = new SqlDataAdapter(findCommand);
            DataTable dataFind = new DataTable();
            findAdapter.Fill(dataFind);
            dataGridView1.DataSource = dataFind;
            connectionLink.Close();
        }
    }
}
