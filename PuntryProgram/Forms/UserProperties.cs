using System;
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
                textBoxLevel.Text = _userStruct.levelName;
                textBoxName.Text = _userStruct.name;
                textBoxSurname.Text = _userStruct.surname;
                textBoxProject.Text = DataMethod.CountFiles(_userStruct.userId, StatusFileEnum.Project).ToString();
                textBoxDraft.Text = DataMethod.CountFiles(_userStruct.userId, StatusFileEnum.Draft).ToString();
                textBoxAllFiles.Text = DataMethod.CountFiles(_userStruct.userId, StatusFileEnum.My).ToString();
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
