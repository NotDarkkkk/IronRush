using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{

    public TMP_Text canvasText;


    [SerializeField] public float Count = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        Count++;
        canvasText.text = Count.ToString();
    }
}
