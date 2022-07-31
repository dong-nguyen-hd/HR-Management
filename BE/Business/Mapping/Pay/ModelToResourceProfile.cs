using AutoMapper;
using Business.Resources.Pay;

namespace Business.Mapping.Pay
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Pay, PayResource>()
                .ForMember(x => x.SocialInsurancePercent, opt => opt.MapFrom(src => src.SocialInsurance))
                .ForMember(x => x.SocialInsurance, opt => opt.MapFrom(src => CalculateSocialInsurance(src)))
                .ForMember(x => x.HealthInsurancePercent, opt => opt.MapFrom(src => src.HealthInsurance))
                .ForMember(x => x.HealthInsurance, opt => opt.MapFrom(src => CalculateHealthInsurance(src)))
                .ForMember(x => x.PITPercent, opt => opt.MapFrom(src => src.PIT))
                .ForMember(x => x.PIT, opt => opt.MapFrom(src => CalculatePIT(src)))
                .ForMember(x => x.Gross, opt => opt.MapFrom(src => CalculateGross(src)))
                .ForMember(x => x.NET, opt => opt.MapFrom(src => CalculateNET(src)));
        }

        #region Private work
        private static decimal CalculateSocialInsurance(Domain.Models.Pay src)
        {
            decimal grossWithoutBonus = (decimal)src.WorkDay * src.BaseSalary / (decimal)src.TotalWorkDay;

            return grossWithoutBonus / 100 * (decimal)src.SocialInsurance;
        }

        private static decimal CalculatePIT(Domain.Models.Pay src)
        {
            decimal grossWithoutBonus = (decimal)src.WorkDay * src.BaseSalary / (decimal)src.TotalWorkDay;

            return grossWithoutBonus / 100 * (decimal)src.PIT;
        }

        private static decimal CalculateHealthInsurance(Domain.Models.Pay src)
        {
            decimal grossWithoutBonus = (decimal)src.WorkDay * src.BaseSalary / (decimal)src.TotalWorkDay;

            return grossWithoutBonus / 100 * (decimal)src.HealthInsurance;
        }

        private static decimal CalculateGross(Domain.Models.Pay src)
        {
            decimal grossWithoutBonus = (decimal)src.WorkDay * src.BaseSalary / (decimal)src.TotalWorkDay;

            return grossWithoutBonus + src.Bonus + src.Allowance;
        }

        private static decimal CalculateNET(Domain.Models.Pay src)
        {
            decimal grossWithoutBonus = (decimal)src.WorkDay * src.BaseSalary / (decimal)src.TotalWorkDay;

            return (CalculateSocialInsurance(src) + CalculateHealthInsurance(src) + CalculatePIT(src)) * grossWithoutBonus + src.Bonus + src.Allowance;
        }
        #endregion
    }
}
