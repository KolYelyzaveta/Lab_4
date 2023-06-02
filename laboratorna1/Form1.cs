using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laboratorna1
{
    public partial class Form1 : Form
    {
        public List<Student> Students { get; set; }
        public List<Group> Groups { get; set; }
        public string cell;

        public Form1()
        {
            Students = new List<Student>() 
            {
                new Student("Геннадий", "Надёжный", "Александрович", "КИУ", "КИУКИ-20-1"),
                new Student("Александр", "Родниченко", "Борисович", "КИУ", "КИУКИ-20-1"),
                new Student("Борис", "Щебенкин", "Владимирович", "КИУ", "КИУКИ-20-1"),
                new Student("Юрий", "Корма", "Юриевич", "КИУ", "КИУКИ-20-3"),
                new Student("Денис", "Курганский", "Олегович", "КИУ", "КИУКИ-20-2"),
                new Student("Илья", "Крочак", "Антонович", "КИУ", "-"),
                new Student("Валентин", "Коваленко", "Ростиславович", "КИУ", "-"),
                new Student("Павел", "Мельниченко", "Эдуардович", "КИУ", "-"),
            };
            Groups = new List<Group>() 
            {
                new Group("КИУКИ-20-1"),
                new Group("КИУКИ-20-2"),
                new Group("КИУКИ-20-3"),
                new Group("КИУКИ-20-4")
            };
            InitializeComponent();
        }

        public static void SerializeJson<T>(string fileName, IEnumerable<T> data)
        {
            var json = new DataContractJsonSerializer(data.GetType());
            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                json.WriteObject(file, data);
            }
        }

        public static IEnumerable<T> DeserializeJson<T>(string fileName, Type result)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }
            var json = new DataContractJsonSerializer(result);
            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                return json.ReadObject(file) as List<T>;
            }
        }

        private void FillGroups()
        {
            foreach (var group in Groups)
            {
                if (group.Students == null)
                {
                    continue;
                }
                group.Students.Clear();
                group.StudentsQuantity = 0;
                foreach (var student in Students)
                {
                    if (group.Id == student.Group)
                    {
                        group.Students.Add(student);
                        group.StudentsQuantity++;
                    }
                }
            }
            SerializeJson<Group>("Groups.json", Groups);
        }

        public void FillData()
        {
            BindingSource binding = new BindingSource
            {
                DataSource = Students
            };
            dataGridView1.DataSource = binding;


            BindingSource binding1 = new BindingSource
            {
                DataSource = Groups
            };
            dataGridView2.DataSource = binding1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var studentsList = DeserializeJson<Student>("Students.json", typeof(List<Student>));
                Students = studentsList.ToList();
                var groupsList = DeserializeJson<Group>("Groups.json", typeof(List<Group>));
                Groups = groupsList.ToList();
            }
            catch { }
            FillData();
            FillGroups();
            SerializeJson("Students.json", Students);
            SerializeJson("Groups.json", Groups);
        }

        private void aSCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Student> orderedASC = Students.OrderBy(student => student.Name).ToList();
            Students = orderedASC;
            File.Delete("Students.json");
            SerializeJson<Student>("Students.json", Students);
            FillData();
        }

        private void dESCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Student> orderedASC = Students.OrderBy(student => student.Name).ToList();
            Students = orderedASC;
            Students.Reverse();
            File.Delete("Students.json");
            SerializeJson<Student>("Students.json", Students);
            FillData();
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].Cells[0].Value == null || dataGridView2.Rows[e.ColumnIndex].Index != 0)
            {
                return;
            }
            else
            {
                cell = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                Form2 groupStudents = new Form2(this, Students, Groups);
                groupStudents.Show();
            }
        }

        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 editForm = new Form3(this, Students, Groups);
            editForm.Show();
        }
    }
}
