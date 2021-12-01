using SalesApplication.Abstractions;

namespace SalesApplication.Data.Responses
{
    public class ActionResponse : IActionResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
    }
}
