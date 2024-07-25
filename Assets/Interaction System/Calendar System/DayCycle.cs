using UnityEngine;
using TMPro;
using System;

public class DayCycle : MonoBehaviour
{
    public TMP_Text timeOfDayText;
    public TMP_Text dateText;
    public TMP_Text dayOfWeekText;

    private string timeOfDay = "Early Morning";
    private string[] timesOfDay = { "Early Morning", "Morning", "Afternoon", "Evening", "Night Time" };
    private int currentIndex = 0;

    private DateTime currentDate;
    private string[] daysOfWeek = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

    void Start()
    {
        if (timeOfDayText == null || dateText == null || dayOfWeekText == null)
        {
            Debug.LogError("DayCycle: One or more TMP_Text references are not assigned in the inspector.");
            return;
        }

        currentDate = DateTime.Today;
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AdvanceTime();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            EnterHouse();
        }
    }

    public void AdvanceTime()
    {
        currentIndex = (currentIndex + 1) % timesOfDay.Length;
        timeOfDay = timesOfDay[currentIndex];

        if (currentIndex == 0)
        {
            currentDate = currentDate.AddDays(1);
            UpdateUI();
        }

        UpdateUI();
    }

    public void EnterHouse()
    {
        timeOfDay = "Evening";
        currentIndex = Array.IndexOf(timesOfDay, timeOfDay);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timeOfDayText != null) timeOfDayText.text = timeOfDay;
        if (dateText != null) dateText.text = currentDate.ToString("M/d");
        if (dayOfWeekText != null) dayOfWeekText.text = daysOfWeek[(int)currentDate.DayOfWeek];
    }
}