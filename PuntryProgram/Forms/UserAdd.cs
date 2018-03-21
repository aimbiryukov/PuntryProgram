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
    public partial class UserAdd : Form
    {
        private UserStruct _userStruct;

        public UserAdd()
        {
            this.InitializeComponent();
        }

        private void EnabledForm(bool value)
        {
            this.panel.Enabled = value;
        }

        private void HidePassword(bool value)
        {
            this.textBoxPassword.UseSystemPasswordChar = value;
            this.buttonShow.BackgroundImage = (value == true) ? Properties.Resources.eye : Properties.Resources.eye_hidden;
        }

        private LevelAccess CheckLevel(string level)
        {
            return (level == "Администратор") ? LevelAccess.Admin : (level == "Редактор") ? LevelAccess.Editor : LevelAccess.User;
        }

        private void AddUser()
        {
            if (MessageBox.Show("Вы действительно желаете добавить данного пользователя?", "Добавление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataMethod.InsertUser(textBoxName.Text, textBoxSurname.Text, textBoxLogin.Text, DataMethod.GetHash(textBoxPassword.Text), DateTime.Now, _userStruct.binary, CheckLevel(comboBoxLevel.Text));
            }
        }

        private void UserAdd_Load(object sender, EventArgs e)
        {
            this.comboBoxLevel.Items.Add("Пользователь");
            this.comboBoxLevel.Items.Add("Редактор");
            this.comboBoxLevel.Items.Add("Администратор");
            this.comboBoxLevel.Text = "Пользователь";
            this.HidePassword(true);
            this.EnabledForm(true);
            this.pictureBoxUser.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.EnabledForm(false);

            if (textBoxName.Text != "" && textBoxSurname.Text != "" && textBoxLogin.Text != "" && textBoxPassword.Text != "" && comboBoxLevel.Text != "")
            {
                if (!DataMethod.CheckLogin(textBoxLogin.Text))
                    this.AddUser();
                else
                    MessageBox.Show("Пользователь с данным логином уже существует.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                MessageBox.Show("Необходимо заполнить все обязательные поля.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            this.EnabledForm(true);
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            if (textBoxPassword.UseSystemPasswordChar == true)
                this.HidePassword(false);
            else
                this.HidePassword(true);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBoxUser_Click(object sender, EventArgs e)
        {
            var image = ImageMethod.GetDirectoryImage();
            this.pictureBoxUser.Image = image;
            _userStruct.binary = (image != null) ? ImageMethod.ImageToBinary(image) : null;
        }

        private void buttonShow_Enter(object sender, EventArgs e)
        {
            this.comboBoxLevel.Focus();
        }
    }
}
