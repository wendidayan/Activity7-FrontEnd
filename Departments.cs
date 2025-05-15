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
    public partial class Departments : Form
    {
        public Departments()
        {
            InitializeComponent();
            LoadDepartmentData();
            LoadProgramsIntoComboBox();
        }

        private void LoadDepartmentData()
        {
            string query = "SELECT department_id, department_name FROM departments";
            DataTable dt = new DataTable();

            try
            {
                DBHelper.conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                dataGridViewDepartments.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load department data: " + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
            }
        }

        private void LoadProgramsIntoComboBox()
        {
            string query = "SELECT program_id, program_name FROM programs";
            try
            {
                DBHelper.conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);


                comboBoxProgram.DataSource = dt;
                comboBoxProgram.DisplayMember = "program_name";
                comboBoxProgram.ValueMember = "program_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load programs: " + ex.Message);
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

        private void Users_Click(object sender, EventArgs e)
        {
            UserList userlistForm = new UserList();
            userlistForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void dataGridViewDepartments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Enrollments enrollForm = new Enrollments();
            enrollForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void btnShowProgramStat_Click(object sender, EventArgs e)
        {
            if (comboBoxProgram.SelectedValue == DBNull.Value || comboBoxProgram.SelectedValue == null)
            {
                MessageBox.Show("Please select a specific program. The function doesn't support 'All'.");
                return;
            }

            int selectedProgramId = Convert.ToInt32(comboBoxProgram.SelectedValue);

            try
            {
                DBHelper.conn.Open();

                string query = "SELECT GetDepartmentByProgramForStoredProcedure(@program_id) AS department_name";
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                cmd.Parameters.AddWithValue("@program_id", selectedProgramId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridViewDepartments.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching department: " + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
            }

        }
    }
}
