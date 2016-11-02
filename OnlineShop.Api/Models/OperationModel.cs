using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Api.Models
{
    public class OperationModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public decimal Amount { get; set; }
        public bool IsSelled { get; set; }
    }
}