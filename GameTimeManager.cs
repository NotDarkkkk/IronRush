using System.Collections;
using UnityEngine;
using TMPro;

public class GameTimeManager : MonoBehaviour
{
    [Header("Time Settings")]
    public float timeScale = 240f;
    public int startHour = 6;
    public int endHour = 20;

    [Header("Day Settings")]
    public int totalDays = 5;
    private int day = 1;

    [Header("Customer Spawning")]
    public float customerSpawnIntervalHours = 2f;
    private float nextCustomerSpawnTime;

    [Header("References")]
    public CustomerSpawner spawner;
    public PlayerWallet wallet;

    [Header("Materials")]
    public GameObject woodPrefab;
    public GameObject ironPrefab;

    [Header("Material Spawn Points")]
    public Transform[] woodSpawnPoints;
    public Transform[] ironSpawnPoints;

    [Header("UI")]
    public GameObject introTextUI;

    [Header("End Day UI")]
    public GameObject endDayPanel;
    public TextMeshProUGUI endDayText;
    public TextMeshProUGUI infoText;

    [Header("Ending UI")]
    public GameObject goodEndingUI;
    public GameObject badEndingUI;

    private float currentTime;
    private bool isEndingDay = false;

    public static GameTimeManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentTime = startHour * 60f;

        if (introTextUI != null)
        {
            introTextUI.SetActive(true);
            StartCoroutine(HideIntro());
        }

        OnNewDay();
    }

    void Update()
    {
        if (isEndingDay) return;

        currentTime += Time.deltaTime * timeScale;

        int hour = GetHour();

        if (hour >= endHour)
        {
            StartCoroutine(EndDayRoutine());
            return;
        }

        if (currentTime >= nextCustomerSpawnTime)
        {
            if (spawner.currentCustomer == null)
            {
                spawner.SpawnCustomer();
                nextCustomerSpawnTime = currentTime + (customerSpawnIntervalHours * 60f);
            }
        }
    }

    // ---------------- INTRO ----------------

    IEnumerator HideIntro()
    {
        yield return new WaitForSeconds(2.5f);

        if (introTextUI != null)
            introTextUI.SetActive(false);
    }

    // ---------------- TIME ----------------
    public string GetFormattedTime()
    {
        return $"{GetHour():00}:{GetMinute():00}";
    }
    public int GetHour()
    {
        return Mathf.FloorToInt(currentTime / 60f);
    }

    public int GetMinute()
    {
        return Mathf.FloorToInt(currentTime % 60f);
    }

    // ---------------- DAY FLOW ----------------

    void OnNewDay()
    {
        Debug.Log("Day " + day + " started!");

        nextCustomerSpawnTime = startHour * 60f;

        SpawnDailyMaterials();
    }

    IEnumerator EndDayRoutine()
    {
        isEndingDay = true;

        spawner.DespawnAllCustomers();

        if (endDayPanel != null)
            endDayPanel.SetActive(true);

        // ---------------- NOT FINAL DAY ----------------
        if (day < totalDays)
        {
            int daysLeft = totalDays - day;

            if (endDayText != null)
                endDayText.text = "Day " + day + " Complete";

            if (infoText != null)
                infoText.text = "Days left: " + daysLeft;

            yield return new WaitForSeconds(2.5f);

            endDayPanel.SetActive(false);

            day++;
            currentTime = startHour * 60f;

            isEndingDay = false;

            OnNewDay();
        }

        // ---------------- FINAL DAY ----------------
        else
        {
            int money = wallet.money;

            if (money >= 40)
            {
                if (goodEndingUI != null)
                    goodEndingUI.SetActive(true);

                Debug.Log("GOOD ENDING");
            }
            else
            {
                if (badEndingUI != null)
                    badEndingUI.SetActive(true);

                Debug.Log("BAD ENDING");
            }
        }
    }

    // ---------------- MATERIALS ----------------

    void SpawnDailyMaterials()
    {
        foreach (Transform point in woodSpawnPoints)
            Instantiate(woodPrefab, point.position, Quaternion.identity);

        foreach (Transform point in ironSpawnPoints)
            Instantiate(ironPrefab, point.position, Quaternion.identity);
    }
}