using UnityEngine;

public class CraftButton : MonoBehaviour
{
    public CraftingBox box;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            box.Craft();
        }
    }
}