using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashReset : MonoBehaviour
{
    [SerializeField]
    UnityEvent dashReset;
    private GameObject dashTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        dashReset.Invoke();
        gameObject.SetActive(false);
        Invoke("SetTrue", 3.0f);
    }

    private void SetTrue()
    {
        gameObject.SetActive(true);
    }
 

}
