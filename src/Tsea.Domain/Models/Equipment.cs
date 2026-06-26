using System;

namespace Tsea.Domain.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Status { get; set; } = "Operante";
        public DateTime InstallationDate { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
    }
}
