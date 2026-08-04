using System.Net;
using System.Net.Http.Json;
using Tsea.Domain.Models;

namespace Tsea.Api.Tests.Integration;

public class EquipmentEndpointsTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;
    private readonly HttpClient _client;

    public EquipmentEndpointsTests(ApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_all_returns_empty_list_when_no_equipment_exists()
    {
        await _factory.ResetDatabaseAsync();

        var response = await _client.GetAsync("/api/equipments");
        var equipment = await response.Content.ReadFromJsonAsync<Equipment[]>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(equipment);
        Assert.Empty(equipment);
    }

    [Fact]
    public async Task Post_creates_equipment_that_can_be_fetched_by_id()
    {
        await _factory.ResetDatabaseAsync();

        var response = await _client.PostAsJsonAsync("/api/equipments", CreateEquipment());
        var created = await response.Content.ReadFromJsonAsync<Equipment>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal($"/api/equipments/{created.Id}", response.Headers.Location?.OriginalString);

        var getResponse = await _client.GetAsync($"/api/equipments/{created.Id}");
        var fetched = await getResponse.Content.ReadFromJsonAsync<Equipment>();

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.NotNull(fetched);
        Assert.Equal(created.SerialNumber, fetched.SerialNumber);
    }

    [Fact]
    public async Task Put_updates_existing_equipment()
    {
        await _factory.ResetDatabaseAsync();
        var created = await CreateAndReadAsync();
        created.Name = "Regulador Atualizado";
        created.Status = "Em manutenção";

        var response = await _client.PutAsJsonAsync($"/api/equipments/{created.Id}", created);
        var getResponse = await _client.GetAsync($"/api/equipments/{created.Id}");
        var updated = await getResponse.Content.ReadFromJsonAsync<Equipment>();

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.NotNull(updated);
        Assert.Equal("Regulador Atualizado", updated.Name);
        Assert.Equal("Em manutenção", updated.Status);
    }

    [Fact]
    public async Task Delete_removes_equipment_and_subsequent_get_returns_not_found()
    {
        await _factory.ResetDatabaseAsync();
        var created = await CreateAndReadAsync();

        var deleteResponse = await _client.DeleteAsync($"/api/equipments/{created.Id}");
        var getResponse = await _client.GetAsync($"/api/equipments/{created.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    private async Task<Equipment> CreateAndReadAsync()
    {
        var response = await _client.PostAsJsonAsync("/api/equipments", CreateEquipment());
        var equipment = await response.Content.ReadFromJsonAsync<Equipment>();

        Assert.NotNull(equipment);
        return equipment;
    }

    private static Equipment CreateEquipment() => new()
    {
        Name = "Regulador de Tensão",
        Type = "Monofásico",
        SerialNumber = Guid.NewGuid().ToString("N"),
        Status = "Operante",
        InstallationDate = new DateTime(2025, 1, 15),
        LastMaintenanceDate = new DateTime(2025, 6, 1)
    };
}
