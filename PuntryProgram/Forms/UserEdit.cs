using System;
using System.Windows.Forms;
using PuntryProgram.Classes;

namespace PuntryProgram.Forms
{
    public partial class UserEdit : Form
    {
        private UserStruct _userStruct;
        private bool _statusUpdate;

        public UserEdit()
        {
            _statusUpdate = false;

            InitializeComponent();

            buttonAction.Text = "Добавить";
        }

        public UserEdit(int userId)
        {
            try
            {
                _userStruct = DataMethod.GetUser(userId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _statusUpdate = true;

            InitializeComponent();
            buttonAction.Text = "Изменить";
        }

        private void EnabledForm(bool value)
        {
            panel.Enabled = value;
        }

        private void HidePassword(bool value)
        {
            textBoxPassword.UseSystemPasswordChar = value;
            buttonShow.BackgroundImage = (value == true) ? Properties.Resources.eye : Properties.Resources.eye_hidden;
        }

        private LevelAccessEnum CheckLevel(string level)
        {
            return (level == "Администратор") ? LevelAccessEnum.Admin : (level == "Редактор") ? LevelAccessEnum.Editor : LevelAccessEnum.User;
        }

        private void AddUser()
        {
            try
            {
                if (MessageBox.Show("Вы действительно желаете добавить данного пользователя?", "Добавить пользователя", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataMethod.InsertUser(textBoxName.Text, textBoxSurname.Text, textBoxLogin.Text, DataMethod.GetHash(textBoxPassword.Text), DateTime.Now, _userStruct.binary, (int)comboBoxLevel.SelectedValue);

                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateUser()
        {
            try
            {
                if (MessageBox.Show("Вы действительно желаете изменить информацию о данном пользователе?", "Измененить пользователя", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataMethod.UpdateUser(_userStruct.userId, textBoxName.Text, textBoxSurname.Text, textBoxLogin.Text, DataMethod.GetHash(textBoxPassword.Text), _userStruct.binary, (int)comboBoxLevel.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            textBoxName.Text = "";
            textBoxSurname.Text = "";
            textBoxLogin.Text = "";
            textBoxPassword.Text = "";
            pictureBoxUser.Image = null;
        }

        private void FillForm()
        {
            if (_statusUpdate == true)
            {
                Text = "Изменение пользователя: " + _userStruct.login;
                comboBoxLevel.Text = _userStruct.levelName;
                textBoxName.Text = _userStruct.name;
                textBoxSurname.Text = _userStruct.surname;
                textBoxLogin.Text = _userStruct.login;
                pictureBoxUser.Image = (_userStruct.binary != null) ? ImageMethod.BinaryToImage(_userStruct.binary) : null;
            }
        }

        private void FillComboBox()
        {
            comboBoxLevel.DataSource = DataMethod.AccessLevelsTable();
            comboBoxLevel.ValueMember = "id";
            comboBoxLevel.DisplayMember = "name";
        }

        private void UserAdd_Load(object sender, EventArgs e)
        {
            FillComboBox();
            HidePassword(true);
            EnabledForm(true);
            FillForm();

            pictureBoxUser.SizeMode = PictureBoxSizeMode.StretchImage;
            textBoxName.Validated += textBoxName_Validated;
            textBoxSurname.Validated += textBoxSurname_Validated;
            textBoxLogin.Validated += textBoxLogin_Validated;
            textBoxPassword.Validated += textBoxPassword_Validated;
            textBoxName.MaxLength = 50;
            textBoxSurname.MaxLength = 50;
            textBoxLogin.MaxLength = 50;
            textBoxPassword.MaxLength = 50;
            Icon = Properties.Resources.edit;
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            if (textBoxPassword.UseSystemPasswordChar == true)
                HidePassword(false);
            else
                HidePassword(true);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBoxUser_Click(object sender, EventArgs e)
        {
            try
            {
                var image = ImageMethod.GetDirectoryImage();

                _userStruct.binary = (image != null) ? ImageMethod.ImageToBinary(image) : null;

                pictureBoxUser.Image = image;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonShow_Enter(object sender, EventArgs e)
        {
            comboBoxLevel.Focus();
        }

        private void buttonAction_Click(object sender, EventArgs e)
        {
            EnabledForm(false);

            try
            {
                if (textBoxName.Text != "" && textBoxSurname.Text != "" && textBoxLogin.Text != "" && textBoxPassword.Text != "" && comboBoxLevel.Text != "")
                {
                    if (!DataMethod.incorrectCharFullName.IsMatch(textBoxName.Text) && !DataMethod.incorrectCharFullName.IsMatch(textBoxSurname.Text))
                    {
                        if (!DataMethod.incorrectChar.IsMatch(textBoxLogin.Text) && !DataMethod.incorrectChar.IsMatch(textBoxPassword.Text))
                        {
                            if (textBoxName.Text.Length <= 50 && textBoxSurname.Text.Length <= 50 && textBoxLogin.Text.Length <= 50 && textBoxPassword.Text.Length <= 50)
                            {
                                if (textBoxLogin.Text.Length >= 4 && textBoxPassword.Text.Length >= 4)
                                {
                                    if (_statusUpdate == true)
                                    {
                                        if (_userStruct.login == textBoxLogin.Text)
                                            UpdateUser();
                                        else
                                        {
                                            if (!DataMethod.CheckLogin(textBoxLogin.Text))
                                                UpdateUser();
                                            else
                                                MessageBox.Show("Пользователь с таким логином уже существует.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        }
                                    }
                                    else
                                    {
                                        if (!DataMethod.CheckLogin(textBoxLogin.Text))
                                            AddUser();
                                        else
                                            MessageBox.Show("Пользователь с данным логином уже существует.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                else
                                    MessageBox.Show("Длина логина и пароля должна быть не меньше 4 символов.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                                MessageBox.Show("Длина полей должна быть не больше 50 символов.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                            MessageBox.Show(@"Логин и пароль не должны содержать следующих символов: \ / : ? \"" < > |.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                        MessageBox.Show("Имя и фамилия не должны содержать спецсимволов.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("Необходимо заполнить все поля.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            EnabledForm(true);
        }

        private int _visibilityTime = 5000;

        private void textBoxName_Validated(object sender, EventArgs e)
        {
            toolTip.ToolTipIcon = ToolTipIcon.Warning;
            toolTip.ToolTipTitle = "Предупреждение";

            try
            {
                if (textBoxName.Text == "")
                    toolTip.Show("Необходимо указать имя.", textBoxName, _visibilityTime);
                else if (DataMethod.incorrectCharFullName.IsMatch(textBoxName.Text))
                    toolTip.Show("Имя не должно содержать спецсимволов.", textBoxName, _visibilityTime);
                else if (textBoxName.Text.Length >= 50)
                    toolTip.Show("Длина полей должна быть не больше 50 символов.", textBoxName, _visibilityTime);
                else
                    toolTip.Hide(textBoxName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxSurname_Validated(object sender, EventArgs e)
        {
            toolTip.ToolTipIcon = ToolTipIcon.Warning;
            toolTip.ToolTipTitle = "Предупреждение";

            try
            {
                if (textBoxSurname.Text == "")
                    toolTip.Show("Необходимо указать фамилию.", textBoxSurname, _visibilityTime);
                else if (DataMethod.incorrectCharFullName.IsMatch(textBoxSurname.Text))
                    toolTip.Show("Фамилия не должна содержать спецсимволов.", textBoxSurname, _visibilityTime);
                else if (textBoxSurname.Text.Length >= 50)
                    toolTip.Show("Длина полей должна быть не больше 50 символов.", textBoxSurname, _visibilityTime);
                else
                    toolTip.Hide(textBoxSurname);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxLogin_Validated(object sender, EventArgs e)
        {
            toolTip.ToolTipIcon = ToolTipIcon.Warning;
            toolTip.ToolTipTitle = "Предупреждение";

            try
            {
                if (textBoxLogin.Text == "")
                    toolTip.Show("Необходимо указать логин.", textBoxLogin, _visibilityTime);
                else if (DataMethod.incorrectChar.IsMatch(textBoxLogin.Text))
                    toolTip.Show(@"Логин не должен содержать следующих символов: \ / : ? "" < > |", textBoxLogin, _visibilityTime);
                else if (textBoxLogin.Text.Length >= 50)
                    toolTip.Show("Длина полей должна быть не больше 50 символов.", textBoxLogin, _visibilityTime);
                else if (textBoxLogin.Text.Length < 4)
                    toolTip.Show("Длина логина должна быть не меньше 4 символов.", textBoxLogin, _visibilityTime);
                else
                    toolTip.Hide(textBoxLogin);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxPassword_Validated(object sender, EventArgs e)
        {
            toolTip.ToolTipIcon = ToolTipIcon.Warning;
            toolTip.ToolTipTitle = "Предупреждение";

            try
            {
                if (textBoxPassword.Text == "")
                    toolTip.Show("Необходимо указать имя.", textBoxPassword, _visibilityTime);
                else if (DataMethod.incorrectChar.IsMatch(textBoxPassword.Text))
                    toolTip.Show(@"Пароль не должен содержать следующих символов: \ / : ? "" < > |", textBoxPassword, _visibilityTime);
                else if (textBoxPassword.Text.Length >= 50)
                    toolTip.Show("Длина полей должна быть не больше 50 символов.", textBoxPassword, _visibilityTime);
                else if (textBoxPassword.Text.Length < 4)
                    toolTip.Show("Длина пароля должна быть не меньше 4 символов.", textBoxPassword, _visibilityTime);
                else
                    toolTip.Hide(textBoxPassword);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonShow_MouseEnter(object sender, EventArgs e)
        {
            if (textBoxPassword.UseSystemPasswordChar == true)
                toolTipHelp.Show("Отобразить пароль.", buttonShow);
            else
                toolTipHelp.Show("Скрыть пароль.", buttonShow);
        }
    }
}
