using PetStore.Queries;
using System;
using System.Collections.Generic;

namespace PetStore.Extensions
{
	public static class ListExtension
	{
		public static PagedList<T> ToPagedList<T>(this List<T> items, QueryParameters queryParameters, int totalItem)
		{
			var totalPages = totalItem / (double)queryParameters.PageSize;
			int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

			return new PagedList<T>(items, queryParameters.Page, queryParameters.PageSize, roundedTotalPages, totalItem);
		}
	}
}