namespace RPSClient
{
    partial class ClientForm
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
            Connect_button = new Button();
            NameBox = new TextBox();
            label1 = new Label();
            PortBox = new TextBox();
            IPBox = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            User_richTextBox = new RichTextBox();
            Rock_button = new Button();
            Paper_button = new Button();
            Scissors_button = new Button();
            Leave_button = new Button();
            label5 = new Label();
            Console_richTextBox = new RichTextBox();
            Player1_richTextBox = new RichTextBox();
            Player2_richTextBox = new RichTextBox();
            Player3_richTextBox = new RichTextBox();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            Username_textBox = new TextBox();
            UserPoint_textBox = new TextBox();
            Player1Name_textBox = new TextBox();
            Player1Point_textBox = new TextBox();
            Player2Name_textBox = new TextBox();
            Player2Point_textBox = new TextBox();
            Player3Name_textBox = new TextBox();
            Player3Point_textBox = new TextBox();
            Console_label = new Label();
            SuspendLayout();
            // 
            // Connect_button
            // 
            Connect_button.Location = new Point(24, 187);
            Connect_button.Name = "Connect_button";
            Connect_button.Size = new Size(94, 29);
            Connect_button.TabIndex = 0;
            Connect_button.Text = "Connect";
            Connect_button.UseVisualStyleBackColor = true;
            Connect_button.Click += Connect_button_Click;
            // 
            // NameBox
            // 
            NameBox.Location = new Point(24, 40);
            NameBox.Name = "NameBox";
            NameBox.Size = new Size(125, 27);
            NameBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 9);
            label1.Name = "label1";
            label1.Size = new Size(75, 20);
            label1.TabIndex = 2;
            label1.Text = "Username";
            // 
            // PortBox
            // 
            PortBox.Location = new Point(24, 148);
            PortBox.Name = "PortBox";
            PortBox.Size = new Size(125, 27);
            PortBox.TabIndex = 3;
            // 
            // IPBox
            // 
            IPBox.Location = new Point(24, 93);
            IPBox.Name = "IPBox";
            IPBox.Size = new Size(125, 27);
            IPBox.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 70);
            label2.Name = "label2";
            label2.Size = new Size(79, 20);
            label2.TabIndex = 5;
            label2.Text = "IP Number";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 125);
            label3.Name = "label3";
            label3.Size = new Size(93, 20);
            label3.TabIndex = 6;
            label3.Text = "Port Number";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(463, 314);
            label4.Name = "label4";
            label4.Size = new Size(75, 20);
            label4.TabIndex = 8;
            label4.Text = "Username";
            // 
            // User_richTextBox
            // 
            User_richTextBox.Location = new Point(463, 370);
            User_richTextBox.Name = "User_richTextBox";
            User_richTextBox.Size = new Size(158, 94);
            User_richTextBox.TabIndex = 11;
            User_richTextBox.Text = "";
            // 
            // Rock_button
            // 
            Rock_button.Location = new Point(394, 470);
            Rock_button.Name = "Rock_button";
            Rock_button.Size = new Size(94, 29);
            Rock_button.TabIndex = 12;
            Rock_button.Text = "R";
            Rock_button.UseVisualStyleBackColor = true;
            Rock_button.Click += Rock_button_Click;
            // 
            // Paper_button
            // 
            Paper_button.Location = new Point(494, 470);
            Paper_button.Name = "Paper_button";
            Paper_button.Size = new Size(94, 29);
            Paper_button.TabIndex = 13;
            Paper_button.Text = "P";
            Paper_button.UseVisualStyleBackColor = true;
            Paper_button.Click += Paper_button_Click;
            // 
            // Scissors_button
            // 
            Scissors_button.Location = new Point(594, 470);
            Scissors_button.Name = "Scissors_button";
            Scissors_button.Size = new Size(94, 29);
            Scissors_button.TabIndex = 14;
            Scissors_button.Text = "S";
            Scissors_button.UseVisualStyleBackColor = true;
            Scissors_button.Click += Scissors_button_Click;
            // 
            // Leave_button
            // 
            Leave_button.Location = new Point(654, 398);
            Leave_button.Name = "Leave_button";
            Leave_button.Size = new Size(94, 29);
            Leave_button.TabIndex = 15;
            Leave_button.Text = "Leave ";
            Leave_button.UseVisualStyleBackColor = true;
            Leave_button.Click += Leave_button_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(563, 314);
            label5.Name = "label5";
            label5.Size = new Size(58, 20);
            label5.TabIndex = 9;
            label5.Text = "Point(s)";
            // 
            // Console_richTextBox
            // 
            Console_richTextBox.Location = new Point(7, 337);
            Console_richTextBox.Name = "Console_richTextBox";
            Console_richTextBox.Size = new Size(367, 164);
            Console_richTextBox.TabIndex = 16;
            Console_richTextBox.Text = "";
            // 
            // Player1_richTextBox
            // 
            Player1_richTextBox.Location = new Point(237, 215);
            Player1_richTextBox.Name = "Player1_richTextBox";
            Player1_richTextBox.Size = new Size(158, 94);
            Player1_richTextBox.TabIndex = 17;
            Player1_richTextBox.Text = "";
            // 
            // Player2_richTextBox
            // 
            Player2_richTextBox.Location = new Point(463, 67);
            Player2_richTextBox.Name = "Player2_richTextBox";
            Player2_richTextBox.Size = new Size(158, 94);
            Player2_richTextBox.TabIndex = 18;
            Player2_richTextBox.Text = "";
            Player2_richTextBox.TextChanged += Player2_richTextBox_TextChanged;
            // 
            // Player3_richTextBox
            // 
            Player3_richTextBox.Location = new Point(673, 215);
            Player3_richTextBox.Name = "Player3_richTextBox";
            Player3_richTextBox.Size = new Size(158, 94);
            Player3_richTextBox.TabIndex = 19;
            Player3_richTextBox.Text = "";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(337, 159);
            label6.Name = "label6";
            label6.Size = new Size(58, 20);
            label6.TabIndex = 23;
            label6.Text = "Point(s)";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(237, 159);
            label7.Name = "label7";
            label7.Size = new Size(75, 20);
            label7.TabIndex = 24;
            label7.Text = "Username";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(463, 9);
            label8.Name = "label8";
            label8.Size = new Size(75, 20);
            label8.TabIndex = 25;
            label8.Text = "Username";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(563, 11);
            label9.Name = "label9";
            label9.Size = new Size(58, 20);
            label9.TabIndex = 26;
            label9.Text = "Point(s)";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(773, 158);
            label10.Name = "label10";
            label10.Size = new Size(58, 20);
            label10.TabIndex = 27;
            label10.Text = "Point(s)";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(673, 158);
            label11.Name = "label11";
            label11.Size = new Size(75, 20);
            label11.TabIndex = 28;
            label11.Text = "Username";
            // 
            // Username_textBox
            // 
            Username_textBox.Location = new Point(463, 337);
            Username_textBox.Name = "Username_textBox";
            Username_textBox.Size = new Size(94, 27);
            Username_textBox.TabIndex = 29;
            // 
            // UserPoint_textBox
            // 
            UserPoint_textBox.Location = new Point(563, 337);
            UserPoint_textBox.Name = "UserPoint_textBox";
            UserPoint_textBox.Size = new Size(58, 27);
            UserPoint_textBox.TabIndex = 30;
            // 
            // Player1Name_textBox
            // 
            Player1Name_textBox.Location = new Point(237, 182);
            Player1Name_textBox.Name = "Player1Name_textBox";
            Player1Name_textBox.Size = new Size(91, 27);
            Player1Name_textBox.TabIndex = 31;
            // 
            // Player1Point_textBox
            // 
            Player1Point_textBox.Location = new Point(337, 182);
            Player1Point_textBox.Name = "Player1Point_textBox";
            Player1Point_textBox.Size = new Size(58, 27);
            Player1Point_textBox.TabIndex = 32;
            // 
            // Player2Name_textBox
            // 
            Player2Name_textBox.Location = new Point(463, 34);
            Player2Name_textBox.Name = "Player2Name_textBox";
            Player2Name_textBox.Size = new Size(94, 27);
            Player2Name_textBox.TabIndex = 33;
            // 
            // Player2Point_textBox
            // 
            Player2Point_textBox.Location = new Point(563, 34);
            Player2Point_textBox.Name = "Player2Point_textBox";
            Player2Point_textBox.Size = new Size(58, 27);
            Player2Point_textBox.TabIndex = 34;
            // 
            // Player3Name_textBox
            // 
            Player3Name_textBox.Location = new Point(673, 182);
            Player3Name_textBox.Name = "Player3Name_textBox";
            Player3Name_textBox.Size = new Size(94, 27);
            Player3Name_textBox.TabIndex = 35;
            Player3Name_textBox.TextChanged += textBox5_TextChanged;
            // 
            // Player3Point_textBox
            // 
            Player3Point_textBox.Location = new Point(773, 182);
            Player3Point_textBox.Name = "Player3Point_textBox";
            Player3Point_textBox.Size = new Size(58, 27);
            Player3Point_textBox.TabIndex = 36;
            // 
            // Console_label
            // 
            Console_label.AutoSize = true;
            Console_label.Location = new Point(7, 314);
            Console_label.Margin = new Padding(1, 0, 1, 0);
            Console_label.Name = "Console_label";
            Console_label.RightToLeft = RightToLeft.Yes;
            Console_label.Size = new Size(62, 20);
            Console_label.TabIndex = 37;
            Console_label.Text = "Console";
            // 
            // ClientForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(905, 511);
            Controls.Add(Console_label);
            Controls.Add(Player3Point_textBox);
            Controls.Add(Player3Name_textBox);
            Controls.Add(Player2Point_textBox);
            Controls.Add(Player2Name_textBox);
            Controls.Add(Player1Point_textBox);
            Controls.Add(Player1Name_textBox);
            Controls.Add(UserPoint_textBox);
            Controls.Add(Username_textBox);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(Player3_richTextBox);
            Controls.Add(Player2_richTextBox);
            Controls.Add(Player1_richTextBox);
            Controls.Add(Console_richTextBox);
            Controls.Add(Leave_button);
            Controls.Add(Scissors_button);
            Controls.Add(Paper_button);
            Controls.Add(Rock_button);
            Controls.Add(User_richTextBox);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(IPBox);
            Controls.Add(PortBox);
            Controls.Add(label1);
            Controls.Add(NameBox);
            Controls.Add(Connect_button);
            Name = "ClientForm";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Connect_button;
        private TextBox NameBox;
        private Label label1;
        private TextBox PortBox;
        private TextBox IPBox;
        private Label label2;
        private Label label3;
        private Label label4;
        private RichTextBox User_richTextBox;
        private Button Rock_button;
        private Button Paper_button;
        private Button Scissors_button;
        private Button Leave_button;
        private Label label5;
        private RichTextBox Console_richTextBox;
        private RichTextBox Player1_richTextBox;
        private RichTextBox Player2_richTextBox;
        private RichTextBox Player3_richTextBox;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private TextBox Username_textBox;
        private TextBox UserPoint_textBox;
        private TextBox Player1Name_textBox;
        private TextBox Player1Point_textBox;
        private TextBox Player2Name_textBox;
        private TextBox Player2Point_textBox;
        private TextBox Player3Name_textBox;
        private TextBox Player3Point_textBox;
        private Label Console_label;
    }
}
