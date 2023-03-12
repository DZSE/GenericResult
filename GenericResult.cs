// ReSharper disable MemberCanBeProtected.Global
#pragma warning disable RECS0017
#pragma warning disable 0108
using System;
using System.Text.Json.Serialization;

namespace DZSE.Common
{
    public class GenericResult
    {
        public GenericResult(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentNullException(nameof(errorMessage));

            Succeeded = false;
            ErrorMessage = errorMessage;
            ExceptionObject = null;
        }

        public GenericResult(Exception exception)
            : this(exception.ToFullErrorMessage())
        {
            ExceptionObject = exception;
        }

        public GenericResult()
        {
            Succeeded = true;
        }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; private set; }

        [JsonIgnore]
        public Exception ExceptionObject { get; private set; }

        [JsonPropertyName("succeeded")]
        public bool Succeeded { get; private set; }

        [JsonIgnore]
        public bool HasError => !Succeeded;

        [JsonIgnore]
        public static GenericResult Success => new GenericResult();

        public static GenericResult Error(Exception exception)
        {
            return new GenericResult(exception);
        }

        public static GenericResult Error(string message)
        {
            return new GenericResult(message);
        }
    }

    public class GenericResult<T> : GenericResult
    {
        public GenericResult()
        {
        }

        public GenericResult(string errorMessage)
            : base(errorMessage)
        {
        }

        public GenericResult(Exception exception)
            : base(exception)
        {
        }

        public GenericResult(T value)
        {
            Value = value;
        }

        [JsonPropertyName("value")]
        public T Value { get; private set; }

        public new static GenericResult<T> Error(string message)
        {
            return new GenericResult<T>(message);
        }

        public static GenericResult<T> Success(T value)
        {
            return new GenericResult<T>(value);
        }
    }
}
