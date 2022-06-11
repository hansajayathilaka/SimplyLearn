using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SimplyLearn
{
    public partial class FormRegi1 : Form
    {
        private readonly Trainer trainer;
        private readonly SimpleTcpClient client;
        public FormRegi1(SimpleTcpClient client, Trainer trainer)
        {
            InitializeComponent();
            this.trainer = trainer;
            this.client = client;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (txtFName.Text.Trim() == "")
            {
                MessageBox.Show("First name is requeird.");
                return;
            }

            if (txtLName.Text.Trim() == "")
            {
                MessageBox.Show("First name is requeird.");
                return;
            }

            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            var email_validator = new Regex(pattern, RegexOptions.IgnoreCase);
            if (email_validator.IsMatch(txtEmail.Text) != true)
            {
                MessageBox.Show("Invalid Email address.");
                return;
            }

            this.trainer.FirstName = txtFName.Text;
            this.trainer.LastName = txtLName.Text;
            this.trainer.Email = txtEmail.Text;

            FormRegi2 formRegi2 = new FormRegi2(client, trainer);
            this.Hide();
            formRegi2.ShowDialog();
            this.Show();

            txtFName.Text = "";
            txtLName.Text = "";
            txtEmail.Text = "";
        }

        private void FormRegi1_Load(object sender, EventArgs e)
        {

        }
    }
}
