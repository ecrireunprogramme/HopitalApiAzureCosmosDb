using HopitalApi.Models;

namespace HopitalApi.Repository;

public interface IPatientRepository
{
  Task<Patient> CreatePatient(Patient patient);

  Task<Patient> GetPatientById(string id);

  Task<Patient> GetPatientByTelephone(string telephone);

  Task<Patient> GetPatientByNumero(string numero);

  Task<ICollection<Patient>> GetPatients(int start, int count);

  Task<Patient> UpdatePatient(Patient patient);
  
  Task<Patient> UpdateTelephonePatient(string id, string telephone);
  
  Task<Patient> UpdateNomPrenomPatient(string id, string nom, string prenom);
  
  Task<Patient> UpdateInfoVital(string id, int temperature, int poids, int taille);

  Task DeletePatient(string id);
}