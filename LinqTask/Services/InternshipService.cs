using LinqTask.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqTask.Services
{
    public class InternshipService
    {
        private readonly IService service;

        public InternshipService(IService service)
        {
            this.service = service;
        }


        public void AddingData()
        {
            var internship = new Internship();
            Console.Write("Internship Name:");
            internship.Name = Console.ReadLine();
            Console.Write("Company Name:");
            internship.Company = new Company
            {
                Name = Console.ReadLine()
            };
            Console.Write("Company Location:");
            internship.Company.Location = Console.ReadLine();
            Console.Write("Company Industry:");
            internship.Company.Industry = Console.ReadLine();
            //Console.Write("Salary:");
            //internship.Details = new InternshipDetails
            //{
            //    Salary = int.Parse(Console.ReadLine())
            //};

            while (true)
            {
                Console.Write("Salary:");
                if (int.TryParse(Console.ReadLine(), out int salary))
                {
                    internship.Details = new InternshipDetails
                    {
                        Salary = salary
                    };
                    break; // Exit the loop if parsing is successful
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer for Salary.");
                }
            }

            // Start Date Input with Validation
            while (true)
            {
                Console.Write("Start Date (yyyy-MM-dd):");
                if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
                {
                    internship.Details.StartDate = startDate;
                    break; // Exit the loop if parsing is successful
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid date in the format 'yyyy-MM-dd'.");
                }
            }

            Console.Write("Skills (comma-separated):");
            internship.Details.Skills = Console.ReadLine().Split(',').Select(skill => skill.Trim()).ToList();

            // Duration Input with Validation
            while (true)
            {
                Console.Write("Duration:");
                if (int.TryParse(Console.ReadLine(), out int duration))
                {
                    internship.Details.Duration = duration;
                    break; // Exit the loop if parsing is successful
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer for Duration.");
                }
            }

            while (true)
            {
                Console.Write("Is remote? (yes/no):");
                string isRemoteInput = Console.ReadLine();
                if (isRemoteInput.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    internship.Details.IsRemote = true;
                    break;
                }
                else if (isRemoteInput.Equals("no", StringComparison.OrdinalIgnoreCase))
                {
                    internship.Details.IsRemote = false;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
                }
            }
            Console.Write("Review:1.Rating:");
            Review review = new Review
            {
                Rating = double.Parse(Console.ReadLine())
            };
            Console.Write("2.Comment:");
            review.Comment = Console.ReadLine();
            internship.Reviews = new List<Review> { review };
            service.AddInternship(internship);
        }

        public void Searching()
        {
            //IEnumerable<Internship> inter = service.SearchInternships().Result;
            //var abc = inter.Where(x => x.Id == 5);

            //ICollection<Internship> list = service.SearchInternships().Result.ToList();
            ////list.Contains()


            //IList<Internship> listing = service.SearchInternships().Result.ToList();
            //listing.Insert(3, internship);    
             
            
            
            
            // Search
            var internships = service.SearchInternships().ToList();


            //Result
            if (internships.Count == 0)
            {
                Console.WriteLine("No internships match the criteria.");
            }
            else
            {
                foreach (var internship in internships)
                {
                    Console.WriteLine($"Id:{internship.Id}");
                    Console.WriteLine($"Internship Name: {internship.Name}");
                    Console.WriteLine($"Company Name: {internship.Company.Name}");
                    Console.WriteLine($"Location: {internship.Company.Location}");
                    Console.WriteLine($"Industry: {internship.Company.Industry}");
                    Console.WriteLine($"Salary: {internship.Details.Salary}");
                    Console.WriteLine();
                }
            }
        }
        public void DeletedData()
        {
            Console.Write("Enter Id of internship to Delete :");
            int Id = Convert.ToInt32(Console.ReadLine());
            var result = service.DeleteInternship(Id);
            if(result != null)
            {
                Console.WriteLine(result.ToString());
            }
        }
    }
}
