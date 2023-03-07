using OrderRecordSystemAPI.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderRecordSystemAPI.Models
{
    public class ServiceOrderItem : IRequiredProperties
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [ForeignKey("Connection")]
        public string ServiceOrderId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public DateTimeOffset EntryDate { get; set; }
        public DateTimeOffset? OutDate { get; set; }
        public string NameOfGoods { get; set; } = string.Empty;
        public bool Accepted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
    }
}
