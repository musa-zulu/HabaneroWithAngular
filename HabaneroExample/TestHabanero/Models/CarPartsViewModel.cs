using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestHabanero.Models
{
    public class CarPartsViewModel
    {
        public Guid CarPartId { get; set; }
        public Guid CarId { get; set; }
        public Guid PartId { get; set; }
        public int Quantity { get; set; }
        public List<SelectListItem> CarSelectListItems { get; set; }
        public List<SelectListItem> PartsSelectListItems { get; set; }

    }
}