using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float dragDistance = 1.5f;
    public KeyCode key = KeyCode.E;

    private PhysicsDragItem nearbyItem;
    private PhysicsDragItem attachedItem;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey(key))
        {
            if (attachedItem == null && nearbyItem != null)
            {
                attachedItem = nearbyItem;
                attachedItem.Attach(rb, dragDistance);
            }
        }
        else
        {
            if (attachedItem != null)
            {
                attachedItem.Detach();
                attachedItem = null;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var item = col.GetComponent<PhysicsDragItem>();
        if (item != null)
            nearbyItem = item;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var item = col.GetComponent<PhysicsDragItem>();
        if (item == nearbyItem)
            nearbyItem = null;
    }
}