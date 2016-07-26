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
    public partial class GestionareIntrebari : Form
    {
        public FormEditorModifica formModifica = new FormEditorModifica();

        OleDbConnection DBConnection = new OleDbConnection();
        OleDbDataAdapter DataAdapter;
        DataTable Intrebari = new DataTable();
        int indexIntrebare = 0;

        ListBox listBoxNivel = new ListBox();
        ListBox listBoxAn = new ListBox();
        ListBox listBoxSpecializare = new ListBox();
        ListBox listBoxDisciplina = new ListBox();
        ListBox listBoxMaterie = new ListBox();
        ListBox listBoxCapitol = new ListBox();

        List<PictureBox> imgRaspunsuri = new List<PictureBox>();

        public GestionareIntrebari()
        {
            InitializeComponent();
        }

        private void ConnectToDatabase()
        {
            DBConnection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=DatabaseMilionar.mdb";
            DBConnection.Open();
        }

        private void RefreshDBConnection()
        {
            if (DBConnection.State.Equals(ConnectionState.Open))
            {
                DBConnection.Close();
                ConnectToDatabase();
            }
        }


        private void updateNivele()
        {
            try
            {
                OleDbCommand command = new OleDbCommand("Select Nivel from Nivele", DBConnection);
                OleDbDataReader reader = command.ExecuteReader();

                comboBoxNivel.Items.Clear();
                while (reader.Read())
                    comboBoxNivel.Items.Add(reader[0].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nu am putut incarca nivelele");
                MessageBox.Show(ex.Message.ToString());
                RefreshDBConnection();
            }
        }

        private void updateAni()
        {
            try
            {
                OleDbCommand command = new OleDbCommand("Select An from AniStudiu", DBConnection);
                OleDbDataReader reader = command.ExecuteReader();

                comboBoxAn.Items.Clear();
                while (reader.Read())
                    comboBoxAn.Items.Add(reader[0].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nu am putut incarca anii de studiu");
                MessageBox.Show(ex.Message.ToString());
                RefreshDBConnection();
            }
        }

        private void updateSpecializari()
        {
            try
            {
                OleDbCommand command = new OleDbCommand("Select Specializare from Specializari", DBConnection);
                OleDbDataReader reader = command.ExecuteReader();

                comboBoxSpecializare.Items.Clear();
                while (reader.Read())
                    comboBoxSpecializare.Items.Add(reader[0].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nu am putut incarca specializarile");
                MessageBox.Show(ex.Message.ToString());
                RefreshDBConnection();
            }
        }

        private void updateDiscipline()
        {
            try
            {
                OleDbCommand command = new OleDbCommand("Select Disciplina from Discipline", DBConnection);
                OleDbDataReader reader = command.ExecuteReader();

                comboBoxDisciplina.Items.Clear();
                while (reader.Read())
                    comboBoxDisciplina.Items.Add(reader[0].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nu am putut incarca disciplinele");
                MessageBox.Show(ex.Message.ToString());
                RefreshDBConnection();
            }
        }

        private void updateMaterii()
        {
            try
            {
                OleDbCommand command = new OleDbCommand("Select Materie from Materii", DBConnection);
                OleDbDataReader reader = command.ExecuteReader();

                comboBoxMaterie.Items.Clear();
                while (reader.Read())
                    comboBoxMaterie.Items.Add(reader[0].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nu am putut incarca materiile");
                MessageBox.Show(ex.Message.ToString());
                RefreshDBConnection();
            }
        }

        private void updateCapitole()
        {
            try
            {
                OleDbCommand command = new OleDbCommand("Select Capitol from Capitole", DBConnection);
                OleDbDataReader reader = command.ExecuteReader();

                comboBoxCapitol.Items.Clear();
                while (reader.Read())
                    comboBoxCapitol.Items.Add(reader[0].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nu am putut incarca capitolele");
                MessageBox.Show(ex.Message.ToString());
                RefreshDBConnection();
            }
        }

        private void updateLists()
        {
            updateNivele();
            updateAni();
            updateSpecializari();
            updateDiscipline();
            updateMaterii();
            updateCapitole();
        }



        private Image getImage(string denumireCamp)
        {
            Image FetchedImage;
            byte[] FetchedBytes = (byte[])(Intrebari.Rows[indexIntrebare][denumireCamp]);
            MemoryStream Stream = new MemoryStream(FetchedBytes);
            FetchedImage = Image.FromStream(Stream);
            return FetchedImage;
        }


        private string getNivelFromID(int ID)
        {
            OleDbCommand getTextCommand = new OleDbCommand();
            OleDbDataReader reader;
            
            getTextCommand.Connection = DBConnection;

            string Text="";
            getTextCommand.CommandText = "SELECT Nivel FROM Nivele WHERE ID=" + ID.ToString();
            reader = getTextCommand.ExecuteReader();
            while (reader.Read())
                Text = reader[0].ToString();
            reader.Close();

            return Text;   
        }

        private string getAnFromID(int ID)
        {
            OleDbCommand getTextCommand = new OleDbCommand();
            OleDbDataReader reader;

            getTextCommand.Connection = DBConnection;

            string Text = "";
            getTextCommand.CommandText = "SELECT An FROM AniStudiu WHERE ID=" + ID.ToString();
            reader = getTextCommand.ExecuteReader();
            while (reader.Read())
                Text = reader[0].ToString();
            reader.Close();

            return Text;
        }

        private string getSpecializareFromID(int ID)
        {
            OleDbCommand getTextCommand = new OleDbCommand();
            OleDbDataReader reader;

            getTextCommand.Connection = DBConnection;

            string Text = "";
            getTextCommand.CommandText = "SELECT Specializare FROM Specializari WHERE ID=" + ID.ToString();
            reader = getTextCommand.ExecuteReader();
            while (reader.Read())
                Text = reader[0].ToString();
            reader.Close();

            return Text;
        }

        private string getDisciplinaFromID(int ID)
        {
            OleDbCommand getTextCommand = new OleDbCommand();
            OleDbDataReader reader;

            getTextCommand.Connection = DBConnection;

            string Text = "";
            getTextCommand.CommandText = "SELECT Disciplina FROM Discipline WHERE ID=" + ID.ToString();
            reader = getTextCommand.ExecuteReader();
            while (reader.Read())
                Text = reader[0].ToString();
            reader.Close();

            return Text;
        }

        private string getMaterieFromID(int ID)
        {
            OleDbCommand getTextCommand = new OleDbCommand();
            OleDbDataReader reader;

            getTextCommand.Connection = DBConnection;

            string Text = "";
            getTextCommand.CommandText = "SELECT Materie FROM Materii WHERE ID=" + ID.ToString();
            reader = getTextCommand.ExecuteReader();
            while (reader.Read())
                Text = reader[0].ToString();
            reader.Close();

            return Text;
        }

        private string getCapitolFromID(int ID)
        {
            OleDbCommand getTextCommand = new OleDbCommand();
            OleDbDataReader reader;

            getTextCommand.Connection = DBConnection;

            string Text = "";
            getTextCommand.CommandText = "SELECT Capitol FROM Capitole WHERE ID=" + ID.ToString();
            reader = getTextCommand.ExecuteReader();
            while (reader.Read())
                Text = reader[0].ToString();
            reader.Close();

            return Text;
        }



        private string getIDNivel(string item)
        {
            OleDbCommand getIDCommand = new OleDbCommand();
            OleDbDataReader reader;

            getIDCommand.Connection = DBConnection;

            string IDNivel = "1";
            getIDCommand.CommandText = "Select ID From Nivele WHERE Nivel='" + item + "'";
            reader = getIDCommand.ExecuteReader();
            while (reader.Read())
                IDNivel = reader[0].ToString();
            reader.Close();
            return IDNivel;

        }

        private string getIDAn(string item)
        {
            OleDbCommand getIDCommand = new OleDbCommand();
            OleDbDataReader reader;

            getIDCommand.Connection = DBConnection;

            string IDAn = "1";
            getIDCommand.CommandText = "Select ID from AniStudiu WHERE An='" + item + "'";
            reader = getIDCommand.ExecuteReader();
            while (reader.Read())
                IDAn = (reader[0]).ToString();
            reader.Close();
            return IDAn;
        }

        private string getIDSpecializare(string item)
        {
            OleDbCommand getIDCommand = new OleDbCommand();
            OleDbDataReader reader;

            getIDCommand.Connection = DBConnection;

            string IDSpecializare = "1";
            getIDCommand.CommandText = "Select ID from Specializari WHERE Specializare='" + item + "'";
            reader = getIDCommand.ExecuteReader();
            while (reader.Read())
                IDSpecializare = (reader[0]).ToString();
            reader.Close();
            return IDSpecializare;
        }

        private string getIDDisciplina(string item)
        {
            OleDbCommand getIDCommand = new OleDbCommand();
            OleDbDataReader reader;

            getIDCommand.Connection = DBConnection;

            string IDDisciplina = "1";
            getIDCommand.CommandText = "Select ID from discipline WHERE Disciplina='" + item + "'";
            reader = getIDCommand.ExecuteReader();
            while (reader.Read())
                IDDisciplina = (reader[0]).ToString();
            reader.Close();
            return IDDisciplina;
        }

        private string getIDMaterie(string item)
        {
            OleDbCommand getIDCommand = new OleDbCommand();
            OleDbDataReader reader;

            getIDCommand.Connection = DBConnection;

            string IDMaterie = "1";
            getIDCommand.CommandText = "Select ID from Materii WHERE Materie='" + item + "'";
            reader = getIDCommand.ExecuteReader();
            while (reader.Read())
                IDMaterie = (reader[0]).ToString();
            reader.Close();
            return IDMaterie;
        }

        private string getIDCapitol(string item)
        {
            OleDbCommand getIDCommand = new OleDbCommand();
            OleDbDataReader reader;

            getIDCommand.Connection = DBConnection;

            string IDCapitol = "1";
            getIDCommand.CommandText = "Select ID from Capitole WHERE Capitol='" + item + "'";
            reader = getIDCommand.ExecuteReader();
            while (reader.Read())
                IDCapitol = (reader[0]).ToString();
            reader.Close();
            return IDCapitol;
        }



        private string makeQueryNivele()
        {
            //Verific cate etichete pentru nivele sunt
            if (listBoxNivel.Items.Count > 0)
            {
                //Cpnstruiesc queryul pentru conditiile de nivele
                string query = "(";
                for (int i = 0; i <= listBoxNivel.Items.Count - 1; i++)
                {
                    query += "Nivel=" + getIDNivel(listBoxNivel.Items[i].ToString());
                    if (i != listBoxNivel.Items.Count - 1)
                        query += " OR ";
                }
                query += ")";
                return query;
            }
            else return "";
        }

        private string makeQueryAni()
        {
            //Verific cate etichete pentru ani sunt
            if (listBoxAn.Items.Count > 0)
            {
                //Construiesc queryul pentru condiitile de ani de studiu
                string query = "(";
                for (int i = 0; i <= listBoxAn.Items.Count - 1; i++)
                {
                    query += "AnStudiu=" + getIDAn(listBoxAn.Items[i].ToString());
                    if (i != listBoxAn.Items.Count - 1)
                        query += " OR ";
                }
                query += ")";
                return query;
            }
            else return "";
        }

        private string makeQuerySpecializari()
        {
            //Verific cate etichete pentru specializari sunt
            if (listBoxSpecializare.Items.Count > 0)
            {
                //Construiesc queryul pentru condiitile de specializari
                string query = "(";
                for (int i = 0; i <= listBoxSpecializare.Items.Count - 1; i++)
                {
                    query += "Specializare=" + getIDSpecializare(listBoxSpecializare.Items[i].ToString());
                    if (i != listBoxSpecializare.Items.Count - 1)
                        query += " OR ";
                }
                query += ")";
                return query;
            }
            else return "";
        }

        private string makeQueryDiscipline()
        {
            //Verific cate etichete pentru specializari sunt
            if (listBoxDisciplina.Items.Count > 0)
            {
                //Construiesc queryul pentru condiitile de specializari
                string query = "(";
                for (int i = 0; i <= listBoxDisciplina.Items.Count - 1; i++)
                {
                    query += "Disciplina=" + getIDDisciplina(listBoxDisciplina.Items[i].ToString());
                    if (i != listBoxDisciplina.Items.Count - 1)
                        query += " OR ";
                }
                query += ")";
                return query;
            }
            else return "";
        }

        private string makeQueryMaterii()
        {
            //Verific cate etichete pentru specializari sunt
            if (listBoxMaterie.Items.Count > 0)
            {
                //Construiesc queryul pentru condiitile de specializari
                string query = "(";
                for (int i = 0; i <= listBoxMaterie.Items.Count - 1; i++)
                {
                    query += "Materie=" + getIDMaterie(listBoxMaterie.Items[i].ToString());
                    if (i != listBoxMaterie.Items.Count - 1)
                        query += " OR ";
                }
                query += ")";
                return query;
            }
            else return "";
        }

        private string makeQueryCapitole()
        {
            //Verific cate etichete pentru specializari sunt
            if (listBoxCapitol.Items.Count > 0)
            {
                //Construiesc queryul pentru condiitile de specializari
                string query = "(";
                for (int i = 0; i <= listBoxCapitol.Items.Count - 1; i++)
                {
                    query += "Capitol=" + getIDCapitol(listBoxCapitol.Items[i].ToString());
                    if (i != listBoxCapitol.Items.Count - 1)
                        query += " OR ";
                }
                query += ")";
                return query;
            }
            else return "";
        }

        private string makeQuery()
        {
            string queryNivele = makeQueryNivele();
            string queryAni = makeQueryAni();
            string querySpecializari = makeQuerySpecializari();
            string queryDiscipline = makeQueryDiscipline();
            string queryMaterii = makeQueryMaterii();
            string queryCapitole = makeQueryCapitole();

            string legatura = " WHERE ";
            string query = "SELECT * FROM Intrebari";

            if (queryNivele!="")
            {
                query += legatura + queryNivele;
                legatura = " AND ";
            }

            if (queryAni != "")
            {
                query += legatura + queryAni;
                legatura = "AND";
            }

            if (querySpecializari != "")
            {
                query += legatura + querySpecializari;
                legatura = " AND ";
            }

            if (queryDiscipline != "")
            {
                query += legatura + queryDiscipline;
                legatura = " AND ";
            }

            if (queryMaterii != "")
            {
                query += legatura + queryMaterii;
                legatura = " AND ";
            }

            if (queryCapitole != "")
            {
                query += legatura + queryCapitole;
                legatura = " AND ";
            }

            return query;

        }



        private void displayEtichete()
        {
            //Sterg tot ce este afisat
            textBoxEtichete.Clear();

            //Afisez nivelele selectate daca exista
            if (listBoxNivel.Items.Count > 0)
            {
                if (textBoxEtichete.Text != String.Empty)
                    textBoxEtichete.Text += Environment.NewLine + Environment.NewLine;

                textBoxEtichete.Text += "Nivele selectate:" + Environment.NewLine;
                for (int i = 0; i <= listBoxNivel.Items.Count - 1; i++)
                {
                    textBoxEtichete.Text += "  -" + listBoxNivel.Items[i].ToString();

                    if (i < listBoxNivel.Items.Count - 1)
                        textBoxEtichete.Text += Environment.NewLine;
                }
            }

            //Afisez anii de studiu selectati daca exista
            if (listBoxAn.Items.Count > 0)
            {
                if (textBoxEtichete.Text != String.Empty)
                    textBoxEtichete.Text += Environment.NewLine + Environment.NewLine;

                textBoxEtichete.Text += "Clase selectate:" + Environment.NewLine;
                for (int i = 0; i <= listBoxAn.Items.Count - 1; i++)
                {
                    textBoxEtichete.Text += "  -" + listBoxAn.Items[i].ToString();

                    if (i < listBoxAn.Items.Count - 1)
                        textBoxEtichete.Text += Environment.NewLine;
                }
            }

            //Afisez anii de studiu selectati daca exista
            if (listBoxSpecializare.Items.Count > 0)
            {
                if (textBoxEtichete.Text != String.Empty)
                    textBoxEtichete.Text += Environment.NewLine + Environment.NewLine;

                textBoxEtichete.Text += "Specializari selectate:" + Environment.NewLine;
                for (int i = 0; i <= listBoxSpecializare.Items.Count - 1; i++)
                {
                    textBoxEtichete.Text += "  -" + listBoxSpecializare.Items[i].ToString();

                    if (i < listBoxSpecializare.Items.Count - 1)
                        textBoxEtichete.Text += Environment.NewLine;
                }
            }

            //Afisez anii de studiu selectati daca exista
            if (listBoxDisciplina.Items.Count > 0)
            {
                if (textBoxEtichete.Text != String.Empty)
                    textBoxEtichete.Text += Environment.NewLine + Environment.NewLine;

                textBoxEtichete.Text += "Discipline selectate:" + Environment.NewLine;
                for (int i = 0; i <= listBoxDisciplina.Items.Count - 1; i++)
                {
                    textBoxEtichete.Text += "  -" + listBoxDisciplina.Items[i].ToString();

                    if (i < listBoxDisciplina.Items.Count - 1)
                        textBoxEtichete.Text += Environment.NewLine;
                }
            }

            //Afisez anii de studiu selectati daca exista
            if (listBoxMaterie.Items.Count > 0)
            {
                if (textBoxEtichete.Text != String.Empty)
                    textBoxEtichete.Text += Environment.NewLine + Environment.NewLine;

                textBoxEtichete.Text += "Materii selectate:" + Environment.NewLine;
                for (int i = 0; i <= listBoxMaterie.Items.Count - 1; i++)
                {
                    textBoxEtichete.Text += "  -" + listBoxMaterie.Items[i].ToString();

                    if (i < listBoxMaterie.Items.Count - 1)
                        textBoxEtichete.Text += Environment.NewLine;
                }
            }

            //Afisez anii de studiu selectati daca exista
            if (listBoxCapitol.Items.Count > 0)
            {
                if (textBoxEtichete.Text != String.Empty)
                    textBoxEtichete.Text += Environment.NewLine + Environment.NewLine;

                textBoxEtichete.Text += "Capitole selectate:" + Environment.NewLine;
                for (int i = 0; i <= listBoxCapitol.Items.Count - 1; i++)
                {
                    textBoxEtichete.Text += "  -" + listBoxCapitol.Items[i].ToString();

                    if (i < listBoxCapitol.Items.Count - 1)
                        textBoxEtichete.Text += Environment.NewLine;
                }
            }
        }

        private void displayIntrebare()
        {
            //Afisez etichetele intrebarii curente
            labelNivel.Text = "Nivel: " + getNivelFromID(Convert.ToInt32(Intrebari.Rows[indexIntrebare]["Nivel"]));
            labelAn.Text = "An de studiu: " + getAnFromID(Convert.ToInt32(Intrebari.Rows[indexIntrebare]["AnStudiu"]));
            labelSpecializare.Text = "Specializare: " + getSpecializareFromID(Convert.ToInt32(Intrebari.Rows[indexIntrebare]["Specializare"]));
            labelDisciplina.Text = "Disciplina: " + getDisciplinaFromID(Convert.ToInt32(Intrebari.Rows[indexIntrebare]["Disciplina"]));
            labelMaterie.Text = "Materie: " + getMaterieFromID(Convert.ToInt32(Intrebari.Rows[indexIntrebare]["Materie"]));
            labelCapitol.Text = "Capitol: " + getCapitolFromID(Convert.ToInt32(Intrebari.Rows[indexIntrebare]["Capitol"]));

            labelNrIntrebare.Text = "Nr. intrebare: " + (indexIntrebare+1).ToString() + "/" + Intrebari.Rows.Count.ToString();

            //Afisez imaginile
            pictureBoxMain.Image = getImage("Intrebare");
            pictureBox1.Image = getImage("Rasp1");
            pictureBox2.Image = getImage("Rasp2");
            pictureBox3.Image = getImage("Rasp3");
            pictureBox4.Image = getImage("Rasp4");

            int RaspunsCorect = Convert.ToInt32(Intrebari.Rows[indexIntrebare]["RaspunsCorect"]);

            //Afisez raspunsul corect
            for (int i = 0; i <= 3; i++)
            {
                if (i % 2 == 0)
                {
                    if (RaspunsCorect == i+1)
                        imgRaspunsuri[i].BackgroundImage = Properties.Resources.slotcorect;
                    else
                        imgRaspunsuri[i].BackgroundImage = Properties.Resources.slot;
                }
                else
                {
                    if (RaspunsCorect == i+1)
                        imgRaspunsuri[i].BackgroundImage = Properties.Resources.slotcorect2;
                    else
                        imgRaspunsuri[i].BackgroundImage = Properties.Resources.slot2;
                }
            }
        }

        private void displayNothing()
        {
            //Afisez etichetele intrebarii curente
            labelNivel.Text = "Nivel: ---";
            labelAn.Text = "An de studiu: ---";
            labelSpecializare.Text = "Specializare: ---";
            labelDisciplina.Text = "Disciplina: ---";
            labelMaterie.Text = "Materie: ---";
            labelCapitol.Text = "Capitol: ---";

            labelNrIntrebare.Text = "Nr. intrebare: 0/0";

            pictureBoxMain.Image = null;

            for (int i = 0; i <= 3; i++)
            {
                imgRaspunsuri[i].Image = null;

                if (i % 2 == 0)
                    imgRaspunsuri[i].BackgroundImage = Properties.Resources.slot;
                else
                    imgRaspunsuri[i].BackgroundImage = Properties.Resources.slot2;    
            }
        }

        private void displaySelections()
        {
            try
            {
                displayEtichete();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Eroare la afisarea etichetelor selectate!");
                MessageBox.Show(ex.Message.ToString());
            }

            try
            {
                Intrebari.Clear();
                Intrebari.Dispose();
                DataAdapter = new OleDbDataAdapter(makeQuery(), DBConnection);
                DataAdapter.Fill(Intrebari);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Eroare la popularizarea tabelului local cu informatii din baza de date!");
                MessageBox.Show(ex.Message.ToString());
            }

            try
            {
                OleDbCommand CountCommand = new OleDbCommand("SELECT Count(*) FROM Intrebari", DBConnection);
                OleDbDataReader reader;
                reader = CountCommand.ExecuteReader();
                while (reader.Read())
                    labelTotalIntrebari.Text = "Total intrebari: " + reader[0].ToString();
                reader.Close();

                labelTotalCuEtc.Text = "Intrebari specifice: " + Intrebari.Rows.Count.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Eroare la afisrea totalului de intrebari!");
                MessageBox.Show(ex.Message.ToString());
            }

            try
            {
                if (Intrebari.Rows.Count > 0)
                    displayIntrebare();
                else
                    displayNothing();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Eroare la afisarea imaginilor cu intrebari!");
                MessageBox.Show(ex.Message.ToString());
            }
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

        private void resizeButton(ref Button button, float procent)
        {
            button.Font = new Font(FontFamily.GenericSansSerif, procent * this.Height, FontStyle.Bold);
        }

        private void remarginListBox(ref ListBox listbox, float left, float top, float right, float bottom)
        {
            listbox.Margin = new Padding(
                Convert.ToInt32(left * listbox.Width),     //left
                Convert.ToInt32(top * listbox.Height),     //top
                Convert.ToInt32(right * listbox.Width),    //right
                Convert.ToInt32(bottom * listbox.Height));//bottom
        }

        private void resizeLabel(ref Label label, float procent)
        {
            label.Font = new Font(FontFamily.GenericSansSerif, procent * this.Height, FontStyle.Bold);
        }

        private void resizeTextBox(ref TextBox textBox, float procent)
        {
            textBox.Font = new Font(FontFamily.GenericSansSerif, procent * this.Height, FontStyle.Bold);
        }

        private void resizeCombobox(ref ComboBox combobox, float procent)
        {
            combobox.Font = new Font(FontFamily.GenericSansSerif, procent * this.Height, FontStyle.Bold);
        }

        private void remarginPictureBox(ref PictureBox picturebox, float left, float top, float right, float bottom)
        {
            picturebox.Margin = new Padding(
                Convert.ToInt32(left * picturebox.Width),
                Convert.ToInt32(top * picturebox.Height),
                Convert.ToInt32(right * picturebox.Width),
                Convert.ToInt32(bottom * picturebox.Height));
        }

        private void repaddingPictureBox(ref PictureBox picturebox, float left, float top, float right, float bottom)
        {
            picturebox.Padding = new Padding(
                Convert.ToInt32(left * picturebox.Width),
                Convert.ToInt32(top * picturebox.Height),
                Convert.ToInt32(right * picturebox.Width),
                Convert.ToInt32(bottom * picturebox.Height));
        }


        private void dimensionare()
        {
            //Redimensinez titlurile
            resizeLabel(ref labelTotalIntrebari, 0.022F);
            resizeLabel(ref labelTotalCuEtc, 0.022F);
            resizeLabel(ref labelNrIntrebare, 0.035F);
            resizeLabel(ref labelEticheteSelectate, 0.018F);

            //Redimiensionez castetele
            resizeCombobox(ref comboBoxNivel, 0.022F);
            resizeCombobox(ref comboBoxAn, 0.022F);
            resizeCombobox(ref comboBoxSpecializare, 0.022F);
            resizeCombobox(ref comboBoxDisciplina, 0.022F);
            resizeCombobox(ref comboBoxMaterie, 0.022F);
            resizeCombobox(ref comboBoxCapitol, 0.022F);

            resizeTextBox(ref textBoxEtichete, 0.018F);

            //Redimensionez etichetele
            resizeLabel(ref labelNivel, 0.023F);
            resizeLabel(ref labelAn, 0.023F);
            resizeLabel(ref labelSpecializare, 0.023F);
            resizeLabel(ref labelDisciplina, 0.023F);
            resizeLabel(ref labelMaterie, 0.023F);
            resizeLabel(ref labelCapitol, 0.023F);

            //Redimesionez butoanele
            remarginButton(ref buttonPrec, 0.022F, 0.0F, 0.01F, 0.01F, 0.01F);
            remarginButton(ref buttonModifica, 0.022F, 0.0F, 0.01F, 0.01F, 0.01F);
            remarginButton(ref buttonSterge, 0.022F, 0.0F, 0.01F, 0.01F, 0.01F);
            remarginButton(ref buttonNext, 0.022F, 0.0F, 0.01F, 0.01F, 0.01F);

            //Redimensionez casetele
            repaddingPictureBox(ref pictureBoxMain, 0.1F, 0.15F, 0.1F, 0.05F);
            repaddingPictureBox(ref pictureBox1, 0.23F, 0.15F, 0.1F, 0.05F);
            repaddingPictureBox(ref pictureBox2, 0.13F, 0.15F, 0.2F, 0.05F);
            repaddingPictureBox(ref pictureBox3, 0.23F, 0.15F, 0.1F, 0.05F);
            repaddingPictureBox(ref pictureBox4, 0.13F, 0.15F, 0.2F, 0.05F);
        }


        private void GestionareIntrebari_Load(object sender, EventArgs e)
        {
            //Redimensionez
            dimensionare();
            
            listBoxDisciplina.Sorted = true;
            listBoxMaterie.Sorted = true;
            listBoxCapitol.Sorted = true;

            imgRaspunsuri.Clear();
            imgRaspunsuri.Add(pictureBox1);
            imgRaspunsuri.Add(pictureBox2);
            imgRaspunsuri.Add(pictureBox3);
            imgRaspunsuri.Add(pictureBox4);

            //Ma conectez la baza de date
            ConnectToDatabase();

            //Populez listele
            updateLists();

            //Afisez prima intrebare
            displaySelections();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GestionareIntrebari_FormClosing(object sender, FormClosingEventArgs e)
        {
            DBConnection.Close();
            listBoxNivel.Items.Clear();
            listBoxAn.Items.Clear();
            listBoxSpecializare.Items.Clear();
            listBoxDisciplina.Items.Clear();
            listBoxMaterie.Items.Clear();
            listBoxCapitol.Items.Clear();
            textBoxEtichete.Clear();
        }

        private void comboBoxNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Adaug selectia in lista
            if (!listBoxNivel.Items.Contains(comboBoxNivel.SelectedItem))
                listBoxNivel.Items.Add(comboBoxNivel.SelectedItem.ToString());
            else
                listBoxNivel.Items.Remove(comboBoxNivel.SelectedItem.ToString());

            //Actualizez lista cu selectii
            indexIntrebare = 0;
            displaySelections();

            //Refac textul afisat in combobox
            this.BeginInvoke((MethodInvoker)delegate { this.comboBoxNivel.Text = "Nivele"; });  
        }

        private void comboBoxAn_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Adaug selectia in lista
            if (!listBoxAn.Items.Contains(comboBoxAn.SelectedItem))
                listBoxAn.Items.Add(comboBoxAn.SelectedItem.ToString());
            else
                listBoxAn.Items.Remove(comboBoxAn.SelectedItem.ToString());

            //Actualizez lista cu selectii
            indexIntrebare = 0;
            displaySelections();

            //Refac textul afisat in combobox
            this.BeginInvoke((MethodInvoker)delegate { this.comboBoxAn.Text = "Ani de studiu"; });
        }

        private void comboBoxSpecializare_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Adaug selectia in lista
            if (!listBoxSpecializare.Items.Contains(comboBoxSpecializare.SelectedItem))
                listBoxSpecializare.Items.Add(comboBoxSpecializare.SelectedItem.ToString());
            else
                listBoxSpecializare.Items.Remove(comboBoxSpecializare.SelectedItem.ToString());

            //Actualizez lista cu selectii
            indexIntrebare = 0;
            displaySelections();

            //Refac textul afisat in combobox
            this.BeginInvoke((MethodInvoker)delegate { this.comboBoxSpecializare.Text = "Specializari"; });
        }

        private void comboBoxDisciplina_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Adaug selectia in lista
            if (!listBoxDisciplina.Items.Contains(comboBoxDisciplina.SelectedItem))
                listBoxDisciplina.Items.Add(comboBoxDisciplina.SelectedItem.ToString());
            else
                listBoxDisciplina.Items.Remove(comboBoxDisciplina.SelectedItem.ToString());

            //Actualizez lista cu selectii
            indexIntrebare = 0;
            displaySelections();

            //Refac textul afisat in combobox
            this.BeginInvoke((MethodInvoker)delegate { this.comboBoxDisciplina.Text = "Discipline"; });
        }

        private void comboBoxMaterie_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Adaug selectia in lista
            if (!listBoxMaterie.Items.Contains(comboBoxMaterie.SelectedItem))
                listBoxMaterie.Items.Add(comboBoxMaterie.SelectedItem.ToString());
            else
                listBoxMaterie.Items.Remove(comboBoxMaterie.SelectedItem.ToString());

            //Actualizez lista cu selectii
            indexIntrebare = 0;
            displaySelections();

            //Refac textul afisat in combobox
            this.BeginInvoke((MethodInvoker)delegate { this.comboBoxMaterie.Text = "Materii"; });
        }

        private void comboBoxCapitol_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Adaug selectia in lista
            if (!listBoxCapitol.Items.Contains(comboBoxCapitol.SelectedItem))
                listBoxCapitol.Items.Add(comboBoxCapitol.SelectedItem.ToString());
            else
                listBoxCapitol.Items.Remove(comboBoxCapitol.SelectedItem.ToString());

            //Actualizez lista cu selectii
            indexIntrebare = 0;
            displaySelections();

            //Refac textul afisat in combobox
            this.BeginInvoke((MethodInvoker)delegate { this.comboBoxCapitol.Text = "Capitole"; });
        }

        private void buttonPrec_Click(object sender, EventArgs e)
        {
            indexIntrebare--;
            if (indexIntrebare < 0)
                indexIntrebare = Intrebari.Rows.Count - 1;

            displaySelections();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            indexIntrebare++;
            if (indexIntrebare > Intrebari.Rows.Count - 1)
                indexIntrebare = 0;

            displaySelections();
        }

        private void buttonSterge_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sigur doresi sa stergi definitiv aseasta intrebare?", "Confirmare", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    Intrebari.Rows[indexIntrebare].Delete();
                    OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(DataAdapter);
                    DataAdapter.Update(Intrebari);

                    //commandBuilder.Dispose();
                    //  DataAdapter.Dispose();
                    //  MessageBox.Show(DBConnection.State.ToString());
                    //  RefreshDBConnection();

                    if (indexIntrebare > 0)
                        indexIntrebare--;
                    displaySelections();
                    MessageBox.Show("Intrebarea a fost stearsa cu succes!", "Stergere reusita");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Eroare la stergere sau la afisare!");
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void buttonModifica_Click(object sender, EventArgs e)
        {
            DBConnection.Close();
            formModifica.Intrebare.Clear();
            formModifica.Intrebare = Intrebari.Clone();
            formModifica.Intrebare.ImportRow(Intrebari.Rows[indexIntrebare]);
            formModifica.Height = BlackMain.mainForm.Height;
            formModifica.Width = BlackMain.mainForm.Width;
            formModifica.ShowDialog();
            ConnectToDatabase();
            displaySelections();
        }
    }
}
