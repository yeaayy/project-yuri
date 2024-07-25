using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public class SaveData
{
    public float playerPositionX;
    public float playerPositionY;
    public float playerPositionZ;
    public string timestamp;
}

public class SaveLoadManager : MonoBehaviour
{
    public PlayerController player;
    public InventoryManager inventoryManager;
    public GameObject saveGameUI;
    public GameObject loadGameUI;
    public GameObject[] slotPanels;
    public TextMeshProUGUI[] saveTimestampTexts;
    public TextMeshProUGUI[] saveSlotNumberTexts;
    public TextMeshProUGUI[] loadTimestampTexts;
    public TextMeshProUGUI[] loadSlotNumberTexts;
    public GameObject saveConfirmationDialog;
    public TextMeshProUGUI saveConfirmationText;
    public Button saveConfirmButton;
    public Button saveCancelButton;
    public GameObject loadConfirmationDialog;
    public TextMeshProUGUI loadConfirmationText;
    public Button loadConfirmButton;
    public Button loadCancelButton;
    public KeyCode saveToggleKey = KeyCode.H;
    public KeyCode loadToggleKey = KeyCode.J;
    public int totalSlots = 3;

    private void Start()
    {
        UpdateSaveSlotDisplay();
        UpdateLoadSlotDisplay();
        SetupSlotPanels();
        saveGameUI.SetActive(false);
        loadGameUI.SetActive(false);
        saveConfirmationDialog.SetActive(false);
        loadConfirmationDialog.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(saveToggleKey))
        {
            saveGameUI.SetActive(!saveGameUI.activeSelf);
            if (saveGameUI.activeSelf)
            {
                loadGameUI.SetActive(false);
                Time.timeScale = 0f; // Freeze time
            }
            else
            {
                Time.timeScale = 1f; // Resume time
            }
        }

        if (Input.GetKeyDown(loadToggleKey))
        {
            loadGameUI.SetActive(!loadGameUI.activeSelf);
            if (loadGameUI.activeSelf)
            {
                saveGameUI.SetActive(false);
                UpdateLoadSlotDisplay();
                Time.timeScale = 0f; // Freeze time
            }
            else
            {
                Time.timeScale = 1f; // Resume time
            }
        }
    }

    private void SetupSlotPanels()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            int slotNumber = i;
            if (!slotPanels[i].TryGetComponent(out EventTrigger trigger))
            {
                trigger = slotPanels[i].AddComponent<EventTrigger>();
            }
            AddEventTrigger(trigger, EventTriggerType.PointerClick, () => OnSlotSelected(slotNumber));
            saveSlotNumberTexts[i].text = "Save Slot " + (i + 1);
            loadSlotNumberTexts[i].text = "Load Slot " + (i + 1);
        }
    }

    private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction action)
    {
        EventTrigger.Entry entry = new() { eventID = eventType };
        entry.callback.AddListener((eventData) => action());
        trigger.triggers.Add(entry);
    }

    public void OnSlotSelected(int slotNumber)
    {
        if (saveGameUI.activeSelf)
        {
            saveConfirmationText.text = File.Exists(GetSavePath(slotNumber)) ?
                "Save data already exists. Do you want to overwrite this data?" :
                "Do you want to save in this slot?";
            saveConfirmationDialog.SetActive(true);
            saveConfirmButton.onClick.RemoveAllListeners();
            saveConfirmButton.onClick.AddListener(() => SaveToSlot(slotNumber));
            saveCancelButton.onClick.RemoveAllListeners();
            saveCancelButton.onClick.AddListener(() => saveConfirmationDialog.SetActive(false));
        }
        else if (loadGameUI.activeSelf)
        {
            loadConfirmationDialog.SetActive(true);
            loadConfirmationText.text = "Do you want to load the game from this slot?";
            loadConfirmButton.onClick.RemoveAllListeners();
            loadConfirmButton.onClick.AddListener(() => ConfirmLoad(slotNumber));
            loadCancelButton.onClick.RemoveAllListeners();
            loadCancelButton.onClick.AddListener(() => loadConfirmationDialog.SetActive(false));
        }
    }

    public void SaveToSlot(int slotNumber)
    {
        Save(slotNumber);
        saveConfirmationDialog.SetActive(false);
        UpdateSaveSlotDisplay();
    }

    public void ConfirmLoad(int slotNumber)
    {
        Load(slotNumber);
        loadConfirmationDialog.SetActive(false);
    }

    private void UpdateSaveSlotDisplay()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            if (File.Exists(GetSavePath(i)))
            {
                saveSlotNumberTexts[i].text = "Save Slot " + (i + 1);
                saveTimestampTexts[i].text = File.GetLastWriteTime(GetSavePath(i)).ToString();
            }
            else
            {
                saveSlotNumberTexts[i].text = "Empty Slot";
                saveTimestampTexts[i].text = "";
            }
        }
    }

    private void UpdateLoadSlotDisplay()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            if (File.Exists(GetSavePath(i)))
            {
                loadSlotNumberTexts[i].text = "Save Slot " + (i + 1);
                loadTimestampTexts[i].text = File.GetLastWriteTime(GetSavePath(i)).ToString();
            }
            else
            {
                loadSlotNumberTexts[i].text = "Empty Slot";
                loadTimestampTexts[i].text = "";
            }
        }
    }

    private string GetSavePath(int slotNumber)
    {
        return Path.Combine(Application.persistentDataPath, $"save{slotNumber}.dat");
    }

    private void Save(int slotNumber)
    {
        var data = new SaveData
        {
            playerPositionX = player.transform.position.x,
            playerPositionY = player.transform.position.y,
            playerPositionZ = player.transform.position.z,
            timestamp = System.DateTime.Now.ToString()
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(GetSavePath(slotNumber), json);
    }

    private void Load(int slotNumber)
    {
        if (File.Exists(GetSavePath(slotNumber)))
        {
            string json = File.ReadAllText(GetSavePath(slotNumber));
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            Vector3 position = new(data.playerPositionX, data.playerPositionY, data.playerPositionZ);
            player.transform.position = position;
        }
    }
}