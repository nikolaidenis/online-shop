using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Api.Models
{
    public class UserLockModel
    {
        public string Username { get; set; }
        public bool IsBlocking { get; set; }
    }
}