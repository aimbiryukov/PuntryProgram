using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PuntryProgram.Classes;

namespace PuntryProgram.Forms
{
    public partial class Login : Form
    {
        private UserStruct _userActive;

        public Login()
        {
            InitializeComponent();

            EnabledForm(true);

            //DataMethod.SPICALDELETE();
            textBoxLogin.MaxLength = 50;
            textBoxPassword.MaxLength = 50;
            textBoxLogin.Validated += textBoxLogin_Validated;
            textBoxPassword.Validated += textBoxPassword_Validated;
        }

        private void EnabledForm(bool value)
        {
            panel.Enabled = value;
            textBoxLogin.Focus();
        }

        private void CheckIn()
        {
            EnabledForm(false);

            try
            {
                if (textBoxLogin.Text != "" && textBoxPassword.Text != "")
                {
                    if (!DataMethod.incorrectChar.IsMatch(textBoxLogin.Text) && !DataMethod.incorrectChar.IsMatch(textBoxPassword.Text))
                    {
                        if (textBoxLogin.Text.Length <= 50 && textBoxPassword.Text.Length <= 50)
                        {
                            if (DataMethod.CheckUser(textBoxLogin.Text, DataMethod.GetHash(textBoxPassword.Text)))
                            {
                                _userActive.userId = DataMethod.GetUserId(textBoxLogin.Text);

                                Main main = new Main(_userActive.userId)
                                {
                                    Owner = this
                                };
                                main.Show();

                                Hide();
                                textBoxLogin.Text = "";
                                textBoxPassword.Text = "";
                            }
                            else
                                MessageBox.Show("Неверный логин или пароль.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                            MessageBox.Show("Длина полей должна быть не больше 50 символов.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                        MessageBox.Show(@"Логин и пароль не должны содержать следующих символов: \ / : ? "" < > |", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("Необходимо заполнить все поля.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

            EnabledForm(true);
        }
        
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            CheckIn();
        }

        private void textBoxLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                CheckIn();
            }
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                CheckIn();
            }
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                DataMethod.DisposeDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int _visibilityTime= 5000;

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
                    toolTip.Show("Необходимо указать пароль.", textBoxPassword, _visibilityTime);
                else if (DataMethod.incorrectChar.IsMatch(textBoxPassword.Text))
                    toolTip.Show(@"Пароль не должен содержать следующих символов: \ / : ? "" < > |", textBoxPassword, _visibilityTime);
                else if (textBoxPassword.Text.Length >= 50)
                    toolTip.Show("Длина полей должна быть не больше 50 символов.", textBoxPassword, _visibilityTime);
                else
                    toolTip.Hide(textBoxPassword);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
