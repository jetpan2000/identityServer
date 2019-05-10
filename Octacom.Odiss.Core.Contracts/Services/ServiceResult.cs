namespace Octacom.Odiss.Core.Contracts.Services
{
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class ServiceResult<TPayloadType> : ServiceResult
    {
        public TPayloadType Payload { get; set; }
    }
}
