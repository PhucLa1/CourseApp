using NPOI.SS.Formula.Functions;
using Repositories.Repositories.Base;
using Repositories.Repositories.IRepo;

namespace Repositories.unitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        ITagExerciseRepository TagExerciseRepository { get; }
        IExerciseRepository ExerciseRepository { get; }
        Task SaveAsync();
    }
}
