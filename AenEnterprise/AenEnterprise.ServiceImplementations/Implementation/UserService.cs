using AenEnterprise.DataAccess;
using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DomainModel;
using AenEnterprise.DomainModel.UserDomain;
using AenEnterprise.ServiceImplementations.Interface;
using AenEnterprise.ServiceImplementations.Mapping;
using AenEnterprise.ServiceImplementations.Messaging.Users;
using AenEnterprise.ServiceImplementations.ViewModel;
using Azure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AenEnterprise.ServiceImplementations.Implementation
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;
        private IRoleRepository _roleRepository;

        public UserService(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IRoleRepository roleRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
          
        }

        public async Task<GetAllUserResponse>CreateUser(CreateUserRequest request)
        {
            GetAllUserResponse response = new GetAllUserResponse();
            User user = new User();
            user.Username = request.Username;
            user.Password = request.Password;
            user.PasswordHash = request.PasswordHash;
            user.PasswordSalt = request.PasswordSalt;
            user.RoleId = 1;
            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveAsync();
            response.user = user.ConvertToUserViewWithRole();
            return response;
        }

        public string GetMyName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
            return result;
        }

        public async Task<GetUserResponse> GetUserById(int id)
        {
           GetUserResponse response = new GetUserResponse();
            User user =await _userRepository.GetByIdAsync(id);
            response.User = user.ConvertToUserViewWithRole();
           return response;
        }

        public async Task<GetUserResponse> GetUserByName(string userName)
        {
            GetUserResponse response = new GetUserResponse();
             
            //GetUserResponse response = new GetUserResponse();
            //Expression<Func<User,bool>> predicate = entity=>entity.Username==userName;

            //IEnumerable<User> users = _userRepository.Find(predicate);

            //response.Users = users.ConvertToUserViews();
            //return response;    
 

            IEnumerable<User> users =await _userRepository.FindAllAsync();
            
            var user=users.FirstOrDefault(u =>u.Username.Contains(userName));
            response.User = user.ConvertToUserViewWithRole();
            return response;

        }

        //public IQueryable<GetUserResponse> GetMatchingEntities(string userName)
        //{
        //    using (var context = new AenEnterpriseDbContext())
        //    {
        //        var matchingEntities = context.Users.Where(u =>u.Username == userName)
        //        .Select(u => new User
        //        {
        //            Username =u.Username e.Property1, // Replace with your desired properties
        //            Property2 = e.Property2,
        //            // Add more properties as needed
        //        });

        //        return matchingEntities;
        //    }
            
        //}


        public async Task<string> GetRefreshToken(string userName)
        {
            GetUserResponse response = new GetUserResponse();
            IEnumerable<User> UserFind =await _userRepository.FindAllAsync();
            var registeredUserFind = UserFind.Where(u => u.Username.Contains(userName)).SingleOrDefault();
            response.User =registeredUserFind.ConvertToUserViewWithRole();
            return response.User.RefreshToken;
        }

        // Update user and add refresh token when login
        public async Task UpdateUser(UpdateUserRequest updateRequest)
        {
            IEnumerable<User> users =await _userRepository.FindAllAsync();
            var user=users.Where(u=>u.Username.Contains(updateRequest.Username)).SingleOrDefault();

            if (user != null)
            {
                user.RefreshToken = updateRequest.RefreshToken;
                 await _userRepository.UpdateAsync(user);
                await _unitOfWork.SaveAsync();
            }
        }

        public void CreateRole(CreateRoleRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateRoleResponse> UpdateRole(int Id)
        {
           throw new NotImplementedException();
        }

        
    }
}
