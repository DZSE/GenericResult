using System;
using Newtonsoft.Json;

namespace Dzse.Common
{
	public class GenericResult
    {
        public GenericResult(string errorMessage)
        {
            Succeeded = errorMessage == null;
            ErrorMessage = errorMessage;
            ExceptionObject = null;
        }

        public GenericResult(Exception exception)
            : this(exception.Message)
        {
            ExceptionObject = exception;
        }

        public GenericResult()
        {
            Succeeded = true;
        }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonIgnore]
        public Exception ExceptionObject { get; set; }

        [JsonProperty("succeeded")]
        public bool Succeeded { get; private set; }

        [JsonIgnore]
        public bool HasError => !Succeeded;

        [JsonIgnore]
        public static GenericResult Success => new GenericResult();
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

        public GenericResult(T value, string errorMessage = null)
            : base(errorMessage)
        {

            if (value != null && !string.IsNullOrEmpty(errorMessage))
            {
                throw new InvalidOperationException(
                    "When the error message is provided, value must be null.");
            }

            Value = value;
        }

        [JsonProperty("value")]
        public T Value { get; set; }
    }
}
