using AutoMapper;
using HopitalApi.Dtos;
using HopitalApi.Models;
using HopitalApi.Repository;

namespace HopitalApi.Services;

public class PatientService : IPatientService
{
  private readonly IPatientRepository _patientRepository;
  private readonly IMapper _mapper;

  public PatientService(IPatientRepository patientRepository, IMapper mapper)
  {
    _patientRepository = patientRepository;
    _mapper = mapper;
  }

  public async Task<Patient> CreatePatient(CreatePatientDto dto)
  {
    var patient = _mapper.Map<Patient>(dto);
    
    patient.Id = Guid.NewGuid();
    patient.DerniereConsultations = patient.DerniereConsultations?.Select(c => new Consultation {
      Id = Guid.NewGuid(),
      CompteRendu = c.CompteRendu,
      Date = c.Date,
      Objet = c.Objet
    })
    .ToArray();

    var patientCreated = await _patientRepository.CreatePatient(patient); 

    return patientCreated;
  }

  public async Task<Patient> GetPatientById(string id)
  {
    var patient = await _patientRepository.GetPatientById(id);

    return patient;
  }

  public async Task<Patient> GetPatientByNumero(string numero)
  {
    return await _patientRepository.GetPatientByNumero(numero);
  }

  public async Task<Patient> GetPatientByTelephone(string telephone)
  {
    return await _patientRepository.GetPatientByTelephone(telephone);
  }

  public async Task<ICollection<Patient>> GetPatients(int start, int count)
  {
    return await _patientRepository.GetPatients(start, count);
  }

  public async Task<Patient> UpdatePatient(UpdatePatientDto dto)
  {
    var patient = _mapper.Map<Patient>(dto);

    return await _patientRepository.UpdatePatient(patient);
  }

  public async Task<Patient> UpdateTelephonePatient(UpdateTelephonePatientDto dto)
  {
    return await _patientRepository.UpdateTelephonePatient(dto.Id.ToString(), dto.Telephone);
  }

  public async Task<Patient> UpdateNomPrenomPatient(UpdateNomPrenomPatientDto dto)
  {
    return await _patientRepository.UpdateNomPrenomPatient(dto.Id.ToString(), dto.Nom, dto.Prenom);
  }

  public async Task<Patient> UpdateInfoVital(UpdateInfoVitalPatientDto dto)
  {
    return await _patientRepository.UpdateInfoVital(dto.Id.ToString(), dto.Temperature, dto.Poids, dto.Taille);
  }

  public async Task DeletePatient(string id)
  {
    await _patientRepository.DeletePatient(id);
  }
}