using System;
using System.ComponentModel.DataAnnotations;

namespace SWD392_Himari.Repository.Entities
{
    public class BodyPart
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        [MaxLength(100)]
        public string BodyPartName { get; set; }

        public string Description { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime? UpdateDate { get; set; }
    }
}
