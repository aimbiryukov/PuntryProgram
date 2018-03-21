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
    public partial class UserList : Form
    {
        private UserStruct _userActive;
        private UserStruct _userStruct;

        public UserList(int userId)
        {
            InitializeComponent();

            try
            {
                _userActive = DataMethod.GetUser(userId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _userStruct = new UserStruct();
            Icon = Properties.Resources.list;
        }

        private void EnabledForm(bool value)
        {
            buttonUpdate.Enabled = value;
            buttonDelete.Enabled = value;
            editToolStripMenuItem.Enabled = value;
            showStripMenuItem.Enabled = value;
            deleteToolStripMenuItem.Enabled = value;
        }

        private void FillTable()
        {
            try
            {
                if (textBoxSearch.Text.Trim() == "")
                    dataGridViewAccounts.DataSource = DataMethod.UsersTable();
                else
                    dataGridViewAccounts.DataSource = DataMethod.SearchUsers(textBoxSearch.Text);

                if (dataGridViewAccounts.DataSource != null)
                {
                    EnabledForm(true);
                    dataGridViewAccounts.Columns[0].Visible = false;
                    dataGridViewAccounts.Columns[5].HeaderText = "Дата регистрации";
                    dataGridViewAccounts.RowHeadersDefaultCellStyle.Padding = new Padding(dataGridViewAccounts.RowHeadersWidth);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteUser()
        {
            try
            {
                if (_userActive.userId == _userStruct.userId)
                    MessageBox.Show("Вы не можете удалить собственную учетную запись.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    if (MessageBox.Show("Вы действительно желаете удалить данного пользователя и все его файлы?", "Удалить пользователя", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        DataMethod.DeleteUser(_userStruct.userId);
                        FillTable();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UserList_Load(object sender, EventArgs e)
        {
            FillTable();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            (new UserEdit()).ShowDialog();
            FillTable();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            FillTable();
        }

        private void dataGridViewAccounts_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
                (new UserProperties(_userStruct.userId)).ShowDialog();
        }

        private void dataGridViewAccounts_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1)
                contextMenuStrip.Show(Cursor.Position);
        }

        private void dataGridViewAccounts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dataGridViewAccounts.Rows.Count > 0)
            {
                e.SuppressKeyPress = true;
                (new UserProperties(_userStruct.userId)).ShowDialog();
            }
            if (e.KeyCode == Keys.Delete && dataGridViewAccounts.Rows.Count > 0)
                DeleteUser();
        }

        private void dataGridViewAccounts_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1 && e.Button == MouseButtons.Right)
            {
                dataGridViewAccounts.CurrentCell = dataGridViewAccounts[e.ColumnIndex, e.RowIndex];
                dataGridViewAccounts.CurrentRow.Selected = true;
            }
        }

        private void dataGridViewAccounts_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
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

        private void dataGridViewAccounts_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            _userStruct.userId = (int)dataGridViewAccounts.Rows[e.RowIndex].Cells[0].Value;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FillTable();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new UserEdit(_userStruct.userId)).ShowDialog();
            FillTable();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteUser();
        }

        private void showStripMenuItem_Click(object sender, EventArgs e)
        {
            (new UserProperties(_userStruct.userId)).ShowDialog();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new UserEdit()).ShowDialog();
            
            FillTable();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            (new UserEdit(_userStruct.userId)).ShowDialog();
            
            FillTable();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteUser();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
