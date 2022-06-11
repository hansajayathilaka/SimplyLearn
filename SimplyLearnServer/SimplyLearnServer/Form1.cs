using Newtonsoft.Json;
using SimplyLearnServer.Model;
using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimplyLearnServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SimpleTcpServer server;
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            server.Start();
            txtInfo.Text += $"Start Server...{Environment.NewLine}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            server = new SimpleTcpServer("127.0.0.1:9000");
            server.Events.ClientConnected += Event_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived;
            btnStart.Enabled = true;
        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                var data = Encoding.UTF8.GetString(e.Data);
                var dataArr = data.Split(';');
                var msgType = dataArr[0];
                var msg = dataArr[1];
                switch (msgType)
                {
                    case "trainer":
                        try
                        {
                            var trainer = JsonConvert.DeserializeObject<Trainer>(msg);
                            DbAccess.SaveTrainer(trainer);
                            this.server.Send(e.IpPort, "trainer;Saved successfilly.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                            this.server.Send(e.IpPort, $"trainer;{ex.Message}");
                        }
                        break;
                    case "login":
                        try
                        {
                            var user = JsonConvert.DeserializeObject<User>(msg);
                            var status = DbAccess.LoginUser(user);
                            if (status)
                            {
                                this.server.Send(e.IpPort, $"login;true");
                            }
                            else
                            {
                                this.server.Send(e.IpPort, $"login;false");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                            this.server.Send(e.IpPort, $"login;{ex.Message}");
                        }
                        break;
                }
                txtInfo.Text += $"{e.IpPort}: Data received.{Environment.NewLine}";
            });
        }

        private void Events_ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort}: Disconneced.{Environment.NewLine}";
                lstClient.Items.Remove(e.IpPort);
            });
        }

        private void Event_ClientConnected(object sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort}: Connected.{Environment.NewLine}";
                lstClient.Items.Add(e.IpPort);
            });
        }
    }
}
