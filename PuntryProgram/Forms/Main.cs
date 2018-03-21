using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PuntryProgram.Classes;

namespace PuntryProgram.Forms
{
    public partial class Main : Form
    {
        private UserStruct _userActive;
        private FileStruct _fileStruct;
        private StatusFile _statusFile;

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

            _statusFile = StatusFile.My;
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
                    if (_statusFile == StatusFile.Favorite)
                        dataGridViewFiles.DataSource = DataMethod.FavoritesTable(_userActive.userId);
                    else if (_statusFile == StatusFile.All)
                        dataGridViewFiles.DataSource = DataMethod.FilesTable(_userActive.userId, StatusFile.All);
                    else if (_statusFile == StatusFile.Archive)
                        dataGridViewFiles.DataSource = DataMethod.FilesTable(_userActive.userId, StatusFile.Archive);
                    else if (_statusFile == StatusFile.Project)
                        dataGridViewFiles.DataSource = DataMethod.FilesTable(_userActive.userId, StatusFile.Project);
                    else if (_statusFile == StatusFile.Review)
                        dataGridViewFiles.DataSource = DataMethod.FilesTable(_userActive.userId, StatusFile.Review);
                    else
                        dataGridViewFiles.DataSource = DataMethod.FilesTable(_userActive.userId, StatusFile.My);
                }
                else
                {
                    if (_statusFile == StatusFile.Favorite)
                        dataGridViewFiles.DataSource = DataMethod.SearchFavorites(_userActive.userId, textBoxSearch.Text);
                    else if (_statusFile == StatusFile.All)
                        dataGridViewFiles.DataSource = DataMethod.SearchFiles(_userActive.userId, textBoxSearch.Text, StatusFile.All);
                    else if (_statusFile == StatusFile.Archive)
                        dataGridViewFiles.DataSource = DataMethod.SearchFiles(_userActive.userId, textBoxSearch.Text, StatusFile.Archive);
                    else if (_statusFile == StatusFile.Review)
                        dataGridViewFiles.DataSource = DataMethod.SearchFiles(_userActive.userId, textBoxSearch.Text, StatusFile.Review);
                    else if (_statusFile == StatusFile.Project)
                        dataGridViewFiles.DataSource = DataMethod.SearchFiles(_userActive.userId, textBoxSearch.Text, StatusFile.Project);
                    else if (_statusFile == StatusFile.Draft)
                        dataGridViewFiles.DataSource = DataMethod.SearchFiles(_userActive.userId, textBoxSearch.Text, StatusFile.Draft);
                    else
                        dataGridViewFiles.DataSource = DataMethod.SearchFiles(_userActive.userId, textBoxSearch.Text, StatusFile.My);
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

        private StatusFile _checkStatusFile;
        private bool _checkFavorite;

        private void FillForm()
        {
            try
            {
                _checkStatusFile = DataMethod.CheckStatusFile(_fileStruct.fileId);
                _checkFavorite = DataMethod.CheckFavorite(_userActive.userId, _fileStruct.fileId);

                if (dataGridViewFiles.Rows.Count > 0)
                {
                    _fileStruct = DataMethod.GetFile(_fileStruct.fileId);
                }

                var level = (_userActive.level == LevelAccess.Admin) ? "Администратор" : (_userActive.level == LevelAccess.Editor) ? "Редактор" : "Пользователь";

                Text = level + ": " + _userActive.name + " " + _userActive.surname + " (" + _userActive.login.ToLower() + ")";
                buttonAccounts.Visible = (_userActive.level == LevelAccess.Admin);
                buttonReview.Visible = (_userActive.level != LevelAccess.User);
                buttonAll.Visible = (_userActive.level != LevelAccess.User);
                label2.Visible = (_userActive.level != LevelAccess.User);
                label3.Visible = (_userActive.level == LevelAccess.Admin);
                reviewToolStripMenuItem.Text = (_checkStatusFile == StatusFile.Review) ? "Отменить проверку" : "Отправить на проверку";
                reviewToolStripMenuItem.Visible = (_fileStruct.userId == _userActive.userId && _userActive.level == LevelAccess.User && _checkStatusFile != StatusFile.Project && _statusFile != StatusFile.Archive);
                favoriteToolStripMenuItem.Text = (_checkFavorite == true) ? "Убрать из избранного" : "Добавить в избранное";
                favoriteToolStripMenuItem.Visible = (_statusFile != StatusFile.Archive);
                showToolStripMenuItem.Visible = (_statusFile != StatusFile.Archive);
                downloadToolStripMenuItem.Visible = (_statusFile != StatusFile.Archive);
                undeleteStripMenuItem.Visible = (_statusFile == StatusFile.Archive);
                deleteToolStripMenuItem.Visible = (_fileStruct.userId == _userActive.userId || _userActive.level == LevelAccess.Admin);
                toolStripSeparator2.Visible = (_statusFile != StatusFile.Archive);
                toolStripSeparator3.Visible = (_statusFile != StatusFile.Archive && (_fileStruct.userId == _userActive.userId || _userActive.level == LevelAccess.Admin));
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
                if (_statusFile == StatusFile.Archive)
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
            if (_statusFile != StatusFile.Archive)
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

                if (_statusFile != StatusFile.Archive)
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

            _statusFile = StatusFile.My;

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

            _statusFile = StatusFile.My;

            ButtonActive(buttonMyFiles);
            FillTable();
        }

        private void buttonMyFiles_Click(object sender, EventArgs e)
        {
            _statusFile = StatusFile.My;

            ButtonActive(buttonMyFiles);
            FillTable();
        }

        private void buttonFavorite_Click(object sender, EventArgs e)
        {
            _statusFile = StatusFile.Favorite;

            ButtonActive(buttonFavorite);
            FillTable();
        }

        private void buttonArchive_Click(object sender, EventArgs e)
        {
            _statusFile = StatusFile.Archive;

            ButtonActive(buttonArchive);
            FillTable();
        }

        private void buttonAll_Click(object sender, EventArgs e)
        {
            _statusFile = StatusFile.All;

            ButtonActive(buttonAll);
            FillTable();
        }

        private void buttonProject_Click(object sender, EventArgs e)
        {
            _statusFile = StatusFile.Project;

            ButtonActive(buttonProject);
            FillTable();
        }

        private void buttonReview_Click(object sender, EventArgs e)
        {
            _statusFile = StatusFile.Review;

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
                if (_checkStatusFile == StatusFile.Review)
                {
                    DataMethod.UpdateStatusFile(_userActive.userId, _fileStruct.fileId, StatusFile.Draft);
                    MessageBox.Show("Проверка файла отменена.");

                    FillTable();
                }
                else
                {
                    DataMethod.UpdateStatusFile(_userActive.userId, _fileStruct.fileId, StatusFile.Review);
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
