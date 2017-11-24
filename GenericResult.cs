using System;

namespace OpenSource
{
	public class GenericResult
	{
		public GenericResult(string errorMessage)
		{
			Succeeded = errorMessage == null;
			ErrorMessage = errorMessage;
		}

		public GenericResult()
		{
			Succeeded = true;
		}

		public string ErrorMessage { get; }

		public bool Succeeded { get; }

		public bool HasError => !Succeeded;
	}

	public class GenericResult<T> : GenericResult
	{
		public GenericResult(string errorMessage)
			: base(errorMessage)
		{
		}

		public GenericResult(T value, string errorMessage)
			: base(errorMessage)
		{
			if (value != null && !string.IsNullOrEmpty(errorMessage))
				throw new InvalidOperationException("When the error message is provided, value must be null.");

			Value = value;
		}

		public GenericResult(T value)
			: this(value, null)
		{
		}

		public T Value { get; set; }
	}
}
