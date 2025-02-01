using AutoMapper;
using System.Net;
using System.Reflection;
using SystemPayment.API.DataModels;
using SystemPayment.API.DTO;

namespace SystemPayment.API.Profiles
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			// PaymentType
			CreateMap<PaymentTypeDto, PaymentType>().ReverseMap();
			CreateMap<PaymentTypeCreateDto, PaymentType>().ReverseMap();
			CreateMap<PaymentTypeUpdateDto, PaymentType>().ReverseMap();

			// Branch
			CreateMap<Branch, BranchDto>().ReverseMap();
			CreateMap<Branch, BranchCreateDto>().ReverseMap();
			CreateMap<Branch, BranchUpdateDto>().ReverseMap();

			// EducationType
			CreateMap<EducationType, EducationTypeDto>().ReverseMap();
			CreateMap<EducationType, EducationTypeCreateDto>().ReverseMap();
			CreateMap<EducationType, EducationTypeUpdateDto>().ReverseMap();

			// EducationYear
			CreateMap<EducationYear, EducationYearDto>().ReverseMap();
			CreateMap<EducationYear, EducationYearCreateDto>().ReverseMap();
			CreateMap<EducationYear, EducationYearUpdateDto>().ReverseMap();

			// PaymentSetting
			CreateMap<PaymentSetting, PaymentSettingDto>()
			 .ForMember(dest => dest.PaymentTypeName, opt => opt.MapFrom(src => src.PaymentType.Name))
				.ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
				.ForMember(dest => dest.EducationTypeName, opt => opt.MapFrom(src => src.EducationType.Name))
				.ForMember(dest => dest.EducationYear, opt => opt.MapFrom(src => src.EducationYear.Year))
				.ReverseMap();
			CreateMap<PaymentSetting, PaymentCheckResultDto>()
			 .ForMember(dest => dest.PaymentTypeName, opt => opt.MapFrom(src => src.PaymentType.Name))
				.ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
				.ForMember(dest => dest.EducationTypeName, opt => opt.MapFrom(src => src.EducationType.Name))
				.ForMember(dest => dest.EducationYear, opt => opt.MapFrom(src => src.EducationYear.Year))
				.ReverseMap();
			CreateMap<PaymentSetting, PaymentSettingCreateDto>().ReverseMap();
			CreateMap<PaymentSetting, PaymentSettingUpdateDto>().ReverseMap();
		}
	}
}
