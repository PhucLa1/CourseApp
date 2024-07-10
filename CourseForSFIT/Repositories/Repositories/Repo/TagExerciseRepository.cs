using Data.Data;
using Data.Entities;
using Repositories.Repositories.Base;
using Repositories.Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.Repo
{
    public class TagExerciseRepository : BaseRepository<TagExercise>, ITagExerciseRepository
    {
        public TagExerciseRepository(CourseForSFITContext context) : base(context)
        {
        }

    }
}
