using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BCS_SimpleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class Service1 : IService1
    {

        #region IService1 Members

        private List<Vehicle> _vehicles = new List<Vehicle>();
        private List<Vehicle> vehicles
        {
            get
            {
                if (_vehicles.Count == 0)
                {
                    //build new list of vehicles
                    _vehicles.Add(new Vehicle() { id = 1, year = 2012, make = "Ford", model = "Mustang", color = "Blue", mileage = 1000 });
                    _vehicles.Add(new Vehicle() { id = 2, year = 2010, make = "Ford", model = "Focus", color = "Red", mileage = 3000 });
                    _vehicles.Add(new Vehicle() { id = 3, year = 2000, make = "Chevrolet", model = "Camaro", color = "Yellow", mileage = 7000 });
                    _vehicles.Add(new Vehicle() { id = 4, year = 2003, make = "Chevrolet", model = "Silverado", color = "White", mileage = 12000 });
                    _vehicles.Add(new Vehicle() { id = 5, year = 2003, make = "Honda", model = "Accord", color = "White", mileage = 6000 });
                    _vehicles.Add(new Vehicle() { id = 6, year = 2003, make = "Toyota", model = "Camry", color = "Green", mileage = 51000 });
                }

                return _vehicles;
            }
        }
        

        public Vehicle GetVehicleByID(int vehicleID)
        {
            //return item by ID
            return (from v in vehicles where v.id == vehicleID select v).SingleOrDefault();
        }

        public List<Vehicle> GetAllVehicles()
        {
            //return full list of items
            return vehicles;
        }

        public string CreateVehicle(Vehicle newVehicle)
        {
            //find max id in list
            int maxID = (from v in vehicles orderby v.id descending select v.id).FirstOrDefault();

            //add new item
            vehicles.Add(new Vehicle()
            {
                id = maxID++,
                year = newVehicle.year,
                make = newVehicle.make,
                model = newVehicle.model,
                color = newVehicle.color,
                mileage = newVehicle.mileage
            });

            //return item ID
            return maxID.ToString();
        }

        public bool UpdateVehicle(int vehicleID, int year, string make, string model, string color, int mileage)
        {
            //find item by ID
            Vehicle oldVehicle = (from v in vehicles where v.id == vehicleID select v).SingleOrDefault();

            if (oldVehicle != null)
            {
                //remove current item
                vehicles.Remove(oldVehicle);

                //update values
                oldVehicle.year = year;
                oldVehicle.make = make;
                oldVehicle.model = model;
                oldVehicle.color = color;
                oldVehicle.mileage = mileage;

                //add updated item back into list
                vehicles.Add(oldVehicle);

                //return
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteVehicle(int vehicleID)
        {
            //find item by ID
            Vehicle vehicle = (from v in vehicles where v.id == vehicleID select v).SingleOrDefault();

            if (vehicle != null)
            {
                //remove found item
                vehicles.Remove(vehicle);

                //return
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
