using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RailRoadController.Entities;

namespace RailRoadController.BL.Locomotive
{
    public interface ILocomotivePersister
    {
        void SaveFleet(List<Locomotive> locomotives);

        List<Locomotive> LoadFleet();
    }

    public class LocomotivePersister : ILocomotivePersister
    {
        private readonly string _locomotiveFilePath;

        public LocomotivePersister(string locomotiveFilePath)
        {
            _locomotiveFilePath = locomotiveFilePath;
        }

        public void SaveFleet(List<Locomotive> locomotives)
        {
            var fleetConfiguration = ToConfig(locomotives);

            var fleetAsString = JsonConvert.SerializeObject(fleetConfiguration);

            File.WriteAllText(_locomotiveFilePath, fleetAsString);
        }

        public List<Locomotive> LoadFleet()
        {
            var fleetAsString = File.ReadAllText(_locomotiveFilePath);

            var fleetConfiguration = JsonConvert.DeserializeObject<List<LocomotiveConfiguration>>(fleetAsString);

            var output = FromConfig(fleetConfiguration);

            return output;
        }

        private List<LocomotiveConfiguration> ToConfig(IEnumerable<Locomotive> locomotives)
        {
            var output = new List<LocomotiveConfiguration>();
            foreach (var locomotive in locomotives)
            {
                output.Add(new LocomotiveConfiguration
                {
                    Address = locomotive.Address,
                    Name = locomotive.Name
                });
            }
            return output;
        }

        private List<Locomotive> FromConfig(IEnumerable<LocomotiveConfiguration> fleetConfiguration)
        {
            var output = new List<Locomotive>();
            foreach (var locoConf in fleetConfiguration)
            {
                output.Add(new Locomotive{Name = locoConf.Name, Address = locoConf.Address});
            }
            return output;
        }
    }
}
