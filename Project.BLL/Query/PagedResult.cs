namespace Project.BLL.Query;

public record PagedResult<T>(IEnumerable<T> Items, int TotalCount, int Page, int PageSize);