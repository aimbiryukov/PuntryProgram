using System;
using System.Data.Linq;
using System.Windows.Forms;

namespace PuntryProgram.Classes
{ 
    public static class FileMethod
    {
        public static void LoadFile(int userId)
        {
            try
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "Выбор файла";
                    openFileDialog.Multiselect = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (var path in openFileDialog.FileNames)
                        {
                            LoadFile(userId, path);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string FileSize(double bytes)
        {
            try
            {
                var kBytes = Math.Round((bytes / 1024), 2);
                var mBytes = Math.Round((kBytes / 1024), 2);
                var gBytes = Math.Round((mBytes / 1024), 2);

                return (gBytes > 1) ? gBytes + " GB" : (mBytes > 1) ? mBytes + " MB" : (kBytes > 1) ? kBytes + " KB" : bytes + " B";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static void LoadFile(int userId, string path)
        {
            try
            {
                var name = System.IO.Path.GetFileNameWithoutExtension(path);
                var expansion = System.IO.Path.GetExtension(path).ToLower();
                var binary = FileToBinary(path);
                var size = new System.IO.FileInfo(path);

                if (DataMethod.CheckFile(name, expansion, userId))
                {
                    if (MessageBox.Show("Данный файл уже существует в вашем хранилище. Желаете заменить его?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        DataMethod.ReplaceFile(name, expansion, (int)size.Length, binary, userId);
                }
                else
                    DataMethod.InsertFile(name, expansion, (int)size.Length, binary, false, userId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void DownloadFile(int userId, int fileId)
        {
            try
            {
                using (var saveFileDialog = new SaveFileDialog())
                {
                    FileStruct fileStruct = DataMethod.GetFile(fileId);

                    saveFileDialog.Title = "Скачивание файла";
                    saveFileDialog.FileName = fileStruct.name + fileStruct.extension;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        BinaryToFile(saveFileDialog.FileName, fileStruct.binary);

                        DataMethod.InsertFileChanges(userId, fileId, "Файл был скачен.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static Binary FileToBinary(string name)
        {
            try
            {
                return System.IO.File.ReadAllBytes(name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static void BinaryToFile(string path, Binary binary)
        {
            try
            {
                System.IO.File.WriteAllBytes(path, binary.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
