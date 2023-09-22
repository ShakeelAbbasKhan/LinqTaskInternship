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

        public void AddInternship(Internship internship)
        {
            internship.Id = Internships.Count + 1;
            Internships.Add(internship);
        }

        public async Task<List<Internship>> SearchInternships()
        {
           // int.Parse()
            Console.WriteLine("Enter the location (or leave empty to skip): ");
            string locationFilter = Console.ReadLine();
            Console.WriteLine("Enter the industry (or leave empty to skip): ");
            string industryFilter = Console.ReadLine();
            Console.WriteLine("Enter the minimum salary (or leave empty to skip): ");
            int? minSalaryFilter = int.TryParse(Console.ReadLine(),out int minSalary) ? minSalary : (int?)null;
            Console.WriteLine("Enter the StartDate (or leave empty to skip): ");
            string startDate = Console.ReadLine();
            //Console.WriteLine("Enter the Duration (or leave empty to skip): ");
            //int duration = int.Parse(Console.ReadLine());
           

            FilteredInternships = Internships
                .Where(internship =>
                    (string.IsNullOrEmpty(locationFilter) || internship.Company.Location.Equals(locationFilter, StringComparison.OrdinalIgnoreCase))
                    && (string.IsNullOrEmpty(industryFilter) || internship.Company.Industry.Equals(industryFilter, StringComparison.OrdinalIgnoreCase))
                    && (!minSalaryFilter.HasValue || internship.Details.Salary >= minSalaryFilter.Value)
                     && (string.IsNullOrEmpty(startDate) || DateTime.Equals(internship.Details.StartDate, DateTime.Parse(startDate)))
                     //&& (duration == 0 || internship.Details.Duration < duration)
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

            var IntershipGroups = FilteredInternships
                .GroupBy(x => new { x.Company.Name, x.Company.Location, x.Details.Salary, x.Details.Duration })
                  .OrderByDescending(g => g.Key.Salary).ThenBy(g => g.Key.Duration)
                  .Select(g => new
                  {
                      Name = g.Key.Name,
                      Location = g.Key.Location,
                      Duration = g.Key.Duration,
                      Internships = g.OrderByDescending(x => x.Reviews)
                  });
            foreach (var group in IntershipGroups)
            {
                Console.Write("Name: {0}, Location: {1},Duration: {2}, Count: {3},", group.Name, group.Location, group.Duration, group.Internships.Count());

                foreach (var intern in group.Internships)
                {
                    string skills = string.Join(", ", intern.Details.Skills);

                    Console.WriteLine($"Salary: {intern.Details.Salary}, Remote: {intern.Details.IsRemote}, Skills: {skills}");
                    // Console.WriteLine(intern.Details.Salary+ "," +  intern.Details.IsRemote + "," + intern.Details.Skills);
                }
            }



            //new


            return FilteredInternships;
        }
    }
}
