namespace CustomerDemo.Model
{
    public class Customer : IEntity
    {
        public Guid Id { get; set; }
        public string? Firstname { get; set; }
        public string? Surename { get; set; }
    }
}
