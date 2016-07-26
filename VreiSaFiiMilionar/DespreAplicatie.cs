using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VreiSaFiiMilionar
{
    public partial class DespreAplicatie : Form
    {
        public DespreAplicatie()
        {
            InitializeComponent();
        }

        private void DespreAplicatie_Load(object sender, EventArgs e)
        {
            this.Height = (int)(Screen.PrimaryScreen.WorkingArea.Height);
            this.Width = (int)(0.80F * this.Height);
            this.Top = 0;
            this.Left = Convert.ToInt32((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2);
        }
    }
}
