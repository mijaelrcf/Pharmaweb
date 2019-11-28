using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharmaweb.Models
{
    public class CustomerDataModels<T>
    {
        public List<T> Customer { get; set; }
        public int PageSize { get; set; }
        public int TotalRecord { get; set; }
        public int NoOfPages { get; set; }
    }
}