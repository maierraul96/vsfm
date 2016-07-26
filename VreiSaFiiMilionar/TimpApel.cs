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
    public partial class TimpApel : Form
    {
        public TimpApel()
        {
            InitializeComponent();
        }

        private bool checkTime()
        {
            if (comboBoxTimp.Text != "Nelimitat" || comboBoxTimp.SelectedIndex != 0)
                try
                {
                    if (Convert.ToInt32(comboBoxTimp.Text) < 0)
                    {
                        MessageBox.Show("Timpul nu poate fi negativ!");
                        return false;
                    }
                    else
                        return true;
                }
                catch
                {
                    MessageBox.Show("Timpul introdus nu este valid!");
                    return false;
                }
            else
            {
                return true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkTime())
            {
                if (comboBoxTimp.Text == "Fara timp" || comboBoxTimp.SelectedIndex == 0)
                    Game.timpApel = -1;
                else
                    Game.timpApel = Convert.ToInt32(comboBoxTimp.Text);
                this.Close();
            }
        }
    }
}
