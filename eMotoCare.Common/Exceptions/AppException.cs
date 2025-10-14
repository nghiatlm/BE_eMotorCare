

namespace eMotoCare.Common.Exceptions
{
    public class AppException : Exception
    {
        public ErrorCode ErrorCode { get; }

        public AppException(ErrorCode errorCode)
            : base(errorCode.ToString())
        {
            ErrorCode = errorCode;
        }

        public AppException(ErrorCode errorCode, string customMessage)
            : base(customMessage)
        {
            ErrorCode = errorCode;
        }
    }
}
