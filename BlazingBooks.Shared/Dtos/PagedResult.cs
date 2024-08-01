namespace BlazingBooks.Shared.Dtos
{
    public record PagedResult<TRecords>(TRecords[] Records, int TotalCount);

}
