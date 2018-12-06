using System;
using Floomeen.Exceptions;
using Floomeen.Flow;
using Floomeen.Meen;
using Xunit;

namespace Floomeen.Tests.ValidatorShould
{

    public class OnInvalidRepeatedTransitions
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
        public void ThrowExceptionWhenSameCommandRepeated()
        {
            //arrange
            var wf = new Floo();

            wf.AddTransition()
                .From("OnState").On("Command").GoTo("State");

            wf.AddTransition()
                .From("OnState").On("Command").GoTo("State");

            //act
            Action act = () => wf.Check();

            //assert
            Assert.Throws<FloomeenException>(act);
        }

    }
}
