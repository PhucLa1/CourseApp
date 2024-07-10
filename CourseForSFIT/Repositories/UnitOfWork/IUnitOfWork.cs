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
        IExerciseCommentRepository ExerciseCommentRepository { get; }
        ITestCaseRepository TestCaseRepository { get; }
        IUserExerciseRepository UserExerciseRepository { get; }

        IExerciseHasTagRepository ExerciseHasTagRepository { get; }
        Task SaveAsync();
    }
}
