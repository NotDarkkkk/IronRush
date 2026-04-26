using UnityEngine;

public class SellArea : MonoBehaviour
{
    public Customer currentCustomer;

    public int baseReward = 10;

    public void SetCustomer(Customer customer)
    {
        currentCustomer = customer;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Item item = col.GetComponent<Item>();

        if (item == null)
            return;

        TrySell(item);
    }

    void Update()
    {
        if (currentCustomer == null && 
            GameTimeManager.Instance != null &&
            GameTimeManager.Instance.spawner != null)
        {
            currentCustomer = GameTimeManager.Instance.spawner.currentCustomer;
        }
    }

    void TrySell(Item item)
    {
        if (currentCustomer == null)
        {
            Debug.Log("No customer to sell to");
            return;
        }

        if (currentCustomer.hasBeenServed)
            return;

        bool correctItem = item.type == currentCustomer.requestedItem;

        currentCustomer.Serve(item.type);

        int reward = CalculateReward(correctItem);

        Debug.Log("Sold item for: " + reward);

        FindAnyObjectByType<PlayerWallet>()?.AddMoney(reward);

        Destroy(item.gameObject);
    }

    int CalculateReward(bool correct)
    {
        if (!correct)
            return 2;

        return baseReward;
    }
}