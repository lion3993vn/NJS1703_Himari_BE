using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392_Himari.Repository.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

        public int Quantity { get; set; }

        public string ImageUrl { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime? UpdateDate { get; set; }

        public int CategoryId { get; set; }

        public int BrandId { get; set; }

        public bool Gender { get; set; } 

        public int PartSymptonId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        [ForeignKey("PartSymptonId")]
        public virtual PartSympton PartSympton { get; set; }
    }
}
