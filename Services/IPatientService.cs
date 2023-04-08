using HopitalApi.Dtos;
using HopitalApi.Models;

namespace HopitalApi.Services;

public interface IPatientService
{
  Task<Patient> CreatePatient(CreatePatientDto dto);

  Task<Patient> GetPatientById(string id);

  Task<Patient> GetPatientByTelephone(string telephone);

  Task<Patient> GetPatientByNumero(string numero);

  Task<ICollection<Patient>> GetPatients(int start, int count);
  
  Task<Patient> UpdatePatient(UpdatePatientDto dto);

  Task<Patient> UpdateTelephonePatient(UpdateTelephonePatientDto dto);
  
  Task<Patient> UpdateNomPrenomPatient(UpdateNomPrenomPatientDto dto);

  Task<Patient> UpdateInfoVital(UpdateInfoVitalPatientDto dto);

   Task DeletePatient(string id);
}