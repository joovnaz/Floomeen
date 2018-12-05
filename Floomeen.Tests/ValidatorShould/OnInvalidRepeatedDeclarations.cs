using System;
using Floomeen.Exceptions;
using Floomeen.Flow;
using Floomeen.Meen;
using Xunit;

namespace Floomeen.Tests.ValidatorShould
{

    public class OnInvalidRepeatedDeclarations
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
        public void ThrowExceptionForOnEnterDeclaration()
        {
            //arrange
            var wf = new Floo();

            //act
            Action act = () => wf.AddTransition()
                                 .From("OnState")
                                 .OnEnter(FakeEnterExitCallback)
                                 .OnEnter(FakeEnterExitCallback);


            //assert
            Assert.Throws<FloomeenException>(act);
        }

        [Fact]
        public void ThrowExceptionForOnExitDeclaration()
        {
            //arrange
            var wf = new Floo();

            //act
            Action act = () => wf.AddTransition()
                .From("OnState")
                .OnExit(FakeEnterExitCallback)
                .OnExit(FakeEnterExitCallback);


            //assert
            Assert.Throws<FloomeenException>(act);
        }

        [Fact]
        public void ThrowExceptionForOnEnterDeclarationOnDifferentRules()
        {
            //arrange
            var wf = new Floo();

            wf.AddTransition().From("State").OnEnter(FakeEnterExitCallback).On("Command").GoTo("State");

            wf.AddTransition().From("State").OnEnter(FakeEnterExitCallback).On("Command").GoTo("State");

            //act
            Action act = () => wf.CheckValidity();

            //assert
            Assert.Throws<FloomeenException>(act);
        }

        [Fact]
        public void ThrowExceptionForOnExitDeclarationOnDifferentRules()
        {
            //arrange
            var wf = new Floo();

            wf.AddTransition().From("State").OnExit(FakeEnterExitCallback).On("Command").GoTo("State");

            wf.AddTransition().From("State").OnExit(FakeEnterExitCallback).On("Command").GoTo("State");

            //act
            Action act = () => wf.CheckValidity();

            //assert
            Assert.Throws<FloomeenException>(act);
        }

    }
}
