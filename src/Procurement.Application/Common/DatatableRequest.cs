namespace Procurement.Application.Common;

public class DatatableRequest
{
    public int Draw { get; set; }
    public int Start { get; set; }
    public int Length { get; set; }
    public DatatableSearch? Search { get; set; }
    public IList<DatatableOrder> Order { get; set; } = new List<DatatableOrder>();
    public IList<DatatableColumn> Columns { get; set; } = new List<DatatableColumn>();
}

public class DatatableSearch
{
    public string? Value { get; set; }
    public bool Regex { get; set; }
}

public class DatatableOrder
{
    public int Column { get; set; }
    public string Dir { get; set; } = "asc";
}

public class DatatableColumn
{
    public string? Data { get; set; }
    public string? Name { get; set; }
    public bool Searchable { get; set; }
    public bool Orderable { get; set; }
}
