namespace VreiSaFiiMilionar
{
    partial class Parola
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dbLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dbLayoutPanel1
            // 
            this.dbLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.dbLayoutPanel1.ColumnCount = 1;
            this.dbLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.dbLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.dbLayoutPanel1.Controls.Add(this.textBox1, 0, 1);
            this.dbLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dbLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.dbLayoutPanel1.Name = "dbLayoutPanel1";
            this.dbLayoutPanel1.RowCount = 2;
            this.dbLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.dbLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.dbLayoutPanel1.Size = new System.Drawing.Size(509, 196);
            this.dbLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(503, 137);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pentru a schimba intreabarea, introduceti parola stabilita:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(31)))), ((int)(((byte)(32)))));
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(25, 140);
            this.textBox1.Margin = new System.Windows.Forms.Padding(25, 3, 25, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(459, 38);
            this.textBox1.TabIndex = 1;
            this.textBox1.UseSystemPasswordChar = true;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Parola
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::VreiSaFiiMilionar.Properties.Resources.BackgroundNivele;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(509, 196);
            this.Controls.Add(this.dbLayoutPanel1);
            this.Name = "Parola";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parola";
            this.dbLayoutPanel1.ResumeLayout(false);
            this.dbLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DBLayoutPanel dbLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
    }
}