using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch
{
    class Program
    {
        private static String esHost;
        private static IndexController index;
        private static SearchController search;
        static async Task Main(string[] args)
        {
            esHost = "http://localhost:9200";
            String firstName = "Ali";
            String lastName = "Rehman";
            int age = 23;
            List<User> listOfUsers = new List<User>();

            User newUser = new User(firstName, lastName, age);
            index = new IndexController(esHost);
            search = new SearchController(esHost);
            bool indexed = await index.indexSingleUser(newUser);
            if (indexed)
            {
                Console.WriteLine("Indexed single User");
            }

            for (int i = 0; i < 3; i++)
            {
                firstName = firstName + " z";
                lastName = lastName + " v";
                var rand = new Random();
                age = rand.Next(20, 60);

                listOfUsers.Add(new User(firstName, lastName, age));
            }

            indexed = await index.indexManyUsers(listOfUsers);
            if (indexed)
            {
                Console.WriteLine("Indexed multiple Users");
            }

            search.searchUsers("v", true);

            Console.ReadLine();
        }
    }
}
