using UnityEngine;
using TMPro;

public class ClockUI : MonoBehaviour
{
    public TextMeshProUGUI clockText;

    void Update()
    {
        if (GameTimeManager.Instance == null || clockText == null)
            return;

        int hour = GameTimeManager.Instance.GetHour();
        int minute = GameTimeManager.Instance.GetMinute();

        clockText.text = $"{hour:00}:{minute:00}";
    }
}