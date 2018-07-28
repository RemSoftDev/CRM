using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Attributes
{
    public class GridAttribute : Attribute
    {
        public bool ShowOnGrid { get; set; }
    }
}