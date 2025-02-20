﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fish_Manage.Models
{
    public partial class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        public int? Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public virtual Order? Order { get; set; }

        public virtual Product? Product { get; set; }
    }
}
