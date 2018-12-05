using System;
using Floomeen.Exceptions;
using Floomeen.Flow;
using Floomeen.Meen;
using Xunit;

namespace Floomeen.Tests.ValidatorShould
{

    public class OnInvalidSettings
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
        public void ThrowExceptionForInvalidSimpleDeclaration()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("State");

            //act
            Action act = () => wf.CheckValidity();

            //assert
            Assert.Throws<FloomeenException>(act);
        }

        [Fact]
        public void ThrowExceptionForIncompleteDeclaration()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("State");

            wf.AddTransition().From("State").On("Command").GoTo("End");

            //act
            Action act = () => wf.CheckValidity();

            //assert
            Assert.Throws<FloomeenException>(act);
        }

        [Fact]
        public void ThrowExceptionForInvalidSettingStateDeclaration()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("MissingState").IsStartState();

            wf.AddTransition().From("State").On("Command").GoTo("End");

            //act
            Action act = () => wf.CheckValidity();

            //assert
            Assert.Throws<FloomeenException>(act);
        }

        [Fact]
        public void ThrowExceptionForInvalidSettingFromStateDeclaration()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("StartState").IsEndState();

            wf.AddTransition().From("StartState").On("Command").GoTo("EndState");

            //act
            Action act = () => wf.CheckValidity();

            //assert
            Assert.Throws<FloomeenException>(act);
        }

        [Fact]
        public void ThrowExceptionForInvalidSettingToStateDeclaration()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("EndState").IsStartState();

            wf.AddTransition().From("State").On("Command").GoTo("EndState");

            //act
            Action act = () => wf.CheckValidity();

            //assert
            Assert.Throws<FloomeenException>(act);
        }

    }
}
