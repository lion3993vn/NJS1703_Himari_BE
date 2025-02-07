using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392_Himari.Repository.Entities
{
    public class ConsultingResult
    {
        [Key]
        public int Id { get; set; } 

        public int CustomerConsultingId { get; set; } 
        public int ProductId { get; set; } 

        [ForeignKey("CustomerConsultingId")]
        public virtual CustomerConsulting CustomerConsulting { get; set; } 

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; } 
    }
}
