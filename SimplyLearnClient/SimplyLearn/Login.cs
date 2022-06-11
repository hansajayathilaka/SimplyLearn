using SuperSimpleTcp;
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

        SimpleTcpClient client;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;
            string userName = this.txtUName.Text.Trim();
            string password = this.txtPassword.Text.Trim();

            var user = new User();
            user.Username = userName;
            user.Password = password;

            try
            {
                IRepository repository = new Repository(client);
                var loginStatus = user.Login(repository);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }

        private void StartRegistration()
        {
            Trainer trainer = new Trainer();
            var formRegi1 = new FormRegi1(client, trainer);
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

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                client.Connect();
                btnConnect.Enabled = false;
                btnLogin.Enabled = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.client = new SimpleTcpClient(txtUrl.Text);
            client.Events.Connected += Events_Connected;
            client.Events.Disconnected += Events_Disconnected;
            client.Events.DataReceived += Events_DataReceived;
            btnLogin.Enabled = false;
        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            { 
                var data = Encoding.UTF8.GetString(e.Data);
                var dataArr = data.Split(';');
                switch (dataArr[0])
                {
                    case "login":
                        if (dataArr[1] == "false")
                        {
                            this.txtPassword.Text = "";
                            MessageBox.Show("Invalid Credentials.");
                        }
                        else if (dataArr[1] == "true")
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
                        else
                        {
                            MessageBox.Show(dataArr[1]);
                        }
                        btnLogin.Enabled = true;
                        break;
                    case "trainer":
                        MessageBox.Show(dataArr[1]);
                        break;
                }
            });
            
        }

        private void Events_Disconnected(object sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                btnConnect.Enabled = true;
            });
            MessageBox.Show("Disconnected...");
        }

        private void Events_Connected(object sender, ConnectionEventArgs e)
        {
            MessageBox.Show("Connected successfully.");
        }
    }
}
