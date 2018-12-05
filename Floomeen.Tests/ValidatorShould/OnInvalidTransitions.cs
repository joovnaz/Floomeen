using System;
using Floomeen.Exceptions;
using Floomeen.Flow;
using Floomeen.Meen;
using Xunit;

namespace Floomeen.Tests.ValidatorShould
{

    public class OnInvalidTransitions
    {
        static Result FakeFunk(ref Context context)
        {
            return null;
        }

        static bool FakeCheck(Result result, Context context)
        {
            return result.Success;
        }


        static void FakeEnterExitCallback(Context context)
        {
        }

        [Fact]
        public void ThrowExceptionWhenMissingEndStateButOnSpecified()
        {
            //arrange
            var wf = new Floo();

            wf.AddTransition()
                .From("OnState")
                .On("SendMessage");

            //act
            Action act = () => wf.CheckValidity();

            //assert
            Assert.Throws<FloomeenException>(act);
        }

        [Fact]
        public void ThrowExceptionWhenMissingConditions()
        {
            //arrange
            var wf = new Floo();

            wf.AddTransition()
                .From("OnState")
                .On("SendMessage")
                .Do(ctx => FakeFunk(ref ctx));

            //act
            Action act = () => wf.CheckValidity();
            
            //assert
            Assert.Throws<FloomeenException>(act);
        }

        [Fact]
        public void ThrowExceptionWhenMissingFirstEndState()
        {
            //arrange
            var wf = new Floo();

            wf.AddTransition()
                .From("OnState")
                .On("SendMessage")
                .Do(ctx => FakeFunk(ref ctx))
                .When(FakeCheck);

            //act
            Action act = () => wf.CheckValidity();

            //assert
            Assert.Throws<FloomeenException>(act);
        }

        [Fact]
        public void ThrowExceptionWhenMissingSecondEndState()
        {
            //arrange
            var wf = new Floo();

            wf.AddTransition()
                .From("OnState")
                .On("SendMessage")
                .Do(ctx => FakeFunk(ref ctx))
                .When(FakeCheck)
                .GoTo("SentState")
                .When(FakeCheck);

            //act
            Action act = () => wf.CheckValidity();

            //assert
            Assert.Throws<FloomeenException>(act);
        }

        [Fact]
        public void ThrowExceptionWhenMissingThirdEndState()
        {
            //arrange
            var wf = new Floo();

            wf.AddTransition()
                .From("OnState")
                .On("SendMessage")
                .Do(ctx => FakeFunk(ref ctx))
                .When(FakeCheck)
                .GoTo("SentState")
                .When(FakeCheck)
                .GoTo("ErrorState")
                .Otherwise()
                //.StayAt("OnState")
                ;

            //act
            Action act = () => wf.CheckValidity();

            //assert
            Assert.Throws<FloomeenException>(act);
        }

        [Fact]
        public void ThrowExceptionWhenSecondRuleMissingThirdEndState()
        {
            //arrange
            var wf = new Floo();

            wf.AddTransition()
                .From("State")
                .On("Command")
                .GoTo("State");

            wf.AddTransition()
                .From("OnState")
                .On("SendMessage")
                .Do(ctx => FakeFunk(ref ctx))
                    .When(FakeCheck)
                    .GoTo("SentState")
                    .When(FakeCheck)
                    .GoTo("ErrorState")
                    .Otherwise()
                    //.StayAt("OnState")
                    ;

            //act
            Action act = () => wf.CheckValidity();

            //assert
            Assert.Throws<FloomeenException>(act);
        }
    }
}
