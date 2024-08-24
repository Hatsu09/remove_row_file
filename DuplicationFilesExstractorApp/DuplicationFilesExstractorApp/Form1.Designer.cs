namespace DuplicationFilesExstractorApp
{
    partial class MainForm
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
            Title = new Label();
            SelectTargetFileDialogBtn = new Button();
            RemoveDuplicateFileBtn = new Button();
            TargetFilePathTextBox = new TextBox();
            SelectTargetDirectoryDialog = new FolderBrowserDialog();
            SuspendLayout();
            // 
            // Title
            // 
            Title.AutoSize = true;
            Title.Font = new Font("Yu Gothic UI", 15F);
            Title.Location = new Point(16, 10);
            Title.Name = "Title";
            Title.Size = new Size(193, 28);
            Title.TabIndex = 0;
            Title.Text = "重複ファイル名抽出AP";
            // 
            // SelectTargetFileDialogBtn
            // 
            SelectTargetFileDialogBtn.Location = new Point(15, 102);
            SelectTargetFileDialogBtn.Name = "SelectTargetFileDialogBtn";
            SelectTargetFileDialogBtn.Size = new Size(100, 50);
            SelectTargetFileDialogBtn.TabIndex = 1;
            SelectTargetFileDialogBtn.Text = "ファイル選択";
            SelectTargetFileDialogBtn.UseVisualStyleBackColor = true;
            SelectTargetFileDialogBtn.Click += SelectTargetFileDialogBtn_Click;
            // 
            // RemoveDuplicateFileBtn
            // 
            RemoveDuplicateFileBtn.Location = new Point(134, 102);
            RemoveDuplicateFileBtn.Name = "RemoveDuplicateFileBtn";
            RemoveDuplicateFileBtn.Size = new Size(100, 50);
            RemoveDuplicateFileBtn.TabIndex = 2;
            RemoveDuplicateFileBtn.Text = "重複ファイル名以外を削除する";
            RemoveDuplicateFileBtn.UseVisualStyleBackColor = true;
            RemoveDuplicateFileBtn.Click += RemoveDuplicateFileBtn_Click;
            // 
            // TargetFilePathTextBox
            // 
            TargetFilePathTextBox.AccessibleDescription = "対象ディレクトリパス";
            TargetFilePathTextBox.AllowDrop = true;
            TargetFilePathTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TargetFilePathTextBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            TargetFilePathTextBox.Font = new Font("Yu Gothic UI", 10F);
            TargetFilePathTextBox.HideSelection = false;
            TargetFilePathTextBox.Location = new Point(15, 60);
            TargetFilePathTextBox.MaximumSize = new Size(750, 25);
            TargetFilePathTextBox.MinimumSize = new Size(100, 25);
            TargetFilePathTextBox.Name = "TargetFilePathTextBox";
            TargetFilePathTextBox.ScrollBars = ScrollBars.Horizontal;
            TargetFilePathTextBox.Size = new Size(750, 25);
            TargetFilePathTextBox.TabIndex = 3;
            // 
            // SelectTargetDirectoryDialog
            // 
            SelectTargetDirectoryDialog.Description = "対象のディレクトリを選択してください";
            SelectTargetDirectoryDialog.OkRequiresInteraction = true;
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(792, 326);
            Controls.Add(TargetFilePathTextBox);
            Controls.Add(RemoveDuplicateFileBtn);
            Controls.Add(SelectTargetFileDialogBtn);
            Controls.Add(Title);
            MaximizeBox = false;
            Name = "MainForm";
            Padding = new Padding(1);
            Text = "重複ファイル名抽出AP";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Title;
        private Button SelectTargetFileDialogBtn;
        private Button RemoveDuplicateFileBtn;
        private TextBox TargetFilePathTextBox;
        private FolderBrowserDialog SelectTargetDirectoryDialog;
    }
}
