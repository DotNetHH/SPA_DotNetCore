using System.Collections.Generic;

namespace WebAPI.RequestPipeline
{
    [System.Serializable]
    public class ValidationException : System.Exception
    {
        public IEnumerable<Error> Errors { get; }
        public ValidationException(IEnumerable<Error> errors)
        {
            this.Errors = errors;
        }

        public ValidationException() { }
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, System.Exception inner) : base(message, inner) { }
        protected ValidationException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}