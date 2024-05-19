namespace RPS_Server
{
    partial class ServerForm
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
            this.playersTextBox = new System.Windows.Forms.RichTextBox();
            this.label_IP = new System.Windows.Forms.Label();
            this.label_Port = new System.Windows.Forms.Label();
            this.label_players = new System.Windows.Forms.Label();
            this.label_WQueue = new System.Windows.Forms.Label();
            this.wqueueTextBox = new System.Windows.Forms.RichTextBox();
            this.IPTextBox = new System.Windows.Forms.TextBox();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.label_log = new System.Windows.Forms.Label();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.Button_listen = new System.Windows.Forms.Button();
            this.Leaderboard_richTextBox = new System.Windows.Forms.RichTextBox();
            this.Leaderboard_label = new System.Windows.Forms.Label();
            this.LeftPlayersTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // playersTextBox
            // 
            this.playersTextBox.Location = new System.Drawing.Point(21, 106);
            this.playersTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.playersTextBox.Name = "playersTextBox";
            this.playersTextBox.Size = new System.Drawing.Size(152, 89);
            this.playersTextBox.TabIndex = 0;
            this.playersTextBox.Text = "";
            // 
            // label_IP
            // 
            this.label_IP.AutoSize = true;
            this.label_IP.Location = new System.Drawing.Point(18, 24);
            this.label_IP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_IP.Name = "label_IP";
            this.label_IP.Size = new System.Drawing.Size(36, 16);
            this.label_IP.TabIndex = 1;
            this.label_IP.Text = "IPv4:";
            // 
            // label_Port
            // 
            this.label_Port.AutoSize = true;
            this.label_Port.Location = new System.Drawing.Point(18, 50);
            this.label_Port.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Port.Name = "label_Port";
            this.label_Port.Size = new System.Drawing.Size(34, 16);
            this.label_Port.TabIndex = 2;
            this.label_Port.Text = "Port:";
            // 
            // label_players
            // 
            this.label_players.AutoSize = true;
            this.label_players.Location = new System.Drawing.Point(18, 79);
            this.label_players.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_players.Name = "label_players";
            this.label_players.Size = new System.Drawing.Size(56, 16);
            this.label_players.TabIndex = 3;
            this.label_players.Text = "Players:";
            // 
            // label_WQueue
            // 
            this.label_WQueue.AutoSize = true;
            this.label_WQueue.Location = new System.Drawing.Point(18, 206);
            this.label_WQueue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_WQueue.Name = "label_WQueue";
            this.label_WQueue.Size = new System.Drawing.Size(98, 16);
            this.label_WQueue.TabIndex = 4;
            this.label_WQueue.Text = "Waiting Queue:";
            // 
            // wqueueTextBox
            // 
            this.wqueueTextBox.Location = new System.Drawing.Point(21, 230);
            this.wqueueTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.wqueueTextBox.Name = "wqueueTextBox";
            this.wqueueTextBox.Size = new System.Drawing.Size(152, 97);
            this.wqueueTextBox.TabIndex = 5;
            this.wqueueTextBox.Text = "";
            // 
            // IPTextBox
            // 
            this.IPTextBox.Location = new System.Drawing.Point(68, 24);
            this.IPTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(105, 22);
            this.IPTextBox.TabIndex = 6;
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(70, 54);
            this.portTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(104, 22);
            this.portTextBox.TabIndex = 7;
            // 
            // label_log
            // 
            this.label_log.AutoSize = true;
            this.label_log.Location = new System.Drawing.Point(18, 341);
            this.label_log.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_log.Name = "label_log";
            this.label_log.Size = new System.Drawing.Size(30, 16);
            this.label_log.TabIndex = 8;
            this.label_log.Text = "Log";
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(21, 369);
            this.logTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(520, 158);
            this.logTextBox.TabIndex = 9;
            this.logTextBox.Text = "";
            // 
            // Button_listen
            // 
            this.Button_listen.Location = new System.Drawing.Point(188, 49);
            this.Button_listen.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Button_listen.Name = "Button_listen";
            this.Button_listen.Size = new System.Drawing.Size(85, 28);
            this.Button_listen.TabIndex = 10;
            this.Button_listen.Text = "listen";
            this.Button_listen.UseVisualStyleBackColor = true;
            this.Button_listen.Click += new System.EventHandler(this.Button_listen_Click);
            // 
            // Leaderboard_richTextBox
            // 
            this.Leaderboard_richTextBox.Location = new System.Drawing.Point(396, 46);
            this.Leaderboard_richTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Leaderboard_richTextBox.Name = "Leaderboard_richTextBox";
            this.Leaderboard_richTextBox.Size = new System.Drawing.Size(144, 299);
            this.Leaderboard_richTextBox.TabIndex = 11;
            this.Leaderboard_richTextBox.Text = "";
            // 
            // Leaderboard_label
            // 
            this.Leaderboard_label.AutoSize = true;
            this.Leaderboard_label.Location = new System.Drawing.Point(396, 24);
            this.Leaderboard_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Leaderboard_label.Name = "Leaderboard_label";
            this.Leaderboard_label.Size = new System.Drawing.Size(86, 16);
            this.Leaderboard_label.TabIndex = 12;
            this.Leaderboard_label.Text = "Leaderboard";
            // 
            // LeftPlayersTextBox
            // 
            this.LeftPlayersTextBox.Location = new System.Drawing.Point(211, 230);
            this.LeftPlayersTextBox.Name = "LeftPlayersTextBox";
            this.LeftPlayersTextBox.Size = new System.Drawing.Size(153, 97);
            this.LeftPlayersTextBox.TabIndex = 13;
            this.LeftPlayersTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(208, 206);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 14;
            this.label1.Text = "Left Players:";
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 545);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LeftPlayersTextBox);
            this.Controls.Add(this.Leaderboard_label);
            this.Controls.Add(this.Leaderboard_richTextBox);
            this.Controls.Add(this.Button_listen);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.label_log);
            this.Controls.Add(this.portTextBox);
            this.Controls.Add(this.IPTextBox);
            this.Controls.Add(this.wqueueTextBox);
            this.Controls.Add(this.label_WQueue);
            this.Controls.Add(this.label_players);
            this.Controls.Add(this.label_Port);
            this.Controls.Add(this.label_IP);
            this.Controls.Add(this.playersTextBox);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ServerForm";
            this.Text = "ServerGUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox playersTextBox;
        private System.Windows.Forms.Label label_IP;
        private System.Windows.Forms.Label label_Port;
        private System.Windows.Forms.Label label_players;
        private System.Windows.Forms.Label label_WQueue;
        private System.Windows.Forms.RichTextBox wqueueTextBox;
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.Label label_log;
        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.Button Button_listen;
        private System.Windows.Forms.RichTextBox Leaderboard_richTextBox;
        private System.Windows.Forms.Label Leaderboard_label;
        private System.Windows.Forms.RichTextBox LeftPlayersTextBox;
        private System.Windows.Forms.Label label1;
    }
}

