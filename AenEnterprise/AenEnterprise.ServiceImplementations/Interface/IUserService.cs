using AenEnterprise.ServiceImplementations.Messaging.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Interface
{
    public interface IUserService
    {
        string GetMyName();
        Task<GetAllUserResponse>CreateUser(CreateUserRequest userRequest);
        Task<GetUserResponse> GetUserById(int id);
        Task<GetUserResponse> GetUserByName(string userName);
        Task UpdateUser(UpdateUserRequest updateRequest);
        Task<string> GetRefreshToken(string userName);

        //Implement Role interface
        void CreateRole(CreateRoleRequest request);
        Task<UpdateRoleResponse> UpdateRole(int Id);
    }
}
