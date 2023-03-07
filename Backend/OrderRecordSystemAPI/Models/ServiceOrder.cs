using OrderRecordSystemAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OrderRecordSystemAPI.Models
{
    public class ServiceOrder : IRequiredProperties
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public DateTimeOffset EntryDate { get; set; }
        public DateTimeOffset? OutDate { get; set; }
        public string NameOfGoods { get; set; } = string.Empty;
        public bool Accepted { get; set; }
        public List<ServiceOrderItem> ListServiceOrderItem { get; set; } = new List<ServiceOrderItem>();
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
    }
}
