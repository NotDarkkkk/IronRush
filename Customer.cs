using System.Collections;
using UnityEngine;
using TMPro;

public class Customer : MonoBehaviour
{
    public ItemType requestedItem;
    public TMP_Text bubbleText;

    [Header("State")]
    public bool hasBeenServed = false;
    private bool hasLeft = false;
    private bool isWarningShown = false;

    private float spawnTimeMinutes;

    [Header("Timing")]
    public float patienceLimitMinutes = 300f;
    public float warningTimeBeforeLeave = 30f;
    public float leaveDelay = 1.5f;

    void Update()
    {
        if (hasLeft) return;

        float currentTime =
            GameTimeManager.Instance.GetHour() * 60f +
            GameTimeManager.Instance.GetMinute();

        if (!hasBeenServed)
        {
            float timeWaiting = currentTime - spawnTimeMinutes;

            if (!isWarningShown &&
                timeWaiting >= patienceLimitMinutes - warningTimeBeforeLeave)
            {
                isWarningShown = true;
                ShowLeavingText();
            }

            if (timeWaiting >= patienceLimitMinutes)
            {
                Leave();
            }
        }
    }

    public void Setup(ItemType item)
    {
        requestedItem = item;

        spawnTimeMinutes =
            GameTimeManager.Instance.GetHour() * 60f +
            GameTimeManager.Instance.GetMinute();

        ShowWaitingText();
    }

    // ---------------- UI ----------------

    void ShowWaitingText()
    {
        if (bubbleText != null)
            bubbleText.text = "I want a " + requestedItem + "!";
    }

    void ShowCorrectText()
    {
        if (bubbleText != null)
            bubbleText.text = "Perfect! Thank you!";
    }

    void ShowWrongText()
    {
        if (bubbleText != null)
            bubbleText.text = "That's not what I wanted...";
    }

    void ShowLeavingText()
    {
        if (bubbleText != null)
            bubbleText.text = "I'm getting impatient...";
    }

    // ---------------- GAME LOGIC ----------------

    public void Serve(ItemType givenItem)
    {
        if (hasLeft) return;

        hasBeenServed = true;

        bool correct = givenItem == requestedItem;

        if (correct)
            ShowCorrectText();
        else
            ShowWrongText();

        GameTimeManager.Instance.spawner.ClearCustomer();

        StartCoroutine(LeaveAfterDelay());
    }

    IEnumerator LeaveAfterDelay()
    {
        yield return new WaitForSeconds(leaveDelay);
        Leave();
    }

    void Leave()
    {
        if (hasLeft) return;

        hasLeft = true;

        Debug.Log("Customer left");

        GameTimeManager.Instance.spawner.ClearCustomer();

        Destroy(gameObject);
    }
}