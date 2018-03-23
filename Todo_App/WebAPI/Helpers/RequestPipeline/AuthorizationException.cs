namespace WebAPI.RequestPipeline
{
    [System.Serializable]
    public class AuthorizationException : System.Exception
    {
        public AuthorizationException() { }
        public AuthorizationException(string message) : base(message) { }
        public AuthorizationException(string message, System.Exception inner) : base(message, inner) { }
        protected AuthorizationException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}