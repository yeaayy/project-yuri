using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class DailyLogManager : MonoBehaviour
{
    public GameObject dailyLogPanel;
    public TMP_Text eventDateText;
    public Transform jobContentParent;
    public Transform bondingContentParent;
    public GameObject logEntryPrefab;
    public GameObject jobDetailPanel;
    public TMP_Text jobTitleText;
    public TMP_Text jobDescriptionText;
    public TMP_Text jobTimePeriodText;
    public TMP_Text jobPayText;
    public TMP_Text jobLocationText;

    private GameObject activePanel;

    private void Start()
    {
        dailyLogPanel.SetActive(false);
        jobDetailPanel.SetActive(false);
        activePanel = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseActivePanel();
        }
    }

    public void ShowDailyLog(DateTime date)
    {
        eventDateText.text = date.ToString("MM/dd (ddd)");

        // Clear previous entries
        foreach (Transform child in jobContentParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in bondingContentParent)
        {
            Destroy(child.gameObject);
        }

        // Add sample entries for demonstration
        AddJobEntry("Chagall Cafe", 2500, "Serve coffee that boosts a person's charm!", "Night", "Paulownia Mall");
        AddBondingEntry("Bonding Event", "Spend time with a friend", "Afternoon", "Shibuya");

        ShowPanel(dailyLogPanel);
    }

    private void AddJobEntry(string jobTitle, int pay, string description, string timePeriod, string location)
    {
        if (logEntryPrefab == null)
        {
            Debug.LogError("Log Entry Prefab is not assigned.");
            return;
        }

        if (jobContentParent == null)
        {
            Debug.LogError("Job Content Parent is not assigned.");
            return;
        }

        GameObject logEntry = Instantiate(logEntryPrefab, jobContentParent);

        TMP_Text[] texts = logEntry.GetComponentsInChildren<TMP_Text>();
        if (texts.Length < 2)
        {
            Debug.LogError("Log Entry Prefab does not have the expected TMP_Text components.");
            return;
        }

        texts[0].text = jobTitle;
        texts[1].text = "Pay: " + pay;

        // Add EventTrigger for click
        if (!logEntry.TryGetComponent<EventTrigger>(out EventTrigger eventTrigger))
        {
            eventTrigger = logEntry.AddComponent<EventTrigger>();
        }
        AddEventTrigger(eventTrigger, EventTriggerType.PointerClick, () => ShowJobDetail(jobTitle, description, timePeriod, pay, location));
    }

    private void ShowJobDetail(string jobTitle, string description, string timePeriod, int pay, string location)
    {
        // Hide job entries container and bonding entries container
        jobContentParent.gameObject.SetActive(false);
        bondingContentParent.gameObject.SetActive(false);

        // Show job detail panel
        jobTitleText.text = "Workplace: " + jobTitle;
        jobDescriptionText.text = "Detail: " + description;
        jobTimePeriodText.text = "Time: " + timePeriod;
        jobPayText.text = "Pay: " + pay;
        jobLocationText.text = "Location: " + location;

        ShowPanel(jobDetailPanel);
    }

    private void AddBondingEntry(string eventName, string description, string timePeriod, string location)
    {
        if (logEntryPrefab == null)
        {
            Debug.LogError("Log Entry Prefab is not assigned.");
            return;
        }

        if (bondingContentParent == null)
        {
            Debug.LogError("Bonding Content Parent is not assigned.");
            return;
        }

        GameObject logEntry = Instantiate(logEntryPrefab, bondingContentParent);

        TMP_Text[] texts = logEntry.GetComponentsInChildren<TMP_Text>();
        if (texts.Length < 2)
        {
            Debug.LogError("Log Entry Prefab does not have the expected TMP_Text components.");
            return;
        }

        texts[0].text = eventName;
        texts[1].text = description;
    }

    private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction action)
    {
        var entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((eventData) => action());
        trigger.triggers.Add(entry);
    }

    private void ShowPanel(GameObject panel)
    {
        if (activePanel != null)
        {
            activePanel.SetActive(false);
        }

        panel.SetActive(true);
        activePanel = panel;
    }

    private void CloseActivePanel()
    {
        if (activePanel != null)
        {
            activePanel.SetActive(false);
            activePanel = null;
        }
    }
}