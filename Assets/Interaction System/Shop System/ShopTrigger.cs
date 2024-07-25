using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopUI;
    private bool isShopOpen = false;
    private bool isPlayerInRange = false;
    public float interactionRadius = 2.0f;
    private Transform playerTransform; // Reference to the player's transform

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Calculate the distance between this object and the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        isPlayerInRange = distanceToPlayer <= interactionRadius;

        if (Input.GetKeyDown(KeyCode.E) && isPlayerInRange && !isShopOpen)
        {
            ToggleShopUI(true);
        }
        else if ((Input.GetKeyDown(KeyCode.E) && isPlayerInRange && isShopOpen) || (!isPlayerInRange && isShopOpen))
        {
            ToggleShopUI(false);
        }
    }

    private void ToggleShopUI(bool open)
    {
        isShopOpen = open;
        shopUI.SetActive(open);

        if (open)
        {
            Debug.Log("Shop UI opened.");

            // Transfer necessary references or data to the shop UI here if needed
            var playerMoney = PlayerMoney.Instance;
            if (playerMoney != null)
            {
                playerMoney.walletTextShop = FindObjectOfType<ShopManager>().walletTextShop;
                playerMoney.UpdateMoneyText();
                Debug.Log("PlayerMoney Instance found and money text updated.");
            }
            else
            {
                Debug.LogError("PlayerMoney Instance not found when opening Shop UI.");
            }
        }
        else
        {
            Debug.Log("Shop UI closed.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}