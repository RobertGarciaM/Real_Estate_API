namespace DTOModels
{
    public class PropertyImageDto
    {
        public Guid Id { get; set; }
        public byte[]? File { get; set; }
        public bool Enabled { get; set; }
        public Guid IdProperty { get; set; }
    }
}
