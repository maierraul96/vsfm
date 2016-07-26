using System;
using System.IO;
using System.Data.OleDb;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VreiSaFiiMilionar
{
    public partial class GameResult : Form
    {
        OleDbCommand saveResult = new OleDbCommand();
        TimeSpan timpScurs;
        string timpScursText;
        DateTime timpActual;
        string premiu;

        public GameResult()
        {
            InitializeComponent();
        }

        private void GameResult_Load(object sender, EventArgs e)
        {
            textBox1.Font = new Font(FontFamily.GenericSansSerif, 0.03F * this.Height, FontStyle.Bold);
            button1.Font = new Font(FontFamily.GenericSansSerif, 0.02F * this.Height, FontStyle.Bold);

            timpScurs = Game.timpSfarsit.Subtract(Game.timpStart);
            timpScursText = String.Format("{0:hh\\:mm\\:ss}", timpScurs);
            timpActual = DateTime.Now;

            Main.player.PlayLooping();

            textBox1.Text = "Felicitari " + Main.setGameForm.textBoxNume.Text +"!" + Environment.NewLine;
            textBox1.Text += Environment.NewLine;

            premiu = null;
            if (Game.nivelRezultat > 0)
            {
                premiu += SetGame.gameForm.DenumiriNivele.Rows[15][1].ToString() + " ";
                premiu += Game.Butoane[Game.nivelRezultat - 1].Text;
            }
            else
                premiu += SetGame.gameForm.DenumiriNivele.Rows[16][1].ToString();

            textBox1.Text += premiu;

            textBox1.Text += Environment.NewLine + Environment.NewLine;
            textBox1.Text += "Timp scurs: " + timpScursText + Environment.NewLine;
            textBox1.Text += "Nume: " + Main.setGameForm.textBoxNume.Text + Environment.NewLine;
            textBox1.Text += "Clasa: " + Main.setGameForm.textBoxClasa.Text + Environment.NewLine;
            textBox1.Text += "Data: " + timpActual.ToString();
            textBox1.Text += Environment.NewLine + Environment.NewLine;
            textBox1.Text += Main.setGameForm.textBoxEtichete.Text;


            saveResult.CommandText = "INSERT INTO Rezultate (Data_si_ora,Nume_elev,Clasa,Timp_scurs,Timp_intrebare,Premiul_obtinut,Nivel_final,Etichete_selectate) VALUES(";
            saveResult.CommandText += "'" + timpActual.ToString() + "',";
            saveResult.CommandText += "'" + Main.setGameForm.textBoxNume.Text + "',";
            saveResult.CommandText += "'" + Main.setGameForm.textBoxClasa.Text + "',";
            saveResult.CommandText += "'" + timpScursText + "',";
            saveResult.CommandText += "'" + Main.setGameForm.comboBoxTimp.Text + "',";
            saveResult.CommandText += "'" + premiu + "',";
            saveResult.CommandText += "'" + Game.nivelRezultat.ToString() + "',";
            saveResult.CommandText += "'" + Main.setGameForm.textBoxEtichete.Text + "')";

            saveResult.Connection = SetGame.DBConnection;

            try
            {
                saveResult.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Eroare la salvarea rezultatelor!");
                MessageBox.Show(ex.ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            this.Close();
        }
    }
}
