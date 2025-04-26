using Rent2Me.Models;

namespace Rent2Me.DTO
{
    public class CarSearchParameters
    {
        public string CarType { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Year { get; set; }
        public string Model { get; set; }
        public string SeatingCapacity { get; set; }

        public int CalculateScore(CarDetails car, CarSearchParameters searchParameters)
        {
            int score = 0;

            if (searchParameters.CarType != null && car.CarType == searchParameters.CarType)
            {
                score += 1;
            }

            if (searchParameters.Brand != null && car.Brand == searchParameters.Brand)
            {
                score += 1;
            }

            if (searchParameters.Color != null && car.Color == searchParameters.Color)
            {
                score += 1;
            }

            if (searchParameters.Model != null && car.Model == searchParameters.Model)
            {
                score += 1;
            }

            if (searchParameters.Year!=null && car.Year == searchParameters.Year)
            {
                score += 1;
            }

            if (searchParameters.SeatingCapacity!=null && car.SeatingCapacity == searchParameters.SeatingCapacity)
            {
                score += 1;
            }
            return score;
        }
    }
}
