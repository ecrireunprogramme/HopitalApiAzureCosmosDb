using HopitalApi.Dtos;
using HopitalApi.Models;
using HopitalApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HopitalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
  private readonly IPatientService _patientService;
  private readonly ILogger<PatientsController> _logger;

  public PatientsController(ILogger<PatientsController> logger, IPatientService patientService)
  {
      _logger = logger;
      _patientService = patientService;
  }

  [HttpPost(Name = "CreatePatient")]
  public async Task<ActionResult<Patient>> CreatePatient(CreatePatientDto body)
  {
    // Verification telephone
    if ((await _patientService.GetPatientByTelephone(body.Telephone)) != null)
    {
      return BadRequest($"Le téléphone {body.Telephone} existe déjà");
    }

    // Verification numero
    if ((await _patientService.GetPatientByNumero(body.Numero)) != null)
    {
      return BadRequest($"Le numéro {body.Numero} existe déjà");
    }

    var response = await _patientService.CreatePatient(body);

    return Ok(response);
  }

  [HttpGet("{patientId}", Name = "GetPatientById")]
  public async Task<ActionResult<Patient>> GetPatientById(string patientId)
  {
      var response = await _patientService.GetPatientById(patientId);

      if (response == null) 
        return NotFound($"Patient {patientId} non trouvé!");

      return Ok(response);
  }
  
  [HttpGet(Name = "GetPatients")]
  public async Task<ActionResult<ICollection<Patient>>> GetPatients(int start = 0, int count = 20)
  {
      var response = await _patientService.GetPatients(start, count);

      return Ok(response);
  }

  [HttpPut("{patientId}", Name = "UpdatePatient")]
  public async Task<ActionResult<Patient>> UpdatePatient(string patientId, UpdatePatientDto body)
  {
    if ((await _patientService.GetPatientById(patientId)) == null)
    {
      return NotFound($"Le patient {patientId} n'existe pas");
    }

    var response = await _patientService.UpdatePatient(body);

    return Ok(response);
  }
  
  [HttpPatch("UpdateTelephone/{patientId}", Name = "UpdateTelephone")]
  public async Task<ActionResult<Patient>> UpdateTelephone(string patientId, UpdateTelephonePatientDto body)
  {
    if ((await _patientService.GetPatientById(patientId)) == null)
    {
      return NotFound($"Le patient {patientId} n'existe pas");
    }

    var response = await _patientService.UpdateTelephonePatient(body);

    return Ok(response);
  }

  
  [HttpPatch("UpdateNomPrenom/{patientId}", Name = "UpdateNomPrenom")]
  public async Task<ActionResult<Patient>> UpdateNomPrenom(string patientId, UpdateNomPrenomPatientDto body)
  {
    if ((await _patientService.GetPatientById(patientId)) == null)
    {
      return NotFound($"Le patient {patientId} n'existe pas");
    }

    var response = await _patientService.UpdateNomPrenomPatient(body);

    return Ok(response);
  }
  
  [HttpPatch("UpdateInfoVital/{patientId}", Name = "UpdateInfoVital")]
  public async Task<ActionResult<Patient>> UpdateInfoVital(string patientId, UpdateInfoVitalPatientDto body)
  {
    if ((await _patientService.GetPatientById(patientId)) == null)
    {
      return NotFound($"Le patient {patientId} n'existe pas");
    }

    var response = await _patientService.UpdateInfoVital(body);

    return Ok(response);
  }

  
  [HttpDelete("{patientId}", Name = "DeletePatient")]
  public async Task<ActionResult<Patient>> DeletePatient(string patientId)
  {
    if ((await _patientService.GetPatientById(patientId)) == null)
    {
      return NotFound($"Le patient {patientId} n'existe pas");
    }

    await _patientService.DeletePatient(patientId);

    return NoContent();
  }
}

