using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APM
{
    public partial class Form_Authentication : Form
    {
        //Declaring Form_Navigation
        Form_Navigation navigationPage = new Form_Navigation();
        public Form_Authentication()
        {
            InitializeComponent();
            textBox_Password.PasswordChar = '•';
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        // --Closing button for the Login Page
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        // --This basically instantiates the Navigation form/page
        // --It is Commenced by hiding the Login Page after successful login
        // --Access is then granted to next page
        private void button_Login_Click(object sender, EventArgs e)
        {
            if(textBox_Password.Text == "1")
            {
                this.Hide();
                navigationPage.Show();
            }
            else
            {
                MessageBox.Show("The password that you've entered is incorrect", "Access Denied!", MessageBoxButtons.RetryCancel);
            }
        }
    }
}
