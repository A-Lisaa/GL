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

    public class ValueIsNullException : InvalidValueException {
        public ValueIsNullException() { }
        public ValueIsNullException(string? message) : base(message) { }
        public ValueIsNullException(string? message, Exception? innerException) : base(message, innerException) { }
        public ValueIsNullException(string? paramName, string? message) : this(message ?? $"{paramName} is null") { }
        public ValueIsNullException(string? paramName, string? message, Exception? innerException) : base(message ?? $"{paramName} is null", innerException) { }
    }
}
