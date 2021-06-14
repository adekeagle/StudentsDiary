using StudentsDiary.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class Main : Form
    {
        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        private List<Group> _groups;

        public bool IsMaximize 
        {
            get 
            {
                return Settings.Default.IsMaximize;
            }
            set
            {
                Settings.Default.IsMaximize = value;
            }
        }


        public Main()
        {

            InitializeComponent();

            _groups = new List<Group>
            {
                new Group { Id = 0, Name = "Wybierz" },
                new Group { Id = 1, Name = "1a" },
                new Group { Id = 2, Name = "2b" }
            };

            InitStudentGroup();
            RefreshDiary();

            //HideColumns();



            SetColumnHeader();
            
            if (IsMaximize)
                WindowState = FormWindowState.Maximized;
        }

        //private void HideColumns()
        //{
        //    dgvDiary.Columns[11].Visible = true;
        //}

        private void InitStudentGroup()
        {
            cmbChooseClass.DataSource = _groups;
            cmbChooseClass.DisplayMember = "Name";
            cmbChooseClass.ValueMember = "Id";
        }

        private void RefreshDiary()
        {
            var students = _fileHelper.DeserializeFromFile();

            var studentGroupId = (cmbChooseClass.SelectedItem as Group).Id;

            if(studentGroupId != 0)
            {
                students = students.Where(x => x.GroupId == studentGroupId).ToList();
            }

            dgvDiary.DataSource = students;
        }

        private void SetColumnHeader()
        {
            dgvDiary.Columns[0].HeaderText = "Numer";
            dgvDiary.Columns[1].HeaderText = "Imię";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Uwagi";
            dgvDiary.Columns[4].HeaderText = "Matematyka";
            dgvDiary.Columns[5].HeaderText = "Technologia";
            dgvDiary.Columns[6].HeaderText = "Fizyka";
            dgvDiary.Columns[7].HeaderText = "Język polski";
            dgvDiary.Columns[8].HeaderText = "Język obcy";
            dgvDiary.Columns[9].HeaderText = "Dodatkowe zajęcia";
            dgvDiary.Columns[10].HeaderText = "Klasa";
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            //addEditStudent.StudentAdded += AddEditStudent_StudentAdded;
            addEditStudent.FormClosing += AddEditStudent_FormClosing;
            addEditStudent.ShowDialog();
            //addEditStudent.StudentAdded -= AddEditStudent_StudentAdded;
        }

        private void AddEditStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary();
        }

        //private void AddEditStudent_StudentAdded()
        //{
        //    RefreshDiary();
        //}

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (true)
            {
                if(dgvDiary.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Proszę zaznacz ucznia którego dane chcesz edytować");
                    return;
                }

                var addEditStudent = new AddEditStudent(Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
                addEditStudent.FormClosing += AddEditStudent_FormClosing;
                addEditStudent.ShowDialog();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia którego chcesz usunąć");
                return;
            }

            var selectedStudent = dgvDiary.SelectedRows[0];
            var confirmDelete = MessageBox.Show($"Czy na pewno chcesz usunąć ucznia {(selectedStudent.Cells[1].Value.ToString() + " " + selectedStudent.Cells[2].Value.ToString()).Trim()}", "Usuwanie ucznia", MessageBoxButtons.OKCancel);
        
            if(confirmDelete == DialogResult.OK)
            {
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshDiary();
            }
        }

        private void DeleteStudent(int id)
        {
            var students = _fileHelper.DeserializeFromFile();
            students.RemoveAll(x => x.Id == id);

            _fileHelper.SerializeToFile(students);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            //if (cmbChooseClass.Text == "1a")
            //    MessageBox.Show("Wybrałeś klasę 1a");

            //string nazwaKlasy =_groups.Where(x => x.Name == cmbChooseClass.Text);

            //MessageBox.Show(nazwaKlasy);

            RefreshDiary();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(WindowState == FormWindowState.Maximized)
                IsMaximize = true;
            else
                IsMaximize = false;

            Settings.Default.Save();
        }
    }
}
