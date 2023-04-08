using AutoMapper;
using HopitalApi.Dtos;
using HopitalApi.Models;

namespace HopitalApi.Profiles;

public class AppProfiles : Profile
{
  public AppProfiles()
  {
    CreateMap<CreatePatientDto, Patient>();
    CreateMap<CreateInfoVitalDto, InfoVital>();
    CreateMap<CreateConsultationDto, Consultation>();
    
    CreateMap<UpdatePatientDto, Patient>();
    CreateMap<UpdateInfoVitalDto, InfoVital>();
    CreateMap<UpdateConsultationDto, Consultation>();
  }
}