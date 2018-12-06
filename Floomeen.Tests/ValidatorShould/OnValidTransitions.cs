using Floomeen.Flow;
using Floomeen.Meen;
using Xunit;

namespace Floomeen.Tests.ValidatorShould
{

    public class OnValidTransitions
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
        public void ReturnTrueWhenRule1IsValid()
        {
            //arrange
            var flow = new Floo();

            flow.AddStateSetting("OnState").IsStartState();

            flow.AddStateSetting("ErrorState").IsEndState();

            flow.AddTransition()
                .From("OnState")
                    .On("SendMessage")
                        .GoTo("ErrorState");

            //act
            bool isValid = flow.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void ReturnTrueWhenRule2IsValid()
        {
            //arrange
            var flow = new Floo();

            flow.AddStateSetting("OnState").IsStartState();

            flow.AddStateSetting("ErrorState").IsEndState();

            flow.AddTransition()
                .From("OnState")
                    .On("SendMessage")
                    .Do(ctx => FakeFunk(ref ctx))
                        .When(FakeCheck).GoTo("ErrorState");

            //act
            bool isValid = flow.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void ReturnTrueWhenRule3IsValid()
        {
            //arrange
            var flow = new Floo();

            flow.AddStateSetting("OnState").IsStartState();

            flow.AddStateSetting("ErrorState").IsEndState();

            flow.AddTransition()
                .From("OnState")
                    .On("SendMessage")
                    .Do(ctx => FakeFunk(ref ctx))
                        .When(FakeCheck)
                            .GoTo("SentState")
                        .When(FakeCheck)
                            .GoTo("ErrorState");

            //act
            bool isValid = flow.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void ReturnTrueWhenRule4IsValid()
        {
            //arrange
            var flow = new Floo();

            flow.AddStateSetting("OnState").IsStartState();

            flow.AddStateSetting("ErrorState").IsEndState();

            flow.AddTransition()
                .From("OnState")
                .On("SendMessage")
                .Do(ctx => FakeFunk(ref ctx))
                    .When(FakeCheck)
                        .GoTo("SentState")
                    .When(FakeCheck)
                        .GoTo("ErrorState")
                    .Otherwise()
                        .StayAt("OnState");

            //act
            bool isValid = flow.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void ReturnTrueWhen2Rule4IsValid()
        {
            //arrange
            var flow = new Floo();

            flow.AddStateSetting("OnState").IsStartState();

            flow.AddStateSetting("ErrorState").IsEndState();

            flow.AddTransition()
                .From("OnState")
                .On("SendMessage")
                .Do(ctx => FakeFunk(ref ctx))
                    .When(FakeCheck)
                        .GoTo("SentState")
                    .When(FakeCheck)
                        .GoTo("ErrorState")
                    .Otherwise()
                        .GoTo("OnState");

            flow.AddTransition()
                .From("OnState")
                .On("SendSmS")
                .Do(ctx => FakeFunk(ref ctx))
                    .When(FakeCheck)
                        .GoTo("SentState");

            //act
            bool isValid = flow.IsValid();

            //assert
            Assert.True(isValid);
        }
    }
}
