namespace CommonLib.Result
{
    public class ResultT<T> : IResultFailure<ResultT<T>>
    {
        public T Value { get; }
        public IReadOnlyList<string> ErrorMessages { get; }
        public string? SuccessMessage { get; }
        public bool IsSuccess => ErrorMessages.Count == 0;

        private ResultT( T value, IEnumerable<string>? errorMessages, string? successMessage )
        {
            Value = value;
            ErrorMessages = errorMessages is null ? new List<string>() : errorMessages.ToList();
            SuccessMessage = successMessage;
        }

        public static ResultT<T> Success( T value, string? successMessage = null )
        {
            return new ResultT<T>( value, null, successMessage );
        }

        public static ResultT<T?> Fail( IEnumerable<string> errorMessages )
        {
            return new ResultT<T?>( default, errorMessages, null );
        }

        public static ResultT<T?> Fail( string errorMessage )
        {
            return new ResultT<T?>( default, new List<string>() { errorMessage }, null );
        }

        public static ResultT<T> Failure( IEnumerable<string> errorMessages )
        {
            return new ResultT<T>( default!, errorMessages, null );
        }
    }
}