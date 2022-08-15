using System;
using System.Linq.Expressions;

namespace Vroom.Common
{
    public class GlobalConstants
    {
        public const string UserIdNullExceptionMessage = "User Id cannot be null";

        public const string DbContextNullExceptionMessege = "Database context cannot be null.";
        public const string DbContextSaveChangesNullExceptionMessege = "DbContext cannot be null.";
        public const string EntityToAddNullExceptionMessage = "Cannot Add null object.";
        public const string EntityToDeleteNullExceptionMessage = "Cannot Delete null object.";
        public const string EntityToHardDeleteNullExceptionMessage = "Cannot Hard Delete null object.";

        public const string BikeRepositoryNullExceptionMessege = "Bike repository cannot be null.";
        public const string UserRepositoryNullExceptionMessege = "User repository cannot be null.";
        public const string MakeRepositoryNullExceptionMessege = "Make repository cannot be null.";
        public const string ModelRepositoryNullExceptionMessege = "Model repository cannot be null.";
        public const string CacheProviderNullExceptionMessege = "Cache Provider cannot be null.";

        public const string BikeServiceNullExceptionMessege = "Bike service cannot be null.";
        public const string ModelServiceNullExceptionMessege = "Model service cannot be null.";
        public const string MakeServiceNullExceptionMessege = "Make Service cannot be null.";
        public const string UserServiceNullExceptionMessege = "User Service cannot be null.";
        public const string AuthenticationProviderNullExceptionMessege = "Authentication provider cannot be null.";
        public const string MapperProviderNullExceptionMessege = "Mapper provider cannot be null.";
        public const string UserManagerNullExceptionMessege = "UserManager cannot be null.";
        public const string RoleManagerNullExceptionMessege = "RoleManager cannot be null.";
        public const string RoleNullExceptionMessege = "Role cannot be null.";
        public const string RoleNullOrEmptyStringExceptionMessege = "Role cannot be null or empty string.";
        public const string SignInManagerNullExceptionMessege = "SignInManager cannot be null.";

        public const string NewsCacheKey = "NewsKey";
        public const string AdministratorRoleName = "Administrator";
        public const string CannotBeZero = "Cannot be zero or less";

        public const int PasswordMinLength = 1;
        public const int Zero = 0;
        public const int MessagePerTake = 5;
        public const int TripsPerTake = 5;
        public const int NewsPerTake = 5;
        public const int UsersPerTake = 5;

        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return string.Format("Parameter {0} cannot be null or empty",expressionBody.Member.Name);
        }
    }
}
