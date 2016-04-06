using System;

namespace TestHabanero.Models
{
    public class PartViewModel:IPartViewModel
    {
        public Guid PartId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }

    public interface IPartViewModel 
    {
        Guid PartId { get; set; }
        string Name { get; set; }
        string Description   { get; set; }
        int Price   { get; set; }
    }
}