using System.Collections.Generic;

namespace SmartHome.Application.Models
{
    public class PaginationResult<T>
    {
        public int ResultTotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Result { get; set; } = new List<T>();
    }
}
