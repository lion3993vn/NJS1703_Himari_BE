using System;
using System.ComponentModel.DataAnnotations;

namespace SWD392_Himari.Repository.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public int? ParentCategoryId { get; set; } 

        [MaxLength(50)]
        public string Status { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime? UpdateDate { get; set; }

        public virtual Category ParentCategory { get; set; }
    }
}
