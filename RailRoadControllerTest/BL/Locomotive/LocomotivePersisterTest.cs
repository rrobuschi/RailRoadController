using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RailRoadController.BL.Locomotive;

namespace RailRoadControllerTest.BL.Locomotive
{
    public class LocomotivePersisterTest
    {
        [Test]
        public void Locomotive_informations_are_correctly_saved_to_disk_and_retrieved()
        {
            var sut = new LocomotivePersister("TempLocoData.txt");

            var input = new List<RailRoadController.BL.Locomotive.Locomotive>
            {
                new RailRoadController.BL.Locomotive.Locomotive {Name = "Locomotive one", Address = "02"},
                new RailRoadController.BL.Locomotive.Locomotive {Name = "locomotive two", Address = "01"}
            };

            sut.SaveFleet(input);

            var output = sut.LoadFleet();

            output.Count.Should().Be(2);
            output.Single(x => x.Address == "01").Name.Should().Be("locomotive two");
            output.Single(x => x.Address == "02").Name.Should().Be("Locomotive one");

            File.Delete("TempLocoData.txt");
        }
    }
}
