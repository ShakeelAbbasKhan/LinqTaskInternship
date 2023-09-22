using LinqTask.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqTask.Services
{
    public interface IService
    {
        public void AddInternship(Internship internship);
        public List<Internship> SearchInternships();
    }
}
