using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class CalendarView : MonoBehaviour
{
    public Button[] dayButtons;
    public TMP_Text monthYearText;
    public Button previousButton;
    public Button nextButton;

    private DateTime currentDate;
    private HashSet<DateTime> importantDates;

    public Action<DateTime> OnDateSelected;

    void Start()
    {
        currentDate = DateTime.Today;
        importantDates = new HashSet<DateTime>();
        UpdateCalendar();

        previousButton.onClick.AddListener(PreviousMonth);
        nextButton.onClick.AddListener(NextMonth);

        foreach (Button button in dayButtons)
        {
            button.onClick.AddListener(() => OnDayButtonClick(button));
        }
    }

    void UpdateCalendar()
    {
        monthYearText.text = currentDate.ToString("MMMM yyyy");

        DateTime firstDayOfMonth = new(currentDate.Year, currentDate.Month, 1);
        int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
        int startDayIndex = (int)firstDayOfMonth.DayOfWeek;

        for (int i = 0; i < dayButtons.Length; i++)
        {
            Button dayButton = dayButtons[i];
            int day = i - startDayIndex + 1;

            if (day >= 1 && day <= daysInMonth)
            {
                dayButton.GetComponentInChildren<TMP_Text>().text = day.ToString();
                dayButton.interactable = true;

                DateTime thisDate = firstDayOfMonth.AddDays(i - startDayIndex);
                if (thisDate <= DateTime.Today && !importantDates.Contains(thisDate.Date))
                {
                    dayButton.GetComponentInChildren<TMP_Text>().color = Color.black;
                }
                else if (importantDates.Contains(thisDate.Date))
                {
                    dayButton.GetComponentInChildren<TMP_Text>().color = Color.red;
                }
                else
                {
                    dayButton.interactable = false;
                }
            }
            else
            {
                dayButton.GetComponentInChildren<TMP_Text>().text = "";
                dayButton.interactable = false;
            }
        }
    }

    public void PreviousMonth()
    {
        currentDate = currentDate.AddMonths(-1);
        UpdateCalendar();
    }

    public void NextMonth()
    {
        currentDate = currentDate.AddMonths(1);
        UpdateCalendar();
    }

    public void SetDate(DateTime date)
    {
        currentDate = date;
        UpdateCalendar();
    }

    public void AddImportantDate(DateTime date)
    {
        importantDates.Add(date.Date);
        UpdateCalendar();
    }

    private void OnDayButtonClick(Button button)
    {
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if (buttonText == null || string.IsNullOrEmpty(buttonText.text))
        {
            return; // No valid day number, so return early
        }

        int day = int.Parse(buttonText.text);
        DateTime selectedDate = new(currentDate.Year, currentDate.Month, day);
        OnDateSelected?.Invoke(selectedDate);

        DailyLogManager logManager = FindObjectOfType<DailyLogManager>();
        if (logManager != null)
        {
            logManager.ShowDailyLog(selectedDate);
        }
        else
        {
            Debug.LogWarning("DailyLogManager not found in the scene.");
        }
    }
}