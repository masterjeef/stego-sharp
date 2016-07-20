namespace StegoSharp.Models
{
    public class Fraction<T> where T : struct
    {
        public T Numerator { get; set; }
        public T Denominator { get; set; }

        public T Result
        {
            get
            {
                dynamic num = Numerator;
                dynamic den = Denominator;
                return num / den;
            }
        }

        public Fraction(T numerator, T denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }
    }
}
