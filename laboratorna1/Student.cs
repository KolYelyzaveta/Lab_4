using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laboratorna1
{
    [Serializable]
    public class Student
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Faculty { get; set; }
        public string Group { get; set; }

        public Student(string name, string surname, string patronymic, string faculty, string group)
        {
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Faculty = faculty;
            Group = group;
        }

        public Student() { }
    }
}
