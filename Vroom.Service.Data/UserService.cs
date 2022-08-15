using AutoMapper;
using Bytes2you.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vroom.Common;
using Vroom.Data.Common;
using Vroom.Data.Contracts;
using Vroom.Data.Models;
using Vroom.Providers.Contracts;
using Vroom.Service.Contracts;
using Vroom.Service.Models;

namespace Vroom.Service.Data
{
    public class UserService : IUserService
    {
        private readonly IEfDbRepository<ApplicationUser> userRepository;
        private readonly IVroomDbContextSaveChanges vroomDbContextSaveChanges;
        private readonly IApplicationUserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public UserService(
            IEfDbRepository<ApplicationUser> userRepository, 
            IApplicationUserManager<ApplicationUser> userManager, 
            IVroomDbContextSaveChanges vroomDbContextSaveChanges, 
            IMapper mapper)
        {

            Guard.WhenArgument<IEfDbRepository<ApplicationUser>>(userRepository, GlobalConstants.UserRepositoryNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IMapper>(mapper, GlobalConstants.MapperProviderNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IVroomDbContextSaveChanges>(vroomDbContextSaveChanges, GlobalConstants.DbContextSaveChangesNullExceptionMessege).IsNull().Throw();
            Guard.WhenArgument<IApplicationUserManager<ApplicationUser>>(userManager, GlobalConstants.UserManagerNullExceptionMessege).IsNull().Throw();

            this.userRepository = userRepository;
            this.vroomDbContextSaveChanges = vroomDbContextSaveChanges;
            this.userManager = userManager;
            this.mapper = mapper;
        }
        public async Task AddToRoleAsync(ApplicationUserServiceModel user, string newRole)
        {
            Guard.WhenArgument<ApplicationUserServiceModel>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();
            Guard.WhenArgument<string>(newRole, GlobalConstants.GetMemberName(() => newRole)).IsNullOrEmpty().Throw();

            var dbUser = this.userRepository.GetById(user.Id);
            await this.userManager.AddToRoleAsync(dbUser, newRole);
        }

        public Task<ApplicationUserServiceModel> GetByIdAsync(string id)
        {
            Guard.WhenArgument<string>(id, GlobalConstants.GetMemberName(() => id)).IsNullOrEmpty().Throw();

            return Task.FromResult(this.mapper.Map<ApplicationUser,ApplicationUserServiceModel>(this.userRepository.GetById(id)));
        }

        public async Task<PagedUserServiceModel> GetFilteredUsersInRoleAsync(string executive, int pageNumber, int pageSize, string sortOrder, string searchString)
        {
            var skip = (pageNumber * pageSize) - pageSize;
            var users = await this.userManager.GetUsersInRoleAsync(executive);

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(x => x.UserName.ToLower().Contains(searchString.ToLower())).ToList();
            }

            if (string.IsNullOrEmpty(sortOrder))
            {
                users = users.OrderBy(x => x.UserName).ToList();
            }
            else
            {
                users = users.OrderByDescending(x => x.UserName).ToList();
            }
            var result = new PagedUserServiceModel { TotalSize = users.Count() };
            var normalUserModels = this.mapper.Map<List<ApplicationUser>, List<ApplicationUserServiceModel>>(users.Skip(skip).Take(pageSize).ToList());

            result.Users = normalUserModels;

            return result;
        }

        public async Task<IEnumerable<ApplicationUserServiceModel>> GetUsersInRoleAsync(string admin)
        {
            Guard.WhenArgument<string>(admin, GlobalConstants.GetMemberName(() => admin)).IsNullOrEmpty().Throw();
            return this.mapper.Map<IList<ApplicationUser>, IList<ApplicationUserServiceModel>>(await this.userManager.GetUsersInRoleAsync(admin));
        }

        public async Task RemoveFromRoleAsync(ApplicationUserServiceModel user, string currentRole)
        {
            Guard.WhenArgument<ApplicationUserServiceModel>(user, GlobalConstants.GetMemberName(() => user)).IsNull().Throw();
            Guard.WhenArgument<string>(currentRole, GlobalConstants.GetMemberName(() => currentRole)).IsNullOrEmpty().Throw();

            var dbUser = this.userRepository.GetById(user.Id);

            Guard.WhenArgument<ApplicationUser>(dbUser, "there is no such user").IsNull().Throw();
           

            await this.userManager.RemoveFromRoleAsync(dbUser, currentRole);
        }
    }
}
