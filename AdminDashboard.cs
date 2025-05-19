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
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
            InitializeComponent();
            LoadEnrollmentData();
            LoadCourseData();
            dashCount();

        }


        private void LoadEnrollmentData()
        {
            string query = @"
        SELECT 
            e.student_id,
            e.enrollment_date
        FROM 
            enrollments e";


            DataTable dt = new DataTable();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DBHelper.connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }

                dataGridViewEnrollments.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load enrollment data: " + ex.Message);
            }
        }

        private void LoadCourseData()
        {
            string query = "SELECT course_id, course_name FROM courses";

            DataTable dt = new DataTable();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(DBHelper.connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }

                dataGridViewCourses.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load course data: " + ex.Message);
            }
        }

        private void dashCount()
        {
            string studentQuery = "SELECT COUNT(*) FROM students";
            string professorQuery = "SELECT COUNT(*) FROM professors";
            string programQuery = "SELECT COUNT(*) FROM programs";
            string departmentQuery = "SELECT COUNT(*) FROM departments";

            try
            {
                if (DBHelper.conn.State == ConnectionState.Open)
                    DBHelper.conn.Close();

                DBHelper.conn.Open();

                using (var studentCmd = new MySqlCommand(studentQuery, DBHelper.conn))
                    labelTotalStudents.Text = studentCmd.ExecuteScalar().ToString();
                    labelTotalStudents.ForeColor = Color.White;

                using (var professorCmd = new MySqlCommand(professorQuery, DBHelper.conn))
                    labelTotalProfessors.Text = professorCmd.ExecuteScalar().ToString();
                    labelTotalProfessors.ForeColor = Color.White;

                using (var programCmd = new MySqlCommand(programQuery, DBHelper.conn))
                    labelTotalPrograms.Text = programCmd.ExecuteScalar().ToString();
                    labelTotalPrograms.ForeColor = Color.White;

                using (var deptCmd = new MySqlCommand(departmentQuery, DBHelper.conn))
                    labelTotalDepartments.Text = deptCmd.ExecuteScalar().ToString();
                    labelTotalDepartments.ForeColor = Color.White;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load dashboard count: " + ex.Message);
            }
            finally
            {
                if (DBHelper.conn.State == ConnectionState.Open)
                    DBHelper.conn.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Classes classForm = new Classes();
            classForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminDashboard adminForm = new AdminDashboard();
            adminForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UserLogin userForm = new UserLogin();
            userForm.Show(); // This shows the form without closing the current one
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Programs progForm = new Programs();
            progForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void label11_Click(object sender, EventArgs e)
        {

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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewEnrollments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewCourses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
