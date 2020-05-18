using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch
{
    class User
    {
        public String firstName { get; set; }
        public String lastName { get; set; }
        public int age { get; set; }

        public User(String fName, String lName, int age)
        {
            this.firstName = fName;
            this.lastName = lName;
            this.age = age;
        }

        public override String ToString()
        {
            return $"{this.firstName} {this.lastName} is {this.age} years old";
        }
    }
}
