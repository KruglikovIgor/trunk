namespace UserControlsKPK2
{
	public partial class SegmentIndicatorControl
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
            this.LableUp = new System.Windows.Forms.Label();
            this.LableDown = new System.Windows.Forms.Label();
            this.SegmentIndication1 = new UserControlsKPK2.SegmentIndication();
            this.SuspendLayout();
            // 
            // LableUp
            // 
            this.LableUp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LableUp.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LableUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LableUp.Location = new System.Drawing.Point(0, 0);
            this.LableUp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LableUp.Name = "LableUp";
            this.LableUp.Size = new System.Drawing.Size(121, 25);
            this.LableUp.TabIndex = 3;
            this.LableUp.Text = "UP";
            this.LableUp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LableDown
            // 
            this.LableDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LableDown.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LableDown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LableDown.Location = new System.Drawing.Point(0, 68);
            this.LableDown.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LableDown.Name = "LableDown";
            this.LableDown.Size = new System.Drawing.Size(121, 23);
            this.LableDown.TabIndex = 1;
            this.LableDown.Text = "DOWN";
            this.LableDown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SegmentIndication1
            // 
            this.SegmentIndication1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SegmentIndication1.BackColor = System.Drawing.Color.Transparent;
            this.SegmentIndication1.BGColor = System.Drawing.Color.Black;
            this.SegmentIndication1.CalculationMethod = UserControlsKPK2.Enums.CalcMethod.MinMax;
            this.SegmentIndication1.DarkColor = System.Drawing.Color.Transparent;
            this.SegmentIndication1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SegmentIndication1.FractPart = 0;
            this.SegmentIndication1.Inactive = false;
            this.SegmentIndication1.InactiveBGColor = System.Drawing.Color.Gray;
            this.SegmentIndication1.InvalidColor = System.Drawing.Color.Red;
            this.SegmentIndication1.LightColor = System.Drawing.Color.Lime;
            this.SegmentIndication1.Location = new System.Drawing.Point(0, 25);
            this.SegmentIndication1.Margin = new System.Windows.Forms.Padding(11, 10, 11, 10);
            this.SegmentIndication1.MaxValue = 0D;
            this.SegmentIndication1.MinValue = 0D;
            this.SegmentIndication1.Name = "SegmentIndication1";
            this.SegmentIndication1.NeedAlign = true;
            this.SegmentIndication1.Size = new System.Drawing.Size(121, 43);
            this.SegmentIndication1.TabIndex = 4;
            this.SegmentIndication1.Unit = "";
            this.SegmentIndication1.Value = 0D;
            this.SegmentIndication1.ValueString = "0";
            this.SegmentIndication1.WholePart = 0;
            // 
            // SegmentIndicatorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.SegmentIndication1);
            this.Controls.Add(this.LableUp);
            this.Controls.Add(this.LableDown);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SegmentIndicatorControl";
            this.Size = new System.Drawing.Size(121, 92);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label LableUp;
		private System.Windows.Forms.Label LableDown;
		private UserControlsKPK2.SegmentIndication SegmentIndication1;
	}
}
