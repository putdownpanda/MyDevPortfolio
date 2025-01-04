namespace IceDeliveryBlazorFrontEnd.Services;
using Radzen;
using Microsoft.AspNetCore.Components;

public partial class ExportService
{
    private readonly NavigationManager navigationManager;

    public ExportService(NavigationManager navigationManager)
    {
        this.navigationManager = navigationManager;
    }

    public void Export(string table, string type, Query query = null)
    {
        navigationManager.NavigateTo(query != null ? query.ToUrl($"/export/{table}/{type}") : $"/export/{table}/{type}", true);
    }
}