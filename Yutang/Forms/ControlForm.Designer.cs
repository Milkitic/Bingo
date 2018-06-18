namespace Yutang.Forms
{
    partial class ControlForm
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
            this.btnSevenA = new System.Windows.Forms.Button();
            this.btnSevenB = new System.Windows.Forms.Button();
            this.btnSevenC = new System.Windows.Forms.Button();
            this.btnSevenD = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSevenA
            // 
            this.btnSevenA.Location = new System.Drawing.Point(6, 20);
            this.btnSevenA.Name = "btnSevenA";
            this.btnSevenA.Size = new System.Drawing.Size(75, 23);
            this.btnSevenA.TabIndex = 0;
            this.btnSevenA.Text = "7灭";
            this.btnSevenA.UseVisualStyleBackColor = true;
            this.btnSevenA.Click += new System.EventHandler(this.btnSevenA_Click);
            // 
            // btnSevenB
            // 
            this.btnSevenB.Location = new System.Drawing.Point(6, 49);
            this.btnSevenB.Name = "btnSevenB";
            this.btnSevenB.Size = new System.Drawing.Size(75, 23);
            this.btnSevenB.TabIndex = 1;
            this.btnSevenB.Text = "7A";
            this.btnSevenB.UseVisualStyleBackColor = true;
            this.btnSevenB.Click += new System.EventHandler(this.btnSevenB_Click);
            // 
            // btnSevenC
            // 
            this.btnSevenC.Location = new System.Drawing.Point(87, 49);
            this.btnSevenC.Name = "btnSevenC";
            this.btnSevenC.Size = new System.Drawing.Size(75, 23);
            this.btnSevenC.TabIndex = 2;
            this.btnSevenC.Text = "7B";
            this.btnSevenC.UseVisualStyleBackColor = true;
            this.btnSevenC.Click += new System.EventHandler(this.btnSevenC_Click);
            // 
            // btnSevenD
            // 
            this.btnSevenD.Location = new System.Drawing.Point(168, 49);
            this.btnSevenD.Name = "btnSevenD";
            this.btnSevenD.Size = new System.Drawing.Size(75, 23);
            this.btnSevenD.TabIndex = 3;
            this.btnSevenD.Text = "7C";
            this.btnSevenD.UseVisualStyleBackColor = true;
            this.btnSevenD.Click += new System.EventHandler(this.btnSevenD_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSevenA);
            this.groupBox1.Controls.Add(this.btnSevenD);
            this.groupBox1.Controls.Add(this.btnSevenB);
            this.groupBox1.Controls.Add(this.btnSevenC);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 85);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "7";
            // 
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 110);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "ControlForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ControlForm";
            this.Load += new System.EventHandler(this.ControlForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSevenA;
        private System.Windows.Forms.Button btnSevenB;
        private System.Windows.Forms.Button btnSevenC;
        private System.Windows.Forms.Button btnSevenD;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}