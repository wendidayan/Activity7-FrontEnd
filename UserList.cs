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
    public partial class UserList : Form
    {
        public UserList()
        {
            InitializeComponent();
            LoadUsers();
            LoadRoles();
        }

        // Helper class to bind role_id and role_name to ComboBox
        public class RoleItem
        {
            public int RoleId { get; set; }
            public string RoleName { get; set; }

            public override string ToString()
            {
                return RoleName;
            }
        }

        private void LoadUsers()
        {
            try
            {
                if (DBHelper.conn.State != ConnectionState.Open)
                {
                    DBHelper.conn.Open();
                }
                string query = "SELECT user_id, role_id, username, email FROM users";
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridViewUsers.DataSource = dt;
                dataGridViewUsers.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
            finally
            {
                if (DBHelper.conn.State == ConnectionState.Open)
                {
                    DBHelper.conn.Close();
                }
            }
        }

        private void LoadRoles()
        {
            cmbRole.Items.Clear();

            try
            {
                if (DBHelper.conn.State != ConnectionState.Open)
                {
                    DBHelper.conn.Open();
                }
                string query = "SELECT role_id, role_name FROM roles";
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    RoleItem role = new RoleItem
                    {
                        RoleId = reader.GetInt32("role_id"),
                        RoleName = reader.GetString("role_name")
                    };
                    cmbRole.Items.Add(role);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading roles: " + ex.Message);
            }
            finally
            {
                if (DBHelper.conn.State == ConnectionState.Open)
                {
                    DBHelper.conn.Close();
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            AdminDashboard adminForm = new AdminDashboard();
            adminForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                RoleItem selectedRole = cmbRole.SelectedItem as RoleItem;
                if (selectedRole == null)
                {
                    MessageBox.Show("Please select a role.");
                    return;
                }

                if (DBHelper.conn.State != ConnectionState.Open)
                {
                    DBHelper.conn.Open();
                }
                string query = "INSERT INTO users (role_id, username, password, email) VALUES (@role_id, @username, @password, @email)";
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                cmd.Parameters.AddWithValue("@role_id", selectedRole.RoleId);
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("User added successfully!");
                    LoadUsers();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding user: " + ex.Message);
            }
            finally
            {
                if (DBHelper.conn.State == ConnectionState.Open)
                {
                    DBHelper.conn.Close();
                }
            }
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                int userId = Convert.ToInt32(dataGridViewUsers.SelectedRows[0].Cells["user_id"].Value);

                try
                {
                    RoleItem selectedRole = cmbRole.SelectedItem as RoleItem;
                    if (selectedRole == null)
                    {
                        MessageBox.Show("Please select a role.");
                        return;
                    }
                    if (DBHelper.conn.State != ConnectionState.Open)
                    {
                        DBHelper.conn.Open();
                    }
                    string query = "UPDATE users SET role_id=@role_id, username=@username, password=@password, email=@email WHERE user_id=@user_id";
                    MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                    cmd.Parameters.AddWithValue("@role_id", selectedRole.RoleId);
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@user_id", userId);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("User updated successfully!");
                        LoadUsers();
                        ClearInputs();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating user: " + ex.Message);
                }
                finally
                {
                    if (DBHelper.conn.State == ConnectionState.Open)
                    {
                        DBHelper.conn.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to edit.");
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                int userId = Convert.ToInt32(dataGridViewUsers.SelectedRows[0].Cells["user_id"].Value);
                try
                {
                    if (DBHelper.conn.State != ConnectionState.Open)
                    {
                        DBHelper.conn.Open();
                    }
                    string query = "DELETE FROM users WHERE user_id=@user_id";
                    MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                    cmd.Parameters.AddWithValue("@user_id", userId);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("User deleted successfully!");
                        LoadUsers();
                        ClearInputs();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting user: " + ex.Message);
                }
                finally
                {
                    if (DBHelper.conn.State == ConnectionState.Open)
                    {
                        DBHelper.conn.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.");
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewUsers.Rows[e.RowIndex];
                txtUsername.Text = row.Cells["username"].Value.ToString();
                txtEmail.Text = row.Cells["email"].Value.ToString();

                int selectedRoleId = Convert.ToInt32(row.Cells["role_id"].Value);
                foreach (RoleItem role in cmbRole.Items)
                {
                    if (role.RoleId == selectedRoleId)
                    {
                        cmbRole.SelectedItem = role;
                        break;
                    }
                }

                // Optional: Load password too if needed
                // (requires adding password column in SELECT query)
            }
        }

        private void ClearInputs()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
            cmbRole.SelectedIndex = -1;
        }

        private void Users_Click(object sender, EventArgs e)
        {
            UserList userlistForm = new UserList();
            userlistForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login for
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Students studentForm = new Students();
            studentForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Professors profForm = new Professors();
            profForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Courses coursesForm = new Courses();
            coursesForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Departments deptForm = new Departments();
            deptForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Programs progForm = new Programs();
            progForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Classes classForm = new Classes();
            classForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UserLogin userForm = new UserLogin();
            userForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Enrollments enrollForm = new Enrollments();
            enrollForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }
    }
}
