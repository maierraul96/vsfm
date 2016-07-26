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
    public partial class BlackMain : Form
    {
        public static Main mainForm = new Main();

        public BlackMain()
        {
            InitializeComponent();
        }

        private void BlackMain_Load(object sender, EventArgs e)
        {
           // MessageBox.Show(this.Width.ToString() + "  " + (this.Width * 9 / 16).ToString());

            /* if (this.Width / this.Height < 16 / 9)
            {
                mainForm.Size = new Size(this.Width, this.Width * 9 / 16);
                MessageBox.Show("Prima");
            }
            else if (this.Width / this.Height > 16 / 9)
            {
                mainForm.Size = new Size(this.Height * 16 / 9, this.Height);
            }
            else
            {
                mainForm.Size = new Size(this.Width / 2, this.Height / 2);
            }*/

            mainForm.Size = new Size(this.Width, this.Width * 9 / 16);

           // MessageBox.Show(mainForm.Width.ToString() + "  " + mainForm.Height.ToString());
            mainForm.ShowDialog();
            Application.Exit();
        }
    }
}
