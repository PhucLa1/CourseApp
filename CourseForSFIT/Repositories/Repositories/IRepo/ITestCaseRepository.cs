using Data.Entities;
using Repositories.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.IRepo
{
    public interface ITestCaseRepository : IBaseRepository<TestCase>
    {
        Task<IEnumerable<TestCase>> GetAllTestCaseNotLockByExerciseId(int exerciseId);
        IQueryable<TestCase> GetAllTestCaseByExerciseId(int exerciseId);
    }
}
