using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Api.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public decimal Balance { get; set; }
    }
}