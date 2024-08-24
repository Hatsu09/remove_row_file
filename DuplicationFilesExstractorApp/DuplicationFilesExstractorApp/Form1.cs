using Microsoft.VisualBasic.FileIO;
using System.CodeDom;

namespace DuplicationFilesExstractorApp
{
    public partial class MainForm : Form
    {
        // ���o�Ώۊg���q���X�g
        public static List<string> TARGET_EXTENSIONS = new List<string>() { ".jpg", ".jpeg", ".JPG", "JPEG" };

        // �ꎞ�f�B���N�g����(Work)
        public static string WORK_DIR_NAME = "work";

        // �ꎞ�f�B���N�g��(trash)
        public static string TRASH_DIR_NAME = "trash";

        /// <summary>
        /// ���C�����\�b�h
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// �Ώۃt�@�C���I���{�^�����������\�b�h
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
        /// �d���t�@�C���ȊO���폜�{�^�����������\�b�h
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveDuplicateFileBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(TargetFilePathTextBox.Text))
            {
                displayMsgBox("�Ώۃt�H���_�[��I�����Ă�������", "�Ώۃt�H���_�[�p�X���I������Ă��܂���");
                return;
            }

            if (!Directory.Exists(TargetFilePathTextBox.Text))
            {
                displayMsgBox("�Ώۃt�H���_�[�����݂��܂���", "�Ώۃt�@�C���p�X�����݂��܂���");
                return;
            }

            // �������s�O�m�F
            MessageBoxButtons messageBoxButtons = MessageBoxButtons.YesNo;
            if (MessageBox.Show("�t�@�C�������d�����Ă��Ȃ��t�@�C�����폜���Ă���낵���ł��傤���H", "�m�F", messageBoxButtons) == DialogResult.Yes)
            {
                Dictionary<string, int> fileCntDict = removeDoesNotDuplicateFile(TargetFilePathTextBox.Text);
                string fileCntContents = "";
                foreach (KeyValuePair<string, int> kvp in fileCntDict)
                {
                    fileCntContents += "\n" + kvp.Key + "�F" + kvp.Value;
                }
                displayMsgBox("�����I��", $"�������I�����܂����B\n{fileCntContents}");
                return;
            }

            displayMsgBox("�������f", "�������s�O�ɒ��f���܂����B");
            return;

        }

        /// <summary>
        /// ���b�Z�[�W�{�b�N�X�\�����\�b�h
        /// </summary>
        /// <param name="caption">�^�C�g��</param>
        /// <param name="message">�{��</param>
        private void displayMsgBox(string caption, string message)
        {
            MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK;
            DialogResult msgBox = MessageBox.Show(message, caption, messageBoxButtons);
        }

        /// <summary>
        /// �t�@�C���폜���\�b�h
        /// �ȉ��̏����̃t�@�C�����폜����B
        /// �E�Ώۊg���q�ł͂Ȃ��t�@�C���B
        /// �E�����f�B���N�g�����ɓ����t�@�C�����̑Ώۊg���q�t�@�C�������݂��Ȃ��t�@�C���B
        /// </summary>
        /// <param name="targetDirPath">���[�U�����C����ʂŎw�肵���f�B���N�g���p�X</param>
        /// <returns>�����t�@�C���������i�[����Dictonary</returns>
        private Dictionary<string, int> removeDoesNotDuplicateFile(string targetDirPath)
        {
            // �w�肳�ꂽ�f�B���N�g���Ɠ����f�B���N�g���Ɉꎞ�f�B���N�g���̍쐬
            string[] dirPathPerDirectoryArr = targetDirPath.Split('\\');
            // ��ƃf�B���N�g��(���o�Ώۃt�@�C�����i�[���邽�߂̃f�B���N�g��)���쐬
            dirPathPerDirectoryArr[dirPathPerDirectoryArr.Length - 1] = WORK_DIR_NAME;
            string workDirPath = string.Join("\\", dirPathPerDirectoryArr);
            makeDir(workDirPath);
            // ��ƃf�B���N�g��(���o�ΏۊO�t�@�C�����i�[���邽�߂̃f�B���N�g��)���쐬
            dirPathPerDirectoryArr[dirPathPerDirectoryArr.Length - 1] = TRASH_DIR_NAME;
            string trashDirPath = string.Join("\\", dirPathPerDirectoryArr);
            makeDir(trashDirPath);

            // �w��f�B���N�g�����̃t�@�C�����o����
            copyTargetFileToWorkDir(targetDirPath, workDirPath, trashDirPath);

            Dictionary<string, int> fileCntDict = new Dictionary<string, int>();
            fileCntDict.Add("���o�Ώۃt�@�C����", returnFileCntInTargetDir(workDirPath));
            fileCntDict.Add("���o�ΏۊO�t�@�C����", returnFileCntInTargetDir(trashDirPath));
            fileCntDict.Add("�S�̂̃t�@�C����", returnFileCntInTargetDir(targetDirPath));

            if (Directory.GetFileSystemEntries(targetDirPath).Length == 0)
            {
                // �Ώۃt�@�C�����Ȃ��ꍇ�A�t�@�C���̍폜�͍s�킸�A�ꎞ�f�B���N�g�����폜���ďI��
                displayMsgBox("�������f", "���o�Ώۂ̃t�@�C�������݂��Ȃ��������߁A�����𒆒f���܂��B�t�@�C���̍폜�͍s���܂���B");
                Directory.Delete(workDirPath, true);
                Directory.Delete(trashDirPath, true);
                return fileCntDict;
            }

            try
            {
                // trash�t�H���_�[���S�~���ֈړ��B
                FileSystem.DeleteDirectory(@trashDirPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);

                // �t�@�C���̃R�s�[���I�������A���f�B���N�g���ƃT�u�f�B���N�g�������S�폜����B
                Directory.Delete(targetDirPath, true);

                // work�t�H���_�[�����l�[������B
                Directory.Move(workDirPath, targetDirPath);

            }
            catch(Exception e) {
                displayMsgBox("�������f", "�t�@�C���̍폜�����Ɏ��s���܂����B");
            }
            return fileCntDict;
            
        }

        /// <summary>
        /// �w�肵���f�B���N�g���p�X���쐬���郁�\�b�h
        /// </summary>
        /// <param name="targetDirPath">�w��f�B���N�g���p�X</param>
        private void makeDir(string targetDirPath)
        {
            string[] dirPathPerBSlashArr = targetDirPath.Split("\\");
            string dirName = dirPathPerBSlashArr[dirPathPerBSlashArr.Length - 1];
            int i = 1;
            while (Directory.Exists(targetDirPath))
            {
                // ���łɓ����̃f�B���N�g�������݂���ꍇ�́A�t�@�C���������ɐ��������čēx���݊m�F����B
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
        /// �폜�ΏۂłȂ��t�@�C�����ꎞ�t�H���_�[�ɃR�s�[���郁�\�b�h
        /// </summary>
        /// <param name="targetDirPath">�R�s�[���f�B���N�g���p�X</param>
        /// <param name="destinationDirPath">���o�Ώۃt�@�C���i�[�f�B���N�g���p�X</param>
        /// <param name="trashDirPath">���o�ΏۊO�t�@�C���i�[�f�B���N�g���p�X</param>
        private void copyTargetFileToWorkDir(string targetDirPath, string destinationDirPath, string trashDirPath)
        {
            // �w��f�B���N�g���z���̃t�@�C�����擾
            string[] targetDirFiles = Directory.GetFiles(targetDirPath);

            // �Ώۊg���q�Ńt�@�C���𒊏o
            List<string> targetFilesWithoutExtensions = new List<string>();
            foreach (string file in targetDirFiles)
            {
                if (TARGET_EXTENSIONS.Contains(Path.GetExtension(file)))
                {
                    targetFilesWithoutExtensions.Add(Path.GetFileNameWithoutExtension(file));
                }
            }

            // �t�@�C����(�g���q������)�d��������f�B���N�g�����ɑ��݂���t�@�C���𒊏o
            foreach (string targetFile in targetDirFiles)
            {
                if (targetFilesWithoutExtensions.Contains(Path.GetFileNameWithoutExtension(targetFile)))
                {
                    copyTargetFile(targetFile, destinationDirPath);
                    continue;
                }
                copyTargetFile(targetFile, trashDirPath);
            }

            // �w��f�B���N�g���z���̃f�B���N�g�����擾
            string[] targetDirs = Directory.GetDirectories(targetDirPath);
            foreach (string dir in targetDirs)
            {
                // �f�B���N�g���̏ꍇ�́A�T�u�f�B���N�g�����쐬���āA�ċA�I�ɏ���
                copyTargetFileToWorkDir(targetDirPath + "\\" + Path.GetFileName(dir), destinationDirPath + "\\" + Path.GetFileName(dir), trashDirPath);
            }
        }

        /// <summary>
        /// �Ώۃt�@�C���R�s�[���\�b�h
        /// </summary>
        /// <param name="targetFilePath">��r���̃t�@�C���p�X(�㏑������)</param>
        /// <param name="destinationDirPath">�R�s�[��̃f�B���N�g���p�X</param>
        private void copyTargetFile(string targetFilePath, string destinationDirPath)
        {
            try
            {
                if (!Directory.Exists(destinationDirPath))
                {
                    Directory.CreateDirectory(destinationDirPath);
                }
                // �����̃t�@�C�����A�ق���2�ȏ㑶�݂���ꍇ�A�Ώۂ̊g���q�t�@�C����2��ڈȍ~�̃R�s�[�͂́A�㏑��
                File.Copy(targetFilePath, destinationDirPath + "\\" + Path.GetFileName(targetFilePath));
            }
            catch (Exception ex)
            {
                Console.WriteLine("�t�@�C���̃R�s�[�Ɏ��s���܂����B");
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// �w�肵���f�B���N�g���z���̃t�@�C������ԋp����B
        /// </summary>
        /// <param name="targetDirPath">�f�B���N�g���p�X</param>
        /// <returns>�f�B���N�g���z���̃t�@�C����</returns>
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
