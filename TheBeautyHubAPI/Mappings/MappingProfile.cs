using AutoMapper;
using TheBeautyHubAPI.Models;
using TheBeautyHubCore.DTOs;
using TheBeautyHubData.Entities;

namespace TheBeautyHubAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Account mappings
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<CreateAccountDto, Account>();
            CreateMap<UpdateAccountDto, Account>();
            CreateMap<CreateAccountRequest, CreateAccountDto>();
            CreateMap<UpdateAccountRequest, UpdateAccountDto>();
            CreateMap<AccountDto, AccountResponse>();

            // User mappings
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<CreateUserRequest, CreateUserDto>();
            CreateMap<UpdateUserRequest, UpdateUserDto>();
            CreateMap<UserDto, UserResponse>();

            // Firm mappings
            CreateMap<Firm, FirmDto>().ReverseMap();
            CreateMap<CreateFirmDto, Firm>();
            CreateMap<UpdateFirmDto, Firm>();
            CreateMap<CreateFirmRequest, CreateFirmDto>();
            CreateMap<UpdateFirmRequest, UpdateFirmDto>();
            CreateMap<FirmDto, FirmResponse>();

            // FirmDetails mappings
            CreateMap<FirmDetails, FirmDetailsDto>().ReverseMap();
            CreateMap<CreateFirmDetailsDto, FirmDetails>();
            CreateMap<UpdateFirmDetailsDto, FirmDetails>();
            CreateMap<CreateFirmDetailsRequest, CreateFirmDetailsDto>();
            CreateMap<UpdateFirmDetailsRequest, UpdateFirmDetailsDto>();
            CreateMap<FirmDetailsDto, FirmDetailsResponse>();

            // Plans mappings
            CreateMap<Plans, PlansDto>().ReverseMap();
            CreateMap<CreatePlanDto, Plans>();
            CreateMap<UpdatePlanDto, Plans>();
            CreateMap<CreatePlanRequest, CreatePlanDto>();
            CreateMap<UpdatePlanRequest, UpdatePlanDto>();
            CreateMap<PlansDto, PlanResponse>();

            // Subscription mappings
            CreateMap<Subscription, SubscriptionDto>().ReverseMap();
            CreateMap<CreateSubscriptionDto, Subscription>();
            CreateMap<UpdateSubscriptionDto, Subscription>();
            CreateMap<CreateSubscriptionRequest, CreateSubscriptionDto>();
            CreateMap<UpdateSubscriptionRequest, UpdateSubscriptionDto>();
            CreateMap<SubscriptionDto, SubscriptionResponse>();
        }
    }
}
