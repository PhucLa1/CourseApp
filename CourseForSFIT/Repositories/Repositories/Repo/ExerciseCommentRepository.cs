using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Repositories.Repositories.Base;
using Repositories.Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.Repo
{
    public class ExerciseCommentRepository : BaseRepository<ExerciseComment>, IExerciseCommentRepository
    {
        public ExerciseCommentRepository(CourseForSFITContext context) : base(context)
        {
            
        }
        public IQueryable<ExerciseComment> GetByExerciseId(int exerciseId)
        {
            return _context.exercise_comment.Where(e => e.ExerciseId == exerciseId).AsQueryable();

        }
    }
}
