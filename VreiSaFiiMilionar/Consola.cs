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
    public partial class Consola : Form
    {
        public Consola()
        {
            InitializeComponent();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (textBox1.Text=="/show")
                {
                    textBox1.Text = "Raspuns corect: "+Game.raspunsCorect.ToString();
                }
                else if (textBox1.Text.Contains("/advanceto"))
                {
                    string nivel = textBox1.Text;
                    nivel=nivel.Remove(0, 11);
                    textBox1.Text = "Avansat la nivelul " + nivel;
                    Game.NivelAcutal = Convert.ToInt32(nivel);
                }
                
            }
            e.Handled = true;
        }
    }
}
