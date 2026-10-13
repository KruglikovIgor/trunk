namespace UserControlsKPK2
{
    partial class SIWithLabel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.UpperNote = new System.Windows.Forms.Label();
            this.siIndicator = new SegmentIndication();
            this.SuspendLayout();
            // 
            // UpperNote
            // 
            this.UpperNote.Dock = System.Windows.Forms.DockStyle.Top;
            this.UpperNote.Location = new System.Drawing.Point(0, 0);
            this.UpperNote.Name = "UpperNote";
            this.UpperNote.Size = new System.Drawing.Size(260, 13);
            this.UpperNote.TabIndex = 1;
            this.UpperNote.Text = "Привет";
            // 
            // siIndicator
            // 
            this.siIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.siIndicator.BackColor = System.Drawing.Color.Transparent;
            this.siIndicator.DarkColor = System.Drawing.Color.Black;
            this.siIndicator.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.siIndicator.FractPart = 0;
            this.siIndicator.InvalidColor = System.Drawing.Color.Red;
            this.siIndicator.LightColor = System.Drawing.Color.Lime;
            this.siIndicator.Location = new System.Drawing.Point(0, 16);
            this.siIndicator.MaxValue = 0;
            this.siIndicator.MinValue = 0;
            this.siIndicator.Name = "siIndicator";
            this.siIndicator.Size = new System.Drawing.Size(260, 84);
            this.siIndicator.TabIndex = 0;
            this.siIndicator.Unit = "%";
            this.siIndicator.Value = 0;
            this.siIndicator.ValueString = "000.00";
            this.siIndicator.WholePart = 0;
            // 
            // SIWithLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UpperNote);
            this.Controls.Add(this.siIndicator);
            this.Name = "SIWithLabel";
            this.Size = new System.Drawing.Size(260, 100);
            this.ResumeLayout(false);

        }

        #endregion

        private SegmentIndication siIndicator;
        private System.Windows.Forms.Label UpperNote;
    }
}
