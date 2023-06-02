using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laboratorna1
{
    [Serializable]
    public class Group
    {
        public string Id { get; set; }
        public int StudentsQuantity { get; set; }
        public List<Student> Students { get; set; }
        public Group(string Id)
        {
            this.Id = Id;
            this.StudentsQuantity = 0;
            this.Students = new List<Student>();
        }

        public Group() { }
    }
}
