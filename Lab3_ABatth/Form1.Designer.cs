namespace Lab3_ABatth
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
            this.components = new System.ComponentModel.Container();
            this.NewCar = new System.Windows.Forms.Timer(this.components);
            this.Crash = new System.Windows.Forms.Timer(this.components);
            this._labelScore = new System.Windows.Forms.Label();
            this.UI_btnPlay = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NewCar
            // 
            this.NewCar.Enabled = true;
            this.NewCar.Interval = 4000;
            // 
            // Crash
            // 
            this.Crash.Enabled = true;
            // 
            // _labelScore
            // 
            this._labelScore.AutoSize = true;
            this._labelScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._labelScore.Location = new System.Drawing.Point(248, 145);
            this._labelScore.Name = "_labelScore";
            this._labelScore.Size = new System.Drawing.Size(297, 73);
            this._labelScore.TabIndex = 0;
            this._labelScore.Text = "Score : 0";
            // 
            // UI_btnPlay
            // 
            this.UI_btnPlay.Enabled = false;
            this.UI_btnPlay.Location = new System.Drawing.Point(261, 307);
            this.UI_btnPlay.Name = "UI_btnPlay";
            this.UI_btnPlay.Size = new System.Drawing.Size(284, 62);
            this.UI_btnPlay.TabIndex = 1;
            this.UI_btnPlay.Text = "Play Again ?";
            this.UI_btnPlay.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(19F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.UI_btnPlay);
            this.Controls.Add(this._labelScore);
            this.Name = "Form1";
            this.Text = "Crash-o-matic";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer NewCar;
        private System.Windows.Forms.Timer Crash;
        private System.Windows.Forms.Label _labelScore;
        private System.Windows.Forms.Button UI_btnPlay;
    }
}

