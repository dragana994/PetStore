namespace PetStore.Queries
{
	public class PagedListMetaData
	{
		public int PageNumber { get; internal set; }
		public int PageSize { get; internal set; }

		public int? TotalPages { get; internal set; }
		public int? TotalItems { get; internal set; }

		public PagedListMetaData(int pageNumber, int pageSize, int? totalPages, int? totalItems)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
			TotalPages = totalPages;
			TotalItems = totalItems;
		}
	}
}