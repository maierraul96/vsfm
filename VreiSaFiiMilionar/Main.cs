using System;
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
    public partial class Main : Form
    {
        public static FormEditor formEditor= new FormEditor();
        public static SetGame setGameForm = new SetGame();
        public Rezultate formRezultate = new Rezultate();
        public DespreAplicatie formDespre = new DespreAplicatie();
        public GestionareIntrebari formGestioneazaIntrebari = new GestionareIntrebari();

        public static SoundPlayer player = new SoundPlayer();

        public Main()
        {
            InitializeComponent();
        }

        private void repaddingPictureBox(ref PictureBox picturebox, float left, float top, float right, float bottom)
        {
            picturebox.Padding = new Padding(
                Convert.ToInt32(left * picturebox.Width),
                Convert.ToInt32(top * picturebox.Height),
                Convert.ToInt32(right * picturebox.Width),
                Convert.ToInt32(bottom * picturebox.Height));
        }

        private void remarginLabel(ref Label label, float left, float top, float right, float bottom)
        {
            label.Margin = new Padding(
                Convert.ToInt32(left * label.Width),     //left
                Convert.ToInt32(top * label.Height),     //top
                Convert.ToInt32(right * label.Width),    //right
                Convert.ToInt32(bottom * label.Height));//bottom
        }

        private void resizeLabel(ref Label label, float procent)
        {
            label.Font = new Font(FontFamily.GenericSansSerif, procent * this.Height, FontStyle.Bold & FontStyle.Italic);
        }


        private void Main_Load(object sender, EventArgs e)
        {
            //Pornesc muzica
            player.SoundLocation = @"Sounds\MainTheme.wav";
            player.LoadAsync();
            player.PlayLooping();

            labelCitat.Font = new Font(FontFamily.GenericSansSerif, 0.019F * this.Height, FontStyle.Italic);
            labelEinstein.Font = new Font(FontFamily.GenericSansSerif, 0.014F * this.Height, FontStyle.Bold);
            labelCuDrag.Font = new Font(FontFamily.GenericSansSerif, 0.02F * this.Height, FontStyle.Bold);

            remarginLabel(ref labelCitat, 0.0F, 0.01F, 0.55F, 0.01F);
            remarginLabel(ref labelEinstein, 0.0F, 0.01F, 0.52F, 0.01F);
            remarginLabel(ref labelCuDrag, 0.0F, 0.01F, 0.5F, 0.01F);

            //Astez textul in mijlocul casetelor
            repaddingPictureBox(ref pictureBox1, 0.0F, 0.1F, 0.0F, 0.0F);
            repaddingPictureBox(ref pictureBox2, 0.0F, 0.1F, 0.0F, 0.0F);
            repaddingPictureBox(ref pictureBox3, 0.0F, 0.1F, 0.0F, 0.0F);
            repaddingPictureBox(ref pictureBox4, 0.0F, 0.1F, 0.0F, 0.0F);
            repaddingPictureBox(ref pictureBox6, 0.0F, 0.1F, 0.0F, 0.0F);
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Properties.Resources.selected;
            pictureBox1.Refresh();
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.BackgroundImage = Properties.Resources.selected;
            pictureBox2.Refresh();
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.BackgroundImage = Properties.Resources.selected;
            pictureBox3.Refresh();
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.BackgroundImage = Properties.Resources.selected;
            pictureBox4.Refresh();
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Properties.Resources.mainslot;
            pictureBox1.Refresh();
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.BackgroundImage = Properties.Resources.mainslot;
            pictureBox2.Refresh();
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.BackgroundImage = Properties.Resources.mainslot;
            pictureBox3.Refresh();
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.BackgroundImage = Properties.Resources.mainslot;
            pictureBox4.Refresh();
        }

        private void pictureBox6_MouseEnter(object sender, EventArgs e)
        {
            pictureBox6.BackgroundImage = Properties.Resources.selected;
            pictureBox6.Refresh();
        }

        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {
            pictureBox6.BackgroundImage = Properties.Resources.mainslot;
            pictureBox6.Refresh();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            formEditor.Height = BlackMain.mainForm.Height;
            formEditor.Width = BlackMain.mainForm.Width;
            formEditor.ShowDialog();
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //this.ShowInTaskbar = false;
            setGameForm.Height = BlackMain.mainForm.Height;
            setGameForm.Width = BlackMain.mainForm.Width;
            setGameForm.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            formRezultate.Height = BlackMain.mainForm.Height;
            formRezultate.Width = BlackMain.mainForm.Width;
            formRezultate.ShowDialog();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            formDespre.ShowDialog();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            formGestioneazaIntrebari.Height = BlackMain.mainForm.Height;
            formGestioneazaIntrebari.Width = BlackMain.mainForm.Width;
            formGestioneazaIntrebari.ShowDialog();
        }
    }
}
