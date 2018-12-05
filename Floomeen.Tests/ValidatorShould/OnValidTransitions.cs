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
        public void ReturnTrueWhenRule0IsValidWithOnEnter()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("OnState").IsStartState();
            wf.AddSetting("ErrorState").IsEndState();

            wf.AddTransition()
                .From("OnState")
                .OnEnter(FakeEnterExitCallback)
                .On("SendMessage")
                .GoTo("ErrorState");

            //act
            bool isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void ReturnTrueWhenRule0IsValidWithOnExit()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("OnState").IsStartState();
            wf.AddSetting("ErrorState").IsEndState();

            wf.AddTransition()
                .From("OnState")
                .OnExit(FakeEnterExitCallback)
                .On("SendMessage")
                .GoTo("ErrorState")
                ;

            //act
            bool isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }
        [Fact]
        public void ReturnTrueWhenRule0IsValidWithOnExitAndOnEnter()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("OnState").IsStartState();
            wf.AddSetting("ErrorState").IsEndState();

            wf.AddTransition()
                .From("OnState")
                .OnExit(FakeEnterExitCallback)
                .OnEnter(FakeEnterExitCallback)
                .On("SendMessage")
                .GoTo("ErrorState")
                ;

            //act
            bool isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void ReturnTrueWhenRule1IsValid()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("OnState").IsStartState();
            wf.AddSetting("ErrorState").IsEndState();
            wf.AddTransition()
                .From("OnState")
                .On("SendMessage")
                .GoTo("ErrorState")
                ;

            //act
            bool isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void ReturnTrueWhenRule2IsValid()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("OnState").IsStartState();
            wf.AddSetting("ErrorState").IsEndState();
            wf.AddTransition()
                .From("OnState")
                .On("SendMessage")
                .Do(ctx => FakeFunk(ref ctx))
                    .When(FakeCheck).GoTo("ErrorState")
                ;

            //act
            bool isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void ReturnTrueWhenRule3IsValid()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("OnState").IsStartState();
            wf.AddSetting("ErrorState").IsEndState();
            wf.AddTransition()
                .From("OnState")
                .On("SendMessage")
                .Do(ctx => FakeFunk(ref ctx))
                    .When(FakeCheck)
                        .GoTo("SentState")
                    .When(FakeCheck)
                        .GoTo("ErrorState")
                ;

            //act
            bool isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void ReturnTrueWhenRule4IsValid()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("OnState").IsStartState();
            wf.AddSetting("ErrorState").IsEndState();
            wf.AddTransition()
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
            bool isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }

        [Fact]
        public void ReturnTrueWhen2Rule4IsValid()
        {
            //arrange
            var wf = new Floo();

            wf.AddSetting("OnState").IsStartState();
            wf.AddSetting("ErrorState").IsEndState();

            wf.AddTransition()
                .From("OnState")
                .On("SendMessage")
                .Do(ctx => FakeFunk(ref ctx))
                    .When(FakeCheck)
                        .GoTo("SentState")
                    .When(FakeCheck)
                        .GoTo("ErrorState")
                    .Otherwise()
                        .GoTo("OnState");

            wf.AddTransition()
                .From("OnState")
                .On("SendSmS")
                .Do(ctx => FakeFunk(ref ctx))
                    .When(FakeCheck)
                        .GoTo("SentState");

            //act
            bool isValid = wf.IsValid();

            //assert
            Assert.True(isValid);
        }
    }
}
