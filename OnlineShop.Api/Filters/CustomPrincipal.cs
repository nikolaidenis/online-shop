using System;
using System.Security.Principal;
using OnlineShop.Core;

namespace OnlineShop.Api.Filters
{
    public class CustomPrincipal:IPrincipal
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public bool IsBlocked { get; set; }
        public string Role { get; set; }

        public bool IsInRole(string role)
        {
            return role == Role && !string.IsNullOrEmpty(role);
        }

        public IIdentity Identity { get; }
    }
}