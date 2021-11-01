namespace JimmyCms.Domain.Messaging.Articles
{
    public class BasicResponse
    {
        public bool Success { get; set; } = true;
        public int ResponseCode { get; set; }
        public object Value { get; set; }
    }
}