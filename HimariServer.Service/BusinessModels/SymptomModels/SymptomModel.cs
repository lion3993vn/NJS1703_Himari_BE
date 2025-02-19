using HimariServer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.SymptomModels
{
    public class SymptomModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? BodyPartId { get; set; }
    }
}
