using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform spawnPoint;

    public Customer currentCustomer;

    [Header("Spawn Delay After Customer Leaves")]
    public float spawnDelayAfterCustomer = 3f;

    private bool canSpawn = true;

    public void SpawnCustomer()
    {
        if (!canSpawn) return;
        if (currentCustomer != null) return;

        GameObject obj = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);

        currentCustomer = obj.GetComponent<Customer>();

        Debug.Log("Spawned customer: " + currentCustomer);

        currentCustomer.Setup(GetRandomItem());

        if (currentCustomer == null)
        {
            Debug.LogError("Customer spawn failed!");
        }
    }

    public void ClearCustomer()
    {
        currentCustomer = null;

        StartCoroutine(SpawnCooldown());
    }

    IEnumerator SpawnCooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnDelayAfterCustomer);
        canSpawn = true;
    }

    public void DespawnAllCustomers()
    {
        if (currentCustomer != null)
        {
            Destroy(currentCustomer.gameObject);
            currentCustomer = null;
        }
    }

    ItemType GetRandomItem()
    {
        return (ItemType)Random.Range(0, System.Enum.GetValues(typeof(ItemType)).Length);
    }
}