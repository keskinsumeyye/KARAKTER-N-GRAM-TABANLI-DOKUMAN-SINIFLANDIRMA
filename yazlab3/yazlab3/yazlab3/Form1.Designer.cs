namespace yazlab3
{
    partial class Form1
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
            this.run = new System.Windows.Forms.Button();
            this.mesgul = new System.Windows.Forms.Label();
            this.degerler = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.asil = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // run
            // 
            this.run.AutoSize = true;
            this.run.Location = new System.Drawing.Point(15, 15);
            this.run.Margin = new System.Windows.Forms.Padding(6);
            this.run.Name = "run";
            this.run.Size = new System.Drawing.Size(424, 23);
            this.run.TabIndex = 0;
            this.run.Text = "Çalıştır!";
            this.run.UseVisualStyleBackColor = true;
            this.run.Click += new System.EventHandler(this.run_Click);
            // 
            // mesgul
            // 
            this.mesgul.AutoSize = true;
            this.mesgul.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mesgul.Location = new System.Drawing.Point(72, 44);
            this.mesgul.Name = "mesgul";
            this.mesgul.Size = new System.Drawing.Size(312, 24);
            this.mesgul.TabIndex = 1;
            this.mesgul.Text = "Program Çalışıyor! Lütfen Bekleyiniz.";
            this.mesgul.Visible = false;
            // 
            // degerler
            // 
            this.degerler.ColumnCount = 4;
            this.degerler.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.63158F));
            this.degerler.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.78947F));
            this.degerler.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.78947F));
            this.degerler.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.78947F));
            this.degerler.Location = new System.Drawing.Point(15, 102);
            this.degerler.Name = "degerler";
            this.degerler.RowCount = 6;
            this.degerler.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.degerler.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.degerler.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.degerler.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.degerler.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.degerler.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.degerler.Size = new System.Drawing.Size(424, 261);
            this.degerler.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 386);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Asıl Kategori";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(310, 386);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Tahmin Edilen";
            this.label2.Visible = false;
            // 
            // asil
            // 
            this.asil.Location = new System.Drawing.Point(15, 402);
            this.asil.Name = "asil";
            this.asil.Size = new System.Drawing.Size(424, 168);
            this.asil.TabIndex = 7;
            this.asil.Text = "";
            this.asil.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 582);
            this.Controls.Add(this.asil);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.degerler);
            this.Controls.Add(this.mesgul);
            this.Controls.Add(this.run);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button run;
        private System.Windows.Forms.Label mesgul;
        private System.Windows.Forms.TableLayoutPanel degerler;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox asil;
    }
}

