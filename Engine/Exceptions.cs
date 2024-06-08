namespace Engine.Exceptions {
    public class NotRegisteredException : Exception {
        public NotRegisteredException() { }
        public NotRegisteredException(string? message) : base(message) { }
        public NotRegisteredException(string? message, Exception? innerException) : base(message, innerException) { }
    }

    public class InvalidValueException : Exception {
        public InvalidValueException() { }
        public InvalidValueException(string? message) : base(message) { }
        public InvalidValueException(string? message, Exception? innerException) : base(message, innerException) { }
    }

    public class ValueNullException : InvalidValueException {
        public ValueNullException() { }
        public ValueNullException(string? message) : base(message) { }
        public ValueNullException(string? message, Exception? innerException) : base(message, innerException) { }
        public ValueNullException(string? paramName, string? message) : this(message ?? $"{paramName} is null") { }
        public ValueNullException(string? paramName, string? message, Exception? innerException) : base(message ?? $"{paramName} is null", innerException) { }
    }
}
