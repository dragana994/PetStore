using PetStore.Exceptions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Extensions
{
	public static class ValidationResultListExtension
	{
		public static List<ErrorMessage> ToErrorMessageList(this List<ValidationResult> validationResults)
		{
			List<ErrorMessage> errorList = new List<ErrorMessage>();

			foreach (var vr in validationResults)
			{
				errorList.Add(new ErrorMessage(vr.ErrorMessage));
			}

			return errorList;
		}
	}
}