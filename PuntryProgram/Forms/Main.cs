using System;
using System.Drawing;
using System.Windows.Forms;
using PuntryProgram.Classes;

namespace PuntryProgram.Forms
{
    public partial class Main : Form
    {
        private UserStruct _userActive;
        private FileStruct _fileStruct;
        private StatusFileEnum _statusFile;

        public Main(int userId)
        {
            try
            {
                _userActive = DataMethod.GetUser(userId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _statusFile = StatusFileEnum.My;
            _fileStruct = new FileStruct();

            InitializeComponent();

            Icon = Properties.Resources.box;
            ButtonActive(buttonMyFiles);
            FillTable();
            FillForm();
        }

        private void ButtonActive(Button button)
        {
            buttonMyFiles.BackColor = (buttonMyFiles == button) ? SystemColors.Control : Color.White;
            buttonAll.BackColor = (buttonAll == button) ? SystemColors.Control : Color.White;
            buttonProject.BackColor = (buttonProject == button) ? SystemColors.Control : Color.White;
            buttonFavorite.BackColor = (buttonFavorite == button) ? SystemColors.Control : Color.White;
            buttonArchive.BackColor = (buttonArchive == button) ? SystemColors.Control : Color.White;
            buttonReview.BackColor = (buttonReview == button) ? SystemColors.Control : Color.White;
        }

        private void FillTable()
        {
            try
            {
                if (textBoxSearch.Text.Trim() == "")
                {
                    if (_statusFile == StatusFileEnum.Favorite)
                        dataGridViewFiles.DataSource = DataMethod.FavoritesTable(_userActive.userId);
                    else if (_statusFile == StatusFileEnum.All)
                        dataGridViewFiles.DataSource = DataMethod.FilesTable(_userActive.userId, StatusFileEnum.All);
                    else if (_statusFile == StatusFileEnum.Archive)
                        dataGridViewFiles.DataSource = DataMethod.FilesTable(_userActive.userId, StatusFileEnum.Archive);
                    else if (_statusFile == StatusFileEnum.Project)
                        dataGridViewFiles.DataSource = DataMethod.FilesTable(_userActive.userId, StatusFileEnum.Project);
                    else if (_statusFile == StatusFileEnum.Review)
                        dataGridViewFiles.DataSource = DataMethod.FilesTable(_userActive.userId, StatusFileEnum.Review);
                    else
                        dataGridViewFiles.DataSource = DataMethod.FilesTable(_userActive.userId, StatusFileEnum.My);
                }
                else
                {
                    if (_statusFile == StatusFileEnum.Favorite)
                        dataGridViewFiles.DataSource = DataMethod.SearchFavorites(_userActive.userId, textBoxSearch.Text);
                    else if (_statusFile == StatusFileEnum.All)
                        dataGridViewFiles.DataSource = DataMethod.SearchFiles(_userActive.userId, textBoxSearch.Text, StatusFileEnum.All);
                    else if (_statusFile == StatusFileEnum.Archive)
                        dataGridViewFiles.DataSource = DataMethod.SearchFiles(_userActive.userId, textBoxSearch.Text, StatusFileEnum.Archive);
                    else if (_statusFile == StatusFileEnum.Review)
                        dataGridViewFiles.DataSource = DataMethod.SearchFiles(_userActive.userId, textBoxSearch.Text, StatusFileEnum.Review);
                    else if (_statusFile == StatusFileEnum.Project)
                        dataGridViewFiles.DataSource = DataMethod.SearchFiles(_userActive.userId, textBoxSearch.Text, StatusFileEnum.Project);
                    else if (_statusFile == StatusFileEnum.Draft)
                        dataGridViewFiles.DataSource = DataMethod.SearchFiles(_userActive.userId, textBoxSearch.Text, StatusFileEnum.Draft);
                    else
                        dataGridViewFiles.DataSource = DataMethod.SearchFiles(_userActive.userId, textBoxSearch.Text, StatusFileEnum.My);
                }

                if (dataGridViewFiles.DataSource != null)
                {
                    dataGridViewFiles.Columns[0].Visible = false;
                    dataGridViewFiles.RowHeadersDefaultCellStyle.Padding = new Padding(dataGridViewFiles.RowHeadersWidth);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string _checkStatusFile;
        private bool _checkFavorite;

        private void FillForm()
        {
            try
            {
                Text = _userActive.levelName + ": " + _userActive.name + " " + _userActive.surname + " (" + _userActive.login.ToLower() + ")";
                buttonAccounts.Visible = (_userActive.levelName == "Администратор");
                buttonReview.Visible = (_userActive.levelName != "Пользователь");
                buttonAll.Visible = (_userActive.levelName != "Пользователь");
                label2.Visible = (_userActive.levelName != "Пользователь");
                label3.Visible = (_userActive.levelName == "Администратор");

                if (dataGridViewFiles.Rows.Count > 0)
                {
                    _fileStruct = DataMethod.GetFile(_fileStruct.fileId);
                    _checkStatusFile = DataMethod.CheckStatusFile(_fileStruct.fileId);
                    _checkFavorite = DataMethod.CheckFavorite(_userActive.userId, _fileStruct.fileId);

                    reviewToolStripMenuItem.Text = (_checkStatusFile == "На проверке") ? "Отменить проверку" : "Отправить на проверку";
                    reviewToolStripMenuItem.Visible = (_fileStruct.userId == _userActive.userId && _userActive.levelName == "Пользователь" && _checkStatusFile != "Проект" && _statusFile != StatusFileEnum.Archive);
                    favoriteToolStripMenuItem.Text = (_checkFavorite == true) ? "Убрать из избранного" : "Добавить в избранное";
                    favoriteToolStripMenuItem.Visible = (_statusFile != StatusFileEnum.Archive);
                    showToolStripMenuItem.Visible = (_statusFile != StatusFileEnum.Archive);
                    downloadToolStripMenuItem.Visible = (_statusFile != StatusFileEnum.Archive);
                    undeleteStripMenuItem.Visible = (_statusFile == StatusFileEnum.Archive);
                    deleteToolStripMenuItem.Visible = (_fileStruct.userId == _userActive.userId || _userActive.levelName == "Администратор");
                    toolStripSeparator2.Visible = (_statusFile != StatusFileEnum.Archive);
                    toolStripSeparator3.Visible = (_statusFile != StatusFileEnum.Archive && (_fileStruct.userId == _userActive.userId || _userActive.levelName == "Администратор"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteFile()
        {
            try
            {
                if (_statusFile == StatusFileEnum.Archive||( _userActive.levelName == "Администратор"&&_userActive.userId!=_fileStruct.userId))
                {
                    if (MessageBox.Show("Вы действительно желаете безвозвратно удалить данный файл?", "Удалить файл", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        DataMethod.DeleteFile(_fileStruct.fileId);

                        FillTable();
                    }
                }                
                else
                {
                    DataMethod.FileToArchive(_userActive.userId, _fileStruct.fileId, true);
                    MessageBox.Show("Файл помещен в архив.");

                    FillTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestoreFile()
        {
            try
            {
                if (MessageBox.Show("Вы действительно желаете восстановить данный файл?", "Восстановить файл", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataMethod.FileToArchive(_userActive.userId, _fileStruct.fileId, false);

                    FillTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewFiles_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_statusFile != StatusFileEnum.Archive)
            {
                (new FileProperties(_fileStruct.fileId, _userActive.userId)).ShowDialog();

                FillTable();
            }
            else            
                RestoreFile();
        }

        private void dataGridViewFiles_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1 && e.Button == MouseButtons.Right)
            {
                dataGridViewFiles.CurrentCell = dataGridViewFiles[e.ColumnIndex, e.RowIndex];
                dataGridViewFiles.CurrentRow.Selected = true;
            }
        }

        private void dataGridViewFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dataGridViewFiles.Rows.Count > 0)
            {
                e.SuppressKeyPress = true;

                if (_statusFile != StatusFileEnum.Archive)
                {
                    (new FileProperties(_fileStruct.fileId, _userActive.userId)).ShowDialog();

                    FillTable();
                }
                else
                    RestoreFile();
            }
            if (e.KeyCode == Keys.Delete && dataGridViewFiles.Rows.Count > 0)
                DeleteFile();
        }

        private void dataGridViewFiles_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string path in paths)
                {
                    FileMethod.LoadFile(_userActive.userId, path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _statusFile = StatusFileEnum.My;

            ButtonActive(buttonMyFiles);
            FillTable();
        }

        private void dataGridViewFiles_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var dataGridView = sender as DataGridView;
            var rowIndex = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, dataGridView.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIndex, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void dataGridViewFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                e.Effect = DragDropEffects.All;
        }

        private void dataGridViewFiles_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1)
                contextMenuStrip.Show(Cursor.Position);
        }

        private void dataGridViewFiles_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewFiles.Rows.Count > 0)
            {
                _fileStruct.fileId = (int)dataGridViewFiles.Rows[e.RowIndex].Cells[0].Value;

                FillForm();
            }
        }

        private void buttonLoadFile_Click(object sender, EventArgs e)
        {
            try
            {
                FileMethod.LoadFile(_userActive.userId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _statusFile = StatusFileEnum.My;

            ButtonActive(buttonMyFiles);
            FillTable();
        }

        private void buttonMyFiles_Click(object sender, EventArgs e)
        {
            _statusFile = StatusFileEnum.My;

            ButtonActive(buttonMyFiles);
            FillTable();
        }

        private void buttonFavorite_Click(object sender, EventArgs e)
        {
            _statusFile = StatusFileEnum.Favorite;

            ButtonActive(buttonFavorite);
            FillTable();
        }

        private void buttonArchive_Click(object sender, EventArgs e)
        {
            _statusFile = StatusFileEnum.Archive;

            ButtonActive(buttonArchive);
            FillTable();
        }

        private void buttonAll_Click(object sender, EventArgs e)
        {
            _statusFile = StatusFileEnum.All;

            ButtonActive(buttonAll);
            FillTable();
        }

        private void buttonProject_Click(object sender, EventArgs e)
        {
            _statusFile = StatusFileEnum.Project;

            ButtonActive(buttonProject);
            FillTable();
        }

        private void buttonReview_Click(object sender, EventArgs e)
        {
            _statusFile = StatusFileEnum.Review;

            ButtonActive(buttonReview);
            FillTable();
        }

        private void buttonAccounts_Click(object sender, EventArgs e)
        {
            (new UserList(_userActive.userId)).ShowDialog();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FillTable();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewFiles.Rows.Count > 0)
            {
                (new FileProperties(_fileStruct.fileId, _userActive.userId)).ShowDialog();

                FillTable();
            }
        }

        private void reviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_checkStatusFile == "На проверке")
                {
                    DataMethod.UpdateStatusFile(_userActive.userId, _fileStruct.fileId, StatusFileEnum.Draft);
                    MessageBox.Show("Проверка файла отменена.");

                    FillTable();
                }
                else
                {
                    DataMethod.UpdateStatusFile(_userActive.userId, _fileStruct.fileId, StatusFileEnum.Review);
                    MessageBox.Show("Файл отправлен на проверку.");

                    FillTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void favoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_checkFavorite == true)
                {
                    DataMethod.DeleteFavorite(_userActive.userId, _fileStruct.fileId);
                    MessageBox.Show("Файл убран из избранного.");

                    FillTable();
                }
                else
                {
                    DataMethod.InsertFavorite(_userActive.userId, _fileStruct.fileId);
                    MessageBox.Show("Файл добавлен в избранное.");

                    FillTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FileMethod.DownloadFile(_userActive.userId, _fileStruct.fileId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteFile();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Login login = this.Owner as Login;
            login.Show();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            FillTable();
        }

        private void undeleteStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataMethod.FileToArchive(_userActive.userId, _fileStruct.fileId, false);
                MessageBox.Show("Файл восстановлен.");

                FillTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
