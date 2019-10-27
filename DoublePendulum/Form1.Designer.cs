namespace DoublePendulum
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.display = new System.Windows.Forms.PictureBox();
            this.btnArms = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.display)).BeginInit();
            this.SuspendLayout();
            // 
            // display
            // 
            this.display.BackColor = System.Drawing.Color.Black;
            this.display.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.display.Location = new System.Drawing.Point(0, 0);
            this.display.Margin = new System.Windows.Forms.Padding(0);
            this.display.Name = "display";
            this.display.Size = new System.Drawing.Size(884, 761);
            this.display.TabIndex = 0;
            this.display.TabStop = false;
            this.display.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AddPendulumMouseClick);
            // 
            // btnArms
            // 
            this.btnArms.Location = new System.Drawing.Point(13, 13);
            this.btnArms.Name = "btnArms";
            this.btnArms.Size = new System.Drawing.Size(25, 25);
            this.btnArms.TabIndex = 1;
            this.btnArms.Text = "X";
            this.btnArms.UseVisualStyleBackColor = true;
            this.btnArms.Click += new System.EventHandler(this.btnArms_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 761);
            this.Controls.Add(this.btnArms);
            this.Controls.Add(this.display);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.display)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox display;
        private System.Windows.Forms.Button btnArms;
    }
}

