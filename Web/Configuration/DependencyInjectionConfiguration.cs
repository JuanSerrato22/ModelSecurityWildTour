using Business.Decorators;
using Business.Implements;
using Business.Interfaces;
using Data.Implements;
using Data.Interfaces;
using Entity.Context;
using Microsoft.EntityFrameworkCore;

namespace Web.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            // Business Services with Decorator Pattern
            services.AddScoped<ActivityService>();
            services.AddScoped<IActivityService>(provider =>
            {
                var activityService = provider.GetRequiredService<ActivityService>();
                var logger = provider.GetRequiredService<ILogger<LoggingActivityServiceDecorator>>();
                return new LoggingActivityServiceDecorator(activityService, logger);
            });
            services.AddScoped<IChangeLogService, ChangeLogService>();
            services.AddScoped<IDestinationService, DestinationService>();
            services.AddScoped<IDestinationActivityService, DestinationActivityService>();
            services.AddScoped<IFormService, FormService>();
            services.AddScoped<IFormModuleService, FormModuleService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IRolFormPermissionService, RolFormPermissionService>();
            services.AddScoped<IRolPermissionService, RolPermissionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserActivityService, UserActivityService>();
            services.AddScoped<IUserRolService, UserRolService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Data Repositories
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IChangeLogRepository, ChangeLogRepository>();
            services.AddScoped<IDestinationRepository, DestinationRepository>();
            services.AddScoped<IDestinationActivityRepository, DestinationActivityRepository>();
            services.AddScoped<IFormRepository, FormRepository>();
            services.AddScoped<IFormModuleRepository, FormModuleRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IRolRepository, RolRepository>();
            services.AddScoped<IRolFormPermissionRepository, RolFormPermissionRepository>();
            services.AddScoped<IRolPermissionRepository, RolPermissionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserActivityRepository, UserActivityRepository>();
            services.AddScoped<IUserRolRepository, UserRolRepository>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                // Configuraciones adicionales para desarrollo
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });

            return services;
        }

        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(Program).Assembly, typeof(ActivityService).Assembly);
            return services;
        }

        public static IServiceCollection AddCrosscutting(this IServiceCollection services)
        {
            // Configuraciones transversales como logging, caching, etc.
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            return services;
        }
    }
}