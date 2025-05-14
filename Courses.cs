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
    public partial class Courses : Form
    {
        public Courses()
        {
            InitializeComponent();
            LoadCourseData();
            this.Load += new EventHandler(Form_Load);
        }

        private void LoadCourseData()
        {
            string query = @"
            SELECT 
                c.course_id, 
                c.course_code, 
                c.course_name, 
                p.program_name 
            FROM 
                courses c
            JOIN 
                programs p ON c.program_id = p.program_id";

                DataTable dt = new DataTable();

            try
            {
                if (DBHelper.conn.State != ConnectionState.Open)
                    DBHelper.conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                dataGridViewCourses.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load course data: " + ex.Message);
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
            profForm.Show(); // This shows the form 
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

        private void dataGridViewProfessors_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

        private void btnLoadCoursesPerProgram_Click(object sender, EventArgs e)
        {
            int programId = Convert.ToInt32(comboBoxPrograms.SelectedValue);  // Get selected program's ID
            LoadCoursesPerProgramFromProcedure(programId);
        }

        private void LoadCoursesPerProgramFromProcedure(int programFilter)
        {
            DataTable dt = new DataTable();

            try
            {
                if (DBHelper.conn.State != ConnectionState.Open)
                    DBHelper.conn.Open();

                // Call stored procedure and pass the programFilter value
                MySqlCommand cmd = new MySqlCommand("GetCourseListPerProgramWithoutCursor", DBHelper.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("program_filter", programFilter == 0 ? (object)DBNull.Value : programFilter);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);

                // Set the DataSource of the DataGridView to show the data
                dataGridViewCourses.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load course list per program: " + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
            }
        }


        private void Form_Load(object sender, EventArgs e)
        {
            PopulateProgramComboBox();
        }

        private void PopulateProgramComboBox()
        {
            try
            {
                string query = "SELECT program_id, program_name FROM programs";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, DBHelper.conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                comboBoxPrograms.DisplayMember = "program_name";  // Display name in ComboBox
                comboBoxPrograms.ValueMember = "program_id";      // Set program_id as the value
                comboBoxPrograms.DataSource = dt;

                // Optionally, add an "All" option to the ComboBox
                DataRow newRow = dt.NewRow();
                newRow["program_id"] = 0;  // Use 0 to indicate "All"
                newRow["program_name"] = "All";
                dt.Rows.InsertAt(newRow, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load programs: " + ex.Message);
            }
        }


    }
}
