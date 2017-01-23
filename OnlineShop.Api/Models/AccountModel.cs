using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Api.Models
{
    public class AccountModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Blocked { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}