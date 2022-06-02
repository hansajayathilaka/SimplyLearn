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
    public partial class FormRegi4 : Form
    {
        private readonly Trainer trainer;
        public FormRegi4(Trainer trainer)
        {
            InitializeComponent();
            this.trainer = trainer;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            IRepository repository = new Repository();
            var res = this.trainer.RegisterTrainer(repository);
            if (res.Error != null)
            {
                switch (res.Error)
                {
                    case RegisterErrors.FirstNameRequired:
                        MessageBox.Show("First name is required.");
                        break;
                    case RegisterErrors.LastNameRequired:
                        MessageBox.Show("Last name is required.");
                        break;
                    case RegisterErrors.EmailRequired:
                        MessageBox.Show("Email is required.");
                        break;
                    case RegisterErrors.NoSessionsProvided:
                        MessageBox.Show("Sessions are not provided.");
                        break;
                    case RegisterErrors.NoSessionsApproved:
                        MessageBox.Show("Sessions are not approved.");
                        break;
                    case RegisterErrors.TrainerDoesNotMeetStandards:
                        MessageBox.Show("Trainer does not meet the standards");
                        break;
                    case RegisterErrors.RegistrationError:
                        MessageBox.Show("Registration error");
                        break;
                    default:
                        MessageBox.Show("Unknown error. Error code " + res.Error);
                        break;
                }
            }
            this.Close();
        }
    }
}
