namespace Api.Model.Email.Entities
{
    public class Attachment
    {
        public string ContentType { get; set; }

        public string Name { get; set; }

        public byte[] Content { get; set; }
    }
}
