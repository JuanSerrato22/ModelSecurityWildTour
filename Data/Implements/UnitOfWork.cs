using Data.Interfaces;
using Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Data.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context,
            IActivityRepository activityRepository,
            IChangeLogRepository changeLogRepository,
            IDestinationRepository destinationRepository,
            IDestinationActivityRepository destinationActivityRepository,
            IFormRepository formRepository,
            IFormModuleRepository formModuleRepository,
            IModuleRepository moduleRepository,
            IPaymentRepository paymentRepository,
            IPermissionRepository permissionRepository,
            IPersonRepository personRepository,
            IRolRepository rolRepository,
            IRolFormPermissionRepository rolFormPermissionRepository,
            IRolPermissionRepository rolPermissionRepository,
            IUserRepository userRepository,
            IUserActivityRepository userActivityRepository,
            IUserRolRepository userRolRepository)
        {
            _context = context;
            ActivityRepository = activityRepository;
            ChangeLogRepository = changeLogRepository;
            DestinationRepository = destinationRepository;
            DestinationActivityRepository = destinationActivityRepository;
            FormRepository = formRepository;
            FormModuleRepository = formModuleRepository;
            ModuleRepository = moduleRepository;
            PaymentRepository = paymentRepository;
            PermissionRepository = permissionRepository;
            PersonRepository = personRepository;
            RolRepository = rolRepository;
            RolFormPermissionRepository = rolFormPermissionRepository;
            RolPermissionRepository = rolPermissionRepository;
            UserRepository = userRepository;
            UserActivityRepository = userActivityRepository;
            UserRolRepository = userRolRepository;
        }

        public IActivityRepository ActivityRepository { get; }
        public IChangeLogRepository ChangeLogRepository { get; }
        public IDestinationRepository DestinationRepository { get; }
        public IDestinationActivityRepository DestinationActivityRepository { get; }
        public IFormRepository FormRepository { get; }
        public IFormModuleRepository FormModuleRepository { get; }
        public IModuleRepository ModuleRepository { get; }
        public IPaymentRepository PaymentRepository { get; }
        public IPermissionRepository PermissionRepository { get; }
        public IPersonRepository PersonRepository { get; }
        public IRolRepository RolRepository { get; }
        public IRolFormPermissionRepository RolFormPermissionRepository { get; }
        public IRolPermissionRepository RolPermissionRepository { get; }
        public IUserRepository UserRepository { get; }
        public IUserActivityRepository UserActivityRepository { get; }
        public IUserRolRepository UserRolRepository { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}