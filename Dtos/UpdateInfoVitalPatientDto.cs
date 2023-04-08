namespace HopitalApi.Dtos;

public class UpdateInfoVitalPatientDto
{
  public Guid Id { get; set; }
  public int Temperature { get; set; }
  public int Poids { get; set; }
  public int Taille { get; set; }
}