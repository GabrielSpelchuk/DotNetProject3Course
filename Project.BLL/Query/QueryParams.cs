namespace Project.BLL.Query;

public class QueryParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
    public string? Search { get; set; }
    
    public string? SortBy { get; set; } 
    public bool Descending { get; set; } = false;

    public int Skip => (PageNumber - 1) * PageSize;
}