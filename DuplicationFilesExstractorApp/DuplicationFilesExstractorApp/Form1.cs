using Microsoft.VisualBasic.FileIO;
using System.CodeDom;

namespace DuplicationFilesExstractorApp
{
    public partial class MainForm : Form
    {
        // 抽出対象拡張子リスト
        public static List<string> TARGET_EXTENSIONS = new List<string>() { ".jpg", ".jpeg", ".JPG", "JPEG" };

        // 一時ディレクトリ名(Work)
        public static string WORK_DIR_NAME = "work";

        // 一時ディレクトリ(trash)
        public static string TRASH_DIR_NAME = "trash";

        /// <summary>
        /// メインメソッド
        /// </summary>
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

            // 処理実行前確認
            MessageBoxButtons messageBoxButtons = MessageBoxButtons.YesNo;
            if (MessageBox.Show("ファイル名が重複していないファイルを削除してもよろしいでしょうか？", "確認", messageBoxButtons) == DialogResult.Yes)
            {
                Dictionary<string, int> fileCntDict = removeDoesNotDuplicateFile(TargetFilePathTextBox.Text);
                string fileCntContents = "";
                foreach (KeyValuePair<string, int> kvp in fileCntDict)
                {
                    fileCntContents += "\n" + kvp.Key + "：" + kvp.Value;
                }
                displayMsgBox("処理終了", $"処理が終了しました。\n{fileCntContents}");
                return;
            }

            displayMsgBox("処理中断", "処理実行前に中断しました。");
            return;

        }

        /// <summary>
        /// メッセージボックス表示メソッド
        /// </summary>
        /// <param name="caption">タイトル</param>
        /// <param name="message">本文</param>
        private void displayMsgBox(string caption, string message)
        {
            MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK;
            DialogResult msgBox = MessageBox.Show(message, caption, messageBoxButtons);
        }

        /// <summary>
        /// ファイル削除メソッド
        /// 以下の条件のファイルを削除する。
        /// ・対象拡張子ではないファイル。
        /// ・同じディレクトリ内に同じファイル名の対象拡張子ファイルが存在しないファイル。
        /// </summary>
        /// <param name="targetDirPath">ユーザがメイン画面で指定したディレクトリパス</param>
        /// <returns>処理ファイル件数を格納したDictonary</returns>
        private Dictionary<string, int> removeDoesNotDuplicateFile(string targetDirPath)
        {
            // 指定されたディレクトリと同じディレクトリに一時ディレクトリの作成
            string[] dirPathPerDirectoryArr = targetDirPath.Split('\\');
            // 作業ディレクトリ(抽出対象ファイルを格納するためのディレクトリ)を作成
            dirPathPerDirectoryArr[dirPathPerDirectoryArr.Length - 1] = WORK_DIR_NAME;
            string workDirPath = string.Join("\\", dirPathPerDirectoryArr);
            makeDir(workDirPath);
            // 作業ディレクトリ(抽出対象外ファイルを格納するためのディレクトリ)を作成
            dirPathPerDirectoryArr[dirPathPerDirectoryArr.Length - 1] = TRASH_DIR_NAME;
            string trashDirPath = string.Join("\\", dirPathPerDirectoryArr);
            makeDir(trashDirPath);

            // 指定ディレクトリ内のファイル抽出処理
            copyTargetFileToWorkDir(targetDirPath, workDirPath, trashDirPath);

            Dictionary<string, int> fileCntDict = new Dictionary<string, int>();
            fileCntDict.Add("抽出対象ファイル数", returnFileCntInTargetDir(workDirPath));
            fileCntDict.Add("抽出対象外ファイル数", returnFileCntInTargetDir(trashDirPath));
            fileCntDict.Add("全体のファイル数", returnFileCntInTargetDir(targetDirPath));

            if (Directory.GetFileSystemEntries(targetDirPath).Length == 0)
            {
                // 対象ファイルがない場合、ファイルの削除は行わず、一時ディレクトリを削除して終了
                displayMsgBox("処理中断", "抽出対象のファイルが存在しなかったため、処理を中断します。ファイルの削除は行われません。");
                Directory.Delete(workDirPath, true);
                Directory.Delete(trashDirPath, true);
                return fileCntDict;
            }

            try
            {
                // trashフォルダーをゴミ箱へ移動。
                FileSystem.DeleteDirectory(@trashDirPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);

                // ファイルのコピーが終わったら、元ディレクトリとサブディレクトリを完全削除する。
                Directory.Delete(targetDirPath, true);

                // workフォルダーをリネームする。
                Directory.Move(workDirPath, targetDirPath);

            }
            catch(Exception e) {
                displayMsgBox("処理中断", "ファイルの削除処理に失敗しました。");
            }
            return fileCntDict;
            
        }

        /// <summary>
        /// 指定したディレクトリパスを作成するメソッド
        /// </summary>
        /// <param name="targetDirPath">指定ディレクトリパス</param>
        private void makeDir(string targetDirPath)
        {
            string[] dirPathPerBSlashArr = targetDirPath.Split("\\");
            string dirName = dirPathPerBSlashArr[dirPathPerBSlashArr.Length - 1];
            int i = 1;
            while (Directory.Exists(targetDirPath))
            {
                // すでに同名のディレクトリが存在する場合は、ファイル名末尾に数字を入れて再度存在確認する。
                dirPathPerBSlashArr[dirPathPerBSlashArr.Length - 1] = dirName + "_" + i;
                targetDirPath = string.Join("\\", dirPathPerBSlashArr);
                i++;
            }
            try
            {
                Directory.CreateDirectory(targetDirPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 削除対象でないファイルを一時フォルダーにコピーするメソッド
        /// </summary>
        /// <param name="targetDirPath">コピー元ディレクトリパス</param>
        /// <param name="destinationDirPath">抽出対象ファイル格納ディレクトリパス</param>
        /// <param name="trashDirPath">抽出対象外ファイル格納ディレクトリパス</param>
        private void copyTargetFileToWorkDir(string targetDirPath, string destinationDirPath, string trashDirPath)
        {
            // 指定ディレクトリ配下のファイルを取得
            string[] targetDirFiles = Directory.GetFiles(targetDirPath);

            // 対象拡張子でファイルを抽出
            List<string> targetFilesWithoutExtensions = new List<string>();
            foreach (string file in targetDirFiles)
            {
                if (TARGET_EXTENSIONS.Contains(Path.GetExtension(file)))
                {
                    targetFilesWithoutExtensions.Add(Path.GetFileNameWithoutExtension(file));
                }
            }

            // ファイル名(拡張子を除く)重複が同一ディレクトリ内に存在するファイルを抽出
            foreach (string targetFile in targetDirFiles)
            {
                if (targetFilesWithoutExtensions.Contains(Path.GetFileNameWithoutExtension(targetFile)))
                {
                    copyTargetFile(targetFile, destinationDirPath);
                    continue;
                }
                copyTargetFile(targetFile, trashDirPath);
            }

            // 指定ディレクトリ配下のディレクトリを取得
            string[] targetDirs = Directory.GetDirectories(targetDirPath);
            foreach (string dir in targetDirs)
            {
                // ディレクトリの場合は、サブディレクトリを作成して、再帰的に処理
                copyTargetFileToWorkDir(targetDirPath + "\\" + Path.GetFileName(dir), destinationDirPath + "\\" + Path.GetFileName(dir), trashDirPath);
            }
        }

        /// <summary>
        /// 対象ファイルコピーメソッド
        /// </summary>
        /// <param name="targetFilePath">比較元のファイルパス(上書きあり)</param>
        /// <param name="destinationDirPath">コピー先のディレクトリパス</param>
        private void copyTargetFile(string targetFilePath, string destinationDirPath)
        {
            try
            {
                if (!Directory.Exists(destinationDirPath))
                {
                    Directory.CreateDirectory(destinationDirPath);
                }
                // 同名のファイルが、ほかに2つ以上存在する場合、対象の拡張子ファイルの2回目以降のコピーはは、上書き
                File.Copy(targetFilePath, destinationDirPath + "\\" + Path.GetFileName(targetFilePath));
            }
            catch (Exception ex)
            {
                Console.WriteLine("ファイルのコピーに失敗しました。");
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 指定したディレクトリ配下のファイル数を返却する。
        /// </summary>
        /// <param name="targetDirPath">ディレクトリパス</param>
        /// <returns>ディレクトリ配下のファイル数</returns>
        private int returnFileCntInTargetDir(string targetDirPath)
        {
            int returnCnt = 0;
            
            if (Directory.Exists(targetDirPath))
            {
                returnCnt = Directory.GetFiles(targetDirPath, "*", System.IO.SearchOption.AllDirectories).Length;
            }
            return returnCnt;
        }
    } 
}
