using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;
using Repositories.Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.Repo
{
    public class TestCaseRepository : BaseRepository<TestCase>, ITestCaseRepository
    {
        public TestCaseRepository(CourseForSFITContext context) : base(context)
        {
            
        }
        public async Task<IEnumerable<TestCase>> GetAllTestCaseNotLockByExerciseId(int exerciseId)
        {
            return await _context.test_case.Where(e => e.ExerciseId == exerciseId && e.IsLock == false).ToListAsync();
        }
        public IQueryable<TestCase> GetAllTestCaseByExerciseId(int exerciseId)
        {
            return _context.test_case.Where(e => e.ExerciseId == exerciseId);
        }
    }
}
