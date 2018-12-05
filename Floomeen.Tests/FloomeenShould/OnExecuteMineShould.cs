using System;
using Floomeen.Meen;
using Xunit;

namespace Floomeen.Tests.FloomeenShould
{

    public class OnExecuteMineShould
    {

        [Fact]
        public void ReturnTrueGivenGoodCommand()
        {
            //arrange
            var machine = Factory<TestingOnOffMachine>.Create();

            var fellow = new POCO{ Id = "NotEmpty" };

            machine.Plug(fellow); // impose OnState

            //assert
            Assert.Equal(machine.GetState(), TestingOnOffMachine.State.On);

            Assert.Equal(0, string.Compare(
                            machine.GetMachine(), 
                            "Floomine.Tests.FloomineShould.TestingOnOffMachine", 
                            StringComparison.InvariantCultureIgnoreCase)                
            );

            Assert.True(string.IsNullOrEmpty(machine.GetStateData()));

            Assert.True(machine.GetChangeOn().Year == DateTime.Now.Year);

            //act
            machine.Execute(TestingOnOffMachine.Command.SwitchOff);

            //assert
            Assert.Equal(machine.GetState(), TestingOnOffMachine.State.Off);
        }
    }
}
