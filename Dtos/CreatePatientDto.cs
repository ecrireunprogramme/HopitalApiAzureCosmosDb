namespace HopitalApi.Dtos;

public class CreatePatientDto
{
  public string? Name { get; set; }
  public string? Prenom { get; set; }
  public string? Numero { get; set; }
  public string? Telephone { get; set; }
  public string? CodeHopital { get; set; }
  public CreateInfoVitalDto? InfoVital { get; set; }
  public CreateConsultationDto[]? DerniereConsultations {get; set; }
}

public class CreateInfoVitalDto
{
  public int Temperature { get; set; }
  public int Poids { get; set; }
  public int Taille { get; set; }
}

public class CreateConsultationDto
{
  public DateTime Date { get; set; }
  public string? Objet { get; set; }
  public string? CompteRendu { get; set; }
}