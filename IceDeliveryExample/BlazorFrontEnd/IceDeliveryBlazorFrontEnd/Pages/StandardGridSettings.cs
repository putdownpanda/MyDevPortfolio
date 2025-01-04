using Radzen;

namespace IceDeliveryBlazorFrontEnd.Pages;

public class StandardGridSettings
{
    public bool AllowColumnReorder { get; set; } = true;
    public bool AllowFiltering { get; set; } = true;
    public bool AllowColumnResize { get; set; } = true;
    public bool AllowSorting { get; set; } = true;
    public bool AllowPaging { get; set; } = true;
    public bool AllowAlternatingRows { get; set; } = true;
    public bool ShowPagingSummary { get; set; } = true;
    public int PageSize { get; set; } = 10;
    public bool AllowColumnPicking { get; set; } = true;
    public bool AllowRowSelectOnRowClick { get; set; } = true;
    public bool RowCLickEnabled { get; set; } = true;
    public int[] PageSizeOptions { get; set; } = new int[] { 10, 20, 30 };
    public HorizontalAlign PagerHorizontalAlign = HorizontalAlign.Left;
    public DataGridGridLines GridLines = DataGridGridLines.None;
    public LogicalFilterOperator LogicalFilterOperator = LogicalFilterOperator.Or;
    public DataGridSelectionMode SelectionMode { get; set; } = DataGridSelectionMode.Single;
    public FilterMode FilterMode = FilterMode.Advanced;
    public FilterCaseSensitivity FilterCaseSensitivity = FilterCaseSensitivity.CaseInsensitive;
    public string ColumnWidth = "100px";
}