using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Api.Models
{
    public class PaymentModel
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}