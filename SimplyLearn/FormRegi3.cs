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
    public partial class FormRegi3 : Form
    {
        private readonly Trainer trainer;
        int location = 10;
        List<TextBox> sessionName = new List<TextBox>();
        List<TextBox> sessionDescription = new List<TextBox>();
        public FormRegi3(Trainer trainer)
        {
            InitializeComponent();
            add_texbox();
            this.panel.AutoScroll = true;
            this.trainer = trainer;
        }

        private void btnAddSession_Click(object sender, EventArgs e)
        {
            add_texbox();
        }

        private void add_texbox()
        {
            int leftLbl = 10;
            int leftTxt = 160;

            // Name
            Label lblName = new Label();
            lblName.Text = @"Please enter session
you like to conduct";
            lblName.Width = 134;
            lblName.Height = 32;
            lblName.Top = location;
            lblName.Left = leftLbl;

            TextBox txtName = new TextBox();
            txtName.Width = 167;
            txtName.Height = 22;
            txtName.Top = location + 10;
            txtName.Left = leftTxt;

            this.panel.Controls.Add(lblName);
            this.panel.Controls.Add(txtName);

            // Description
            Label lblDesc = new Label();
            lblDesc.Text = @"Please enter a
description on
experience with this
session";
            lblDesc.Width = 123;
            lblDesc.Height = 64;
            lblDesc.Top = location + 43;
            lblDesc.Left = leftLbl;

            TextBox txtDesc = new TextBox();
            txtDesc.Multiline = true;
            txtDesc.Width = 167;
            txtDesc.Height = 69;
            txtDesc.Top = location + 38;
            txtDesc.Left = leftTxt;

            this.panel.Controls.Add(lblDesc);
            this.panel.Controls.Add(txtDesc);

            sessionName.Add(txtName);
            sessionDescription.Add(txtDesc);

            location += 120;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            List<Session> session = new List<Session>();
            for (int i = 0; i < this.sessionName.Count; i++)
            {
                session.Add(new Session(this.sessionName[i].Text, this.sessionDescription[i].Text));
            }

            this.trainer.Sessions = session;

            FormRegi4 formRegi4 = new FormRegi4(trainer);
            this.Hide();
            formRegi4.ShowDialog();
            this.Close();
        }
    }
}
