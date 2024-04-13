using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatingApp.DAL.Entities;
using DatingApp.DAL.Specification.Infrastructure;

namespace DatingApp.DAL.Specification.UserSpecification
{
    public sealed class UserByUsernameSpecification : BaseSpecification<AppUser>
    {
        public UserByUsernameSpecification(string username) 
            : base(u => u.UserName == username.ToLower())
        {

        }
    }
}
