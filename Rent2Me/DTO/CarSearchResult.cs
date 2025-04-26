using Rent2Me.Models;

namespace Rent2Me.DTO
{
    public class CarSearchResult
    {
        public CarDetails Car { get; set; }
        public int SimilarityScore { get; set; }
    }
}
