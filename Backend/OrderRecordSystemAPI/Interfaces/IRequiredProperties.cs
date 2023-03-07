namespace OrderRecordSystemAPI.Interfaces
{
    public interface IRequiredProperties
    {
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
        string ModifiedBy { get; set; }
    }
}
