using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace University_Information_System
{
    public partial class ForgotPasswordForm : Form
    {
        public ForgotPasswordForm()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            try
            {
                DBHelper.conn.Open();

                string checkQuery = "SELECT * FROM users WHERE username = @username AND email = @email";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, DBHelper.conn);
                checkCmd.Parameters.AddWithValue("@username", username);
                checkCmd.Parameters.AddWithValue("@email", email);

                MySqlDataReader reader = checkCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Close(); // close the reader before executing another command

                    string updateQuery = "UPDATE users SET password = @newPassword WHERE username = @username AND email = @email";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, DBHelper.conn);
                    updateCmd.Parameters.AddWithValue("@newPassword", newPassword); // You can hash later
                    updateCmd.Parameters.AddWithValue("@username", username);
                    updateCmd.Parameters.AddWithValue("@email", email);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        UserLogin userForm = new UserLogin();
                        userForm.Show(); // This shows the form without closing the current one
                        this.Hide(); // Hides the login form
                    }
                    else
                    {
                        MessageBox.Show("Failed to update password.");
                    }
                }
                else
                {
                    reader.Close();
                    MessageBox.Show("Username and email not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                DBHelper.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void ForgotPassLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UserLogin userForm = new UserLogin();
            userForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
