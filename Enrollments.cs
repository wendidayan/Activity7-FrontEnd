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
using Excel = Microsoft.Office.Interop.Excel;

namespace University_Information_System
{

    public partial class Enrollments : Form
    {
        private bool showingStatus = false;

        public Enrollments()
        {
            InitializeComponent();
            LoadEnrollmentData();
            LoadStudentComboBox();
            btnStudStat.Text = "Show Student Status"; // Default button label
            btnArchive.Click += btnArchive_Click;
        }

        private void LoadEnrollmentData()
        {
            string query = @"
        SELECT 
            e.enrollment_id,
            s.fname,
            s.lname,
            p.program_name,
            e.enrollment_date
        FROM 
            enrollments e
        INNER JOIN 
            students s ON e.student_id = s.student_id
        INNER JOIN 
            programs p ON e.program_id = p.program_id";

            DataTable dt = new DataTable();

            try
            {
                DBHelper.conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, DBHelper.conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                dataGridViewEnrollments.DataSource = dt; // Make sure you have this DataGridView on your form
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load enrollment data: " + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
            }
        }

        private void LoadStudentComboBox()
        {
            try
            {
                DBHelper.conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT student_id, CONCAT(fname, ' ', lname) AS full_name FROM students", DBHelper.conn);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                // Add "All" row manually
                DataRow allRow = dt.NewRow();
                allRow["student_id"] = 0;
                allRow["full_name"] = "All";
                dt.Rows.InsertAt(allRow, 0);

                comboBoxStudents.DataSource = dt;
                comboBoxStudents.DisplayMember = "full_name";
                comboBoxStudents.ValueMember = "student_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load students: " + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
            }
        }

        private void LoadArchiveData()
        {
            const string sql = @"
        SELECT 
            enrollment_id,
            student_id,
            program_id,
            enrollment_date
        FROM enrollments_archive
        ORDER BY enrollment_date DESC";

            var dt = new DataTable();
            try
            {
                DBHelper.conn.Open();
                var cmd = new MySqlCommand(sql, DBHelper.conn);
                var adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                dataGridViewEnrollments.DataSource = dt;
                dataGridViewEnrollments.Columns["enrollment_date"].DefaultCellStyle.Format = "yyyy-MM-dd";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load archive data: " + ex.Message);
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

        private void Users_Click(object sender, EventArgs e)
        {
            UserList userlistForm = new UserList();
            userlistForm.Show(); // This shows the form without closing the current one
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

        private void button10_Click(object sender, EventArgs e)
        {
            Enrollments enrollForm = new Enrollments();
            enrollForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UserLogin userForm = new UserLogin();
            userForm.Show(); // This shows the form without closing the current one
            this.Hide(); // Hides the login form
        }

        private void dataGridViewEnrollments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnStudStat_Click(object sender, EventArgs e)
        {
            object selectedValue = comboBoxStudents.SelectedValue;

            if (showingStatus)
            {
                // Switch back to enrollment details
                LoadEnrollmentData();
                btnStudStat.Text = "Show Student Status";
                showingStatus = false;
                return;
            }

            try
            {
                DBHelper.conn.Open();

                string query;
                MySqlCommand cmd;

                if (Convert.ToInt32(selectedValue) == 0)
                {
                    // All students - show their status
                    query = @"
                SELECT 
                    s.student_id,
                    CONCAT(s.fname, ' ', s.lname) AS full_name,
                    IF(e.student_id IS NOT NULL, 'Enrolled', 'Not Enrolled') AS status
                FROM students s
                LEFT JOIN enrollments e ON s.student_id = e.student_id
                GROUP BY s.student_id";
                    cmd = new MySqlCommand(query, DBHelper.conn);
                }
                else
                {
                    // Specific student - show status
                    query = @"
                SELECT 
                    s.student_id,
                    CONCAT(s.fname, ' ', s.lname) AS full_name,
                    IF(e.student_id IS NOT NULL, 'Enrolled', 'Not Enrolled') AS status
                FROM students s
                LEFT JOIN enrollments e ON s.student_id = e.student_id
                WHERE s.student_id = @id
                GROUP BY s.student_id";
                    cmd = new MySqlCommand(query, DBHelper.conn);
                    cmd.Parameters.AddWithValue("@id", selectedValue);
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridViewEnrollments.DataSource = dt;

                btnStudStat.Text = "Show Enrollment Details";
                showingStatus = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking status: " + ex.Message);
            }
            finally
            {
                DBHelper.conn.Close();
            }
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            LoadArchiveData();
        }

        private void comboBoxStudents_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            if (dataGridViewEnrollments.DataSource == null)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            try
            {
                Excel.Application excelApp = new Excel.Application();
                excelApp.Workbooks.Add(Type.Missing);
                Excel._Worksheet worksheet = (Excel._Worksheet)excelApp.ActiveSheet;
                worksheet.Name = "Enrollment Report";

                int colCount = dataGridViewEnrollments.Columns.Count;

                // Helper function usage to get Excel column letters
                string lastColLetter = GetExcelColumnName(colCount);

                // Determine title start and end columns to avoid overlapping logo in column 1 (A)
                int titleStartCol = 2; // Start from column B
                int titleEndCol;
                int titleFontSize;

                if (colCount <= 2)
                {
                    titleEndCol = 3;    // Merge from B to C (2 columns wide)
                    titleFontSize = 8; // Smaller font for fewer columns
                }
                else if (colCount <= 4)
                {
                    titleEndCol = 5;    // Merge from B to E (4 columns wide)
                    titleFontSize = 10;
                }
                else
                {
                    titleEndCol = colCount; // Merge till last column
                    titleFontSize = 14;     // Normal font size
                }

                string titleStartLetter = GetExcelColumnName(titleStartCol);
                string titleEndLetter = GetExcelColumnName(titleEndCol);

                // Insert logo at A1
                string logoPath = @"C:\Users\wende\Downloads\logo.png";
                if (System.IO.File.Exists(logoPath))
                {
                    Excel.Range logoCell = worksheet.Cells[1, 1];
                    Excel.Pictures pics = (Excel.Pictures)worksheet.Pictures(System.Reflection.Missing.Value);
                    Excel.Picture logo = pics.Insert(logoPath, Type.Missing);
                    logo.Top = (float)(double)logoCell.Top;
                    logo.Left = (float)(double)logoCell.Left;

                    // Adjust logo height based on number of columns
                    if (colCount <= 2)
                        logo.Height = 40; // smaller logo
                    else if (colCount <= 4)
                        logo.Height = 50;
                    else
                        logo.Height = 60; // default bigger logo
                }

                // Merge title range dynamically
                Excel.Range titleRange = worksheet.Range[$"{titleStartLetter}1:{titleEndLetter}4"];
                titleRange.Merge();
                titleRange.Value = "UNIVERSITY ENROLLMENT DETAILS";
                titleRange.Font.Bold = true;
                titleRange.Font.Size = titleFontSize;
                titleRange.Font.Color = System.Drawing.ColorTranslator.ToOle(Color.DarkBlue);
                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                titleRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                int headerRow = 5;

                // Define custom headers
                Dictionary<string, string> customHeaders = new Dictionary<string, string>()
        {
            { "enrollment_id", "Enrollment ID" },
            { "fname", "First Name" },
            { "lname", "Last Name" },
            { "program_name", "Program Name" },
            { "enrollment_date", "Enrollment Date" },
            { "student_id", "Student ID" },
            { "program_id", "Program ID" },
            { "full_name", "Full Name" },
            { "status", "Status" }
        };

                // Add headers with custom labels
                for (int i = 0; i < colCount; i++)
                {
                    string headerText = dataGridViewEnrollments.Columns[i].Name;
                    if (customHeaders.ContainsKey(headerText))
                        headerText = customHeaders[headerText];

                    Excel.Range headerCell = worksheet.Cells[headerRow, i + 1];
                    headerCell.Value = headerText;
                    headerCell.Font.Bold = true;
                    headerCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightSlateGray);
                    headerCell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }

                // Add data rows
                for (int i = 0; i < dataGridViewEnrollments.Rows.Count; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        object value = dataGridViewEnrollments.Rows[i].Cells[j].Value;

                        // Format enrollment_date as YYYY-MM-DD
                        if (dataGridViewEnrollments.Columns[j].Name == "enrollment_date" && value != null)
                        {
                            if (DateTime.TryParse(value.ToString(), out DateTime dt))
                            {
                                value = dt.ToString("yyyy-MM-dd");
                            }
                        }

                        worksheet.Cells[i + headerRow + 1, j + 1] = value;
                    }
                }

                // Autofit columns
                worksheet.Columns.AutoFit();

                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting to Excel: " + ex.Message);
            }
        }

        // Helper function to get Excel column letter from number
        private string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";
            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                columnNumber = (columnNumber - modulo - 1) / 26;
            }
            return columnName;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();

            DataTable dt = (DataTable)dataGridViewEnrollments.DataSource;

            if (dt == null) return;

            DataView dv = dt.DefaultView;

            // Build filter expression dynamically based on current columns
            List<string> filters = new List<string>();

            foreach (DataColumn col in dt.Columns)
            {
                if (col.DataType == typeof(string))
                {
                    filters.Add($"CONVERT([{col.ColumnName}], System.String) LIKE '%{keyword}%'");
                }
                else
                {
                    filters.Add($"CONVERT([{col.ColumnName}], System.String) LIKE '%{keyword}%'");
                }
            }

            dv.RowFilter = string.Join(" OR ", filters);

            if (string.IsNullOrWhiteSpace(keyword))
            {
                dv.RowFilter = string.Empty;
                return;
            }

        }
    }
}
