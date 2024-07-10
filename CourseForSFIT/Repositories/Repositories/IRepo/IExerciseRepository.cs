using Data.Entities;
using Dtos.Results;
using Repositories.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.IRepo
{
    public interface IExerciseRepository : IBaseRepository<Exercise>
    {
    }
}
