//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineShop.Core
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public decimal Balance { get; set; }
        public bool IsBlocked { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string ActivationCode { get; set; }
    
        public virtual Role Role { get; set; }
    }
}
