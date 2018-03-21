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
    public partial class UserProperties : Form
    {
        private UserStruct _userStruct;

        public UserProperties(int userId)
        {
            try
            {
                _userStruct = DataMethod.GetUser(userId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            InitializeComponent();

            Icon = Properties.Resources.info;
        }

        private void FillForm()
        {
            try
            {
                Text = "Свойства пользователя: " + _userStruct.login;
                textBoxLogin.Text = _userStruct.login;
                textBoxLevel.Text = (_userStruct.level == LevelAccess.Admin) ? "Администратор" : (_userStruct.level == LevelAccess.Editor) ? "Редактор" : "Пользователь";
                textBoxName.Text = _userStruct.name;
                textBoxSurname.Text = _userStruct.surname;
                textBoxProject.Text = DataMethod.CountFiles(_userStruct.userId, StatusFile.Project).ToString();
                textBoxDraft.Text = DataMethod.CountFiles(_userStruct.userId, StatusFile.Draft).ToString();
                textBoxAllFiles.Text = DataMethod.CountFiles(_userStruct.userId, StatusFile.My).ToString();
                textBoxSizeFiles.Text = FileMethod.FileSize(DataMethod.SizeAllFiles(_userStruct.userId));
                textBoxDatetimeAT.Text = _userStruct.dateTimeAT.ToString();
                pictureBoxUser.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBoxUser.Image = (_userStruct.binary != null) ? ImageMethod.BinaryToImage(_userStruct.binary) : null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            FillForm();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
