using Microsoft.EntityFrameworkCore;
using Tsea.Api.Data;
using Tsea.Domain.Models;

namespace Tsea.Api.Tests.Unit;

public class AppDbContextModelTests
{
    [Fact]
    public void Equipment_configuration_requires_name_and_serial_number_with_expected_lengths()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);
        var entityType = context.Model.FindEntityType(typeof(Equipment));

        Assert.NotNull(entityType);

        var name = entityType.FindProperty(nameof(Equipment.Name));
        var serialNumber = entityType.FindProperty(nameof(Equipment.SerialNumber));

        Assert.NotNull(name);
        Assert.False(name.IsNullable);
        Assert.Equal(150, name.GetMaxLength());

        Assert.NotNull(serialNumber);
        Assert.False(serialNumber.IsNullable);
        Assert.Equal(50, serialNumber.GetMaxLength());
    }

    [Fact]
    public void New_equipment_starts_with_operante_status()
    {
        var equipment = new Equipment();

        Assert.Equal("Operante", equipment.Status);
    }
}
