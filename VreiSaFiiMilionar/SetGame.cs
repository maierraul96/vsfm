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
    public partial class SetGame : Form
    {
        public static Game gameForm = new Game();
        public DenumiriNivele denumiriNivele = new DenumiriNivele();

        public static OleDbConnection DBConnection = new OleDbConnection(); //Conexiune
        OleDbDataAdapter DataAdapterIDuri; //Definesc un DataAdapter pentru a aduce informatii din baza de date;
        OleDbDataAdapter DataAdapterIntrebari;
        OleDbDataAdapter adapterDenumiri;
        OleDbDataAdapter adapterSelectii;

        DataTable LocalDataTableIDuri = new DataTable();
        DataTable LocalDataTableIntrbari = new DataTable();
        DataTable TabelDenumiri = new DataTable();
        DataTable BufferSelectii = new DataTable();

        ListBox listBoxAn = new ListBox();
        ListBox listBoxSpecializare = new ListBox();
        ListBox listBoxDisciplina = new ListBox();
        ListBox listBoxMaterie = new ListBox();
        ListBox listBoxCapitol = new ListBox();
        
        
        public SetGame()
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

        private void updateBufferSelectii(int adancime)
        {
            string queryAni = makeQueryAni();
            string querySpecializari = makeQuerySpecializari();
            string queryDiscipline = makeQueryDiscipline();
            string queryMaterii = makeQueryMaterii();


            string query = "SELECT Specializare, Disciplina, Materie, Capitol FROM Intrebari";
            string legatura = " WHERE ";

            if (queryAni != "" && adancime >=0) 
            {
                query += legatura + queryAni;
                legatura = "AND";
            }

            if (querySpecializari != "" && adancime>=1)
            {
                query += legatura + querySpecializari;
                legatura = " AND ";
            }

            if (queryDiscipline != "" && adancime>=2)
            {
                query += legatura + queryDiscipline;
                legatura = " AND ";
            }

            if (queryMaterii != "" && adancime>=3)
            {
                query += legatura + queryMaterii;
                legatura = " AND ";
            }

            try
            {
                BufferSelectii.Clear();
                adapterSelectii = new OleDbDataAdapter(query, DBConnection);
                adapterSelectii.Fill(BufferSelectii);
            }
            catch (Exception e)
            {
                MessageBox.Show("Eroare la reactualizarea etichetelor disponibile!");
                MessageBox.Show(e.Message.ToString());
            }

        }

        private void updateListaSpecializari()
        {
            try
            {
                comboBoxSpecializare.Items.Clear();
                for (int i = 0; i < BufferSelectii.Rows.Count; i++)
                {
                    string Specializare = getSpecializareFromID(Convert.ToInt16(BufferSelectii.Rows[i]["Specializare"]));
                    if (!comboBoxSpecializare.Items.Contains(Specializare))
                        comboBoxSpecializare.Items.Add(Specializare);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Eroare la acrualizarea listei cu specializari!");
                MessageBox.Show(e.Message.ToString());
            }
            updateListaDiscipline();
        }

        private void updateListaDiscipline()
        {
            try
            {
                comboBoxDisciplina.Items.Clear();
                for (int i = 0; i < BufferSelectii.Rows.Count; i++)
                {
                    string Disciplina = getDisciplinaFromID(Convert.ToInt16(BufferSelectii.Rows[i]["Disciplina"]));
                    if (!comboBoxDisciplina.Items.Contains(Disciplina))
                        comboBoxDisciplina.Items.Add(Disciplina);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Eroare la acrualizarea listei cu Discipline!");
                MessageBox.Show(e.Message.ToString());
            }
            updateListaMaterii();
        }

        private void updateListaMaterii()
        {
            try
            {
                comboBoxMaterie.Items.Clear();
                for (int i = 0; i < BufferSelectii.Rows.Count; i++)
                {
                    string Materie = getMaterieFromID(Convert.ToInt16(BufferSelectii.Rows[i]["Materie"]));
                    if (!comboBoxMaterie.Items.Contains(Materie))
                        comboBoxMaterie.Items.Add(Materie);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Eroare la acrualizarea listei cu materii!");
                MessageBox.Show(e.Message.ToString());
            }
            updateListaCapitole();
        }

        private void updateListaCapitole()
        {
            try
            {
                comboBoxCapitol.Items.Clear();
                for (int i = 0; i < BufferSelectii.Rows.Count; i++)
                {
                    string Capitol = getCapitolFromID(Convert.ToInt16(BufferSelectii.Rows[i]["Capitol"]));
                    if (!comboBoxCapitol.Items.Contains(Capitol))
                        comboBoxCapitol.Items.Add(Capitol);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Eroare la acrualizarea listei cu capitole!");
                MessageBox.Show(e.Message.ToString());
            }
        }

        
        /* private string makeQuery(string nivel)
         {
             string queryAni = makeQueryAni();
             string querySpecializari = makeQuerySpecializari();
             string queryDiscipline = makeQueryDiscipline();
             string queryMaterii = makeQueryMaterii();
             string queryCapitole = makeQueryCapitole();

             OleDbCommand getIDCommand = new OleDbCommand();
             OleDbDataReader reader;

             getIDCommand.Connection = DBConnection;

             //Initializare cu 1 e doar pentru a preveni o eroare de compilare
             //Obtin ID-ul nivelului
             string IDNivel = "1";
             getIDCommand.CommandText = "Select ID from Nivele WHERE Nivel='" + nivel + "'";
             reader = getIDCommand.ExecuteReader();
             while (reader.Read())
                 IDNivel = (reader[0]).ToString();
             reader.Close();

             string query = "SELECT ID FROM Intrebari WHERE Nivel=" + IDNivel;

             if (queryAni != "")
                 query += " AND " + queryAni;

             if (querySpecializari != "")
                 query += " AND " + querySpecializari;

             if (queryDiscipline != "")
                 query += " AND " + queryDiscipline;

             if (queryMaterii != "")
                 query += " AND " + queryMaterii;

             if (queryCapitole != "")
                 query += " AND " + queryCapitole;

             return query;

         }*/

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
            updateAni();
            updateSpecializari();
            updateDiscipline();
            updateMaterii();
            updateCapitole();
        }

        private bool setLocalData()
        {
            if (DBConnection.State.Equals(ConnectionState.Closed))
                DBConnection.Open();

            Random rnd = new Random();
            int SelectedQS = 0;
            int nivel = 1;
            bool ExistingQuestions = true;

            LocalDataTableIDuri.Clear();
            LocalDataTableIntrbari.Clear();

            string qurySpecific = "";
            gameForm.querylist.Clear();

            while (nivel <= 15 & ExistingQuestions == true)
            {
                qurySpecific = makeQuery("Nivel" + nivel.ToString());
                DataAdapterIDuri = new OleDbDataAdapter(qurySpecific, DBConnection);
                gameForm.querylist.Add(qurySpecific);
                DataAdapterIDuri.Fill(LocalDataTableIDuri);

                if (LocalDataTableIDuri.Rows.Count == 0)
                {
                    ExistingQuestions = false;
                    LocalDataTableIDuri.Clear();
                    LocalDataTableIntrbari.Clear();
                    MessageBox.Show("Nu sunt suficiente intrebari pentru etichetele selectate. Nu am putut gasi nicio intrebare de nivel " + nivel.ToString() + " care sa indeplineasca toate cerintele");
                }
                else
                {
                    SelectedQS = rnd.Next(0, LocalDataTableIDuri.Rows.Count);
                    DataAdapterIntrebari = new OleDbDataAdapter("Select ID,Intrebare,Rasp1,Rasp2,Rasp3,Rasp4,RaspunsCorect FROM Intrebari WHERE ID=" + LocalDataTableIDuri.Rows[SelectedQS]["ID"].ToString(), DBConnection);
                    DataAdapterIntrebari.Fill(LocalDataTableIntrbari);
                }
                LocalDataTableIDuri.Clear();
                nivel++;
            }
            if (ExistingQuestions == true)
                return true;
            else
                return false;
        }

        private bool setDenumiriNivele()
        {
            if (DBConnection.State.Equals(ConnectionState.Closed))
                DBConnection.Open();

            try
            {
                TabelDenumiri.Clear();

                adapterDenumiri = new OleDbDataAdapter("SELECT * FROM DenumiriNivele", DBConnection);
                adapterDenumiri.Fill(TabelDenumiri);
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Problema cu denumirile nivelelor");
                MessageBox.Show(ex.ToString());
                return false;
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

        private void resizeListBox(ref ListBox listbox, float procent)
        {
            listbox.Font = new Font(FontFamily.GenericSansSerif, procent * this.Height, FontStyle.Bold);
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
            //Redimensionez casetele combo
            resizeCombobox(ref comboBoxAn, 0.022F);
            resizeCombobox(ref comboBoxSpecializare, 0.022F);
            resizeCombobox(ref comboBoxDisciplina, 0.022F);
            resizeCombobox(ref comboBoxMaterie, 0.022F);
            resizeCombobox(ref comboBoxCapitol, 0.022F);

            //Redimensionez comboboxul pentru timp
            resizeCombobox(ref comboBoxTimp, 0.022F);

            //Redimensionex butnonul pentru denumirea nivelelelor
            resizeButton(ref buttonDenumiriNivele, 0.022F);

            //Redimesnionez titlul
            resizeLabel(ref labelTitlu, 0.025F);

            //Redimensinez subtitlurile
            resizeLabel(ref labelEtichete, 0.015F);
            resizeLabel(ref labelSelectii, 0.0155F);
            resizeLabel(ref labelNume, 0.022F);
            resizeLabel(ref labelClasa, 0.022F);
            

            //Redimensionez caseta de selectii
            resizeTextBox(ref textBoxEtichete, 0.017F);
            resizeTextBox(ref textBoxNume, 0.022F);
            resizeTextBox(ref textBoxClasa, 0.022F);
            

            //Redimensionez butonul de start
            remarginPictureBox(ref pictureBox1, 0.0F, 0.05F, 0.0F, 0.05F);
            repaddingPictureBox(ref pictureBox1, 0.0F, 0.25F, 0.0F, 0.01F);
        }

        private bool checkTime()
        {
            if (comboBoxTimp.Text != "Fara timp" || comboBoxTimp.SelectedIndex != 0)
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


        private string getNivelFromID(int ID)
        {
            OleDbCommand getTextCommand = new OleDbCommand();
            OleDbDataReader reader;

            getTextCommand.Connection = DBConnection;

            string Text = "";
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


        private string makeQueryAni()
        {
            //Verific cate etichete pentru ani sunt
            if (listBoxAn.Items.Count > 0)
            { 
                //Construiesc queryul pentru condiitile de ani de studiu
                string query = "(";
                for (int i = 0; i <= listBoxAn.Items.Count-1; i++)
                {
                    query += "AnStudiu=" + getIDAn(listBoxAn.Items[i].ToString());
                    if (i != listBoxAn.Items.Count-1)
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
                for (int i = 0; i <= listBoxSpecializare.Items.Count-1; i++)
                {
                    query += "Specializare=" + getIDSpecializare(listBoxSpecializare.Items[i].ToString());
                    if (i != listBoxSpecializare.Items.Count-1)
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
                for (int i = 0; i <= listBoxDisciplina.Items.Count-1; i++)
                {
                    query += "Disciplina=" + getIDDisciplina(listBoxDisciplina.Items[i].ToString());
                    if (i != listBoxDisciplina.Items.Count-1)
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
                for (int i = 0; i <= listBoxMaterie.Items.Count-1; i++)
                {
                    query += "Materie=" + getIDMaterie(listBoxMaterie.Items[i].ToString());
                    if (i != listBoxMaterie.Items.Count-1)
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
                for (int i = 0; i <= listBoxCapitol.Items.Count-1; i++)
                {
                    query += "Capitol=" + getIDCapitol(listBoxCapitol.Items[i].ToString());
                    if (i != listBoxCapitol.Items.Count-1)
                        query += " OR ";
                }
                query += ")";
                return query;
            }
            else return "";
        }

        private string makeQuery(string nivel)
        {
            string queryAni = makeQueryAni();
            string querySpecializari = makeQuerySpecializari();
            string queryDiscipline = makeQueryDiscipline();
            string queryMaterii = makeQueryMaterii();
            string queryCapitole = makeQueryCapitole();

            OleDbCommand getIDCommand = new OleDbCommand();
            OleDbDataReader reader;

            getIDCommand.Connection = DBConnection;

            //Initializare cu 1 e doar pentru a preveni o eroare de compilare
            //Obtin ID-ul nivelului
            string IDNivel = "1";
            getIDCommand.CommandText = "Select ID from Nivele WHERE Nivel='" + nivel + "'";
            reader = getIDCommand.ExecuteReader();
            while (reader.Read())
                IDNivel = (reader[0]).ToString();
            reader.Close();

            string query = "SELECT ID FROM Intrebari WHERE Nivel="+IDNivel;

            if (queryAni != "")
                query += " AND " + queryAni;

            if (querySpecializari != "")
                query += " AND " + querySpecializari;

            if (queryDiscipline != "")
                query += " AND " + queryDiscipline;

            if (queryMaterii != "")
                query += " AND " + queryMaterii;

            if (queryCapitole != "")
                query += " AND " + queryCapitole;

            return query;

        }

        private void displaySelections()
        {
            //Sterg tot ce este afisat
            textBoxEtichete.Clear();

            //Afisez anii de studiu selectati daca exista
            if (listBoxAn.Items.Count>0)
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



        private void SetGame_Load(object sender, EventArgs e)
        {
            dimensionare();
            ConnectToDatabase();
            updateLists();
        }

        private void buttonAddAn_Click(object sender, EventArgs e)
        {
            if (comboBoxAn.Items.Contains(comboBoxAn.Text) & !listBoxAn.Items.Contains(comboBoxAn.Text))
                listBoxAn.Items.Add(comboBoxAn.Text);
        }

        private void buttonAddSpecializare_Click(object sender, EventArgs e)
        {
            if (comboBoxSpecializare.Items.Contains(comboBoxSpecializare.Text) & !listBoxSpecializare.Items.Contains(comboBoxSpecializare.Text))
                listBoxSpecializare.Items.Add(comboBoxSpecializare.Text);
        }

        private void buttonAddDisciplina_Click(object sender, EventArgs e)
        {
            if (comboBoxDisciplina.Items.Contains(comboBoxDisciplina.Text) & !listBoxDisciplina.Items.Contains(comboBoxDisciplina.Text))
                listBoxDisciplina.Items.Add(comboBoxDisciplina.Text);
        }

        private void buttonAddMaterie_Click(object sender, EventArgs e)
        {
            if (comboBoxMaterie.Items.Contains(comboBoxMaterie.Text) & !listBoxMaterie.Items.Contains(comboBoxMaterie.Text))
                listBoxMaterie.Items.Add(comboBoxMaterie.Text);
        }

        private void buttonAddCapitol_Click(object sender, EventArgs e)
        {
            if (comboBoxCapitol.Items.Contains(comboBoxCapitol.Text) & !listBoxCapitol.Items.Contains(comboBoxCapitol.Text))
                listBoxCapitol.Items.Add(comboBoxCapitol.Text);
        }

        private void SetGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            DBConnection.Close();
            listBoxAn.Items.Clear();
            listBoxSpecializare.Items.Clear();
            listBoxDisciplina.Items.Clear();
            listBoxMaterie.Items.Clear();
            listBoxCapitol.Items.Clear();
            textBoxEtichete.Clear();
            textBoxClasa.Clear();
            textBoxNume.Clear();
            comboBoxTimp.SelectedItem = null;
            comboBoxTimp.Text = "Timp (secunde)";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (setLocalData() == true && setDenumiriNivele() == true && checkTime() == true)
            {
                gameForm.GameTable = this.LocalDataTableIntrbari;
                gameForm.DenumiriNivele = this.TabelDenumiri;

                if (comboBoxTimp.Text == "Fara timp" || comboBoxTimp.SelectedIndex == 0)
                    gameForm.timpIntrebare = -1;
                else
                    gameForm.timpIntrebare = Convert.ToInt32(comboBoxTimp.Text);
                
                Main.player.Stop();
                gameForm.Height = BlackMain.mainForm.Height;
                gameForm.Width = BlackMain.mainForm.Width;
                gameForm.ShowDialog();
                Main.player.PlayLooping();
                this.Close();
            }
        }

        private void pictureBox1_MouseEnter_1(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Properties.Resources.selected;
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Properties.Resources.mainslot;
            pictureBox1.Refresh();
        }

        private void comboBoxAn_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Adaug selectia in lista
            if (!listBoxAn.Items.Contains(comboBoxAn.SelectedItem))
                listBoxAn.Items.Add(comboBoxAn.SelectedItem.ToString());
            else
                listBoxAn.Items.Remove(comboBoxAn.SelectedItem.ToString());

            //Actualizez lista cu selectii
            displaySelections();
            updateBufferSelectii(0);
            updateListaSpecializari();

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
            displaySelections();
            updateBufferSelectii(1);
            updateListaDiscipline();

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
            displaySelections();
            updateBufferSelectii(2);
            updateListaMaterii();

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
            displaySelections();
            updateBufferSelectii(3);
            updateListaCapitole();

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

            //Refac textul afisat in combobox
            this.BeginInvoke((MethodInvoker)delegate { this.comboBoxCapitol.Text = "Capitole"; });

            //Actualizez lista cu selectii
            displaySelections();
        }

        private void buttonDenumiriNivele_MouseEnter(object sender, EventArgs e)
        {
            buttonDenumiriNivele.BackgroundImage = Properties.Resources.selected;
            buttonDenumiriNivele.Refresh();
        }

        private void buttonDenumiriNivele_MouseLeave(object sender, EventArgs e)
        {
            buttonDenumiriNivele.BackgroundImage = Properties.Resources.mainslot;
            buttonDenumiriNivele.Refresh();
        }

        private void buttonDenumiriNivele_Click(object sender, EventArgs e)
        {
            denumiriNivele.ShowDialog();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetGame_SizeChanged(object sender, EventArgs e)
        {
            dimensionare();
        }
    }
}
