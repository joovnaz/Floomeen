namespace Floomeen.Meen
{
    public class FlooInt
    {
        public int Value { get; }

        public FlooInt(int value)
        {
            Value = value;
        }

        public static FlooInt operator +(FlooInt a, int b)
        { 
            return new FlooInt(a.Value + b);
        }

        public static FlooInt operator -(FlooInt a, int b)
        {
            return new FlooInt(a.Value - b);
        }

        public static FlooInt operator *(FlooInt a, int b)
        {
            return new FlooInt(a.Value * b);
        }

        public static bool operator <(FlooInt a, int b)
        {
            return a.Value < b;
        }

        public static bool operator >(FlooInt a, int b)
        {
            return a.Value > b;
        }

        public static bool operator <=(FlooInt a, int b)
        {
            return a.Value <= b;
        }

        public static bool operator >=(FlooInt a, int b)
        {
            return a.Value >= b;
        }

        public static bool operator !=(FlooInt a, int b)
        {
            return a.Value != b;
        }

        public static bool operator ==(FlooInt a, int b)
        {
            return a.Value == b;
        }

        public override bool Equals(object o)
        {
            if (o is FlooInt oint)
            {
                return oint.Value == Value;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

    }
}