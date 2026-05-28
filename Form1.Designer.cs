namespace CybersecurityChatbotGUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            rtbChat = new RichTextBox();
            txtInput = new TextBox();
            btnSend = new Button();
            lblLogo = new Label();
            SuspendLayout();
            // 
            // rtbChat
            // 
            rtbChat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtbChat.BackColor = Color.Black;
            rtbChat.BorderStyle = BorderStyle.None;
            rtbChat.Font = new Font("Consolas", 10F);
            rtbChat.ForeColor = Color.Cyan;
            rtbChat.Location = new Point(10, 120);
            rtbChat.Name = "rtbChat";
            rtbChat.ReadOnly = true;
            rtbChat.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbChat.Size = new Size(860, 400);
            rtbChat.TabIndex = 0;
            rtbChat.Text = "";
            // 
            // txtInput
            // 
            txtInput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtInput.BackColor = Color.Black;
            txtInput.BorderStyle = BorderStyle.FixedSingle;
            txtInput.Font = new Font("Consolas", 11F);
            txtInput.ForeColor = Color.White;
            txtInput.Location = new Point(10, 535);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(720, 29);
            txtInput.TabIndex = 1;
            // 
            // btnSend
            // 
            btnSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSend.BackColor = Color.Cyan;
            btnSend.Cursor = Cursors.Hand;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.Font = new Font("Consolas", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSend.ForeColor = Color.Black;
            btnSend.Location = new Point(740, 535);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(130, 40);
            btnSend.TabIndex = 2;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = false;
            // 
            // lblLogo
            // 
            lblLogo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblLogo.AutoSize = true;
            lblLogo.Font = new Font("Consolas", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLogo.ForeColor = Color.Cyan;
            lblLogo.Location = new Point(10, 10);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(584, 27);
            lblLogo.TabIndex = 3;
            lblLogo.Text = "CYBERBOT - Protecting South African Citizens";
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(882, 603);
            Controls.Add(lblLogo);
            Controls.Add(btnSend);
            Controls.Add(txtInput);
            Controls.Add(rtbChat);
            MinimumSize = new Size(900, 650);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cybersecurity Awareness Chatbot";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox rtbChat;
        private TextBox txtInput;
        private Button btnSend;
        private Label lblLogo;
    }
}
