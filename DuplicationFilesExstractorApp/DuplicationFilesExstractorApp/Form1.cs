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
            removeDoesNotDuplicateFile(TargetFilePathTextBox.Text);

            displayMsgBox("�����I��", "�������I�����܂����B");
        }

        /// <summary>
        /// ���b�Z�[�W�{�b�N�X�\�����\�b�h
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
            // �w�肳�ꂽ�f�B���N�g���Ɠ����f�B���N�g����work�t�H���_�[���쐬
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
                Console.WriteLine("work�t�H���_�[�̍쐬�Ɏ��s���܂����B");
                Console.WriteLine(ex.ToString());
                return;
            }

            // ����f�B���N�g�����ŁA�Ώۊg���q�t�@�C���Ɗg���q�ȊO�̃t�@�C�������d������t�@�C��������work�t�H���_�[�ɃR�s�[
            copyTargetFileToWorkDir(targetDirPath, workDirPath, targetExtensiions);

            //TODO: �����̎���
            // �R�s�[���I�������A���f�B���N�g�����폜����B

            // work�t�H���_�[�����l�[������B

        }

        private void copyTargetFileToWorkDir(string targetDirPath, string destinationDirPath, List<string> targetExtensions)
        {
            // �w��f�B���N�g���z���̃t�@�C�����擾
            string[] targetDirFiles = Directory.GetFiles(targetDirPath);

            // �Ώۊg���q�Ńt�@�C���𒊏o
            List<string> targetExtensionsFileList = new List<string>();
            foreach (string file in targetDirFiles)
            {
                if (targetExtensions.Contains(Path.GetExtension(file)))
                {
                    Console.WriteLine("target file(target extensions):" + file);
                    targetExtensionsFileList.Add(file);
                }
            }

            // �t�@�C����(�g���q������)�d��������f�B���N�g�����ɑ��݂���t�@�C���𒊏o
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

            // �w��f�B���N�g���z���̃f�B���N�g�����擾
            string[] targetDirs = Directory.GetDirectories(targetDirPath);
            foreach (string dir in targetDirs)
            {
                // �f�B���N�g���̏ꍇ�́A�T�u�f�B���N�g�����쐬���āA�ċA�I�ɏ���
                copyTargetFileToWorkDir(targetDirPath + "\\" + Path.GetFileName(dir), destinationDirPath + "\\" + Path.GetFileName(dir), targetExtensions);
            }
        }

        /// <summary>
        /// �Ώۃt�@�C���R�s�[���\�b�h
        /// </summary>
        /// <param name="targetExtensionsFilePath">��r���̃t�@�C���p�X(�㏑������)</param>
        /// <param name="otherFilePath">��r�Ώۃt�@�C���p�X(�㏑���Ȃ�)</param>
        /// <param name="destinationDirPath">�R�s�[��̃f�B���N�g���p�X</param>
        private void copyTargetFile(string targetExtensionsFilePath, string otherFilePath, string destinationDirPath)
        {
            try
            {
                if (!Directory.Exists(destinationDirPath))
                {
                    Directory.CreateDirectory(destinationDirPath);
                }
                // �����̃t�@�C�����A�ق���2�ȏ㑶�݂���ꍇ�A�Ώۂ̊g���q�t�@�C����2��ڈȍ~�̃R�s�[�͂́A�㏑��
                File.Copy(targetExtensionsFilePath, destinationDirPath + "\\" + Path.GetFileName(targetExtensionsFilePath), true);
                File.Copy(otherFilePath, destinationDirPath + "\\" + Path.GetFileName(otherFilePath));
            }
            catch (Exception ex)
            {
                Console.WriteLine("�t�@�C���̃R�s�[�Ɏ��s���܂����B");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
