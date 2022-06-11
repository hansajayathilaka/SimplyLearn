using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimplyLearn
{
    public partial class FormRegi2 : Form
    {
        private readonly Trainer trainer;
        private readonly SimpleTcpClient client;
        public FormRegi2(SimpleTcpClient client, Trainer trainer)
        {
            InitializeComponent();
            this.trainer = trainer;
            this.client = client;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            string temp;
            temp = txtExperience.Text.Trim();
            if (temp == "")
            {
                MessageBox.Show("Years of experience is required.");
                return;
            }
            temp = txtExperience.Text.Trim();
            if (temp == "")
            {
                MessageBox.Show("Years of experience is required.");
                return;
            }

            bool hasBlog = false;
            if (rbtnYes.Checked == true)
            {
                hasBlog = true;
                temp = txtUrl.Text.Trim();
                if (temp == "")
                {
                    MessageBox.Show("Blog URL is required.");
                    return;
                }

                temp = comboxBrowser.Text.Trim();
                if (temp == "")
                {
                    MessageBox.Show("Browser is required.");
                    return;
                }

                temp = comboxEmployer.Text.Trim();
                if (temp == "")
                {
                    MessageBox.Show("Previous employer is required.");
                    return;
                }
            }

            if (comboxVersion.Text == "" || comboxBrowser.Text == "")
            {
                MessageBox.Show("Browser is required.");
                return;
            } else
                this.trainer.Browser = new WebBrowser(comboxBrowser.Text, Convert.ToInt32(comboxVersion.Text));

            this.trainer.YearsOfExperience = Convert.ToInt32(txtExperience.Text);
            this.trainer.HasBlog = hasBlog;
            this.trainer.BlogURL = txtUrl.Text;
            this.trainer.Employer = comboxEmployer.Text;

            var certificationsList = new List<string>();

            temp = txtCertificate1.Text.Trim();
            if (temp != "")
                certificationsList.Add(temp);
            temp = txtCertificate2.Text.Trim();
            if (temp != "")
                certificationsList.Add(temp);
            temp = txtCertificate3.Text.Trim();
            if (temp != "")
                certificationsList.Add(temp);
            temp = txtCertificate4.Text.Trim();
            if (temp != "")
                certificationsList.Add(temp);

            this.trainer.ListOfCertifications = certificationsList;

            FormRegi3 formRegi3 = new FormRegi3(client, trainer);
            this.Hide();
            formRegi3.ShowDialog();
            this.Close();
        }

        private void FormRegi2_Load(object sender, EventArgs e)
        {
            comboxBrowser.Items.Add("IE");
            comboxBrowser.Items.Add("ME");
            comboxBrowser.Items.Add("MF");
            comboxBrowser.Items.Add("GC");
            comboxBrowser.Items.Add("OP");
            comboxBrowser.Items.Add("SA");
            comboxBrowser.Items.Add("DO");
            comboxBrowser.Items.Add("KQ");
            comboxBrowser.Items.Add("LY");
            comboxBrowser.Items.Add("Other");

            comboxEmployer.Items.Add("Salefore");
            comboxEmployer.Items.Add("Google");
            comboxEmployer.Items.Add("Microsoft");
            comboxEmployer.Items.Add("Amerzon");
            comboxEmployer.Items.Add("None");
            comboxEmployer.SelectedIndex = 4;

            comboxVersion.Items.Add("1");
            comboxVersion.Items.Add("2");
            comboxVersion.Items.Add("3");
            comboxVersion.Items.Add("4");
            comboxVersion.Items.Add("5");
            comboxVersion.Items.Add("6");
        }

        private void txtExperience_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.Handled = (!char.IsDigit(e.KeyChar) && (e.KeyChar != '\b')))
            {
                MessageBox.Show("Alow numbers only");
            }
        }
    }
}
