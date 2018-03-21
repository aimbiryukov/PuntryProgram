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
    public partial class FileForm : Form
    {
        private FileStruct _fileStruct;
        private UserStruct _userStruct;
        private UserStruct _userActive;
        private ChangeStruct _changeStruct;

        public FileForm(int fileId, int userId)
        {
            _fileStruct = DataMethod.GetFile(fileId);
            _changeStruct = DataMethod.GetChange(fileId);
            _userStruct = DataMethod.GetUser(_fileStruct.userId);
            _userActive = DataMethod.GetUser(userId);

            this.InitializeComponent();
        }

        private bool _statusReview;
        private bool _statusUpdate = false;

        private void FillForm()
        {
            StatusFile statusFile = DataMethod.CheckStatusFile(_fileStruct.fileId);

            _statusReview = (statusFile == StatusFile.Review) ? true : false;

            this.RefreshTable();
            this.comboBoxStatusFile.Text = (statusFile == StatusFile.Project) ? "Проект" : "Черновик";
            this.Text = "Свойства файла: " + _fileStruct.name + _fileStruct.expansion.ToLower();
            this.textBoxName.Text = _fileStruct.name;
            this.richTextBoxComment.Text = _fileStruct.comment;
            this.textBoxExpansion.Text = _fileStruct.expansion.ToUpper();
            this.textBoxAT.Text = _changeStruct.dateTimeAT.ToString();
            this.textBoxUP.Text = _changeStruct.dateTimeUP.ToString();
            this.linkLabelLogin.Text = _userStruct.login;
            this.textBoxSize.Text = FileMethod.FileSize(Convert.ToDouble(_fileStruct.size));
        }

        private void RefreshTable()
        {
            this.dataGridViewHistory.DataSource = DataMethod.ChangesTable(_fileStruct.fileId);
        }

        private void SearchChanges()
        {

        }

        private void UpdateFile()
        {
            if (_statusUpdate == true)
            {
                if (MessageBox.Show("Вы действительно желаете изменить информацию о файле?", "Изменение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _statusUpdate = false;
                    _userStruct = DataMethod.GetUser(_userStruct.userId);
                    _fileStruct = DataMethod.GetFile(_fileStruct.fileId);
                    _changeStruct = DataMethod.GetChange(_fileStruct.fileId);

                    DataMethod.UpdateFile(_userActive.userId, _fileStruct.fileId, textBoxName.Text, richTextBoxComment.Text, comboBoxStatusFile.Text);
                    
                    this.textBoxName.Enabled = false;
                    this.richTextBoxComment.Enabled = false;
                    this.comboBoxStatusFile.Enabled = false;
                    this.buttonUpdate.Text = "Изменить";
                }
            }
            else
            {
                _statusUpdate = true;

                this.textBoxName.Enabled = true;
                this.richTextBoxComment.Enabled = true;
                this.comboBoxStatusFile.Enabled = true;
                this.buttonUpdate.Text = "Сохранить";
            }

            this.FillForm();
        }

        private void ReviewFile()
        {
            if (_statusReview == true)
            {
                if (MessageBox.Show("Вы действительно желаете отменить проверку данного файла?", "Отмена", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _statusReview = false;

                    DataMethod.UpdateStatusFile(_userActive.userId, _fileStruct.fileId, StatusFile.Draft);

                    this.buttonStatus.Text = "Отправить на проверку";
                }
            }
            else
            {
                if (MessageBox.Show("Вы действительно желаете отправить данный файл на проверку?", "Проверка", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _statusReview = true;

                    DataMethod.UpdateStatusFile(_userActive.userId, _fileStruct.fileId, StatusFile.Review);

                    this.buttonStatus.Text = "Отменить проверку";
                }
            }

            this.FillForm();
        }

        private void FileForm_Load(object sender, EventArgs e)
        {
            this.comboBoxStatusFile.Items.Add("Черновик");
            this.comboBoxStatusFile.Items.Add("Проект");
            this.FillForm();

            this.textBoxName.Enabled = false;
            this.richTextBoxComment.Enabled = false;
            this.comboBoxStatusFile.Enabled = false;
            this.buttonUpdate.Text = "Изменить";
        }

        private void linkLabelLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            (new UserForm(_userStruct.userId)).Show();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            this.UpdateFile();
        }

        private void linkLabelShow_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            (new FileHistory(_fileStruct.fileId)).ShowDialog();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            FileMethod.DownloadFile(_userActive.userId, _fileStruct.fileId);
        }

        private void buttonReview_Click(object sender, EventArgs e)
        {
            this.ReviewFile();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            this.SearchChanges();
        }

        private void dataGridViewHistory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }

        private void dataGridViewHistory_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1)
                this.contextMenuStrip.Show(Cursor.Position);
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
            this.RefreshTable();
        }
    }
}
