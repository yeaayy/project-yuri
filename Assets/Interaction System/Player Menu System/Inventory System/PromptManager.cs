using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PromptManager : MonoBehaviour
{
    public static PromptManager Instance { get; private set; }

    [System.Serializable]
    public class Prompt
    {
        public Sprite itemSprite;
        public string itemName;
        public int itemQuantity;
    }

    public GameObject promptPanelTemplate; // Reference to the template of PromptPanel
    public Transform promptContainer;      // Container to hold all prompts

    private readonly Queue<Prompt> promptQueue = new();
    private bool isShowing = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowPrompt(Sprite itemSprite, string itemName, int itemQuantity)
    {
        Prompt newPrompt = new()
        {
            itemSprite = itemSprite,
            itemName = itemName,
            itemQuantity = itemQuantity
        };

        promptQueue.Enqueue(newPrompt);
        if (!isShowing)
        {
            StartCoroutine(DisplayPrompts());
        }
    }

    private IEnumerator DisplayPrompts()
    {
        isShowing = true;

        // Get the template prompt panel
        GameObject promptPanel = promptPanelTemplate;

        List<GameObject> activePrompts = new(); // Keep track of active prompts

        while (promptQueue.Count > 0)
        {
            Prompt currentPrompt = promptQueue.Dequeue();

            // Clone the template prompt panel
            GameObject promptInstance = Instantiate(promptPanel, promptContainer);
            promptInstance.SetActive(true); // Ensure the instantiated panel is active

            Image promptImage = promptInstance.transform.Find("PromptImage").GetComponent<Image>();
            TextMeshProUGUI itemNameText = promptInstance.transform.Find("ItemNameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI itemQuantityText = promptInstance.transform.Find("ItemQuantityText").GetComponent<TextMeshProUGUI>();

            if (promptImage == null) Debug.LogError("PromptImage not found");
            if (itemNameText == null) Debug.LogError("ItemNameText not found");
            if (itemQuantityText == null) Debug.LogError("ItemQuantityText not found");

            promptImage.sprite = currentPrompt.itemSprite;
            itemNameText.text = currentPrompt.itemName;
            itemQuantityText.text = "x" + currentPrompt.itemQuantity.ToString();

            // Position the new prompt
            RectTransform rectTransform = promptInstance.GetComponent<RectTransform>();
            rectTransform.SetParent(promptContainer); // Set parent explicitly
            rectTransform.localScale = Vector3.one; // Reset scale to avoid scaling issues

            // Calculate vertical position
            float panelHeight = rectTransform.sizeDelta.y; // Height of the panel
            float spacing = 150f; // Vertical spacing between panels
            float posY = -activePrompts.Count * (panelHeight + spacing); // Calculate Y position

            rectTransform.anchoredPosition = new Vector2(0, posY);

            activePrompts.Add(promptInstance);

            yield return new WaitForSeconds(1.5f);
        }

        // Wait for a brief moment before fading out or destroying prompts
        yield return new WaitForSeconds(1.0f);

        // Fade out or destroy prompts from bottom to top
        foreach (var prompt in activePrompts)
        {
            StartCoroutine(FadeOutAndDestroy(prompt));
            yield return new WaitForSeconds(0.5f); // Adjust as needed for spacing between fades
        }

        isShowing = false;
    }

    private IEnumerator FadeOutAndDestroy(GameObject promptInstance)
    {
        if (!promptInstance.TryGetComponent(out CanvasGroup canvasGroup))
        {
            canvasGroup = promptInstance.AddComponent<CanvasGroup>();
        }

        float fadeDuration = 1.0f; // Duration of fade out
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(promptInstance);
    }
}