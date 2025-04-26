using System.ComponentModel.DataAnnotations;

namespace Rent2Me.Models
{
    public class Contract
    {
        [Key]
        public int Id { get; set; }
        public string ContractPath { get; set; }
    }
}
