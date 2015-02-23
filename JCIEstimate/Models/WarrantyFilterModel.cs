using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JCIEstimate.Models
{
    public class WarrantyFilterModel
    {
        public string filterState;
    }

    public class FilterOptionModel
    {
        public string text { get; set; }
        public string value { get; set; }
        public bool selected { get; set; }
    }
}