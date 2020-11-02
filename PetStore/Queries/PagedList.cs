using System.Collections.Generic;

namespace PetStore.Queries
{
	public class PagedList<T>
	{
		public List<T> Items { get; set; }
		public PagedListMetaData MetaData { get; set; }

		public PagedList(List<T> items, int pageNumber, int pageSize, int totalPages, int totalItems)
		{
			Items = items;
			MetaData = new PagedListMetaData(pageNumber, pageSize, totalPages, totalItems);
		}
	}
}