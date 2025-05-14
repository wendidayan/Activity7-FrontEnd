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
    public partial class Classes : Form
    {
        public Classes()
        {
            InitializeComponent();
            LoadClassData();
        }

        private void LoadClassData()
        {
            string query = @"
        SELECT 
            c.class_id,
            co.course_name,
            p.fname,
            p.lname,
            c.schedule
        FROM 
            classes c
        INNER JOIN 
            courses co ON c.course_id = co.course_id
        LEFT JOIN 
            professors p ON c.professor_id = p.professor_id";

            DataTable dt = new DataTable();

            try
            {
                DBHelper.conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                dataGridViewClasses.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load class data: " + ex.Message);
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

        private void dataGridViewClasses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Enrollments enrollForm = new Enrollments();
            enrollForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }
    }
}
