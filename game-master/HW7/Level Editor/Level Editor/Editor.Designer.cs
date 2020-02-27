namespace Level_Editor
{
    partial class Editor
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
            this.tileSelectorGroupBox = new System.Windows.Forms.GroupBox();
            this.recepticalLabel = new System.Windows.Forms.Label();
            this.recepticalPictureBox = new System.Windows.Forms.PictureBox();
            this.closedDoorPictureBox = new System.Windows.Forms.PictureBox();
            this.closedDoorLabel = new System.Windows.Forms.Label();
            this.openDoorLabel = new System.Windows.Forms.Label();
            this.floorLabel = new System.Windows.Forms.Label();
            this.wallLabel = new System.Windows.Forms.Label();
            this.openDoorPictureBox = new System.Windows.Forms.PictureBox();
            this.wallPictureBox = new System.Windows.Forms.PictureBox();
            this.floorPictureBox = new System.Windows.Forms.PictureBox();
            this.currentTileGroupBox = new System.Windows.Forms.GroupBox();
            this.currentTilePictureBox = new System.Windows.Forms.PictureBox();
            this.saveFileButton = new System.Windows.Forms.Button();
            this.loadFileButton = new System.Windows.Forms.Button();
            this.mapGroupBox = new System.Windows.Forms.GroupBox();
            this.togglePastFuture = new System.Windows.Forms.Button();
            this.interactableSelectorGroupBox = new System.Windows.Forms.GroupBox();
            this.interactableSelectorComboBox = new System.Windows.Forms.ComboBox();
            this.tileSelectorGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recepticalPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closedDoorPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.openDoorPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wallPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.floorPictureBox)).BeginInit();
            this.currentTileGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentTilePictureBox)).BeginInit();
            this.interactableSelectorGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tileSelectorGroupBox
            // 
            this.tileSelectorGroupBox.Controls.Add(this.recepticalLabel);
            this.tileSelectorGroupBox.Controls.Add(this.recepticalPictureBox);
            this.tileSelectorGroupBox.Controls.Add(this.closedDoorPictureBox);
            this.tileSelectorGroupBox.Controls.Add(this.closedDoorLabel);
            this.tileSelectorGroupBox.Controls.Add(this.openDoorLabel);
            this.tileSelectorGroupBox.Controls.Add(this.floorLabel);
            this.tileSelectorGroupBox.Controls.Add(this.wallLabel);
            this.tileSelectorGroupBox.Controls.Add(this.openDoorPictureBox);
            this.tileSelectorGroupBox.Controls.Add(this.wallPictureBox);
            this.tileSelectorGroupBox.Controls.Add(this.floorPictureBox);
            this.tileSelectorGroupBox.Location = new System.Drawing.Point(12, 37);
            this.tileSelectorGroupBox.Name = "tileSelectorGroupBox";
            this.tileSelectorGroupBox.Size = new System.Drawing.Size(129, 252);
            this.tileSelectorGroupBox.TabIndex = 0;
            this.tileSelectorGroupBox.TabStop = false;
            this.tileSelectorGroupBox.Text = "Tile Selector";
            // 
            // recepticalLabel
            // 
            this.recepticalLabel.AutoSize = true;
            this.recepticalLabel.Location = new System.Drawing.Point(0, 216);
            this.recepticalLabel.Name = "recepticalLabel";
            this.recepticalLabel.Size = new System.Drawing.Size(62, 13);
            this.recepticalLabel.TabIndex = 9;
            this.recepticalLabel.Text = "Receptacle";
            this.recepticalLabel.Click += new System.EventHandler(this.recepticalLabel_Click);
            // 
            // recepticalPictureBox
            // 
            this.recepticalPictureBox.BackColor = System.Drawing.Color.Blue;
            this.recepticalPictureBox.Location = new System.Drawing.Point(62, 204);
            this.recepticalPictureBox.Name = "recepticalPictureBox";
            this.recepticalPictureBox.Size = new System.Drawing.Size(40, 40);
            this.recepticalPictureBox.TabIndex = 8;
            this.recepticalPictureBox.TabStop = false;
            this.recepticalPictureBox.Click += new System.EventHandler(this.recepticalPictureBox_Click);
            // 
            // closedDoorPictureBox
            // 
            this.closedDoorPictureBox.BackColor = System.Drawing.Color.Brown;
            this.closedDoorPictureBox.Location = new System.Drawing.Point(62, 158);
            this.closedDoorPictureBox.Name = "closedDoorPictureBox";
            this.closedDoorPictureBox.Size = new System.Drawing.Size(40, 40);
            this.closedDoorPictureBox.TabIndex = 7;
            this.closedDoorPictureBox.TabStop = false;
            this.closedDoorPictureBox.Click += new System.EventHandler(this.closedDoorPictureBox_Click);
            // 
            // closedDoorLabel
            // 
            this.closedDoorLabel.AutoSize = true;
            this.closedDoorLabel.Location = new System.Drawing.Point(-1, 172);
            this.closedDoorLabel.Name = "closedDoorLabel";
            this.closedDoorLabel.Size = new System.Drawing.Size(65, 13);
            this.closedDoorLabel.TabIndex = 6;
            this.closedDoorLabel.Text = "Closed Door";
            // 
            // openDoorLabel
            // 
            this.openDoorLabel.AutoSize = true;
            this.openDoorLabel.Location = new System.Drawing.Point(3, 127);
            this.openDoorLabel.Name = "openDoorLabel";
            this.openDoorLabel.Size = new System.Drawing.Size(59, 13);
            this.openDoorLabel.TabIndex = 5;
            this.openDoorLabel.Text = "Open Door";
            // 
            // floorLabel
            // 
            this.floorLabel.AutoSize = true;
            this.floorLabel.Location = new System.Drawing.Point(10, 80);
            this.floorLabel.Name = "floorLabel";
            this.floorLabel.Size = new System.Drawing.Size(30, 13);
            this.floorLabel.TabIndex = 4;
            this.floorLabel.Text = "Floor";
            // 
            // wallLabel
            // 
            this.wallLabel.AutoSize = true;
            this.wallLabel.Location = new System.Drawing.Point(12, 33);
            this.wallLabel.Name = "wallLabel";
            this.wallLabel.Size = new System.Drawing.Size(28, 13);
            this.wallLabel.TabIndex = 3;
            this.wallLabel.Text = "Wall";
            // 
            // openDoorPictureBox
            // 
            this.openDoorPictureBox.BackColor = System.Drawing.Color.Peru;
            this.openDoorPictureBox.Location = new System.Drawing.Point(62, 112);
            this.openDoorPictureBox.Name = "openDoorPictureBox";
            this.openDoorPictureBox.Size = new System.Drawing.Size(40, 40);
            this.openDoorPictureBox.TabIndex = 2;
            this.openDoorPictureBox.TabStop = false;
            this.openDoorPictureBox.Click += new System.EventHandler(this.openDoorPictureBox_Click);
            // 
            // wallPictureBox
            // 
            this.wallPictureBox.BackColor = System.Drawing.Color.Black;
            this.wallPictureBox.Location = new System.Drawing.Point(62, 20);
            this.wallPictureBox.Name = "wallPictureBox";
            this.wallPictureBox.Size = new System.Drawing.Size(40, 40);
            this.wallPictureBox.TabIndex = 1;
            this.wallPictureBox.TabStop = false;
            this.wallPictureBox.Click += new System.EventHandler(this.wallPictureBox_Click);
            // 
            // floorPictureBox
            // 
            this.floorPictureBox.BackColor = System.Drawing.Color.Gray;
            this.floorPictureBox.Location = new System.Drawing.Point(62, 66);
            this.floorPictureBox.Name = "floorPictureBox";
            this.floorPictureBox.Size = new System.Drawing.Size(40, 40);
            this.floorPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.floorPictureBox.TabIndex = 0;
            this.floorPictureBox.TabStop = false;
            this.floorPictureBox.Click += new System.EventHandler(this.floorPictureBox_Click);
            // 
            // currentTileGroupBox
            // 
            this.currentTileGroupBox.Controls.Add(this.currentTilePictureBox);
            this.currentTileGroupBox.Location = new System.Drawing.Point(45, 368);
            this.currentTileGroupBox.Name = "currentTileGroupBox";
            this.currentTileGroupBox.Size = new System.Drawing.Size(59, 63);
            this.currentTileGroupBox.TabIndex = 1;
            this.currentTileGroupBox.TabStop = false;
            this.currentTileGroupBox.Text = "Current";
            // 
            // currentTilePctureBox
            // 
            this.currentTilePictureBox.BackColor = System.Drawing.Color.Black;
            this.currentTilePictureBox.Location = new System.Drawing.Point(15, 19);
            this.currentTilePictureBox.Name = "currentTilePctureBox";
            this.currentTilePictureBox.Size = new System.Drawing.Size(30, 30);
            this.currentTilePictureBox.TabIndex = 0;
            this.currentTilePictureBox.TabStop = false;
            // 
            // saveFileButton
            // 
            this.saveFileButton.Location = new System.Drawing.Point(77, 437);
            this.saveFileButton.Name = "saveFileButton";
            this.saveFileButton.Size = new System.Drawing.Size(62, 31);
            this.saveFileButton.TabIndex = 2;
            this.saveFileButton.Text = "Save File";
            this.saveFileButton.UseVisualStyleBackColor = true;
            this.saveFileButton.Click += new System.EventHandler(this.saveFileButton_Click);
            // 
            // loadFileButton
            // 
            this.loadFileButton.Location = new System.Drawing.Point(9, 437);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(62, 31);
            this.loadFileButton.TabIndex = 3;
            this.loadFileButton.Text = "Load File";
            this.loadFileButton.UseVisualStyleBackColor = true;
            this.loadFileButton.Click += new System.EventHandler(this.loadFileButton_Click);
            // 
            // mapGroupBox
            // 
            this.mapGroupBox.Location = new System.Drawing.Point(148, 12);
            this.mapGroupBox.Name = "mapGroupBox";
            this.mapGroupBox.Size = new System.Drawing.Size(640, 456);
            this.mapGroupBox.TabIndex = 4;
            this.mapGroupBox.TabStop = false;
            this.mapGroupBox.Text = "Past Level Map";
            // 
            // togglePastFuture
            // 
            this.togglePastFuture.Location = new System.Drawing.Point(15, 9);
            this.togglePastFuture.Name = "togglePastFuture";
            this.togglePastFuture.Size = new System.Drawing.Size(118, 22);
            this.togglePastFuture.TabIndex = 5;
            this.togglePastFuture.Text = "Future Level";
            this.togglePastFuture.UseVisualStyleBackColor = true;
            this.togglePastFuture.Click += new System.EventHandler(this.togglePastFuture_Click);
            // 
            // interactableSelectorGroupBox
            // 
            this.interactableSelectorGroupBox.Controls.Add(this.interactableSelectorComboBox);
            this.interactableSelectorGroupBox.Location = new System.Drawing.Point(9, 305);
            this.interactableSelectorGroupBox.Name = "interactableSelectorGroupBox";
            this.interactableSelectorGroupBox.Size = new System.Drawing.Size(130, 57);
            this.interactableSelectorGroupBox.TabIndex = 6;
            this.interactableSelectorGroupBox.TabStop = false;
            this.interactableSelectorGroupBox.Text = "Interactable Selector";
            // 
            // interactableSelectorComboBox
            // 
            this.interactableSelectorComboBox.FormattingEnabled = true;
            this.interactableSelectorComboBox.Items.AddRange(new object[] {
            "None",
            "Chicken PastNothing",
            "Chicken Egg",
            "Chicken Chick",
            "Chicken Chicken",
            "Chicken Bones",
            "Chicken FutureNothing",
            "Flower Dirt",
            "Flower WateredDirt",
            "Flower Seeds",
            "Flower WateredSeeds",
            "Flower Flower",
            "Flower WateredFlower",
            "Tree PastNothing",
            "Tree Sapling",
            "Tree Tree",
            "Tree DeadTree",
            "Tree FutureNothing",
            "WateringCan  WateringCan"});
            this.interactableSelectorComboBox.Location = new System.Drawing.Point(6, 19);
            this.interactableSelectorComboBox.Name = "interactableSelectorComboBox";
            this.interactableSelectorComboBox.Size = new System.Drawing.Size(121, 21);
            this.interactableSelectorComboBox.TabIndex = 0;
            this.interactableSelectorComboBox.SelectedIndexChanged += new System.EventHandler(this.interactableSelectorComboBox_SelectedIndexChanged);
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 480);
            this.Controls.Add(this.togglePastFuture);
            this.Controls.Add(this.interactableSelectorGroupBox);
            this.Controls.Add(this.currentTileGroupBox);
            this.Controls.Add(this.loadFileButton);
            this.Controls.Add(this.mapGroupBox);
            this.Controls.Add(this.saveFileButton);
            this.Controls.Add(this.tileSelectorGroupBox);
            this.Name = "Editor";
            this.Text = " ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Editor_FormClosing);
            this.tileSelectorGroupBox.ResumeLayout(false);
            this.tileSelectorGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recepticalPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closedDoorPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.openDoorPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wallPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.floorPictureBox)).EndInit();
            this.currentTileGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.currentTilePictureBox)).EndInit();
            this.interactableSelectorGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox tileSelectorGroupBox;
        private System.Windows.Forms.PictureBox openDoorPictureBox;
        private System.Windows.Forms.PictureBox wallPictureBox;
        private System.Windows.Forms.PictureBox floorPictureBox;
        private System.Windows.Forms.GroupBox currentTileGroupBox;
        private System.Windows.Forms.PictureBox currentTilePictureBox;
        private System.Windows.Forms.Button saveFileButton;
        private System.Windows.Forms.Button loadFileButton;
        private System.Windows.Forms.GroupBox mapGroupBox;
        private System.Windows.Forms.Button togglePastFuture;
        private System.Windows.Forms.Label openDoorLabel;
        private System.Windows.Forms.Label floorLabel;
        private System.Windows.Forms.Label wallLabel;
        private System.Windows.Forms.Label closedDoorLabel;
        private System.Windows.Forms.PictureBox closedDoorPictureBox;
        private System.Windows.Forms.Label recepticalLabel;
        private System.Windows.Forms.PictureBox recepticalPictureBox;
        private System.Windows.Forms.GroupBox interactableSelectorGroupBox;
        private System.Windows.Forms.ComboBox interactableSelectorComboBox;
    }
}