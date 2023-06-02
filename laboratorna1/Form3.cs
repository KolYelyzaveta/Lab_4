using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laboratorna1
{
    public partial class Form3 : Form
    {
        public Form1 form_ { get; set; }
        public List<Student> Students { get; set; }
        public List<Group> Groups { get; set; }
        public Form3(Form1 form3, List<Student> students, List<Group> groups)
        {
            this.form_ = form3;
            this.Students = this.form_.Students;
            this.Groups = this.form_.Groups;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Id не может быть пустым!", "Attention!", MessageBoxButtons.OK);
                return;
            }
            Group newGroup = new Group();
            newGroup.Id = textBox1.Text;
            newGroup.StudentsQuantity = 0;
            newGroup.Students = new List<Student>();
            Groups.Add(newGroup);
            SaveAll();
            form_.FillData();
            MessageBox.Show("Группа добавлена!", "Attention!", MessageBoxButtons.OK);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = false;
            foreach (var group in Groups)
            {
                if (group.Id == textBox1.Text)
                {
                    Groups.Remove(group);
                    MessageBox.Show("Группа удалена!", "Attention!", MessageBoxButtons.OK);
                    flag = true;
                    dataGridView2.Rows.Clear();
                    foreach (var student in Students)
                    {
                        if (student.Group == textBox1.Text)
                        {
                            student.Group = "-";
                        }
                        form_.FillData();
                        SaveAll();
                        GetStudents();
                    }
                    SaveAll();
                    return;
                }
            }
            if (!flag)
            {
                MessageBox.Show("Группы с таким Id нету!", "Attention!", MessageBoxButtons.OK);
            }
        }

        public static void SerializeJson<T>(string fileName, IEnumerable<T> data)
        {
            var json = new DataContractJsonSerializer(data.GetType());
            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                json.WriteObject(file, data);
            }
        }

        public void GetStudents()
        {
            List<Student> freeStudents = Students.Where(student => student.Group == "-").ToList();
            BindingSource binding2 = new BindingSource
            {
                DataSource = freeStudents
            };
            dataGridView1.DataSource = binding2;
        }

        public void SaveAll()
        {
            File.Delete("Students.json");
            SerializeJson("Students.json", Students);
            File.Delete("Groups.json");
            SerializeJson("Groups.json", Groups);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (var group in Groups)
            {
                if (group.Id == textBox1.Text && !String.IsNullOrWhiteSpace(textBox2.Text) && !String.IsNullOrEmpty(textBox2.Text))
                {
                    group.Id = textBox2.Text;
                    MessageBox.Show("Идентификатор группы изменён!", "Attention!", MessageBoxButtons.OK);
                    form_.FillData();
                    foreach (var student in group.Students)
                    {
                        student.Group = textBox2.Text;
                    }
                    BindingSource binding3 = new BindingSource
                    {
                        DataSource = group.Students
                    };
                    dataGridView2.DataSource = binding3;
                }
                if (group.Id == textBox1.Text && String.IsNullOrWhiteSpace(textBox2.Text) && String.IsNullOrEmpty(textBox2.Text))
                {
                    BindingSource binding3 = new BindingSource
                    {
                        DataSource = group.Students
                    };
                    dataGridView2.DataSource = binding3;
                }
            }
            SaveAll();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedStudent = dataGridView1.SelectedRows[0].DataBoundItem as Student;
                foreach (var group in Groups)
                {
                    if (group.StudentsQuantity == 35)
                    {
                        MessageBox.Show("В группе уже есть 35 студентов!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (group.Id == textBox1.Text)
                    {
                        group.Students.Add(selectedStudent);
                        selectedStudent.Group = textBox1.Text;
                        group.StudentsQuantity++;
                        GetStudents();
                        BindingSource binding3 = new BindingSource
                        {
                            DataSource = group.Students
                        };
                        dataGridView2.DataSource = binding3;
                        form_.FillData();
                        SaveAll();
                    }
                }
            }
            catch { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedStudent = dataGridView2.SelectedRows[0].DataBoundItem as Student;
                foreach (var group in Groups)
                {
                    if (group.Id == textBox1.Text)
                    {
                        group.Students.Remove(selectedStudent);
                        selectedStudent.Group = "-";
                        group.StudentsQuantity--;
                        GetStudents();
                        BindingSource binding3 = new BindingSource
                        {
                            DataSource = group.Students
                        };
                        dataGridView2.DataSource = binding3;
                        form_.FillData();
                        SaveAll();
                    }
                }
            }
            catch { }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            GetStudents();
        }
    }
}
