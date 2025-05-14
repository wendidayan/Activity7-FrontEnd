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
    public partial class Students : Form
    {
        public Students()
        {
            InitializeComponent();
            LoadUsersToComboBox();
            LoadProgramsToComboBox();
            LoadStudents();
        }

        private void LoadUsersToComboBox()
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
                cmbUser.DataSource = new BindingSource(users, null);
                cmbUser.DisplayMember = "Value";
                cmbUser.ValueMember = "Key";
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

        private void LoadProgramsToComboBox()
        {
            try
            {
                if (DBHelper.conn.State != ConnectionState.Open)
                {
                    DBHelper.conn.Open();
                }
                string query = "SELECT program_id, program_name FROM programs";
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                Dictionary<int, string> programs = new Dictionary<int, string>();
                while (reader.Read())
                {
                    programs.Add(reader.GetInt32("program_id"), reader.GetString("program_name"));
                }
                cmbProgram.DataSource = new BindingSource(programs, null);
                cmbProgram.DisplayMember = "Value";
                cmbProgram.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading programs: " + ex.Message);
            }
            finally
            {
                if (DBHelper.conn.State == ConnectionState.Open)
                {
                    DBHelper.conn.Close();
                }
            }
        }


        private void LoadStudents()
        {
            try
            {
                if (DBHelper.conn.State != ConnectionState.Open)
                {
                    DBHelper.conn.Open();
                }

                string query = @"
            SELECT 
                s.fname AS 'First Name',
                s.lname AS 'Last Name',
                p.program_name AS 'Program',
                s.enrollment_date AS 'Enrollment Date'
            FROM 
                students s
            INNER JOIN 
                programs p ON s.program_id = p.program_id";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, DBHelper.conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridViewStudents.DataSource = table;
                dataGridViewStudents.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading students: " + ex.Message);
            }
            finally
            {
                if (DBHelper.conn.State == ConnectionState.Open)
                {
                    DBHelper.conn.Close();
                }
            }
        }




        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

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

        private void button8_Click(object sender, EventArgs e)
        {
            UserLogin userForm = new UserLogin();
            userForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Professors profForm = new Professors();
            profForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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
                DataGridViewRow row = dataGridViewStudents.Rows[e.RowIndex];
                txtFirstName.Text = row.Cells["First Name"].Value.ToString();
                txtLastName.Text = row.Cells["Last Name"].Value.ToString();
                dateTimeEnrollment.Value = Convert.ToDateTime(row.Cells["enrollment_date"].Value);
                dateTimeGraduation.Value = Convert.ToDateTime(row.Cells["graduation_date"].Value);
                cmbUser.SelectedValue = Convert.ToInt32(row.Cells["user_id"].Value);
                cmbProgram.SelectedValue = Convert.ToInt32(row.Cells["program_id"].Value);
            }
        }

        private void AddStudent_Click(object sender, EventArgs e)
        {
            try
            {
                int userId = ((KeyValuePair<int, string>)cmbUser.SelectedItem).Key;
                int programId = ((KeyValuePair<int, string>)cmbProgram.SelectedItem).Key;
                string fname = txtFirstName.Text;
                string lname = txtLastName.Text;
                string enrollmentDate = dateTimeEnrollment.Value.ToString("yyyy-MM-dd");
                string graduationDate = dateTimeGraduation.Value.ToString("yyyy-MM-dd");

                DBHelper.conn.Open();
                string query = @"INSERT INTO students 
                        (user_id, program_id, enrollment_date, graduation_date, fname, lname) 
                         VALUES 
                        (@user_id, @program_id, @enrollment_date, @graduation_date, @fname, @lname)";
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@program_id", programId);
                cmd.Parameters.AddWithValue("@enrollment_date", enrollmentDate);
                cmd.Parameters.AddWithValue("@graduation_date", graduationDate);
                cmd.Parameters.AddWithValue("@fname", fname);
                cmd.Parameters.AddWithValue("@lname", lname);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student added successfully.");
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding student: " + ex.Message);
            }
            finally
            {
                if (DBHelper.conn.State == ConnectionState.Open)
                {
                    DBHelper.conn.Close();
                }
            }
        }

        private void UpdateStudent_Click(object sender, EventArgs e)
        {
            if (dataGridViewStudents.CurrentRow == null) return;

            int studentId = Convert.ToInt32(dataGridViewStudents.CurrentRow.Cells["student_id"].Value);
            int userId = ((KeyValuePair<int, string>)cmbUser.SelectedItem).Key;
            int programId = ((KeyValuePair<int, string>)cmbProgram.SelectedItem).Key;
            string fname = txtFirstName.Text;
            string lname = txtLastName.Text;
            string enrollmentDate = dateTimeEnrollment.Value.ToString("yyyy-MM-dd");
            string graduationDate = dateTimeGraduation.Value.ToString("yyyy-MM-dd");

            try
            {
                DBHelper.conn.Open();
                string query = @"UPDATE students SET 
                            user_id=@user_id, 
                            program_id=@program_id, 
                            enrollment_date=@enrollment_date, 
                            graduation_date=@graduation_date, 
                            fname=@fname, 
                            lname=@lname 
                         WHERE student_id=@student_id";
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@program_id", programId);
                cmd.Parameters.AddWithValue("@enrollment_date", enrollmentDate);
                cmd.Parameters.AddWithValue("@graduation_date", graduationDate);
                cmd.Parameters.AddWithValue("@fname", fname);
                cmd.Parameters.AddWithValue("@lname", lname);
                cmd.Parameters.AddWithValue("@student_id", studentId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student updated successfully.");
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating student: " + ex.Message);
            }
            finally
            {
                if (DBHelper.conn.State == ConnectionState.Open)
                {
                    DBHelper.conn.Close();
                }
            }
        }

        private void DeleteStudent_Click(object sender, EventArgs e)
        {
            if (dataGridViewStudents.CurrentRow == null) return;
            int studentId = Convert.ToInt32(dataGridViewStudents.CurrentRow.Cells["student_id"].Value);

            try
            {
                DBHelper.conn.Open();
                string query = "DELETE FROM students WHERE student_id=@student_id";
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                cmd.Parameters.AddWithValue("@student_id", studentId);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student deleted successfully.");
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting student: " + ex.Message);
            }
            finally
            {
                if (DBHelper.conn.State == ConnectionState.Open)
                {
                    DBHelper.conn.Close();
                }
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

