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
            Assert.Equal(machine.CurrentState, TestingOnOffMachine.State.On);

            if (machine.BoundFellow.SupportMachine)

                Assert.True(string.IsNullOrEmpty(machine.BoundFellow.StateData));

            if (machine.BoundFellow.SupportChangedOn)

                Assert.True(machine.BoundFellow.ChangedOn.Year == DateTime.Now.Year);

            //act
            machine.Execute(TestingOnOffMachine.Command.SwitchOff);

            //assert
            Assert.Equal(machine.CurrentState, TestingOnOffMachine.State.Off);
        }
    }
}
