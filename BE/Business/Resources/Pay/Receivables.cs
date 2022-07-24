namespace Business.Resources.Pay
{
    public class Receivables
    {
        #region Property
        public byte PIT { get; } // Personal income tax
        public byte SocialInsurance { get; } = 5; // unit %
        public byte HealthInsurance { get; } = 1; // unit %
        #endregion

        #region Constructor
        public Receivables(decimal gross)
        {
            if (0 < gross && gross <= 5_000_000m)
                this.PIT = 5;
            if (5_000_000m < gross && gross <= 10_000_000m)
                this.PIT = 10;
            if (10_000_000m < gross && gross <= 18_000_000m)
                this.PIT = 15;
            if (18_000_000m < gross && gross <= 32_000_000m)
                this.PIT = 20;
            if (32_000_000m < gross && gross <= 52_000_000m)
                this.PIT = 25;
            if (52_000_000m < gross && gross <= 80_000_000m)
                this.PIT = 30;
            if (gross > 80_000_000m)
                this.PIT = 35;
        }
        #endregion
    }
}
