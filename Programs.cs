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
    public partial class Programs : Form
    {
        private bool showingPrograms= false;

        public Programs()
        {
            InitializeComponent();
            LoadProgramData();
        }

        private void LoadProgramData()
        {
            string query = @"
        SELECT 
            p.program_id, 
            p.program_name, 
            d.department_name
        FROM 
            programs p
        INNER JOIN 
            departments d ON p.department_id = d.department_id";

            DataTable dt = new DataTable();

            try
            {
                DBHelper.conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                dataGridViewPrograms.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load program data: " + ex.Message);
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

        private void dataGridViewPrograms_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
            if (!showingPrograms)
            {
                DataTable dt = new DataTable();

                try
                {
                    DBHelper.conn.Open();

                    MySqlCommand cmd = new MySqlCommand("ModifiedGetStudentCountByProgramWithoutCursor", DBHelper.conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);

                    dataGridViewPrograms.DataSource = dt;
                    btnShowProgramStat.Text = "Show Program Details";
                    showingPrograms = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching student counts: " + ex.Message);
                }
                finally
                {
                    DBHelper.conn.Close();
                }
            }
            else
            {
                LoadProgramData();
                btnShowProgramStat.Text = "Load Total Enrolled Students Per Program";
                showingPrograms = false;
            }

        }
    }
}
