using LinqTask.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqTask.Services
{
    public class Service : IService
    {
        public List<Internship> Internships  = new List<Internship>();
        public List<Internship> FilteredInternships = new List<Internship>();
        private static int nextInternshipId = 1;

        public void AddInternship(Internship internship)
        {
            internship.Id = nextInternshipId;
            Internships.Add(internship);
            nextInternshipId++;
        }

        public List<Internship> SearchInternships()
        {
           // int.Parse()
            Console.WriteLine("Enter location (or leave empty to skip): ");
            string locationFilter = Console.ReadLine();
            Console.WriteLine("Enter industry (or leave empty to skip): ");
            string industryFilter = Console.ReadLine();
            Console.WriteLine("Enter minimum salary (or leave empty to skip): ");
            int? minSalaryFilter = int.TryParse(Console.ReadLine(),out int minSalary) ? minSalary : (int?)null;
            Console.WriteLine("Enter the StartDate (or leave empty to skip): ");
            string startDate = Console.ReadLine();
            Console.WriteLine("Enter duration (or leave empty to skip): ");
            int? duration = int.TryParse(Console.ReadLine(), out int durationD) ? durationD : (int?)null;


            FilteredInternships = Internships
                .Where(internship =>
                    (string.IsNullOrEmpty(locationFilter) || internship.Company.Location.Equals(locationFilter, StringComparison.OrdinalIgnoreCase))
                    && (string.IsNullOrEmpty(industryFilter) || internship.Company.Industry.Equals(industryFilter, StringComparison.OrdinalIgnoreCase))
                    && (!minSalaryFilter.HasValue || internship.Details.Salary >= minSalaryFilter.Value)
                     && (string.IsNullOrEmpty(startDate) || DateTime.Equals(internship.Details.StartDate, DateTime.Parse(startDate)))
                     && (duration == 0 || internship.Details.Duration == duration)
                     )
                .ToList();
            // Sorting
            //var sortAverageReview = FilteredInternships.OrderByDescending(x => x.Reviews.Average(y => y.Rating));
            //var sortSalary = sortAverageReview.ThenByDescending(x => x.Details.Salary);
            //var sortduration = sortSalary.ThenBy(x => x.Details.Duration);
            //var sortCompany = sortduration.ThenBy(x => x.Company.Name);
            var sortedInternships = FilteredInternships
                .OrderByDescending(FilteredInternships => FilteredInternships.Reviews.Average(review => review.Rating))
                .ThenByDescending(FilteredInternships => FilteredInternships.Details.Salary)
                .ThenBy(FilteredInternships => FilteredInternships.Details.Duration)
                .ThenBy(FilteredInternships => FilteredInternships.Company.Name)
                .ToList();

            // Grouping
            var groupedInternships = sortedInternships
                .GroupBy(internship => internship.Company.Name)
                .Select(group => new
                {
                    CompanyName = group.Key,
                    IndustryGroups = group.GroupBy(internship => internship.Company.Industry)
                })
                .ToList();
            //new

            //var IntershipGroups = FilteredInternships
            //    .GroupBy(x => new { x.Company.Name, x.Company.Location, x.Details.Salary, x.Details.Duration })
            //      .OrderByDescending(g => g.Key.Salary).ThenBy(g => g.Key.Duration)
            //      .Select(g => new
            //      {
            //          Name = g.Key.Name,
            //          Location = g.Key.Location,
            //          Duration = g.Key.Duration,
            //          Internships = g.OrderByDescending(x => x.Reviews)
            //      });
            //foreach (var group in IntershipGroups)
            //{
            //    Console.Write("Name: {0}, Location: {1},Duration: {2}, Count: {3},", group.Name, group.Location, group.Duration, group.Internships.Count());

            //    foreach (var intern in group.Internships)
            //    {
            //        string skills = string.Join(", ", intern.Details.Skills);

            //        Console.WriteLine($"Salary: {intern.Details.Salary}, Remote: {intern.Details.IsRemote}, Skills: {skills}");
            //        // Console.WriteLine(intern.Details.Salary+ "," +  intern.Details.IsRemote + "," + intern.Details.Skills);
            //    }
            //}



            //new


            return FilteredInternships;
        }

        public string DeleteInternship(int id)
        {
            
            var removedCount = Internships.Where(x=>x.Id==id).FirstOrDefault();
            if (removedCount != null)
            {
                Internships.Remove(removedCount);
                return "Internship deleted successfully";
            }
            else
            {
                return "No internship found with id :"+id;
            }
        }

        public string UpdateInternship(int id)
        {
            var updateData = Internships.Where(x => x.Id == id).FirstOrDefault();
            if (updateData != null)
            {
               // var internship = new Internship();
                Console.Write("Internship Name:");
                updateData.Name = Console.ReadLine();
                Console.Write("Company Name:");
                updateData.Company = new Company
                {
                    Name = Console.ReadLine()
                };
                Console.Write("Company Location:");
                updateData.Company.Location = Console.ReadLine();
                Console.Write("Company Industry:");
                updateData.Company.Industry = Console.ReadLine();
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
                        updateData.Details = new InternshipDetails
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
                        updateData.Details.StartDate = startDate;
                        break; // Exit the loop if parsing is successful
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid date in the format 'yyyy-MM-dd'.");
                    }
                }

                Console.Write("Skills (comma-separated):");
                updateData.Details.Skills = Console.ReadLine().Split(',').Select(skill => skill.Trim()).ToList();

                // Duration Input with Validation
                while (true)
                {
                    Console.Write("Duration:");
                    if (int.TryParse(Console.ReadLine(), out int duration))
                    {
                        updateData.Details.Duration = duration;
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
                        updateData.Details.IsRemote = true;
                        break;
                    }
                    else if (isRemoteInput.Equals("no", StringComparison.OrdinalIgnoreCase))
                    {
                        updateData.Details.IsRemote = false;
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
                updateData.Reviews = new List<Review> { review };
                //service.AddInternship(updateData);
                return "Internship updated successfully";
            }
            else
            {
                return "No internship found with id :" + id;
            }
        }
    }
}
