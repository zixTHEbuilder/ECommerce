namespace ECommerce.Dtos
{
    public class PagedResult<T>
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public List<T> Data { get; set; } = new List<T>();
        public int TotalProducts { get; set; }
    }
}
