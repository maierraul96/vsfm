namespace VreiSaFiiMilionar
{
    partial class DespreAplicatie
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dbLayoutPanel1 = new VreiSaFiiMilionar.DBLayoutPanel(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dbLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dbLayoutPanel1
            // 
            this.dbLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.dbLayoutPanel1.ColumnCount = 3;
            this.dbLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.dbLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.dbLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.dbLayoutPanel1.Controls.Add(this.pictureBox1, 1, 1);
            this.dbLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dbLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.dbLayoutPanel1.Name = "dbLayoutPanel1";
            this.dbLayoutPanel1.RowCount = 3;
            this.dbLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.dbLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93F));
            this.dbLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2F));
            this.dbLayoutPanel1.Size = new System.Drawing.Size(437, 494);
            this.dbLayoutPanel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::VreiSaFiiMilionar.Properties.Resources.Disclaimer2;
            this.pictureBox1.Location = new System.Drawing.Point(46, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(343, 453);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // DespreAplicatie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::VreiSaFiiMilionar.Properties.Resources.BackgroundNivele;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(437, 494);
            this.Controls.Add(this.dbLayoutPanel1);
            this.Name = "DespreAplicatie";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Despre Aplicatie";
            this.Load += new System.EventHandler(this.DespreAplicatie_Load);
            this.dbLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DBLayoutPanel dbLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}