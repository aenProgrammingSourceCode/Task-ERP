using AenEnterprise.DataAccess;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.DomainModel.UserDomain;
using AenEnterprise.ServiceImplementations.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Mapping
{
    public static class UserExtentionMethods
    {
        public static UserView ConvertToUserViewWithRole(this User user)
        {
            using (var context = new AenEnterpriseDbContext())
            {
                if (user == null)
                {
                    return null;
                }

                User userWithRole;

                // Check if the SalesOrder is already being tracked by the DbContext
                var entry = context.ChangeTracker.Entries<User>().FirstOrDefault(e => e.Entity.Id == user.Id);

                if (entry == null)
                {
                    // Ensure the Customer navigation property is loaded
                    if (user.Role == null)
                    {
                        userWithRole = context.Users
                            .AsNoTracking()
                            .Include(u => u.Role)
                            .FirstOrDefault(u => u.Id == user.Id);
                    }
                    else
                    {
                        userWithRole = user;
                    }
                }
                else
                {
                    userWithRole = entry.Entity;
                }


                return new UserView
                {
                    Username = userWithRole.Username,
                    RoleName = userWithRole.Role.RoleName,  
                    TokenExpires = userWithRole.TokenExpires,  
                    Id = userWithRole.Id,
                    PasswordHash = userWithRole.PasswordHash,
                    PasswordSalt = userWithRole.PasswordSalt,
                    RefreshToken = userWithRole.RefreshToken,
                    RoleId = userWithRole.RoleId,
                    TokenCreated = userWithRole.TokenCreated     
                };
            }
        }

        public static IList<UserView> ConvertToUserWithRoleViews(this IEnumerable<User> users)
        {
            IList<UserView> userViews = new List<UserView>();
            foreach (User user in users)
            {
                userViews.Add(user.ConvertToUserViewWithRole());
            }

            return userViews;
        }

    }
}
