using System;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IActivityRepository ActivityRepository { get; }
        IChangeLogRepository ChangeLogRepository { get; }
        IDestinationRepository DestinationRepository { get; }
        IDestinationActivityRepository DestinationActivityRepository { get; }
        IFormRepository FormRepository { get; }
        IFormModuleRepository FormModuleRepository { get; }
        IModuleRepository ModuleRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IPersonRepository PersonRepository { get; }
        IRolRepository RolRepository { get; }
        IRolFormPermissionRepository RolFormPermissionRepository { get; }
        IRolPermissionRepository RolPermissionRepository { get; }
        IUserRepository UserRepository { get; }
        IUserActivityRepository UserActivityRepository { get; }
        IUserRolRepository UserRolRepository { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}