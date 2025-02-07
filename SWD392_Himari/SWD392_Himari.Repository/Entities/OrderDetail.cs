using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392_Himari.Repository.Entities
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; } 

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } 

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } 
    }
}
