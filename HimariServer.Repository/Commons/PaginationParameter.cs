using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HimariServer.Repository.Commons
{
    public class PaginationParameter
    {
        const int maxPageSize = 50;

        [FromQuery(Name = "page-index")]
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 10;
        [JsonIgnore]

        [FromQuery(Name = "page-size")]
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
