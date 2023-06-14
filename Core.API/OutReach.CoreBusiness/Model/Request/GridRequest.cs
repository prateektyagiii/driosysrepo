using System;
using System.Collections.Generic;
using System.Text;

namespace OutReach.CoreBusiness.Model.Request
{
    public class GridRequest<T>
    {
        public int? pagenumber { get; set; } = null;
        public int? pagesize { get; set; } = null;

        public T data { get; set; } 
    }
}
