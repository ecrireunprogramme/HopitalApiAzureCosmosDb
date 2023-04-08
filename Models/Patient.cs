using Newtonsoft.Json;

namespace HopitalApi.Models;

public class Patient
{
  [JsonProperty("id")]
  public Guid Id { get; set; }
  public string? Name { get; set; }
  public string? Prenom { get; set; }
  public string? Numero { get; set; }
  public string? Telephone { get; set; }
  public string? CodeHopital { get; set; }
  public DateTime DateCreation { get; set; } = DateTime.Now;
  public InfoVital? InfoVital { get; set; }
  public Consultation[]? DerniereConsultations {get; set; }
}

public class InfoVital
{
  public int Temperature { get; set; }
  public int Poids { get; set; }
  public int Taille { get; set; }
}

public class Consultation
{
  [JsonProperty("id")]
  public Guid Id { get; set; }
  public DateTime Date { get; set; }
  public string? Objet { get; set; }
  public string? CompteRendu { get; set; }
}