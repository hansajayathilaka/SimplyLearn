using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimplyLearn
{
    public partial class Login : Form
    {
        public int sessionCount = 0;
        public Login()
        {
            InitializeComponent();
            this.lblSessionCount.Text = this.sessionCount.ToString();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = this.txtUName.Text.Trim();
            string password = this.txtPassword.Text.Trim();

            var user = new User();
            user.Username = userName;
            user.Password = password;

            try
            {
                IRepository repository = new Repository();
                var loginStatus = user.Login(repository);
                if (loginStatus == false)
                {
                    this.txtPassword.Text = "";
                    MessageBox.Show("Invalid Credentials.");
                } else
                {
                    this.txtUName.Text = "";
                    this.txtPassword.Text = "";

                    ThreadStart starter = this.StartRegistration;

                    starter += () =>
                    {
                        this.sessionCount--;
                        this.UpdateLabel(this.sessionCount.ToString());
                    };

                    var _thread = new Thread(starter) { IsBackground = true };
                    _thread.Start();
                    this.sessionCount++;
                    this.lblSessionCount.Text = this.sessionCount.ToString();
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }
        private void StartRegistration()
        {
            Trainer trainer = new Trainer();
            var formRegi1 = new FormRegi1(trainer);
            formRegi1.ShowDialog();
        }

        public void UpdateLabel(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateLabel), new object[] { value });
                return;
            }
            lblSessionCount.Text = value;
        }
    }

}
