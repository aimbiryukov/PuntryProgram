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
    public partial class UserForm : Form
    {
        private UserStruct _userStruct;

        public UserForm(int userId)
        {
            _userStruct = DataMethod.GetUser(userId);

            this.InitializeComponent();
        }

        private void FillForm()
        {
            this.Text += _userStruct.login;
            this.textBoxLogin.Text = _userStruct.login;
            this.textBoxLevel.Text = _userStruct.levelName;
            this.textBoxName.Text = _userStruct.name;
            this.textBoxSurname.Text = _userStruct.surname;
            this.textBoxProject.Text = DataMethod.CountFiles(_userStruct.userId, StatusFile.Project).ToString();
            this.textBoxReview.Text = DataMethod.CountFiles(_userStruct.userId, StatusFile.Review).ToString();
            this.textBoxDraft.Text = DataMethod.CountFiles(_userStruct.userId, StatusFile.Draft).ToString();
            this.textBoxAllFiles.Text = DataMethod.CountFiles(_userStruct.userId, StatusFile.My).ToString();
            this.textBoxSizeFiles.Text = FileMethod.FileSize(DataMethod.SizeAllFiles(_userStruct.userId));
            this.textBoxDatetimeAT.Text = _userStruct.dateTimeAT.ToString();
            this.pictureBoxUser.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBoxUser.Image = (_userStruct.binary != null) ? ImageMethod.BinaryToImage(_userStruct.binary) : null;
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            this.FillForm();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
