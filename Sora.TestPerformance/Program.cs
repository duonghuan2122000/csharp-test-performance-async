using System.Diagnostics;

namespace Sora.TestPerformance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine("Main Thread");
            var people = Person.GeneratePeople(100000);
            ProcessPersons(people);
            Console.WriteLine("End Main Thread");
            sw.Stop();
            Console.WriteLine($"Main Thread: {sw.ElapsedMilliseconds} ms giây");
            Console.ReadKey();
        }

        /// <summary>
        /// Hàm xử lý 1 person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        static async Task<string> ProcessPerson(Person person, bool error = false)
        {
            await Task.Delay(1000);
            if (error)
                throw new Exception();
            string message = $"Person: Id={person.Id}, Name={person.Name}";
            return message;
        }

        static async Task ProcessPersons(List<Person> people)
        {
            var sw = Stopwatch.StartNew();

            // Cách 1: Thông thường
            //var messages = new List<string>();
            //foreach (var person in people)
            //{
            //    var msg = await ProcessPerson(person);
            //    messages.Add(msg);
            //}

            // Cách 2: Sử dụng Task.WhenAll()
            //var tasks = new List<Task<string>>();
            //int i = 0;
            //foreach (var person in people)
            //{
            //    if(i == 100)
            //    {
            //        tasks.Add(ProcessPerson(person, true));
            //    } else
            //    {
            //        tasks.Add(ProcessPerson(person));
            //    }
            //    i++;
            //}
            //try
            //{
            //    var messages = await Task.WhenAll(tasks);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            // Cách 3: Sử dụng Task.Run kết hợp Task.WhenAll
            var tasks = new List<Task<string>>();

            await Task.Run(() =>
            {
                foreach (var person in people)
                {
                    tasks.Add(ProcessPerson(person));
                }
            });
            var messages = await Task.WhenAll(tasks);

            //Console.WriteLine($"{string.Join(";", messages)}");
            sw.Stop();
            Console.WriteLine($"ProccessPersons: {people.Count} in {sw.ElapsedMilliseconds} ms giây");
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static List<Person> GeneratePeople(int number)
        {
            List<Person> people = new List<Person>();
            for(int i = 0; i < number; i++)
            {
                Person person = new Person
                {
                    Id = i + 1,
                    Name = "Person " + (i + 1)
                };
                people.Add(person);
            }
            return people;
        }
    }
}
