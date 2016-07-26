using System;
using System.IO;
using System.Data.OleDb;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;

namespace VreiSaFiiMilionar
{
    public partial class FormEditor : Form
    {
        //Declaratii baza de date
        OleDbConnection DBConnection = new OleDbConnection(); //Conexiune

        //Declaratii pentru word
        object oMissing = System.Reflection.Missing.Value;
        object oEndOfDoc = "\\endofdoc"; //Un bookmark predefinit
        Word._Application oWord;
        Word._Document oDoc;

        int raspunsCorect = 0;
        
        public FormEditor()
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

        private byte[] ConvertImageToByte(Image InputImage)
        {
            //Transform imaginea intr-un sir de byte, pentru a o putea stoca in baza de date

            //Imaginea ==> Bitmap
            Bitmap BmpImage = new Bitmap(InputImage);

            //Declar o zona de memorie ca sir de byte-uri
            MemoryStream MyStream = new MemoryStream();

            //Copiez imaginea in memoria RAM, cu formatul .png
            BmpImage.Save(MyStream, System.Drawing.Imaging.ImageFormat.Png);

            //Pun in sir informatia(imaginea) din RAM
            byte[] ImageAsByte = MyStream.ToArray();

            //Returnez sirul
            return ImageAsByte;
        }

        private void StoreData()
        {
            //Verific sa fie deschisa conexiunea cu baza de date
            if (DBConnection.State.Equals(ConnectionState.Closed))
                DBConnection.Open();

            try
            {
                //Declar variabile pentru enunt si intrebari, sub forma de siruri de byte
                byte[] imgEnunt = ConvertImageToByte(pictureBoxMainSlot.Image);
                byte[] imgRasp1 = ConvertImageToByte(pictureBox1.Image);
                byte[] imgRasp2 = ConvertImageToByte(pictureBox2.Image);
                byte[] imgRasp3 = ConvertImageToByte(pictureBox3.Image);
                byte[] imgRasp4 = ConvertImageToByte(pictureBox4.Image);

                //Definesc o comanda pentru a obtine ID-urile
                OleDbCommand getIDCommand = new OleDbCommand();
                OleDbDataReader reader;

                getIDCommand.Connection = DBConnection;

                //Initializare cu 1 e doar pentru a preveni o eroare de compilare
                //Obtin ID-ul nivelului
                string IDNivel = "1";
                getIDCommand.CommandText="Select ID from Nivele WHERE Nivel='" + comboBoxNivel.Text + "'";
                reader = getIDCommand.ExecuteReader();
                while (reader.Read())
                    IDNivel = (reader[0]).ToString();
                reader.Close();

                //Obtin ID-ul anului de studiu
                string IDAn = "1";
                getIDCommand.CommandText = "Select ID from AniStudiu WHERE An='" + comboBoxAn.Text + "'";
                reader = getIDCommand.ExecuteReader();
                while (reader.Read())
                    IDAn = (reader[0]).ToString();
                reader.Close();

                //Obtin ID-ul specializarii
                string IDSpecializare = "1";
                getIDCommand.CommandText = "Select ID from Specializari WHERE Specializare='" + comboBoxSpecializare.Text + "'";
                reader = getIDCommand.ExecuteReader();
                while (reader.Read())
                    IDSpecializare = (reader[0]).ToString();
                reader.Close();

                //Obtin ID-ul disciplinei
                string IDDisciplina = "1";
                getIDCommand.CommandText = "Select ID from Discipline WHERE Disciplina='" + comboBoxDisciplina.Text + "'";
                reader = getIDCommand.ExecuteReader();
                while (reader.Read())
                    IDDisciplina = (reader[0]).ToString();
                reader.Close();

                //Obtin ID-ul materiei
                string IDMaterie = "1";
                getIDCommand.CommandText = "Select ID from Materii WHERE Materie='" + comboBoxMaterie.Text + "'";
                reader = getIDCommand.ExecuteReader();
                while (reader.Read())
                    IDMaterie = (reader[0]).ToString();
                reader.Close();

                //Obtin ID-ul capitolului
                string IDCapitol = "1";
                getIDCommand.CommandText = "Select ID from Capitole WHERE Capitol='" + comboBoxCapitol.Text + "'";
                reader = getIDCommand.ExecuteReader();
                while (reader.Read())
                    IDCapitol = (reader[0]).ToString();
                reader.Close();

                //Obtin ID-ul capitolului
                string IDRaspC = "1";
                getIDCommand.CommandText = "Select ID from RaspunsCorect WHERE Raspuns=" + raspunsCorect.ToString();
                reader = getIDCommand.ExecuteReader();
                while (reader.Read())
                    IDRaspC = (reader[0]).ToString();
                reader.Close();

                //Declar si contruesc sirul pentru comanda de inserare inregistrare
                string query = "Insert INTO Intrebari (Nivel,AnStudiu,Specializare,Disciplina,Materie,Capitol,Intrebare,Rasp1,Rasp2,Rasp3,Rasp4,RaspunsCorect) VALUES";
                query+="("+IDNivel+","+IDAn+","+IDSpecializare+","+IDDisciplina+","+IDMaterie+","+IDCapitol+",";
                query += "@ImgEn,@ImgR1,@ImgR2,@ImgR3,@ImgR4,";
                query += IDRaspC + ")";

                //Creez comanda pentru inserare inregistrare noua
                OleDbCommand OledbInsert = new OleDbCommand(query, DBConnection);

                //Adaug parametrii necesari pentru imagini
                OleDbParameter ImgEnParameter = OledbInsert.Parameters.AddWithValue("@Intrebare", SqlDbType.Binary);
                ImgEnParameter.Value = imgEnunt;
                ImgEnParameter.Size = imgEnunt.Length;

                OleDbParameter ImgR1Parameter = OledbInsert.Parameters.AddWithValue("@Rasp1", SqlDbType.Binary);
                ImgR1Parameter.Value = imgRasp1;
                ImgR1Parameter.Size = imgRasp1.Length;

                OleDbParameter ImgR2Parameter = OledbInsert.Parameters.AddWithValue("@Rasp2", SqlDbType.Binary);
                ImgR2Parameter.Value = imgRasp2;
                ImgR2Parameter.Size = imgRasp2.Length;

                OleDbParameter ImgR3Parameter = OledbInsert.Parameters.AddWithValue("@Rasp3", SqlDbType.Binary);
                ImgR3Parameter.Value = imgRasp3;
                ImgR3Parameter.Size = imgRasp3.Length;

                OleDbParameter ImgR4Parameter = OledbInsert.Parameters.AddWithValue("@Rasp4", SqlDbType.Binary);
                ImgR4Parameter.Value = imgRasp4;         
                ImgR4Parameter.Size = imgRasp4.Length;

                //Execut comanda
                OledbInsert.ExecuteNonQuery();
                MessageBox.Show("Intrebarea a fost inregistrata cu succes");
           }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                MessageBox.Show(ex.StackTrace.ToString());
            }
        }

        private bool validateData()
        {
            if (!comboBoxNivel.Items.Contains(comboBoxNivel.Text))
            {
                MessageBox.Show("Nivelul selectat nu se afla in baza de date. Selecteaza un nivel din lista!");
                return false;
            }

            else if(!comboBoxAn.Items.Contains(comboBoxAn.Text))
            {
                MessageBox.Show("Anul de studiu selectat nu se afla in baza de date. Selecteaza un an de studiu din lista sau adauga unul nou dand clic pe butonul '+'");
                return false;
            }

            else if (!comboBoxSpecializare.Items.Contains(comboBoxSpecializare.Text))
            {
                MessageBox.Show("Specializarea selectata nu se afla in baza de date. Selecteaza o specializare din lista sau adauga una noua dand clic pe butonul '+'");
                return false;
            }

            else if (!comboBoxDisciplina.Items.Contains(comboBoxDisciplina.Text))
            {
                MessageBox.Show("Disciplina selectata nu se afla in baza de date. Selecteaza o disciplina din lista sau adauga una noua dand clic pe butonul '+'");
                return false;
            }

            else if (!comboBoxMaterie.Items.Contains(comboBoxMaterie.Text))
            {
                MessageBox.Show("Materia selectata nu se afla in baza de date. Selecteaza o materie din lista sau adauga una noua dand clic pe butonul '+'");
                return false;
            }

            else if (!comboBoxCapitol.Items.Contains(comboBoxCapitol.Text))
            {
                MessageBox.Show("Capitolul selectat nu se afla in baza de date. Selecteaza un capitol din lista sau adauga unul nou dand clic pe butonul '+'");
                return false;
            }

            else if (pictureBoxMainSlot.Image==null)
            {
                MessageBox.Show("Te rog introdu un enunt!");
                return false;
            }

            else if (pictureBox1.Image==null)
            {
                MessageBox.Show("Te rog introdu raspunsul A");
                return false;
            }

            else if (pictureBox2.Image == null)
            {
                MessageBox.Show("Te rog introdu raspunsul B");
                return false;
            }

            else if (pictureBox3.Image == null)
            {
                MessageBox.Show("Te rog introdu raspunsul C");
                return false;
            }

            else if (pictureBox4.Image == null)
            {
                MessageBox.Show("Te rog introdu raspunsul D");
                return false;
            }

            else if (raspunsCorect==0)
            {
                MessageBox.Show("Alege raspunsul corect dand clic pe acesta");
                return false;
            }

            return true;
        }

        private void openWord(int size)
        {
            //Deschid o noua instanta word
            oWord = new Word.Application();
            oWord.Visible = true;

            //Creez un document nou cu valori implicite (oMissing)
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            //Setez dimensiunea textului
            oDoc.Content.Font.Size = size;

            //Ma asigur ca se afiseaza wordul
            oWord.Activate();
            oDoc.Activate();

            oWord.WindowState = Microsoft.Office.Interop.Word.WdWindowState.wdWindowStateMinimize;
            oWord.WindowState = Microsoft.Office.Interop.Word.WdWindowState.wdWindowStateNormal;
            // this.WindowState = FormWindowState.Minimized;
        }

        private void transformAllInWordToImage()
        {
            //selectez tot din document
            oDoc.Content.Select();
            //MessageBox.Show("Content selectat");

            //Schimb culoarea fontului in alb
            oDoc.Content.Font.ColorIndex = Word.WdColorIndex.wdWhite;
            //MessageBox.Show("Culoare schimbata");

            //Bifez optiunea "Bold"
            oDoc.Content.Font.Bold = 1;
            //MessageBox.Show("Bold activat");

            //Copiez totul in clipboard
            oDoc.Content.Copy();
           // MessageBox.Show("Content copiat");

            //Sterg tot continutul documentului deoarece voi face "paste" sub forma de imagine
            oDoc.Content.Delete();
            //MessageBox.Show("Content sters");

            //Definesc un obiect de tipul "EnhancedMetafile" pentru a putea face "paste special"
            object objDataTypeMetafile = Word.WdPasteDataType.wdPasteEnhancedMetafile;
            //MessageBox.Show("obiect definit");

            //Execut actiunea "paste special" totul fiind o imagine acum
            oDoc.Content.PasteSpecial(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref objDataTypeMetafile, ref oMissing, ref oMissing);
            //MessageBox.Show("Paste soecial succes");

            //Salvez documentul
            oDoc.SaveAs(Directory.GetCurrentDirectory() + @"\ecuatie.docx");
            
            //MessageBox.Show("decument salvat");

            //Inchid Word-ul
            oWord.Quit();
           // MessageBox.Show("comabda inchidere");

            //Astept pana cand Word-ul s-a inchis complet (asta e chiar tare :) )
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oWord);
           // MessageBox.Show("code 0");
        }

        private void ExtractImages(string name)
        {
            //Extrag imaginea din document folosind o librarie gratis ce am gasit-o pe internet, numite spire.doc

            //Incarc documentul
            Document document = new Document(Directory.GetCurrentDirectory() + @"\ecuatie.docx");

            //Parcurg fiecare sectiune
            foreach (Section section in document.Sections)
            {
                //Parcurg fiecare paragraf din sectiune
                foreach (Paragraph paragraph in section.Paragraphs)
                {
                    //Parcurg fiecare obiect din paragraf
                    foreach (DocumentObject docObject in paragraph.ChildObjects)
                    {
                        //Daca obiectul este o imagine atunci o extrag
                        if (docObject.DocumentObjectType == DocumentObjectType.Picture)
                        {
                            DocPicture pic = docObject as DocPicture;
                            String imgName = String.Format(Directory.GetCurrentDirectory() + @"\img" + name + ".png");

                               //Salvez imaginea
                               pic.Image.Save(imgName, System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }
                }
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

        private void insertAn()
        {
            if (DBConnection.State.Equals(ConnectionState.Closed))
                DBConnection.Open();

            try
            {
                OleDbCommand insertCommand = new OleDbCommand("Insert INTO AniStudiu (An) VALUES ('" + comboBoxAn.Text + "')", DBConnection);
                insertCommand.ExecuteNonQuery();
                MessageBox.Show("Anul de studiu '" + comboBoxAn.Text + "' a fost inregistrat cu scucces");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nu am putut inregistra noul an de studiu. Incearca din nou");
                MessageBox.Show(ex.Message.ToString());
            }

            RefreshDBConnection();
        }
        
        private void insertSpecializare()
        {
            if (DBConnection.State.Equals(ConnectionState.Closed))
                DBConnection.Open();

            try
            {
                OleDbCommand insertCommand = new OleDbCommand("Insert INTO Specializari (Specializare) VALUES ('" + comboBoxSpecializare.Text + "')", DBConnection);
                insertCommand.ExecuteNonQuery();
                MessageBox.Show("Specializarea '" + comboBoxSpecializare.Text + "' a fost inregistrata cu scucces");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nu am putut inregistra noua specializare. Incearca din nou");
                MessageBox.Show(ex.Message.ToString());
            }

            RefreshDBConnection();
        }

        private void insertDisciplina()
        {
            if (DBConnection.State.Equals(ConnectionState.Closed))
                DBConnection.Open();

            try
            {
                OleDbCommand insertCommand = new OleDbCommand("Insert INTO Discipline (Disciplina) VALUES ('" + comboBoxDisciplina.Text + "')", DBConnection);
                insertCommand.ExecuteNonQuery();
                MessageBox.Show("Disciplina '" + comboBoxDisciplina.Text + "' a fost inregistrata cu scucces");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nu am putut inregistra noua disciplina. Incearca din nou");
                MessageBox.Show(ex.Message.ToString());
            }

            RefreshDBConnection();
        }

        private void insertMaterie()
        {
            if (DBConnection.State.Equals(ConnectionState.Closed))
                DBConnection.Open();

            try
            {
                OleDbCommand insertCommand = new OleDbCommand("Insert INTO Materii (Materie) VALUES ('" + comboBoxMaterie.Text + "')", DBConnection);
                insertCommand.ExecuteNonQuery();
                MessageBox.Show("Materia '" + comboBoxMaterie.Text + "' a fost inregistrata cu scucces");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nu am putut inregistra noua materie. Incearca din nou");
                MessageBox.Show(ex.Message.ToString());
            }

            RefreshDBConnection();
        }

        private void insertCapitol()
        {
            if (DBConnection.State.Equals(ConnectionState.Closed))
                DBConnection.Open();

            try
            {
                OleDbCommand insertCommand = new OleDbCommand("Insert INTO Capitole (Capitol) VALUES ('" + comboBoxCapitol.Text + "')", DBConnection);
                insertCommand.ExecuteNonQuery();
                MessageBox.Show("Capitoloul '" + comboBoxCapitol.Text + "' a fost inregistrat cu scucces");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nu am putut inregistra noul capitol. Incearca din nou");
                MessageBox.Show(ex.Message.ToString());
            }

            RefreshDBConnection();
        }

        private void remarginButton (ref Button button, float procent, float left, float top, float right, float bottom)
        {
            button.Margin = new Padding(
                Convert.ToInt32(left * button.Width),     //left
                Convert.ToInt32(top * button.Height),     //top
                Convert.ToInt32(right * button.Width),    //right
                Convert.ToInt32(bottom * button.Height));//bottom
            button.Font = new Font(FontFamily.GenericSansSerif, procent * this.Height, FontStyle.Bold);
        }

        private void resizeLabel(ref Label label, float procent)
        {
            label.Font = new Font(FontFamily.GenericSansSerif, procent * this.Height, FontStyle.Bold);
        }

        private void resizeCombobox(ref ComboBox combobox, float procent)
        {
            combobox.Font = new Font(FontFamily.GenericSansSerif, procent * this.Height, FontStyle.Bold);
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
            //Redimensionez butonele cu "+"
            remarginButton(ref buttonAddNivel, 0.03F, 0.0F, 0.0F, 0.40F, 0.25F);
            remarginButton(ref buttonAddAn, 0.03F, 0.0F, 0.0F, 0.40F, 0.25F);
            remarginButton(ref buttonAddSpecializare, 0.03F, 0.0F, 0.0F, 0.40F, 0.25F);
            remarginButton(ref buttonAddDisciplina, 0.03F, 0.0F, 0.0F, 0.40F, 0.25F);
            remarginButton(ref buttonAddMaterie, 0.03F, 0.0F, 0.0F, 0.40F, 0.25F);
            remarginButton(ref buttonAddCapitol, 0.03F, 0.0F, 0.0F, 0.40F, 0.25F);

            //Redimensionez butoanele Editare/Salvare enunt
            remarginButton(ref buttonEditEn, 0.023F, 0.01F, 0.25F, 0.0F, 0.25F);
            remarginButton(ref buttonSalvEn, 0.023F, 0.0F, 0.25F, 0.01F, 0.25F);

            //Reimensionez butoanele Ediatre/Salvare raspuns 1
            remarginButton(ref buttonEditR1, 0.02F, 0.01F, 0.20F, 0.0F, 0.01F);
            remarginButton(ref buttonSalvR1, 0.02F, 0.01F, 0.01F, 0.0F, 0.20F);

            //Reimensionez butoanele Ediatre/Salvare raspuns 2
            remarginButton(ref buttonEditR2, 0.02F, 0.0F, 0.20F, 0.01F, 0.01F);
            remarginButton(ref buttonSalvR2, 0.02F, 0.0F, 0.01F, 0.01F, 0.20F);

            //Reimensionez butoanele Ediatre/Salvare raspuns 3
            remarginButton(ref buttonEditR3, 0.02F, 0.01F, 0.20F, 0.0F, 0.01F);
            remarginButton(ref buttonSalvR3, 0.02F, 0.01F, 0.01F, 0.0F, 0.20F);

            //Reimensionez butoanele Ediatre/Salvare raspuns 4
            remarginButton(ref buttonEditR4, 0.02F, 0.0F, 0.20F, 0.01F, 0.01F);
            remarginButton(ref buttonSalvR4, 0.02F, 0.0F, 0.01F, 0.01F, 0.20F);

            //Redimesionez butonul "Salveaza intrebarea"
            remarginButton(ref buttonSalvIntrebarea, 0.028F, 0.17F, 0.1F, 0.01F, 0.1F);

            //Redimensionez etichete
            resizeLabel(ref labelNivel, 0.035F);
            resizeLabel(ref labelAn, 0.035F);
            resizeLabel(ref labelSpecializare, 0.035F);
            resizeLabel(ref labelDisciplina, 0.035F);
            resizeLabel(ref labelMaterie, 0.035F);
            resizeLabel(ref labelCapitol, 0.035F);

            //Redimensionez titlu
            resizeLabel(ref labelTitlu, 0.04F);

            //Redimensionez listele
            resizeCombobox(ref comboBoxNivel, 0.03F);
            resizeCombobox(ref comboBoxAn, 0.03F);
            resizeCombobox(ref comboBoxSpecializare, 0.03F);
            resizeCombobox(ref comboBoxDisciplina, 0.03F);
            resizeCombobox(ref comboBoxMaterie, 0.03F);
            resizeCombobox(ref comboBoxCapitol, 0.03F);

            //Centrez pozele din interiorul casetelor negre
            repaddingPictureBox(ref pictureBoxMainSlot, 0.1F, 0.15F, 0.1F, 0.05F);
            repaddingPictureBox(ref pictureBox1, 0.23F, 0.15F, 0.1F, 0.05F);
            repaddingPictureBox(ref pictureBox2, 0.13F, 0.15F, 0.2F, 0.05F);
            repaddingPictureBox(ref pictureBox3, 0.23F, 0.15F, 0.1F, 0.05F);
            repaddingPictureBox(ref pictureBox4, 0.13F, 0.15F, 0.2F, 0.05F);
        }



        private void FormEditor_Load(object sender, EventArgs e)
        {
            //Redimensionez totul la scara
            dimensionare();

            //Ma conectez la baza de date
            ConnectToDatabase();

            //Actualizez listele
            updateLists();

           // this.WindowState = FormWindowState.Maximized;
            this.Activate();
        }

        private void FormEditor_SizeChanged(object sender, EventArgs e)
        {
            dimensionare();
        }

        private void buttonEditEn_Click(object sender, EventArgs e)
        {
            openWord(18);
        }

        private void buttonSalvEn_Click(object sender, EventArgs e)
        {
            try
            {
                transformAllInWordToImage();

                if (pictureBoxMainSlot.Image != null)
                {
                    pictureBoxMainSlot.Image.Dispose();
                    pictureBoxMainSlot.Image = null;
                }

                ExtractImages("Enunt");
                pictureBoxMainSlot.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\imgEnunt.png");
            }
            catch
            {
                MessageBox.Show("Inainte sa salvezi enuntul, mai intai trebuie sa il editezi apasand butonul 'Editeaza enunt'");
            }
        }

        private void buttonEditR1_Click(object sender, EventArgs e)
        {
            openWord(28);
        }

        private void buttonEditR2_Click(object sender, EventArgs e)
        {
            openWord(28);
        }

        private void buttonEditR3_Click(object sender, EventArgs e)
        {
            openWord(28);
        }

        private void buttonEditR4_Click(object sender, EventArgs e)
        {
            openWord(28);
        }

        private void buttonSalvR1_Click(object sender, EventArgs e)
        {
            try
            {
                transformAllInWordToImage();

                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                }
                
                ExtractImages("Rasp1");
                pictureBox1.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\imgRasp1.png");
            }
            catch
            {
                MessageBox.Show("Inainte sa salvezi raspunsul A, mai intai trebuie sa il editezi apasand butonul 'Editeaza'");
            }  
        }

        private void buttonSalvR2_Click(object sender, EventArgs e)
        {
            try
            {
                transformAllInWordToImage();

                if (pictureBox2.Image != null)
                {
                    pictureBox2.Image.Dispose();
                    pictureBox2.Image = null;
                }

                ExtractImages("Rasp2");
                pictureBox2.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\imgRasp2.png");
            }
            catch
            {
                MessageBox.Show("Inainte sa salvezi raspunsul B, mai intai trebuie sa il editezi apasand butonul 'Editeaza'");
            }
        }

        private void buttonSalvR3_Click(object sender, EventArgs e)
        {
            try
            {
                transformAllInWordToImage();

                if (pictureBox3.Image != null)
                {
                    pictureBox3.Image.Dispose();
                    pictureBox3.Image = null;
                }

                ExtractImages("Rasp3");
                pictureBox3.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\imgRasp3.png");
            }
            catch
            {
                MessageBox.Show("Inainte sa salvezi raspunsul C, mai intai trebuie sa il editezi apasand butonul 'Editeaza'");
            }
        }

        private void buttonSalvR4_Click(object sender, EventArgs e)
        {
            try
            {
                transformAllInWordToImage();

                if (pictureBox4.Image != null)
                {
                    pictureBox4.Image.Dispose();
                    pictureBox4.Image = null;
                }

                ExtractImages("Rasp4");
                pictureBox4.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\imgRasp4.png");
            }
            catch
            {
                MessageBox.Show("Inainte sa salvezi raspunsul D, mai intai trebuie sa il editezi apasand butonul 'Editeaza'");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            raspunsCorect = 1;
            pictureBox1.BackgroundImage = Properties.Resources.slotcorect;
            pictureBox2.BackgroundImage = Properties.Resources.slot2;
            pictureBox3.BackgroundImage = Properties.Resources.slot;
            pictureBox4.BackgroundImage = Properties.Resources.slot2;

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            raspunsCorect = 2;
            pictureBox1.BackgroundImage = Properties.Resources.slot;
            pictureBox2.BackgroundImage = Properties.Resources.slotcorect2;
            pictureBox3.BackgroundImage = Properties.Resources.slot;
            pictureBox4.BackgroundImage = Properties.Resources.slot2;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            raspunsCorect = 3;
            pictureBox1.BackgroundImage = Properties.Resources.slot;
            pictureBox2.BackgroundImage = Properties.Resources.slot2;
            pictureBox3.BackgroundImage = Properties.Resources.slotcorect;
            pictureBox4.BackgroundImage = Properties.Resources.slot2;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            raspunsCorect = 4;
            pictureBox1.BackgroundImage = Properties.Resources.slot;
            pictureBox2.BackgroundImage = Properties.Resources.slot2;
            pictureBox3.BackgroundImage = Properties.Resources.slot;
            pictureBox4.BackgroundImage = Properties.Resources.slotcorect2;
        }

        private void buttonSalvIntrebarea_Click(object sender, EventArgs e)
        {
            if (validateData() == true)
            {
                StoreData();

                if (pictureBoxMainSlot.Image != null)
                {
                    pictureBoxMainSlot.Image.Dispose();
                    pictureBoxMainSlot.Image = null;
                }

                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                }

                if (pictureBox2.Image != null)
                {
                    pictureBox2.Image.Dispose();
                    pictureBox2.Image = null;
                }

                if (pictureBox3.Image != null)
                {
                    pictureBox3.Image.Dispose();
                    pictureBox3.Image = null;
                }
                

                if (pictureBox4.Image != null)
                {
                    pictureBox4.Image.Dispose();
                    pictureBox4.Image = null;
                }
                
                raspunsCorect = 0;

                pictureBox1.BackgroundImage = Properties.Resources.slot;
                pictureBox2.BackgroundImage = Properties.Resources.slot2;
                pictureBox3.BackgroundImage = Properties.Resources.slot;
                pictureBox4.BackgroundImage = Properties.Resources.slot2;
            }
        }

        private void buttonAddNivel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("15 nivele mi se par suficiente, avand in vedere ca in realitate pentru o intrebare de nivelul 15 primesti 1 000 000 $");
        }

        private void buttonAddAn_Click(object sender, EventArgs e)
        {
            if (!comboBoxAn.Items.Contains(comboBoxAn.Text))
            {
                if (!comboBoxAn.Text.Equals(""))
                {
                    insertAn();
                    updateAni();
                }
                else MessageBox.Show("Trebuie sa ii dai un nume noului an de studiu");
            }
            else MessageBox.Show("Acest an de studiu exista deja");
        }

        private void buttonAddSpecializare_Click(object sender, EventArgs e)
        {
            if (!comboBoxSpecializare.Items.Contains(comboBoxSpecializare.Text))
            {
                if (!comboBoxSpecializare.Text.Equals(""))
                {
                    insertSpecializare();
                    updateSpecializari();
                }
                else MessageBox.Show("Trebuie sa ii dai un nume noii specializari");
            }
            else MessageBox.Show("Acesta specializare exista deja");
        }

        private void buttonAddDisciplina_Click(object sender, EventArgs e)
        {
            if (!comboBoxDisciplina.Items.Contains(comboBoxDisciplina.Text))
            {
                if (!comboBoxDisciplina.Text.Equals(""))
                {
                    insertDisciplina();
                    updateDiscipline();
                }
                else MessageBox.Show("Trebuie sa ii dai un nume noii discipline");
            }
            else MessageBox.Show("Acesta disciplina exista deja");
        }

        private void buttonAddMaterie_Click(object sender, EventArgs e)
        {
            if (!comboBoxMaterie.Items.Contains(comboBoxMaterie.Text))
            {
                if (!comboBoxMaterie.Text.Equals(""))
                {
                    insertMaterie();
                    updateMaterii();
                }
                else MessageBox.Show("Trebuie sa ii dai un nume noii meterii");
            }
            else MessageBox.Show("Acesta materie exista deja");
        }

        private void buttonAddCapitol_Click(object sender, EventArgs e)
        {
            if (!comboBoxCapitol.Items.Contains(comboBoxCapitol.Text))
            {
                if (!comboBoxCapitol.Text.Equals(""))
                {
                    insertCapitol();
                    updateCapitole();
                }
                else MessageBox.Show("Trebuie sa ii dai un nume noului capitol");
            }
            else MessageBox.Show("Acest capitol exista deja");
        }

        private void FormEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            DBConnection.Close();
            DialogResult = DialogResult.Cancel;
        }

        private void FormEditor_ResizeBegin(object sender, EventArgs e)
        {
           
        }

        private void FormEditor_ResizeEnd(object sender, EventArgs e)
        {
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tableLayoutPanelMain_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
