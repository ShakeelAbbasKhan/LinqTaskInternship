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
            Console.Write("Salary:");
            internship.Details = new InternshipDetails
            {
                Salary = int.Parse(Console.ReadLine())
            };
            Console.Write("Start Date:");
            internship.Details.StartDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", null);
            Console.Write("Skills:");
            internship.Details.Skills = Console.ReadLine().Split(',').Select(skill => skill.Trim()).ToList();
            Console.Write("Duration:");
            internship.Details.Duration = int.Parse(Console.ReadLine());
            Console.Write("Is remote? (yes/no):");
            internship.Details.IsRemote = Console.ReadLine().Equals("yes", StringComparison.OrdinalIgnoreCase);
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
                    Console.WriteLine($"Internship Name: {internship.Name}");
                    Console.WriteLine($"Company Name: {internship.Company.Name}");
                    Console.WriteLine($"Location: {internship.Company.Location}");
                    Console.WriteLine($"Industry: {internship.Company.Industry}");
                    Console.WriteLine($"Salary: ${internship.Details.Salary}");
                    Console.WriteLine();
                }
            }
        }
    }
}
