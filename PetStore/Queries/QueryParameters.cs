namespace PetStore.Queries
{
	public class QueryParameters
	{
		public bool Paging { get; set; } = true;
		public int Page { get; set; } = 1;
		public int PageSize { get; set; } = 10;

		public string Search { get; set; }
	}
}