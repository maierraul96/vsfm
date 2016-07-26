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
    public partial class Rezultate : Form
    {
        OleDbConnection DBConnection = new OleDbConnection();
        OleDbDataAdapter dataAdapter;
        DataTable tabelRezultate = new DataTable();
        DataTable auxiliar = new DataTable();

        public Rezultate()
        {
            InitializeComponent();
        }

        private void repaddigLabel(ref Label PB, float left, float top, float right, float bottom)
        {
            PB.Padding = new Padding(
                Convert.ToInt32(left * PB.Width),
                Convert.ToInt32(top * PB.Height),
                Convert.ToInt32(right * PB.Width),
                Convert.ToInt32(bottom * PB.Height));
        }

        private void Rezultate_Load(object sender, EventArgs e)
        {
            label1.Font= new Font(FontFamily.GenericSansSerif, 0.033F * this.Height, FontStyle.Bold);
            repaddigLabel(ref label1, 0.0F, 0.0F, 0.12F, 0.0F);

            dataGridView1.ColumnHeadersDefaultCellStyle.Font= new Font(FontFamily.GenericSansSerif, 0.0198F * this.Height, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.Font= new Font(FontFamily.GenericSansSerif, 0.018F * this.Height, FontStyle.Bold);

            DBConnection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=DatabaseMilionar.mdb";
            DBConnection.Open();

            tabelRezultate.Clear();
            dataAdapter = new OleDbDataAdapter("SELECT Data_si_ora,Nume_elev,Clasa,Timp_scurs,Timp_intrebare,Premiul_obtinut,Nivel_final,Etichete_selectate FROM Rezultate", DBConnection);
            dataAdapter.Fill(tabelRezultate);

            auxiliar.Clear();
            auxiliar = tabelRezultate.Clone();

            for (int i=0;i<tabelRezultate.Rows.Count;i++)
            {
                auxiliar.ImportRow(tabelRezultate.Rows[tabelRezultate.Rows.Count - 1 - i]);
            }
            
            dataGridView1.DataSource = auxiliar.AsDataView();
        }

        private void Rezultate_FormClosing(object sender, FormClosingEventArgs e)
        {
            DBConnection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
