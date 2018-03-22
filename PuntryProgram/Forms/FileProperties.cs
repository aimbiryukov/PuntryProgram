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
    public partial class FileProperties : Form
    {
        private FileStruct _fileStruct;
        private UserStruct _userStruct;
        private UserStruct _userActive;

        public FileProperties(int fileId, int userId)
        {
            try
            {
                _fileStruct = DataMethod.GetFile(fileId);
                _userStruct = DataMethod.GetUser(_fileStruct.userId);
                _userActive = DataMethod.GetUser(userId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            InitializeComponent();

            FillForm();
            buttonUpdate.Text = "Изменить";
            richTextBoxComment.Enabled = false;
            textBoxName.Enabled = false;
            EnabledForm(true);
            textBoxName.Validated += textBoxName_Validated;
            richTextBoxComment.Validated += richTextBoxComment_Validated;
            textBoxName.MaxLength = 50;
            richTextBoxComment.MaxLength = 250;
            Icon = Properties.Resources.info;
        }

        private void FillTable()
        {
            try
            {
                if (textBoxSearch.Text.Trim() == "")
                    dataGridViewHistory.DataSource = DataMethod.ChangesTable(_fileStruct.fileId);
                else
                    dataGridViewHistory.DataSource = DataMethod.SearchChanges(_fileStruct.fileId, textBoxSearch.Text);

                if (dataGridViewHistory.DataSource != null)
                {
                    dataGridViewHistory.Columns[0].HeaderText = "Что изменилось";
                    dataGridViewHistory.Columns[1].HeaderText = "Кто изменил";
                    dataGridViewHistory.Columns[2].HeaderText = "Дата изменения";
                    dataGridViewHistory.RowHeadersDefaultCellStyle.Padding = new Padding(dataGridViewHistory.RowHeadersWidth);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private StatusFile _statusFile;
        private bool _statusFavorite;

        private void FillForm()
        {
            FillTable();

            try
            {
                _statusFile = DataMethod.CheckStatusFile(_fileStruct.fileId);
                _statusFavorite = DataMethod.CheckFavorite(_userActive.userId, _fileStruct.fileId);

                Text = "Свойства файла: " + _fileStruct.name + _fileStruct.extension;
                textBoxName.Text = _fileStruct.name;
                richTextBoxComment.Text = _fileStruct.comment;
                textBoxExpansion.Text = _fileStruct.extension.ToUpper();
                textBoxAT.Text = _fileStruct.dateTimeAT.ToString();
                textBoxUP.Text = _fileStruct.dateTimeUP.ToString();
                linkLabelLogin.Text = _userStruct.login;
                textBoxSize.Text = FileMethod.FileSize(Convert.ToDouble(_fileStruct.size));
                labelStatusFile.Text = (_statusFile == StatusFile.Project) ? "Проект" : (_statusFile == StatusFile.Review) ? "На проверке" : "Черновик";
                labelStatusFile.ForeColor = (_statusFile == StatusFile.Project) ? Color.DarkGreen : (_statusFile == StatusFile.Review) ? Color.DarkOrange : Color.DarkRed;
                buttonUpdate.Enabled = (_userStruct.userId == _userActive.userId || _userActive.level != LevelAccess.User);
                buttonFavorite.Text = (_statusFavorite == true) ? "Убрать из избранного" : "Добавить в избранное";
                buttonStatus.Text = (_statusFile == StatusFile.Review) ? "Отменить проверку" : "Отправить на проверку";
                buttonStatus.Visible = (_userStruct.userId == _userActive.userId && _userActive.level == LevelAccess.User && _statusFile != StatusFile.Project);
                buttonYes.Visible = (_userActive.level != LevelAccess.User);
                buttonYes.Enabled = (_statusFile != StatusFile.Project);
                buttonNo.Visible = (_userActive.level != LevelAccess.User);
                buttonNo.Enabled = (_statusFile != StatusFile.Draft);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnabledForm(bool value)
        {
            panel.Enabled = value;
        }

        private void linkLabelLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            (new UserProperties(_userStruct.userId)).Show();
        }

        private bool _statusUpdate = false;

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_statusUpdate == true)
                {
                    EnabledForm(false);

                    if (textBoxName.Text != "")
                    {
                        if (!DataMethod.incorrectChar.IsMatch(textBoxName.Text) && !DataMethod.incorrectChar.IsMatch(richTextBoxComment.Text))
                        {
                            if (textBoxName.Text.Length <= 50)
                            {
                                if (richTextBoxComment.Text.Length <= 250)
                                {
                                    if (MessageBox.Show("Вы действительно желаете изменить информацию о данном файле?", "Изменить файл", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        DataMethod.UpdateFile(_userActive.userId, _fileStruct.fileId, textBoxName.Text.Trim(), richTextBoxComment.Text.Trim());

                                        _statusUpdate = false;
                                        _userStruct = DataMethod.GetUser(_userStruct.userId);
                                        _fileStruct = DataMethod.GetFile(_fileStruct.fileId);

                                        FillForm();
                                        textBoxName.Enabled = false;
                                        richTextBoxComment.Enabled = false;
                                        buttonUpdate.Text = "Изменить";
                                    }
                                }
                                else
                                    MessageBox.Show("Описание файла должно быть не больше 250 символов.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                                MessageBox.Show("Название файла должно быть не больше 50 символов.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                            MessageBox.Show(@"Название и описание файла не должны содержать следующих символов: \ / : ? \"" < > |.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                        MessageBox.Show("Не указано имя файла.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    EnabledForm(true);
                }
                else
                {
                    _statusUpdate = true;

                    textBoxName.Enabled = true;
                    textBoxName.Focus();
                    richTextBoxComment.Enabled = true;
                    buttonUpdate.Text = "Сохранить";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            EnabledForm(false);

            try
            {
                FileMethod.DownloadFile(_userActive.userId, _fileStruct.fileId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            EnabledForm(true);
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            EnabledForm(false);

            try
            {
                DataMethod.UpdateStatusFile(_userActive.userId, _fileStruct.fileId, StatusFile.Project);
                MessageBox.Show("Файл утвержден.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            EnabledForm(true);
            FillForm();
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            EnabledForm(false);

            try
            {
                DataMethod.UpdateStatusFile(_userActive.userId, _fileStruct.fileId, StatusFile.Draft);
                MessageBox.Show("Файл отклонен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            EnabledForm(true);
            FillForm();
        }

        private void buttonReview_Click(object sender, EventArgs e)
        {
            EnabledForm(false);

            try
            {
                if (_statusFile == StatusFile.Review)
                {
                    DataMethod.UpdateStatusFile(_userActive.userId, _fileStruct.fileId, StatusFile.Draft);
                    MessageBox.Show("Проверка файла отменена.");
                }
                else
                {
                    DataMethod.UpdateStatusFile(_userActive.userId, _fileStruct.fileId, StatusFile.Review);
                    MessageBox.Show("Файл отправлен на проверку.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            EnabledForm(true);
            FillForm();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            FillTable();
        }

        private void dataGridViewHistory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }

        private void dataGridViewHistory_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1)
                contextMenuStrip.Show(Cursor.Position);
        }

        private void dataGridViewHistory_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
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

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FillTable();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonFavorite_Click(object sender, EventArgs e)
        {
            try
            {
                if (_statusFavorite == true)
                {
                    DataMethod.DeleteFavorite(_userActive.userId, _fileStruct.fileId);
                    MessageBox.Show("Файл убран из избранного.");
                }
                else
                {
                    DataMethod.InsertFavorite(_userActive.userId, _fileStruct.fileId);
                    MessageBox.Show("Файл добавлен в избранное.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            FillForm();
        }

        private int _visibilityTime = 5000;

        private void textBoxName_Validated(object sender, EventArgs e)
        {
            toolTip.ToolTipIcon = ToolTipIcon.Warning;
            toolTip.ToolTipTitle = "Предупреждение";

            try
            {
                if (textBoxName.Text == "")
                    toolTip.Show("Необходимо указать название.", textBoxName, _visibilityTime);
                else if (DataMethod.incorrectChar.IsMatch(textBoxName.Text))
                    toolTip.Show(@"Название не должно содержать следующих символов: \ / : ? "" < > |", textBoxName, _visibilityTime);
                else if (textBoxName.Text.Length >= 50)
                    toolTip.Show("Название файла должно быть не больше 50 символов.", textBoxName, _visibilityTime);
                else
                    toolTip.Hide(textBoxName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void richTextBoxComment_Validated(object sender, EventArgs e)
        {
            toolTip.ToolTipIcon = ToolTipIcon.Warning;
            toolTip.ToolTipTitle = "Предупреждение";

            try
            {
                if (richTextBoxComment.Text.Length >= 250)
                    toolTip.Show("Описание должно быть не больше 250 символов.", richTextBoxComment, _visibilityTime);
                else if (DataMethod.incorrectChar.IsMatch(richTextBoxComment.Text))
                    toolTip.Show(@"Описание не должно содержать следующих символов: \ / : ? "" < > |", richTextBoxComment, _visibilityTime);
                else
                    toolTip.Hide(richTextBoxComment);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FileProperties_Load(object sender, EventArgs e)
        {

        }
    }
}
