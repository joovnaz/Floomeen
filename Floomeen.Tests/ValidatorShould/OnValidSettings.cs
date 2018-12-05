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

            wf.AddSetting("StartState").IsStartState();

            wf.AddTransition().From("StartState").On("Command").GoTo("EndState");

            //act
            bool isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void BeTrueForEndStateDeclaration()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("EndState").IsEndState();

            wf.AddTransition().From("StartState").On("Command").GoTo("EndState");

            //act
            bool isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void BeTrueForEndStateDeclarationWithEventOnEnterEvent()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("EndState")
              .IsEndState()
              .OnEnterEvent(FakeEnterExitCallback);

            wf.AddTransition().From("StartState").On("Command").GoTo("EndState");

            //act
            bool isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void BeTrueForEndStateDeclarationWithEvents()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("EndState")
                .IsEndState()
                .OnEnterEvent(FakeEnterExitCallback)
                .OnExitEvent(FakeEnterExitCallback);

            wf.AddTransition().From("StartState").On("Command").GoTo("EndState");

            //act
            bool isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }
    }
}
