namespace Client.Entities
{
    public class ValidationException
    {
        public string Message { get; set; }
        public object Value { get; set; }
    }
}