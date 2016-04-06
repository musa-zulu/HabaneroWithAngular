using System;

namespace TestHabanero.Models
{
    public class CarViewModel: ICarViewModel
    {
        public Guid CarId { get; set; }
        public string Make{get; set;}
        public string Model { get; set; }
     
    }

    public interface ICarViewModel
    {
        Guid CarId { get; set; }
        string Make { get; set; }
        string Model { get; set; }
      
    }
}