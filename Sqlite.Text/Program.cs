using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlite.Text
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseContext context = new DatabaseContext();
            Console.WriteLine("Enter name");
            string name = Console.ReadLine();

            Warehouse warehouse = new Warehouse()
            {
                Name = name

            };
            context.Warehouse.Add(warehouse);
            context.SaveChanges();

            var data = context.Warehouse.ToList();
            foreach (var item in data)
            {
                Console.Write(string.Format("ID : {0}  Name : {1}", item.Id, item.Name));
            }

            Console.ReadKey();


        }
    }
}
