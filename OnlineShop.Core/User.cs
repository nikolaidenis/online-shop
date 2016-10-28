namespace OnlineShop.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(60)]
        public string UserName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(200)]
        public string Password { get; set; }

        [StringLength(50)]
        public string PasswordSalt { get; set; }

        [StringLength(50)]
        public string PasswordHash { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoleId { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "money")]
        public decimal Balance { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool IsBlocked { get; set; }

        public virtual Role Role { get; set; }
    }
}
