namespace PetStore.Exceptions
{
	public class ErrorMessage
	{
		public int? StatusCode { get; set; }
		public string Message { get; set; }

		public ErrorMessage(string message) : this(null, message)
		{ }

		public ErrorMessage(int? statusCode, string message)
		{
			StatusCode = statusCode;
			Message = message;
		}

		public override string ToString()
		{
			return $"Status Code: {StatusCode} Message: {Message}";
		}
	}
}