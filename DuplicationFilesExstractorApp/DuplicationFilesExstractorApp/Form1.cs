using System.CodeDom;

namespace DuplicationFilesExstractorApp
{
    public partial class MainForm : Form
    {
        public static List<string> targetExtensiions = new List<string>() { ".jpg", ".jpeg" };

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 対象ファイル選択ボタン押下時メソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectTargetFileDialogBtn_Click(object sender, EventArgs e)
        {
            if (SelectTargetDirectoryDialog.ShowDialog() == DialogResult.OK)
            {
                TargetFilePathTextBox.Text = SelectTargetDirectoryDialog.SelectedPath;
            }
        }

        /// <summary>
        /// 重複ファイル以外を削除ボタン押下時メソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveDuplicateFileBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(TargetFilePathTextBox.Text))
            {
                displayMsgBox("対象フォルダーを選択してください", "対象フォルダーパスが選択されていません");
                return;
            }

            if (!Directory.Exists(TargetFilePathTextBox.Text))
            {
                displayMsgBox("対象フォルダーが存在しません", "対象ファイルパスが存在しません");
                return;
            }
            removeDoesNotDuplicateFile(TargetFilePathTextBox.Text);

            displayMsgBox("処理終了", "処理が終了しました。");
        }

        /// <summary>
        /// メッセージボックス表示メソッド
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        private void displayMsgBox(string caption, string message)
        {
            MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK;
            DialogResult msgBox = MessageBox.Show(message, caption, messageBoxButtons);
        }


        private void removeDoesNotDuplicateFile(string targetDirPath)
        {
            // 指定されたディレクトリと同じディレクトリにworkフォルダーを作成
            string[] dirPathPerDirectoryArr = targetDirPath.Split('\\');
            dirPathPerDirectoryArr[dirPathPerDirectoryArr.Length - 1] = "work";
            string workDirPath = string.Join("\\", dirPathPerDirectoryArr);
            //foreach (string dir in dirPathPerDirectoryArr)
            //{
            //    workDirPath = workDirPath + dir;
            //    if (!dir.Equals(dirPathPerDirectoryArr.Last()))
            //    {
            //        workDirPath = workDirPath + "\\";
            //    }
            //}
            int i = 1;
            while (Directory.Exists(workDirPath))
            {
                string[] workDirPathPerBSlashArr = workDirPath.Split("\\");
                workDirPathPerBSlashArr[workDirPathPerBSlashArr.Length - 1] = "work_" + i;
                workDirPath = string.Join("\\", workDirPathPerBSlashArr);
                i++;
            }
            try
            {
                Directory.CreateDirectory(workDirPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("workフォルダーの作成に失敗しました。");
                Console.WriteLine(ex.ToString());
                return;
            }

            // 同一ディレクトリ内で、対象拡張子ファイルと拡張子以外のファイル名が重複するファイル両方をworkフォルダーにコピー
            copyTargetFileToWorkDir(targetDirPath, workDirPath, targetExtensiions);

            //TODO: 処理の実装
            // コピーが終わったら、元ディレクトリを削除する。

            // workフォルダーをリネームする。

        }

        private void copyTargetFileToWorkDir(string targetDirPath, string destinationDirPath, List<string> targetExtensions)
        {
            // 指定ディレクトリ配下のファイルを取得
            string[] targetDirFiles = Directory.GetFiles(targetDirPath);

            // 対象拡張子でファイルを抽出
            List<string> targetExtensionsFileList = new List<string>();
            foreach (string file in targetDirFiles)
            {
                if (targetExtensions.Contains(Path.GetExtension(file)))
                {
                    Console.WriteLine("target file(target extensions):" + file);
                    targetExtensionsFileList.Add(file);
                }
            }

            // ファイル名(拡張子を除く)重複が同一ディレクトリ内に存在するファイルを抽出
            foreach (string targetExtensionsFilePath in targetExtensionsFileList)
            {
                foreach (string otherFilePath in targetDirFiles.Except(targetExtensionsFileList))
                {
                    if (Path.GetFileNameWithoutExtension(targetExtensionsFilePath).Equals(Path.GetFileNameWithoutExtension(otherFilePath)))
                    {
                        Console.WriteLine("target file(duplicate file name):" + targetExtensionsFilePath);
                        Console.WriteLine("target file(duplicate file name):" + otherFilePath);
                        copyTargetFile(targetExtensionsFilePath, otherFilePath, destinationDirPath);
                    }
                }
            }

            // 指定ディレクトリ配下のディレクトリを取得
            string[] targetDirs = Directory.GetDirectories(targetDirPath);
            foreach (string dir in targetDirs)
            {
                // ディレクトリの場合は、サブディレクトリを作成して、再帰的に処理
                copyTargetFileToWorkDir(targetDirPath + "\\" + Path.GetFileName(dir), destinationDirPath + "\\" + Path.GetFileName(dir), targetExtensions);
            }
        }

        /// <summary>
        /// 対象ファイルコピーメソッド
        /// </summary>
        /// <param name="targetExtensionsFilePath">比較元のファイルパス(上書きあり)</param>
        /// <param name="otherFilePath">比較対象ファイルパス(上書きなし)</param>
        /// <param name="destinationDirPath">コピー先のディレクトリパス</param>
        private void copyTargetFile(string targetExtensionsFilePath, string otherFilePath, string destinationDirPath)
        {
            try
            {
                if (!Directory.Exists(destinationDirPath))
                {
                    Directory.CreateDirectory(destinationDirPath);
                }
                // 同名のファイルが、ほかに2つ以上存在する場合、対象の拡張子ファイルの2回目以降のコピーはは、上書き
                File.Copy(targetExtensionsFilePath, destinationDirPath + "\\" + Path.GetFileName(targetExtensionsFilePath), true);
                File.Copy(otherFilePath, destinationDirPath + "\\" + Path.GetFileName(otherFilePath));
            }
            catch (Exception ex)
            {
                Console.WriteLine("ファイルのコピーに失敗しました。");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
