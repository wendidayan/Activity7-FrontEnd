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
            LoadComboBoxes();
        }

        private void LoadClassData()
        {
            string query = @"
                SELECT 
                    c.class_id,
                    co.course_name,
                    CONCAT(p.fname, ' ', p.lname) AS professor_name,
                    c.schedule
                FROM 
                    classes c
                INNER JOIN 
                    courses co ON c.course_id = co.course_id
                LEFT JOIN 
                    professors p ON c.professor_id = p.professor_id";

            try
            {
                EnsureConnectionOpen();
                using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewClasses.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load class data:\n" + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
            }
        }

        private void LoadComboBoxes()
        {
            LoadComboBox(comboBoxCourse, "SELECT course_id, course_name FROM courses", "course_name", "course_id");
            LoadComboBox(comboBoxProfessor, "SELECT professor_id, CONCAT(fname, ' ', lname) AS full_name FROM professors", "full_name", "professor_id");
        }

        private void LoadComboBox(ComboBox comboBox, string query, string displayMember, string valueMember)
        {
            try
            {
                EnsureConnectionOpen();
                using (MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    comboBox.DataSource = dt;
                    comboBox.DisplayMember = displayMember;
                    comboBox.ValueMember = valueMember;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load data: " + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
            }
        }

        private void EnsureConnectionOpen()
        {
            if (DBHelper.conn.State != ConnectionState.Open)
            {
                DBHelper.conn.Open();
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

        private int selectedClassId = -1;
        private void dataGridViewClasses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewClasses.Rows[e.RowIndex];
                selectedClassId = Convert.ToInt32(row.Cells["class_id"].Value);
                comboBoxCourse.Text = row.Cells["course_name"].Value.ToString();
                comboBoxProfessor.Text = row.Cells["professor_name"].Value.ToString();
                textBoxSchedule.Text = row.Cells["schedule"].Value.ToString();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Enrollments enrollForm = new Enrollments();
            enrollForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void addProf_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureConnectionOpen();
                using (MySqlCommand cmd = new MySqlCommand("AddClass", DBHelper.conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_course_id", comboBoxCourse.SelectedValue);
                    cmd.Parameters.AddWithValue("@p_professor_id", comboBoxProfessor.SelectedValue);
                    cmd.Parameters.AddWithValue("@p_schedule", textBoxSchedule.Text);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Class added successfully.");
                LoadClassData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (selectedClassId == -1)
            {
                MessageBox.Show("Please select a class to update.");
                return;
            }

            try
            {
                EnsureConnectionOpen();
                using (MySqlCommand cmd = new MySqlCommand("UpdateClass", DBHelper.conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_class_id", selectedClassId);
                    cmd.Parameters.AddWithValue("@p_course_id", comboBoxCourse.SelectedValue);
                    cmd.Parameters.AddWithValue("@p_professor_id", comboBoxProfessor.SelectedValue);
                    cmd.Parameters.AddWithValue("@p_schedule", textBoxSchedule.Text);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Class updated successfully.");
                LoadClassData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (selectedClassId == -1)
            {
                MessageBox.Show("Please select a class to delete.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this class?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    EnsureConnectionOpen();
                    using (MySqlCommand cmd = new MySqlCommand("DeleteClass", DBHelper.conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_class_id", selectedClassId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Class deleted successfully.");
                    LoadClassData();
                    selectedClassId = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    DBHelper.conn.Close();
                }
            }
        }

        private void btnLoadProfwithClass_Click(object sender, EventArgs e)
        {
            try
            {
                EnsureConnectionOpen();

                using (MySqlCommand cmd = new MySqlCommand("GetProfessorsWithClasses", DBHelper.conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridViewClasses.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading professors with classes:\n" + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
            }

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
