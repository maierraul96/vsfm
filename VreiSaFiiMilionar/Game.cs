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
using System.Media;

namespace VreiSaFiiMilionar
{
    public partial class Game : Form
    {
        public static GameResult gameResult = new GameResult(); //Afiseaza rezultatul jocului curent
        Parola formPassword = new Parola(); //Pentru introducerea parolei de schimbare a intrbarii.

        OleDbConnection DBConnection = new OleDbConnection(); //Conexiunea cu baza de date

        public DataTable GameTable = new DataTable(); //Retine toate intrebarile selectate (15) pentru jocul curent
        public DataTable DenumiriNivele = new DataTable(); //Retin denumirile nivelelor
        DataTable NewQuestion = new DataTable(); //Folosit pentru a aduce o intrebare noua din baza de date
        DataTable IntrebariPotrivite = new DataTable(); //Toate intrbarile potrivite unui query(nivel)
                                                        //Din care se selecteaza una singura in (NewQuestion)

        OleDbDataAdapter DataAdapterIDuri; //Folosit in corelatie cu (IntrebariPotrivite)
        OleDbDataAdapter DataAdapterIntrebare; //Folosit in corelatie cu (NewQuestion)

        int SelectedQS = 0; //Intrebarea selectata aleatoriu din (IntrebariPotrivite)
        int idPrec = 0;     //ID-ul intrebarii initiale pentru a se incerca sa nu se repete. (Daca se poate)
        Random rnd = new Random();

        public List<string> querylist = new List<string>();  //Lista cu query-uri pentru fiecare nivel, in caz ca
                                                             //Trebuie schimbata intrebarea.

        
        public static int NivelAcutal = 0; //Nivelul la care ma aflu
        public static int nivelRezultat = 0; //Nivelul rezultat

        int nivelCall = 0; //Nivelul la care se suna un prieten

        public int timpIntrebare; //Timpul alocat fiecarei intrebari
        int timp = 60; //Timpul afisat efectiv pe ecran.
        public static int timpApel = 60; //Timpul alocat apelului.

        public static DateTime timpStart;    //Folosite pentru a masura durata
        public static DateTime timpSfarsit;  //unui joc.

        bool auto = false; //Modul auto. (explicat in documentatie)
        bool waitingForInput = false;  //Determina daca se asteapta un input de la utiliztor sau nu.

        string info; //Afiseaza mereu informatii despe joc. (Ex: nivel, actiuni...)

        List<Image> FlashuriNext = new List<Image>(); //contine 2 imagini pentru a anima butonul "Mai departe (>>))"
        int idFlash = 0; //Id-ul pozei din lista FlashuriNext.

        public static bool cashOut = false; //Daca ai utilizatorul renunta in timpul jocului.

        bool selectat = false; //Daca un raspuns a fost selectat sau nu

        SoundPlayer player = new SoundPlayer(); //Definesc playerul muzical;

        
        List<PictureBox> Raspunsuri = new List<PictureBox>(); //Lista cu casetele de raspuns.
        public static List<Button> Butoane = new List<Button>(); //Lista cu DENUMIRILE NIVELELOR.

        Image backgroundRaspuns; //Folosesc pentru animatia de flash, dupa afisarea raspunsului corect.
        int nrFlashuri = 0; //Numarul de flash-uri pentru animatia de raspuns.

        int raspunsSelectat = 0; //Raspunsul selectat de utilizator.
        public static int raspunsCorect = 0; //Raspunsul corect aferent intrebarii.

        bool stareNivele = false; //Fals = nivele inchise .... True = nivele deschise

        
        public Game()
        {
            InitializeComponent();
        }

        private void ConnectToDatabase()
        {
            DBConnection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=DatabaseMilionar.mdb";
            DBConnection.Open();
        }

        private void resetValues()
        {
            //Resetaeaza valorile intitaile
            NivelAcutal = 0;
            nivelCall = 0;
            cashOut = false;
            nrFlashuri = 0;
            selectat = false;
            timerSuspans.Interval = 4000;
            labelTimp.Text = null;
            stareNivele = false;

            foreach (Button buton in Butoane)
                buton.BackgroundImage = null;

            dbLayoutPanel1.ColumnStyles[4].Width = 0;

            buttonFifty.BackgroundImage = Properties.Resources.FiftyFifty;
            buttonCall.BackgroundImage = Properties.Resources.Call;
            buttonAsk.BackgroundImage = Properties.Resources.Ask;

            buttonFifty.Enabled = true;
            buttonCall.Enabled = true;
            buttonAsk.Enabled = true;

            player.Stop();
        }

        private void stopTimers()
        {
            //Opresc timerele
            timerSuspans.Stop();
            timerOpenNivele.Stop();
            timerFlash.Stop();
            timerLetsPlay.Stop();
            timerCloseNivele.Stop();
            timerBefore.Stop();
            timerInTime.Stop();
            timerAfter.Stop();
            timerTimp.Stop();
        }


        private Image getImage(string denumireCamp)
        {
            //Declar imaginea
            Image FetchedImg;

            //Transform sirul de btiti
            byte[] FetchedBytes = (byte[])GameTable.Rows[NivelAcutal - 1][denumireCamp];

            //Inserez imaginea in RAM
            MemoryStream stream = new MemoryStream(FetchedBytes);

            //Convertesc sirul de biti in imagine
            FetchedImg = Image.FromStream(stream);

            //Returnez imaginea
            return FetchedImg;
        }

        private Image getImageChangeQS(string denumireCamp)
        {
            //Declar imaginea
            Image FetchedImg;

            //Transform sirul de btiti
            byte[] FetchedBytes = (byte[])NewQuestion.Rows[0][denumireCamp];

            //Inserez imaginea in RAM
            MemoryStream stream = new MemoryStream(FetchedBytes);

            //Convertesc sirul de biti in imagine
            FetchedImg = Image.FromStream(stream);

            //Returnez imaginea
            return FetchedImg;
        }

        private void updateImages()
        {
            //Inserez imaginile
            pictureBoxMainSlot.Image = getImage("Intrebare");
            pictureBox1.Image = getImage("Rasp1");
            pictureBox2.Image = getImage("Rasp2");
            pictureBox3.Image = getImage("Rasp3");
            pictureBox4.Image = getImage("Rasp4");

            pictureBox1.BackgroundImage = Properties.Resources.slot;
            pictureBox2.BackgroundImage = Properties.Resources.slot2;
            pictureBox3.BackgroundImage = Properties.Resources.slot;
            pictureBox4.BackgroundImage = Properties.Resources.slot2;
        }


        private void urmatoareaIntrebare() //Toate demersurile necesare pentru o noua intrebare
        {
            if (NivelAcutal == 0)
                timpStart = DateTime.Now;


            NivelAcutal++;
            selectat = false;
            raspunsSelectat = 0;
            player.SoundLocation = @"Sounds\LetsPlay\Niv" + NivelAcutal + "LP.wav";
            player.Play();
            timerLetsPlay.Start();
            updateImages();
            timerSuspans.Interval += 150;
            idPrec = 0;

            labelInfo.Text = "Intrebare pentru " + Butoane[NivelAcutal - 1].Text;
            info = "Intrebare pentru " + Butoane[NivelAcutal - 1].Text;

            if (NivelAcutal == 11)
            {
                buttonChange.Enabled = true;
                buttonChange.BackgroundImage = Properties.Resources.Changeqs;
            }

            if (timpIntrebare >= 0)
            {
                timp = timpIntrebare;
                timerTimp.Start();
            }
            //Actualizez raspunsul corect
            raspunsCorect = Convert.ToInt32(GameTable.Rows[NivelAcutal - 1]["RaspunsCorect"]);
        }

        private bool changeQuestion() //Returneaza true daca s-a efectuat schmibarea
        {
            //Aduc intrebarile (ID-urile) care se potrivesc cerintelor. Nevel, selectii, etc.
            DataAdapterIDuri = new OleDbDataAdapter(querylist[NivelAcutal - 1], DBConnection);
            IntrebariPotrivite.Clear();
            DataAdapterIDuri.Fill(IntrebariPotrivite);

            //Daca singura intrebare e cea care deja e afisata
            if (IntrebariPotrivite.Rows.Count<=1)
            {
                MessageBox.Show("Imi pare rau dar nu mai exista alta intrebare care sa corespunda cerintelor!");
                return false;
            }
            else
            {
                ///selectez o noua intrebare. Daca se poate, diferita.

                if (IntrebariPotrivite.Rows.Count <= 2)
                    idPrec = 0;

                SelectedQS = rnd.Next(0, IntrebariPotrivite.Rows.Count);
                while (IntrebariPotrivite.Rows[SelectedQS]["ID"] == GameTable.Rows[NivelAcutal - 1]["ID"] || Convert.ToInt32(IntrebariPotrivite.Rows[SelectedQS]["ID"])==idPrec) 
                {
                    SelectedQS = rnd.Next(0, IntrebariPotrivite.Rows.Count);
                }
                idPrec = Convert.ToInt32(IntrebariPotrivite.Rows[SelectedQS]["ID"]);

                DataAdapterIntrebare = new OleDbDataAdapter("Select ID,Intrebare,Rasp1,Rasp2,Rasp3,Rasp4,RaspunsCorect FROM Intrebari WHERE ID=" + IntrebariPotrivite.Rows[SelectedQS]["ID"].ToString(), DBConnection);
                NewQuestion.Clear();
                DataAdapterIntrebare.Fill(NewQuestion);

                //Inserez imaginile
                pictureBoxMainSlot.Image = getImageChangeQS("Intrebare");
                pictureBox1.Image = getImageChangeQS("Rasp1");
                pictureBox2.Image = getImageChangeQS("Rasp2");
                pictureBox3.Image = getImageChangeQS("Rasp3");
                pictureBox4.Image = getImageChangeQS("Rasp4");

                //Actualizez raspunsul corect
                raspunsCorect = Convert.ToInt32(NewQuestion.Rows[0]["RaspunsCorect"]);

                if (timpIntrebare >= 0)
                {
                    timp = timpIntrebare;
                    timerTimp.Start();
                }

                return true;
            }

        }

        private void cliked() //Se apeleaza cand se alege un raspuns
            //Blocheaza sa nu se mai poata schimba si face demersurile necesare
        {
            selectat = true;
            player.SoundLocation = @"Sounds\FinalAnswer\Niv" + NivelAcutal + "FA.wav";
            player.Play();

            timerTimp.Stop();
            timerApel.Stop();
            labelTimp.Text = null;

            backgroundRaspuns = Raspunsuri[raspunsCorect - 1].BackgroundImage; //Se salveaza 'background-ul'
                                                            //raspunsului corect pentru a se efectua animatia.

            timerSuspans.Start(); //Timpul de suspans.
        }

        private int ultimulPrag() //Determina care este nivelul la care concurentul a parasit jocul.
        {
            if (cashOut == true)
                return NivelAcutal - 1;
            else
            {
                if (NivelAcutal > 15)
                    return 15;
                else if (NivelAcutal > 10)
                    return 10;
                else if (NivelAcutal > 5)
                    return 5;
                else return 0;
            }
        }

        private void showResult() //Porneste Fromul cu rezultaul obtinut.
        {
            stopTimers(); //Se opresc toate timerele.
            player.Stop();
            nivelRezultat = ultimulPrag(); //Se memoreaza nivelul la care s-a ajuns sau ultimul prag.

            gameResult.Height = BlackMain.mainForm.Height;
            gameResult.Width = BlackMain.mainForm.Width;
            gameResult.ShowDialog();
            this.Close();
        }



        private void repaddigPictureBox(ref PictureBox PB, float left, float top, float right, float bottom)
        //Modifica "Padingul" pictureboxului primit ca parametru.
        {
            PB.Padding = new Padding(
                Convert.ToInt32(left * PB.Width),
                Convert.ToInt32(top * PB.Height),
                Convert.ToInt32(right * PB.Width),
                Convert.ToInt32(bottom * PB.Height));
        }

        private void resizeButton(ref Button button, float procent)
        //Schimba dimensiunea fontului butonului primit ca parametru
        {
            button.Font = new Font(FontFamily.GenericSansSerif, procent * this.Height, FontStyle.Bold);
        }

        private void resizeLabel(ref Label label, float procent)
        //Schimba dimensiunea fontului labelului primit ca parametru
        {
            label.Font = new Font(FontFamily.GenericSansSerif, procent * this.Height, FontStyle.Bold);
        }

        private void dimensionare()
        //Realizeaza toate redimensionarile necesare, in raport cu inaltimea ecranului, pentru a pastra aspectul
        {
            dimensionareCasete();

            //Redimensionez
            resizeButton(ref button1, 0.02F);
            resizeButton(ref button2, 0.02F);
            resizeButton(ref button3, 0.02F);
            resizeButton(ref button4, 0.02F);
            resizeButton(ref button5, 0.02F);
            resizeButton(ref button6, 0.02F);
            resizeButton(ref button7, 0.02F);
            resizeButton(ref button8, 0.02F);
            resizeButton(ref button9, 0.02F);
            resizeButton(ref button10, 0.02F);
            resizeButton(ref button11, 0.02F);
            resizeButton(ref button12, 0.02F);
            resizeButton(ref button13, 0.02F);
            resizeButton(ref button14, 0.02F);
            resizeButton(ref button15, 0.02F);

            resizeButton(ref buttonOpenNivele, 0.015F);
            resizeLabel(ref labelTimp, 0.035F);
            resizeLabel(ref labelInfo, 0.02F);
        }

        private void dimensionareCasete()
        //Redmiensioneaza casetele de rasounsuri si de enunt.
        {
            //Redimensinez
            repaddigPictureBox(ref pictureBoxMainSlot, 0.1F, 0.2F, 0.1F, 0.01F);
            repaddigPictureBox(ref pictureBox1, 0.25F, 0.2F, 0.2F, 0.01F);
            repaddigPictureBox(ref pictureBox2, 0.15F, 0.2F, 0.25F, 0.01F);
            repaddigPictureBox(ref pictureBox3, 0.25F, 0.2F, 0.2F, 0.01F);
            repaddigPictureBox(ref pictureBox4, 0.15F, 0.2F, 0.25F, 0.01F);
        }



        private void Game_Load(object sender, EventArgs e)
        {
            ConnectToDatabase();

            //Creez o lista cu catetele(picturebox-uri) de raspuns
            Raspunsuri.Clear();
            Raspunsuri.Add(pictureBox1);
            Raspunsuri.Add(pictureBox2);
            Raspunsuri.Add(pictureBox3);
            Raspunsuri.Add(pictureBox4);

            //Creez lista cu butoanele (nivelele)
            Butoane.Clear();
            Butoane.Add(button1);
            Butoane.Add(button2);
            Butoane.Add(button3);
            Butoane.Add(button4);
            Butoane.Add(button5);
            Butoane.Add(button6);
            Butoane.Add(button7);
            Butoane.Add(button8);
            Butoane.Add(button9);
            Butoane.Add(button10);
            Butoane.Add(button11);
            Butoane.Add(button12);
            Butoane.Add(button13);
            Butoane.Add(button14);
            Butoane.Add(button15);

            //Actualizez denumirile nivelelor
            for (int i = 0; i <= Butoane.Count - 1; i++)
                Butoane[i].Text = DenumiriNivele.Rows[Butoane.Count-1 - i][1].ToString();

            //Lista pentru animatia de "Flash"
            FlashuriNext.Clear();
            FlashuriNext.Add(Properties.Resources.Next1);
            FlashuriNext.Add(Properties.Resources.Next2);

            //Redimensionez controalele
            dimensionare();

            //Incarc imaginile
            urmatoareaIntrebare();
        }

        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopTimers();
            resetValues();
            DBConnection.Close();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (selectat == false)
            {
                pictureBox1.BackgroundImage = Properties.Resources.slotactive;
                pictureBox1.Refresh();
            }
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            if (selectat == false)
            {
                pictureBox3.BackgroundImage = Properties.Resources.slotactive;
                pictureBox3.Refresh();
            }
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            if (selectat == false)
            {
                pictureBox2.BackgroundImage = Properties.Resources.slotactive2;
                pictureBox2.Refresh();
            }
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            if (selectat == false)
            {
                pictureBox4.BackgroundImage = Properties.Resources.slotactive2;
                pictureBox4.Refresh();
            }
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            if (selectat == false)
            {
                pictureBox1.BackgroundImage = Properties.Resources.slot;
                pictureBox1.Refresh();
            }
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            if (selectat == false)
            {
                pictureBox3.BackgroundImage = Properties.Resources.slot;
                pictureBox3.Refresh();
            }
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            if (selectat == false)
            {
                pictureBox2.BackgroundImage = Properties.Resources.slot2;
                pictureBox2.Refresh();
            }
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            if (selectat == false)
            {
                pictureBox4.BackgroundImage = Properties.Resources.slot2;
                pictureBox4.Refresh();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (selectat == false && pictureBox1.Image != null)
            {
                raspunsSelectat = 1;
                cliked();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (selectat == false && pictureBox3.Image != null)
            {
                raspunsSelectat = 3;
                cliked();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (selectat == false && pictureBox2.Image != null)
            {
                raspunsSelectat = 2;
                cliked();
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (selectat == false && pictureBox4.Image != null)
            {
                raspunsSelectat = 4;
                cliked();
            }
        }



        private void buttonFifty_Click(object sender, EventArgs e)
        {
            //Aleg 2 raspunsuri GRESITE pentru a le elimina
            int a = raspunsCorect;
            int b = raspunsCorect;

            Random rnd = new Random();

            while (a == raspunsCorect)
                a = rnd.Next(1, 5);

            while (b == raspunsCorect || b == a)
                b = rnd.Next(1, 5);

            buttonFifty.BackgroundImage = Properties.Resources.FiftyFiftyX;
            buttonFifty.Enabled = false;

            Raspunsuri[a - 1].Image = null;
            Raspunsuri[b - 1].Image = null;
        }

        private void buttonCall_Click(object sender, EventArgs e)
        {
            nivelCall = NivelAcutal;
            timerTimp.Stop();
            labelTimp.Text = "";

            buttonCall.BackgroundImage = Properties.Resources.CallX;
            buttonCall.Enabled = false;

            TimpApel formTimpApel = new TimpApel();
            formTimpApel.ShowDialog();

            if (timpApel > 0)
            {
                labelTimp.Text = "Apel: " + timpApel.ToString();
                timerApel.Start();
            }
            else
                labelTimp.Text = "Apel: Nelimitat";

            player.SoundLocation = @"Sounds\Phone\TicTac.wav";
            player.PlayLooping();

        }

        private void buttonAsk_Click(object sender, EventArgs e)
        {
            buttonAsk.BackgroundImage = Properties.Resources.AskX;
            buttonAsk.Enabled = false;

            timerTimp.Stop();
            labelTimp.Text = null;
        }

        private void buttonCashOut_Click(object sender, EventArgs e)
        {
            if (selectat == false)
            {
                timpSfarsit = DateTime.Now;
                cashOut = true;
                showResult();
            }
        }




        private void timerLetsPlay_Tick(object sender, EventArgs e)
        {
            timerLetsPlay.Stop();
            if (selectat == false && nivelCall != NivelAcutal)
            {
                player.SoundLocation = @"Sounds\Question\Niv" + NivelAcutal + "QS.wav";
                player.PlayLooping();
            }
        } //Muzica lats' play

        private void timerSuspans_Tick(object sender, EventArgs e) //Dupa selectie
        {
            timerSuspans.Stop();

            if (raspunsCorect==raspunsSelectat)
            {
                player.SoundLocation = @"Sounds\Win\Niv" + NivelAcutal + "WN.wav";
                player.Play();
            }
            else
            {
                player.SoundLocation = @"Sounds\Lose\Niv" + NivelAcutal + "LS.wav";
                player.Play();
            }

            timerFlash.Start();
        }


        private void timerFlash_Tick(object sender, EventArgs e)
        {
            if (nrFlashuri % 2 == 0)
            {
                if (raspunsCorect == 1 || raspunsCorect == 3)
                {
                    Raspunsuri[raspunsCorect - 1].BackgroundImage = Properties.Resources.slotcorect;
                    Raspunsuri[raspunsCorect - 1].Refresh();
                }
                else
                {
                    Raspunsuri[raspunsCorect - 1].BackgroundImage = Properties.Resources.slotcorect2;
                    Raspunsuri[raspunsCorect - 1].Refresh();
                }
            }
            else
            {
                Raspunsuri[raspunsCorect - 1].BackgroundImage = backgroundRaspuns;
                Raspunsuri[raspunsCorect - 1].Refresh();
            }

            nrFlashuri++;

            int pragFlashuri = 20;
            if (NivelAcutal == 15 && raspunsCorect==raspunsSelectat) pragFlashuri = 60;

            if (nrFlashuri >= pragFlashuri)
            {
                timerFlash.Stop();
                nrFlashuri = 0;
                if (auto==true)
                    checkAnswer();
                else
                {
                    timerFlashForInput.Start();
                    labelInfo.Text = "Click pentru a trece mai departe";
                    waitingForInput = true;
                }
            }
        } //Flashuri

        private void checkAnswer()
        {
            waitingForInput = false;
            if (raspunsSelectat == raspunsCorect)
            {
                if (NivelAcutal < 15)
                {
                    if (stareNivele == false)
                        timerOpenNivele.Start();
                    else
                        timerBefore.Start();

                    //selectat = false;
                }
                else
                {
                    timpSfarsit = DateTime.Now;
                    NivelAcutal++;
                    showResult();
                    this.Close();
                }
            }
            else
            {
                timpSfarsit = DateTime.Now;
                showResult();
                this.Close();
            }
        }

        private void buttonOpenNivele_Click(object sender, EventArgs e)
        {
            //stareNivele = !stareNivele;

            if (stareNivele == false)
            {
                stareNivele = true;
                timerOpenNivele.Start();
            }
            else
                timerCloseNivele.Start();

        } //Butonul inchide/deschide lista

        private void timerOpenNivele_Tick(object sender, EventArgs e)
        {
            if (dbLayoutPanel1.ColumnStyles[4].Width < 30)
                dbLayoutPanel1.ColumnStyles[4].Width += 0.08F * (100 - dbLayoutPanel1.ColumnStyles[4].Width);
            else
            {
                timerOpenNivele.Stop();
                //dimensionareCasete();
                if (stareNivele==false)
                    timerBefore.Start();
            }
            labelTimp.Refresh();
            dimensionareCasete();
            dbLayoutPanel2.Refresh();
            dbLayoutPanel1.Refresh();
            dbLayoutPanel3.Refresh();
            
        } //Deschide lista cu nivele

        private void timerCloseNivele_Tick(object sender, EventArgs e)
        {
            if (dbLayoutPanel1.ColumnStyles[4].Width - 0.08F * (100 - dbLayoutPanel1.ColumnStyles[4].Width) > 0)
                dbLayoutPanel1.ColumnStyles[4].Width -= 0.08F * (100 - dbLayoutPanel1.ColumnStyles[4].Width);
            else
            {
                timerCloseNivele.Stop();
                dbLayoutPanel1.ColumnStyles[4].Width = 0;

                if (stareNivele == false)
                    urmatoareaIntrebare();
                else
                    stareNivele = false;
            }
            labelTimp.Refresh();
            dimensionareCasete();
            dbLayoutPanel2.Refresh();
            dbLayoutPanel1.Refresh();
            dbLayoutPanel3.Refresh();
            
        } //Inchide lista cu nivele



        private void timerBefore_Tick(object sender, EventArgs e)
        {
            timerBefore.Stop();
            if (NivelAcutal >= 2)
                Butoane[NivelAcutal-2].BackgroundImage=null;
            timerInTime.Start();
        }

        private void timerInTime_Tick(object sender, EventArgs e)
        {
            timerInTime.Stop();

            pictureBoxMainSlot.Image = null;
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;

            if (NivelAcutal < 15 && NivelAcutal>=1)
                Butoane[NivelAcutal-1].BackgroundImage = Properties.Resources.mainslot;

            timerAfter.Start();
        }

        private void timerAfter_Tick(object sender, EventArgs e)
        {
            timerAfter.Stop();
            if (stareNivele == true)
            {
                //stareNivele = true;
                urmatoareaIntrebare();
            }
            else
                timerCloseNivele.Start();
        }

        private void timerTimp_Tick(object sender, EventArgs e)
        {
            labelTimp.Text = timp.ToString();
            timp--;
            labelTimp.Refresh();

            if (timp <= -1)
            {
                timerTimp.Stop();
                selectat = true;
                raspunsSelectat = 0;

                if (raspunsCorect == 1 || raspunsCorect == 3)
                    backgroundRaspuns = Properties.Resources.slot;
                else
                    backgroundRaspuns = Properties.Resources.slot2;

                player.SoundLocation = @"Sounds\Lose\Niv" + NivelAcutal + "LS.wav";
                player.Play();

                timerFlash.Start();
            }
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            if (changeQuestion() == true)
            {
                buttonChange.Enabled = false;
                buttonChange.BackgroundImage = Properties.Resources.ChangeqsX;
            }
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            if (formPassword.ShowDialog() == DialogResult.OK)
                changeQuestion();
        }

        private void timerApel_Tick(object sender, EventArgs e)
        {
            timpApel--;
            labelTimp.Text = "Apel: " + timpApel.ToString();
            

            if (timpApel==1)
            {
                player.SoundLocation = @"Sounds\Phone\Finish.wav";
                player.Play();
            }


            if (timpApel<=0)
            {
                timerApel.Stop();
                timpApel = 60;
                timerTimp.Start();

                if (selectat==false)
                {
                    player.SoundLocation = @"Sounds\Question\Niv" + NivelAcutal + "QS.wav";
                    player.PlayLooping();
                }
            }
        }

        private void buttonAuto_Click(object sender, EventArgs e)
        {
            if (waitingForInput==true)
            {
                timerFlashForInput.Stop();
                buttonAuto.BackgroundImage = Properties.Resources.AutoOFF;
                checkAnswer();
            }
            else
            {
                if (auto==false)
                {
                    auto = true;
                    buttonAuto.BackgroundImage = Properties.Resources.AutoON;
                    labelInfo.Text = "Trecerea automata PORNITA";
                }
                else
                {
                    auto = false;
                    buttonAuto.BackgroundImage = Properties.Resources.AutoOFF;
                    labelInfo.Text = "Trecerea automata OPRITA";
                }
            }
        }

        private void timerFlashForInput_Tick(object sender, EventArgs e)
        {
            buttonAuto.BackgroundImage = FlashuriNext[idFlash];
            idFlash++;
            if (idFlash >= 2)
                idFlash = 0;
            buttonAuto.Refresh();
        }

        private void buttonCashOut_MouseEnter(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                info = labelInfo.Text;
                labelInfo.Text = "Renunta";
                labelInfo.Refresh();
            }
        }

        private void buttonCashOut_MouseLeave(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                labelInfo.Text = info;
                labelInfo.Refresh();
            }

        }

        private void buttonReload_MouseEnter(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                info = labelInfo.Text;
                labelInfo.Text = "Schimba intrebarea";
                labelInfo.Refresh();
            }
        }

        private void buttonReload_MouseLeave(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                labelInfo.Text = info;
                labelInfo.Refresh();
            }
        }

        private void buttonAuto_MouseEnter(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                info = labelInfo.Text;
                if (auto == false)
                    labelInfo.Text = "PORNESTE trecerea automata";
                else
                    labelInfo.Text = "OPRESTE trecerea automata";
                labelInfo.Refresh();
            }
        }

        private void buttonAuto_MouseLeave(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                labelInfo.Text = info;
                labelInfo.Refresh();
            }
        }

        private void buttonFifty_MouseEnter(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                info = labelInfo.Text;
                labelInfo.Text = "Fifty-Fifty";
                labelInfo.Refresh();
            }
        }

        private void buttonFifty_MouseLeave(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                labelInfo.Text = info;
                labelInfo.Refresh();
            }
        }

        private void buttonCall_MouseEnter(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                info = labelInfo.Text;
                labelInfo.Text = "Suna un prieten";
                labelInfo.Refresh();
            }
        }

        private void buttonCall_MouseLeave(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                labelInfo.Text = info;
                labelInfo.Refresh();
            }
        }

        private void buttonAsk_MouseEnter(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                info = labelInfo.Text;
                labelInfo.Text = "Intreaba publicul";
                labelInfo.Refresh();
            }
        }

        private void buttonAsk_MouseLeave(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                labelInfo.Text = info;
                labelInfo.Refresh();
            }
        }

        private void buttonChange_MouseEnter(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                info = labelInfo.Text;
                labelInfo.Text = "Schimba intrebarea";
                labelInfo.Refresh();
            }
        }

        private void buttonChange_MouseLeave(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                labelInfo.Text = info;
                labelInfo.Refresh();
            }
        }

        private void buttonOpenNivele_MouseEnter(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                info = labelInfo.Text;

                if (stareNivele == false)
                    labelInfo.Text = "Vezi nivelul la care te afli";
                else
                    labelInfo.Text = "Inchide nivelele";
                labelInfo.Refresh();
            }
        }

        private void buttonOpenNivele_MouseLeave(object sender, EventArgs e)
        {
            if (waitingForInput == false)
            {
                labelInfo.Text = info;
                labelInfo.Refresh();
            }
        }

        private void Game_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString()=="`")
            {
                Consola formConsola = new Consola();
                formConsola.Show();
            }
            e.Handled = true;
        }
    }
}
