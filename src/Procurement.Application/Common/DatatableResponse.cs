namespace Procurement.Application.Common;

public class DatatableResponse<T>
{
    public int Draw { get; set; }
    public int RecordsTotal { get; set; }
    public int RecordsFiltered { get; set; }
    public IList<T> Data { get; set; } = new List<T>();
}
