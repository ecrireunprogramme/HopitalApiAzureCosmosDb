namespace HopitalApi.Dtos;

public class UpdateNomPrenomPatientDto
{
  public Guid Id { get; set; }
  public string Nom { get; set; }
  public string Prenom { get; set; }
}