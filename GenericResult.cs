using System.Text.Json.Serialization;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace RIDataHub.Loader.Business.Contracts;

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
        : this(exception.Message)
    {
        ExceptionObject = exception;
    }

    public GenericResult()
    {
        Succeeded = true;
    }

    [JsonProperty("errorMessage", NullValueHandling = NullValueHandling.Ignore)]
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Exception? ExceptionObject { get; set; }

    [JsonProperty("succeeded")]
    [JsonPropertyName("succeeded")]
    public bool Succeeded { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public bool HasError => !Succeeded;

    [Newtonsoft.Json.JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public static GenericResult Success => new();

    public static GenericResult WithError(Exception exception)
    {
        return new GenericResult(exception);
    }

    public static GenericResult WithError(string message)
    {
        return new GenericResult(message);
    }

    public static GenericResult<T> WithError<T>(string message) => new(message);

    public static GenericResult<T> WithSuccess<T>(T value) => new(value);

    public static GenericResult<T> WithValidation<T>(ValidationResult validation, T value)
    {
        var result = new GenericResult<T>
        {
            Succeeded = validation.IsValid,
            ErrorMessage = validation.ToString(),
            Value = value
        };

        return result;
    }

    public static GenericResult<T> WithValidation<T>(ValidationResult validation)
    {
        var result = new GenericResult<T>();
        result.SetValidation(validation);
        return result;
    }

    public static GenericResult WithValidation(ValidationResult validation)
    {
        var result = new GenericResult
        {
            Succeeded = validation.IsValid,
            ErrorMessage = validation.ToString(),
        };

        return result;
    }

    public void SetValidation(ValidationResult validation)
    {
        Succeeded = validation.IsValid;
        ErrorMessage = validation.ToString();
    }

    public void SetValidation(IEnumerable<ValidationResult> validations)
    {
        var validationResults = validations as ValidationResult[] ?? validations.ToArray();
        Succeeded = validationResults.All(v => v.IsValid);
        ErrorMessage = string.Join(Environment.NewLine, validationResults.Select(vr => vr.ToString()));
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
    [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore, Required = Required.Default)]
    public T Value { get; set; } = default!;

    public new static GenericResult<T> WithError(string message)
    {
        return new GenericResult<T>
        {
            Succeeded = false,
            ErrorMessage = message
        };
    }

    public static GenericResult<T> WithError(GenericResult genericResult)
    {
        var result = new GenericResult<T>()
        {
            Succeeded = false,
            ErrorMessage = genericResult.ErrorMessage,
            ExceptionObject = genericResult.ExceptionObject
        };

        return result;
    }
}
