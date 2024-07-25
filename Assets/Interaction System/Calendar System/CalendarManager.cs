using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarManager : MonoBehaviour
{
    public static CalendarView calendarViewInstance;
    public GameObject calendarPanel;

    void Awake()
    {
        calendarViewInstance = FindObjectOfType<CalendarView>();
        calendarPanel.SetActive(false);
    }

    public void ShowCalendar()
    {
        calendarPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void HideCalendar()
    {
        calendarPanel.SetActive(false);
        Time.timeScale = 1;
    }
}