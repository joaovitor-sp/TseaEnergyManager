using Microsoft.EntityFrameworkCore;
using Tsea.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
     db.Database.Migrate();
}

app.MapGet("/api/equipments", async (AppDbContext db) =>
    await db.Equipments.ToListAsync());

app.MapGet("/api/equipments/{id}", async (int id, AppDbContext db) =>
    await db.Equipments.FindAsync(id)
        is Tsea.Domain.Models.Equipment equipment
            ? Results.Ok(equipment)
            : Results.NotFound());

app.MapPost("/api/equipments", async (Tsea.Domain.Models.Equipment equipment, AppDbContext db) =>
{
    db.Equipments.Add(equipment);
    await db.SaveChangesAsync();
    return Results.Created($"/api/equipments/{equipment.Id}", equipment);
});

app.MapPut("/api/equipments/{id}", async (int id, Tsea.Domain.Models.Equipment inputEquipment, AppDbContext db) =>
{
    var equipment = await db.Equipments.FindAsync(id);
    if (equipment is null) return Results.NotFound();

    equipment.Name = inputEquipment.Name;
    equipment.Type = inputEquipment.Type;
    equipment.SerialNumber = inputEquipment.SerialNumber;
    equipment.Status = inputEquipment.Status;
    equipment.InstallationDate = inputEquipment.InstallationDate;
    equipment.LastMaintenanceDate = inputEquipment.LastMaintenanceDate;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/equipments/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Equipments.FindAsync(id) is Tsea.Domain.Models.Equipment equipment)
    {
        db.Equipments.Remove(equipment);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.Run();

public partial class Program
{
}
