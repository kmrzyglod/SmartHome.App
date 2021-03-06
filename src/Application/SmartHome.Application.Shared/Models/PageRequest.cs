﻿using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Shared.Models
{
    public class PageRequest: IPageRequest
    {
        public int PageNumber { get => _pageNumber ?? 1;
            set => _pageNumber = value;
        }

        public int PageSize { get => _pageSize ?? 10;
            set => _pageSize = value;
        }
        
        private int? _pageSize;
        private int? _pageNumber;
    }
}
