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
    public partial class FileHistory : Form
    {
        private FileStruct _fileStruct;

        public FileHistory(int fileId)
        {
            _fileStruct = DataMethod.GetFile(fileId);

            this.InitializeComponent();
        }

        private void RefreshTable()
        {
            this.dataGridViewFiles.DataSource = DataMethod.ChangesTable(_fileStruct.fileId);
        }

        private void FillForm()
        {
            this.Text += _fileStruct.name + _fileStruct.expansion;
        }

        private void SearchChanges()
        {

        }

        private void FileHistory_Load(object sender, EventArgs e)
        {
            this.RefreshTable();
            this.FillForm();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            this.SearchChanges(); 
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

        private void dataGridViewFiles_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1)
                this.contextMenuStrip.Show(Cursor.Position);
        }

        private void dataGridViewFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.RefreshTable();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
