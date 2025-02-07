using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392_Himari.Repository.Entities
{
    public class Payment
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        [MaxLength(50)]
        public string PaymentCode { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public float Amount { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } 
    }
}
