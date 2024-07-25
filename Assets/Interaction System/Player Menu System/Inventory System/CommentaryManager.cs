using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommentaryManager : MonoBehaviour
{
    public static CommentaryManager Instance { get; private set; }

    [System.Serializable]
    public class Commentary
    {
        public string message;
        public Sprite icon;
    }

    public GameObject commentaryPanelTemplate; // Reference to the template of CommentaryPanel
    public Transform commentaryContainer;      // Container to hold all commentaries

    private readonly Queue<Commentary> commentaryQueue = new();
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

    public void ShowCommentary(string message, Sprite icon = null)
    {
        Commentary newCommentary = new()
        {
            message = message,
            icon = icon
        };

        commentaryQueue.Enqueue(newCommentary);
        if (!isShowing)
        {
            StartCoroutine(DisplayCommentaries());
        }
    }

    private IEnumerator DisplayCommentaries()
    {
        isShowing = true;

        // List to keep track of active commentaries
        List<GameObject> activeCommentaries = new();

        while (commentaryQueue.Count > 0)
        {
            Commentary currentCommentary = commentaryQueue.Dequeue();

            // Clone the template commentary panel
            GameObject commentaryInstance = Instantiate(commentaryPanelTemplate, commentaryContainer);
            commentaryInstance.SetActive(true); // Ensure the instantiated panel is active

            Image commentaryImage = commentaryInstance.transform.Find("CommenterSprite").GetComponent<Image>();
            TextMeshProUGUI messageText = commentaryInstance.transform.Find("CommenterMessage").GetComponent<TextMeshProUGUI>();

            if (commentaryImage == null) Debug.LogError("CommenterSprite not found");
            if (messageText == null) Debug.LogError("CommenterMessage not found");

            if (currentCommentary.icon != null)
            {
                commentaryImage.sprite = currentCommentary.icon;
                commentaryImage.gameObject.SetActive(true);
            }
            else
            {
                commentaryImage.gameObject.SetActive(false);
            }

            messageText.text = currentCommentary.message;

            // Position the new commentary
            RectTransform rectTransform = commentaryInstance.GetComponent<RectTransform>();
            rectTransform.SetParent(commentaryContainer); // Set parent explicitly
            rectTransform.localScale = Vector3.one; // Reset scale to avoid scaling issues

            // Calculate vertical position
            float panelHeight = rectTransform.sizeDelta.y; // Height of the panel
            float spacing = 150f; // Vertical spacing between panels
            float posY = -activeCommentaries.Count * (panelHeight + spacing); // Calculate Y position

            rectTransform.anchoredPosition = new Vector2(0, posY);

            activeCommentaries.Add(commentaryInstance);

            yield return new WaitForSeconds(2.0f); // Display time for each commentary
        }

        // Wait for a brief moment before fading out or destroying commentaries
        yield return new WaitForSeconds(1.0f);

        // Fade out or destroy commentaries from bottom to top
        foreach (var commentary in activeCommentaries)
        {
            StartCoroutine(FadeOutAndDestroy(commentary));
            yield return new WaitForSeconds(0.5f); // Adjust as needed for spacing between fades
        }

        isShowing = false;
    }

    private IEnumerator FadeOutAndDestroy(GameObject commentaryInstance)
    {
        if (!commentaryInstance.TryGetComponent(out CanvasGroup canvasGroup))
        {
            canvasGroup = commentaryInstance.AddComponent<CanvasGroup>();
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

        Destroy(commentaryInstance);
    }
}