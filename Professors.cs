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
    public partial class Professors : Form
    {
        public Professors()
        {
            InitializeComponent();
            LoadDepartments();
            LoadUsers();
            LoadProfessors();
        }

        private void LoadDepartments()
        {
            try
            {
                if (DBHelper.conn.State != ConnectionState.Open)
                    DBHelper.conn.Open();

                string query = "SELECT department_id, department_name FROM departments";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, DBHelper.conn);
                DataTable table = new DataTable();
                adapter.Fill(table);

                cbDepartment.DataSource = table;
                cbDepartment.DisplayMember = "department_name";
                cbDepartment.ValueMember = "department_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading departments: " + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
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
                string query = "SELECT user_id, username FROM users";
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                Dictionary<int, string> users = new Dictionary<int, string>();
                while (reader.Read())
                {
                    users.Add(reader.GetInt32("user_id"), reader.GetString("username"));
                }
                cbUser.DataSource = new BindingSource(users, null);
                cbUser.DisplayMember = "Value";
                cbUser.ValueMember = "Key";
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

        private void LoadProfessors()
        {
            try
            {
                if (DBHelper.conn.State != ConnectionState.Open)
                    DBHelper.conn.Open();

                string query = @"
            SELECT 
                p.professor_id,
                p.fname AS 'First Name',
                p.lname AS 'Last Name',
                d.department_name AS 'Department',
                p.hire_date AS 'Hire Date'
            FROM 
                professors p
            INNER JOIN 
                departments d ON p.department_id = d.department_id";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, DBHelper.conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridViewProfessors.DataSource = table;
                dataGridViewProfessors.Columns["professor_id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading professors: " + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            AdminDashboard adminForm = new AdminDashboard();
            adminForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
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

        private void button8_Click(object sender, EventArgs e)
        {
            UserLogin userForm = new UserLogin();
            userForm.Show(); // This shows the form without closing the current one
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

        private void dataGridViewStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtFname.Text = dataGridViewProfessors.Rows[e.RowIndex].Cells["First Name"].Value.ToString();
                txtLname.Text = dataGridViewProfessors.Rows[e.RowIndex].Cells["Last Name"].Value.ToString();
                dtHireDate.Value = Convert.ToDateTime(dataGridViewProfessors.Rows[e.RowIndex].Cells["Hire Date"].Value);
                // Department and User IDs are not shown, so store them separately if needed.
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (DBHelper.conn.State != ConnectionState.Open)
                    DBHelper.conn.Open();

                string query = @"INSERT INTO professors (user_id, department_id, hire_date, fname, lname)
                         VALUES (@user, @dept, @hireDate, @fname, @lname)";
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                cmd.Parameters.AddWithValue("@user", cbUser.SelectedValue);
                cmd.Parameters.AddWithValue("@dept", cbDepartment.SelectedValue);
                cmd.Parameters.AddWithValue("@hireDate", dtHireDate.Value);
                cmd.Parameters.AddWithValue("@fname", txtFname.Text.Trim());
                cmd.Parameters.AddWithValue("@lname", txtLname.Text.Trim());

                cmd.ExecuteNonQuery();
                MessageBox.Show("Professor added.");
                LoadProfessors();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add failed: " + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewProfessors.SelectedRows.Count == 0) return;

            int professorId = Convert.ToInt32(dataGridViewProfessors.SelectedRows[0].Cells["professor_id"].Value);

            try
            {
                if (DBHelper.conn.State != ConnectionState.Open)
                    DBHelper.conn.Open();

                string query = @"UPDATE professors 
                         SET fname=@fname, lname=@lname, department_id=@dept, hire_date=@hireDate, user_id=@user 
                         WHERE professor_id=@id";
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                cmd.Parameters.AddWithValue("@fname", txtFname.Text.Trim());
                cmd.Parameters.AddWithValue("@lname", txtLname.Text.Trim());
                cmd.Parameters.AddWithValue("@dept", cbDepartment.SelectedValue);
                cmd.Parameters.AddWithValue("@hireDate", dtHireDate.Value);
                cmd.Parameters.AddWithValue("@user", cbUser.SelectedValue);
                cmd.Parameters.AddWithValue("@id", professorId);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Professor updated.");
                LoadProfessors();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update failed: " + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewProfessors.SelectedRows.Count == 0) return;

            int professorId = Convert.ToInt32(dataGridViewProfessors.SelectedRows[0].Cells["professor_id"].Value);

            try
            {
                if (DBHelper.conn.State != ConnectionState.Open)
                    DBHelper.conn.Open();

                string query = "DELETE FROM professors WHERE professor_id=@id";
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                cmd.Parameters.AddWithValue("@id", professorId);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Professor deleted.");
                LoadProfessors();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed: " + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
            }
        }

        private void Users_Click(object sender, EventArgs e)
        {
            UserList userlistForm = new UserList();
            userlistForm.Show(); // This shows the form without closing the current one
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
