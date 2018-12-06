using Floomeen.Flow;
using Floomeen.Meen;
using Xunit;

namespace Floomeen.Tests.ValidatorShould
{

    public class OnValidSettings
    {
        static Result FakeFunk(ref Context context)
        {
            return null;
        }

        static bool FakeCheck(Result result)
        {
            return result.Success;
        }
        
        static void FakeEnterExitCallback(Context context)
        {
        }

        [Fact]
        public void BeTrueForStartStateDeclaration()
        {
            //arrange
            var wf = new Floo();

            wf.AddStateSetting("StartState")
                .IsStartState();

            wf.AddTransition()
                .From("StartState")
                .On("Command")
                    .GoTo("EndState");

            //act
            bool isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void BeTrueWhenMissingStartStateOnDeclaration()
        {
            //arrange
            var wf = new Floo();

            wf.AddStateSetting("StartState")
                .IsStartState()
                .OnEnterEvent(FakeEnterExitCallback);

            wf.AddStateSetting("EndState")
                .IsEndState();

            wf.AddTransition()
                .From("StartState")
                .On("Command")
                    .GoTo("EndState");

            //act
            bool isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void BeTrueWhenMissingStartStateOnDeclaratioWithEventOnEnterEvent()
        {
            //arrange
            var wf = new Floo();

            wf.AddStateSetting("StartState")
                .IsStartState()
                .OnEnterEvent(FakeEnterExitCallback)
                .OnExitEvent(FakeEnterExitCallback);

            wf.AddStateSetting("EndState")
              .IsEndState()
              .OnEnterEvent(FakeEnterExitCallback);

            wf.AddTransition()
                .From("StartState")
                .On("Command")
                    .GoTo("EndState");

            //act
            var isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void BeTrueWhenMissingStartStateOnDeclaratioWithEvents()
        {
            //arrange
            var wf = new Floo();

            wf.AddStateSetting("StartState")
                .IsStartState();

            wf.AddStateSetting("EndState")
                .IsEndState()
                .OnEnterEvent(FakeEnterExitCallback)
                .OnExitEvent(FakeEnterExitCallback);

            wf.AddTransition()
                .From("StartState")
                .On("Command")
                .GoTo("EndState");

            //act
            var isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }
    }
}
