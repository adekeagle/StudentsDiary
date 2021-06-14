using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {
        //public delegate void MySimpleDelegate();
        //public event MySimpleDelegate StudentAdded;

        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        private int _studentId;
        private Student _student;

        private List<Group> _groups;

        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            _studentId = id;



            //var studentClasses = GetStudentClass();

            //foreach (var item in studentClasses)
            //{
            //    cboClassName.Items.Add(item.Name);
            //}
            //StudentGroups();

            _groups = new List<Group>
            {
                new Group { Id = 0, Name = "Wybierz" },
                new Group { Id = 1, Name = "1a" },
                new Group { Id = 2, Name = "2b" }
            };

            InitStudentGroup();

            GetStudentData();

            tbFirstName.Select();
        }

        //private void OnStudentAdded()
        //{
        //    StudentAdded?.Invoke();
        //}

        private void InitStudentGroup()
        {
            cboClassName.DataSource = _groups;
            cboClassName.DisplayMember = "Name";
            cboClassName.ValueMember = "Id";
        }

        private void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edytowanie danych ucznia";

                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                    throw new Exception("Brak użytkownika o podanym Id");

                FillTextBoxes();
            }
        }

        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            tbMath.Text = _student.Math;
            tbPhysics.Text = _student.Physics;
            tbTechnology.Text = _student.Technology;
            tbPolishLang.Text = _student.PolishLang;
            tbForeignLang.Text = _student.ForeignLang;
            rtbComments.Text = _student.Comments;
            cbIsAdditionalActivities.Checked = _student.AdditionalActivities;
            cboClassName.SelectedItem = _groups.FirstOrDefault(x => x.Id == _student.GroupId);
        }

        private void btnConfirm_ClickAsync(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            //var zmienna = _student.IsParticipateInExtraActivities ? "TAK" : "NIE";

            if (_studentId != 0)
                students.RemoveAll(x => x.Id == _studentId);
            else
                AssignIdToNewStudent(students);

            AddNewUserToList(students);

            _fileHelper.SerializeToFile(students);

            //OnStudentAdded();
            //await LongProcessAsync();

            Close();
        }

        //private async Task LongProcessAsync()
        //{
        //    await Task.Run(() =>
        //    {
        //        Thread.Sleep(7000);
        //    });
        //}

        private void AddNewUserToList(List<Student> students)
        {
            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = rtbComments.Text,
                PolishLang = tbPolishLang.Text,
                ForeignLang = tbForeignLang.Text,
                Math = tbMath.Text,
                Physics = tbPhysics.Text,
                Technology = tbTechnology.Text,
                AdditionalActivities = cbIsAdditionalActivities.Checked,
                GroupId = (cboClassName.SelectedItem as Group).Id
            };

            students.Add(student);
        }


        private void AssignIdToNewStudent(List<Student> students)
        {
            var studentWithHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();

            _studentId = studentWithHighestId == null ? 1 : studentWithHighestId.Id + 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbIsAdditionalActivities_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsAdditionalActivities.Checked)
                _student.AdditionalActivities = true;
            else
                _student.AdditionalActivities = false;
        }

        private void cboClassName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //private void cboClassName_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var studentClasses = GetStudentClass();

        //    _student.GroupId = studentClasses.Where(x => x.Name == Name);
        //}
    }
}