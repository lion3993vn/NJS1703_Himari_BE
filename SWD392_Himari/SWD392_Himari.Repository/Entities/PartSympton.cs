using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392_Himari.Repository.Entities
{
    public class PartSympton
    {
        [Key]
        public int Id { get; set; }

        public int BodyPartId { get; set; } 

        [ForeignKey("BodyPartId")]
        public virtual BodyPart BodyPart { get; set; } 
    }
}
