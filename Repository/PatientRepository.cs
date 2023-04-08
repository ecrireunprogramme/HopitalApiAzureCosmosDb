using System.Configuration;
using HopitalApi.Helpers;
using HopitalApi.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace HopitalApi.Repository;

public class PatientRepository : IPatientRepository
{
  private readonly Container _patientContainer;

  public PatientRepository(CosmosClient client, string databaseId, string containerId)
  {
    _patientContainer = client.GetContainer(databaseId, containerId);
  }

  public async Task<Patient> CreatePatient(Patient patient)
  {
    ItemResponse<Patient> patientResponse = await _patientContainer.CreateItemAsync<Patient>(
      patient, new PartitionKey(AppHelpers.CodeHopital));

    return patientResponse.Resource;
  }

  public async Task<Patient> GetPatientById(string id)
  {
    try 
    {
      var patientResponse = await _patientContainer.ReadItemAsync<Patient>(id, new PartitionKey(AppHelpers.CodeHopital));

      return patientResponse.Resource;
    }
    catch (CosmosException e)  when (e.StatusCode == System.Net.HttpStatusCode.NotFound) 
    {
      return null;
    }
  }
  
  public async Task<Patient> GetPatientByTelephone(string telephone)
  {
    // Get LINQ IQueryable object
    IOrderedQueryable<Patient> queryable = _patientContainer.GetItemLinqQueryable<Patient>();

    // Construct LINQ query
    var matches = queryable
        .Where(p => p.Telephone == telephone);

    // Convert to feed iterator
    using FeedIterator<Patient> linqFeed = matches.ToFeedIterator();

    // Iterate query result pages
    while (linqFeed.HasMoreResults)
    {
      FeedResponse<Patient> response = await linqFeed.ReadNextAsync();

      return response.Resource.FirstOrDefault();
    }

    return null;
  }

  public async Task<Patient> GetPatientByNumero(string numero)
  {
    // Get LINQ IQueryable object
    IOrderedQueryable<Patient> queryable = _patientContainer.GetItemLinqQueryable<Patient>();

    // Construct LINQ query
    var matches = queryable
        .Where(p => p.Numero == numero);

    // Convert to feed iterator
    using FeedIterator<Patient> linqFeed = matches.ToFeedIterator();

    // Iterate query result pages
    while (linqFeed.HasMoreResults)
    {
      FeedResponse<Patient> response = await linqFeed.ReadNextAsync();

      return response.Resource.FirstOrDefault();
    }

    return null;
  }
  
  public async Task<ICollection<Patient>> GetPatients(int start, int count)
  {
    List<Patient> patients = new();

    // Get LINQ IQueryable object
    IOrderedQueryable<Patient> queryable = _patientContainer.GetItemLinqQueryable<Patient>();

    // Construct LINQ query
    var matches = queryable
      .Skip(start)
      .Take(count);

    // Convert to feed iterator
    using FeedIterator<Patient> linqFeed = matches.ToFeedIterator();

    // Iterate query result pages
    while (linqFeed.HasMoreResults)
    {
      FeedResponse<Patient> response = await linqFeed.ReadNextAsync();

      foreach (Patient patient in response)
      {
        patients.Add(patient);
      }
    }

    return patients;
  }

  public async Task<Patient> UpdatePatient(Patient patient)
  {
    ItemResponse<Patient> patientResponse = await _patientContainer.ReplaceItemAsync<Patient>(
      patient, patient.Id.ToString(), new PartitionKey(patient.CodeHopital));

    return patientResponse.Resource;
  }
  
  public async Task<Patient> UpdateTelephonePatient(string id, string telephone)
  {
    List<PatchOperation> operations = new ()
    {
        PatchOperation.Replace("/Telephone", telephone),
    };

    ItemResponse<Patient> patientResponse = await _patientContainer.PatchItemAsync<Patient>(
        id: id,
        partitionKey: new PartitionKey(AppHelpers.CodeHopital),
        patchOperations: operations
    );

    return patientResponse.Resource;
  }

  public async Task<Patient> UpdateNomPrenomPatient(string id, string nom, string prenom)
  {
    List<PatchOperation> operations = new ()
    {
        PatchOperation.Replace("/Name", nom),
        PatchOperation.Replace("/Prenom", prenom),
    };

    ItemResponse<Patient> patientResponse = await _patientContainer.PatchItemAsync<Patient>(
        id: id,
        partitionKey: new PartitionKey(AppHelpers.CodeHopital),
        patchOperations: operations
    );

    return patientResponse.Resource;
  }

  public async Task<Patient> UpdateInfoVital(string id, int temperature, int poids, int taille)
  {
    List<PatchOperation> operations = new ()
    {
        PatchOperation.Replace("/InfoVital/Temperature", temperature),
        PatchOperation.Replace("/InfoVital/Poids", poids),
        PatchOperation.Replace("/InfoVital/Taille", taille),
    };

    ItemResponse<Patient> patientResponse = await _patientContainer.PatchItemAsync<Patient>(
        id: id,
        partitionKey: new PartitionKey(AppHelpers.CodeHopital),
        patchOperations: operations
    );

    return patientResponse.Resource;
  }

  public async Task DeletePatient(string id)
  {
    await _patientContainer.DeleteItemAsync<Patient>(id, new PartitionKey(AppHelpers.CodeHopital));
  }

  public static async Task<PatientRepository> GetInstance(IConfigurationSection configSection)
  {
    var endPointUri = configSection.GetSection("EndPointUri").Value;
    var authorizationKey = configSection.GetSection("AuthorizationKey").Value;
    var databaseName = configSection.GetSection("DatabaseName").Value;
    var patientContainerName = configSection.GetSection("PatientContainerName").Value;

    if (string.IsNullOrEmpty(endPointUri))
    {
      throw new ArgumentNullException("Veuillez fournir le parametre endPointUri");
    }

    if (string.IsNullOrEmpty(authorizationKey))
    {
      throw new ArgumentNullException("Veuillez fournir le parametre AuthorizationKey");
    }

    CosmosClient client = new CosmosClient(endPointUri, authorizationKey);

    Database database = await client.CreateDatabaseIfNotExistsAsync(databaseName, throughput: 400);

    await database.DefineContainer(patientContainerName, "/CodeHopital")
      .WithUniqueKey()
        .Path("/Telephone")
      .Attach()
      .WithUniqueKey()
        .Path("/Numero")
      .Attach()
      .CreateIfNotExistsAsync();

    //await database.CreateContainerIfNotExistsAsync(patientContainerName, partitionKeyPath: "/CodeHopital");

    PatientRepository patientRepo = new PatientRepository(client, databaseName, patientContainerName);

    return patientRepo;
  }

}