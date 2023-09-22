using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LinqTask.DataModel
{
    public class Internship
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Company Company { get; set; }
        public InternshipDetails Details { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
