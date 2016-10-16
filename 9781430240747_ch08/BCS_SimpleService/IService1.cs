using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BCS_SimpleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        Vehicle GetVehicleByID(int vehicleID);

        [OperationContract]
        List<Vehicle> GetAllVehicles();

        [OperationContract]
        string CreateVehicle(Vehicle newVehicle);

        [OperationContract]
        bool UpdateVehicle(int vehicleID, int year, string make, string model, string color, int mileage);

        [OperationContract]
        bool DeleteVehicle(int vehicleID);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class Vehicle
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int year { get; set; }

        [DataMember]
        public string make { get; set; }

        [DataMember]
        public string model { get; set; }

        [DataMember]
        public string color { get; set; }

        [DataMember]
        public int mileage { get; set; }
    }
}
