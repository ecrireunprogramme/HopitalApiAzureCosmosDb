namespace HopitalApi.Dtos;

public class UpdatePatientDto
{
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public string? Prenom { get; set; }
  public string? Numero { get; set; }
  public string? Telephone { get; set; }
  public string? CodeHopital { get; set; }
  public UpdateInfoVitalDto? InfoVital { get; set; }
  public UpdateConsultationDto[]? DerniereConsultations {get; set; }
}

public class UpdateInfoVitalDto
{
  public int Temperature { get; set; }
  public int Poids { get; set; }
  public int Taille { get; set; }
}

public class UpdateConsultationDto
{
  public Guid Id { get; set; }
  public DateTime Date { get; set; }
  public string? Objet { get; set; }
  public string? CompteRendu { get; set; }
}