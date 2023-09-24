namespace DTOModels
{
    public class OwnerDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public byte[]? Photo { get; set; }

        public DateTime Birthday { get; set; }
    }
}
