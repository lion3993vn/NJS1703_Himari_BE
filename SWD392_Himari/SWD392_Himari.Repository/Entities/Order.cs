using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_Himari.Repository.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        [MaxLength(50)]
        public string OrderCode { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public float OrderPrice { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        [Required]
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; } 
    }
}
