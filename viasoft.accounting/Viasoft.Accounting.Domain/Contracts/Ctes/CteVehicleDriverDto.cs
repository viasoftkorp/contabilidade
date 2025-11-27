using System;

namespace Viasoft.Accounting.Domain.Contracts.Ctes
{
    public class CteVehicleDriverDto
    {
        public Guid? VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string PrimaryTow{ get; set; }
        public string SecondaryTow{ get; set; }
        public Guid? DriverId { get; set; }
        public string DriverName{ get; set; }
    }
}