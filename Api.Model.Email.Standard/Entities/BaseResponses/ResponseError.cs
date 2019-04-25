namespace Api.Model.Email.Entities
{
    public class ResponseError
    {
        public string ErrorCode { get; set; }
        public string FieldName { get; set; }
        public string Message { get; set; }
    }
}
