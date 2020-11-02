using System;
using System.Collections.Generic;

namespace PetStore.Exceptions
{
	public class ValidatorException : Exception
	{
		public List<ErrorMessage> Errors { get; set; }

		public ValidatorException(string message) : this(new List<ErrorMessage> { new ErrorMessage(message) })
		{
		}

		public ValidatorException(ErrorMessage error) : this(new List<ErrorMessage> { error })
		{ }

		public ValidatorException(List<ErrorMessage> errors)
		{
			Errors = errors;
		}
	}
}