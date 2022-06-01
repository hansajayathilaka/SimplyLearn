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
            this.trainer.SaveTrainer(repository);
            this.Close();
        }
    }
}
