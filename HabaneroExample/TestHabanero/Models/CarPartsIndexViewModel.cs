using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using TestHabanero.BO;

namespace TestHabanero.Models
{
    public class CarPartsIndexViewModel:ICarPartsIndexViewModel
    {
      
        public Guid CarPartId { get; set; }
        [DisplayName("Quantity")]

        public int Quantity { get; set; }
        [DisplayName("Car Make")]

        public string CarMake { get; set; }
        [DisplayName("Car Model")]

        public string CarModel { get; set; }
        [DisplayName("Part Name")]

        public string PartName { get; set; }
        [DisplayName("Part Price")]
        public string PartPrice { get; set; }

        public Guid PartId { get; set; }
        public Guid CarId { get; set; }
      
    }

    public interface ICarPartsIndexViewModel
    {
        Guid CarPartId { get; set; }
        int Quantity { get; set; }
        string CarMake { get; set; }
        string CarModel { get; set; }
        string PartName { get; set; }
        string PartPrice { get; set; }
        Guid PartId { get; set; }
        Guid CarId { get; set; }
    }
}