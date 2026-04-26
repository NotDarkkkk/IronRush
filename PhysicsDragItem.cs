using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpringJoint2D))]
public class PhysicsDragItem : MonoBehaviour
{
    private SpringJoint2D joint;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<SpringJoint2D>();

        joint.enabled = false;
        joint.autoConfigureDistance = false;
    }

    public void Attach(Rigidbody2D playerRb, float distance)
    {
        joint.connectedBody = playerRb;
        joint.distance = distance;

        joint.frequency = 2f;
        joint.dampingRatio = 0.7f;

        joint.enabled = true;
    }

    public void Detach()
    {
        joint.enabled = false;
        joint.connectedBody = null;
    }
}