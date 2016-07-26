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
    public partial class DenumiriNivele : Form
    {

        DataTable Denumiri = new DataTable();
        OleDbDataAdapter adapter;

        List<TextBox> Texte = new List<TextBox>();

        private void schimbaDenumiri()
        {
            if (SetGame.DBConnection.State.Equals(ConnectionState.Closed))
                SetGame.DBConnection.Open();
            try
            {
                for (int i = 0; i <= Denumiri.Rows.Count - 1; i++)
                {
                    //Denumiri.NewRow();
                    Denumiri.Rows[i].SetField<string>(1, Texte[i].Text);
                }

                OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(adapter);
                adapter.Update(Denumiri);

                if (SetGame.DBConnection.State.Equals(ConnectionState.Open))
                {
                    SetGame.DBConnection.Close();
                    SetGame.DBConnection.Open();
                }

               // MessageBox.Show("Denumirile nivelelor s-au actualizat cu succes");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Eroare la salvare");
                MessageBox.Show(ex.ToString());
            }
                
        }

        private void getDenumiri()
        {
            if (SetGame.DBConnection.State.Equals(ConnectionState.Closed))
                SetGame.DBConnection.Open();
            try
            {
                Denumiri.Clear();

                adapter = new OleDbDataAdapter("SELECT * FROM DenumiriNivele", SetGame.DBConnection);
                adapter.Fill(Denumiri);

                for (int i = 0; i <= Denumiri.Rows.Count - 1; i++)
                {
                    Texte[i].Text = Denumiri.Rows[i][1].ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Eroare la incarcare");
                MessageBox.Show(ex.ToString());
            }

        }

        public DenumiriNivele()
        {
            InitializeComponent();
        }

        private void resizeLabel(ref Label label, float procent)
        {
            label.Font = new Font(FontFamily.GenericSansSerif, procent * this.Height, FontStyle.Bold);
        }

        private void resizeTextBox(ref TextBox textBox, float procent)
        {
            textBox.Font = new Font(FontFamily.GenericSansSerif, procent * this.Height, FontStyle.Bold);
        }

        private void remarginButton(ref Button button, float procent, float left, float top, float right, float bottom)
        {
            button.Margin = new Padding(
                Convert.ToInt32(left * button.Width),     //left
                Convert.ToInt32(top * button.Height),     //top
                Convert.ToInt32(right * button.Width),    //right
                Convert.ToInt32(bottom * button.Height));//bottom

            button.Font = new Font(FontFamily.GenericSansSerif, procent * this.Height, FontStyle.Bold);
        }


        private void dimensionare()
        {
            resizeLabel(ref label1, 0.02F);
            resizeLabel(ref label2, 0.02F);
            resizeLabel(ref label3, 0.02F);
            resizeLabel(ref label4, 0.02F);
            resizeLabel(ref label5, 0.02F);
            resizeLabel(ref label6, 0.02F);
            resizeLabel(ref label7, 0.02F);
            resizeLabel(ref label8, 0.02F);
            resizeLabel(ref label9, 0.02F);
            resizeLabel(ref label10, 0.02F);
            resizeLabel(ref label11, 0.02F);
            resizeLabel(ref label12, 0.02F);
            resizeLabel(ref label13, 0.02F);
            resizeLabel(ref label14, 0.02F);
            resizeLabel(ref label15, 0.02F);

            resizeLabel(ref label16, 0.012F);
            resizeLabel(ref label17, 0.012F);


            resizeTextBox(ref textBoxNiv1, 0.017F);
            resizeTextBox(ref textBoxNiv2, 0.017F);
            resizeTextBox(ref textBoxNiv3, 0.017F);
            resizeTextBox(ref textBoxNiv4, 0.017F);
            resizeTextBox(ref textBoxNiv5, 0.017F);
            resizeTextBox(ref textBoxNiv6, 0.017F);
            resizeTextBox(ref textBoxNiv7, 0.017F);
            resizeTextBox(ref textBoxNiv8, 0.017F);
            resizeTextBox(ref textBoxNiv9, 0.017F);
            resizeTextBox(ref textBoxNiv10, 0.017F);
            resizeTextBox(ref textBoxNiv11, 0.017F);
            resizeTextBox(ref textBoxNiv12, 0.017F);
            resizeTextBox(ref textBoxNiv13, 0.017F);
            resizeTextBox(ref textBoxNiv14, 0.017F);
            resizeTextBox(ref textBoxNiv15, 0.017F);

            resizeTextBox(ref textBoxPrefix, 0.017F);
            resizeTextBox(ref textBoxNimic, 0.017F);

            remarginButton(ref buttonSalveaza, 0.017F, 0.2F, 0.25F, 0.1F, 0.1F);
            remarginButton(ref buttonInchide, 0.017F, 0.2F, 0.25F, 0.2F, 0.1F);
        }

        private void DenumiriNivele_Load(object sender, EventArgs e)
        {
            dimensionare();

            Texte.Clear();
            Texte.Add(textBoxNiv15);
            Texte.Add(textBoxNiv14);
            Texte.Add(textBoxNiv13);
            Texte.Add(textBoxNiv12);
            Texte.Add(textBoxNiv11);
            Texte.Add(textBoxNiv10);
            Texte.Add(textBoxNiv9);
            Texte.Add(textBoxNiv8);
            Texte.Add(textBoxNiv7);
            Texte.Add(textBoxNiv6);
            Texte.Add(textBoxNiv5);
            Texte.Add(textBoxNiv4);
            Texte.Add(textBoxNiv3);
            Texte.Add(textBoxNiv2);
            Texte.Add(textBoxNiv1);
            Texte.Add(textBoxPrefix);
            Texte.Add(textBoxNimic);

            getDenumiri();
        }

        private void DenumiriNivele_SizeChanged(object sender, EventArgs e)
        {
            dimensionare();
        }

        private void buttonSalveaza_Click(object sender, EventArgs e)
        {
            schimbaDenumiri();
            this.Close();
        }

        private void buttonInchide_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
