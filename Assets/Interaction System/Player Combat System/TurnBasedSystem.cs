using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnBasedSystem : MonoBehaviour
{
    public PartyMemberUI[] partyMembers;
    public EnemyUI[] enemyUIs;
    public CommandsMenu commandsMenu;
    public AttackMessage attackMessage;
    public TurnOrderDisplay turnOrderDisplay;
    public TMP_Text enemyNameText;

    public Transform[] playerSpawnPoints;
    public Transform[] enemySpawnPoints;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    private List<Character> characters;
    private int currentTurnIndex = 0;
    private Character selectedEnemy;

    private void Start()
    {
        InitializeCharacters();
        SetRandomTurnOrder();
        StartTurn();
        attackMessage.HideMessage(); // Ensure it's hidden at start
        foreach (var enemyUI in enemyUIs)
        {
            enemyUI.Hide(); // Ensure all enemy UIs are hidden at start
        }
    }

    void InitializeCharacters()
    {
        characters = new List<Character>();

        if (playerPrefab == null || enemyPrefab == null)
        {
            Debug.LogError("Player or Enemy prefab is not assigned!");
            return;
        }

        // Initialize player characters
        for (int i = 0; i < partyMembers.Length; i++)
        {
            if (i >= playerSpawnPoints.Length)
            {
                Debug.LogWarning("More party members than player spawn points!");
                break;
            }

            Transform spawnPoint = playerSpawnPoints[i];
            spawnPoint.gameObject.SetActive(false);  // Deactivate placeholder

            GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            PartyMemberUI playerUI = partyMembers[i];
            PlayerCharacter playerCharacter = player.GetComponent<PlayerCharacter>();

            if (playerCharacter == null)
            {
                Debug.LogError($"Character component missing on player at index {i}");
                continue;
            }

            playerCharacter.Initialize(100, 100, 50, 50, true, playerUI.portraitImage.sprite, playerUI);
            characters.Add(playerCharacter);
        }

        // Set remaining player spawn points inactive
        for (int i = partyMembers.Length; i < playerSpawnPoints.Length; i++)
        {
            playerSpawnPoints[i].gameObject.SetActive(false);
        }

        // Initialize enemy characters
        for (int i = 0; i < enemyUIs.Length; i++)
        {
            if (i >= enemySpawnPoints.Length)
            {
                Debug.LogWarning("More enemy UIs than enemy spawn points!");
                break;
            }

            Transform spawnPoint = enemySpawnPoints[i];
            spawnPoint.gameObject.SetActive(false);  // Deactivate placeholder

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            EnemyUI enemyUI = enemyUIs[i];
            EnemyCharacter enemyCharacter = enemy.GetComponent<EnemyCharacter>();

            if (enemyCharacter == null)
            {
                Debug.LogError($"Character component missing on enemy at index {i}");
                continue;
            }

            enemyCharacter.Initialize(100, 100, 0, 0, false, null, enemyUI);
            characters.Add(enemyCharacter);
        }

        // Set remaining enemy spawn points inactive
        for (int i = enemyUIs.Length; i < enemySpawnPoints.Length; i++)
        {
            enemySpawnPoints[i].gameObject.SetActive(false);
        }
    }

    void SetRandomTurnOrder()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            int randomIndex = Random.Range(0, characters.Count);
            (characters[i], characters[randomIndex]) = (characters[randomIndex], characters[i]);
        }

        List<Sprite> turnOrder = new List<Sprite>();
        foreach (var character in characters)
        {
            turnOrder.Add(character.isPlayer ? character.portrait : null);
        }
        turnOrderDisplay.SetTurnOrder(turnOrder);
    }

    void StartTurn()
    {
        if (characters.Count == 0) return;

        Character currentCharacter = characters[currentTurnIndex];
        if (currentCharacter.isPlayer)
        {
            commandsMenu.ShowMenu();
        }
        else
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        Character target = GetRandomPlayerCharacter();
        if (target != null)
        {
            attackMessage.ShowMessage("Enemy attacks!");
            target.TakeDamage(20); // Example damage value
            Invoke(nameof(EndTurn), 2);
        }
    }

    public void OnAttackButton()
    {
        if (selectedEnemy != null)
        {
            attackMessage.ShowMessage("Player attacks!");
            selectedEnemy.TakeDamage(20); // Example damage value
            Invoke(nameof(EndTurn), 2);
        }
    }

    public void OnSkillButton()
    {
        attackMessage.ShowMessage("Player uses skill!");
        Invoke(nameof(EndTurn), 2);
    }

    public void OnGuardButton()
    {
        attackMessage.ShowMessage("Player guards!");
        Invoke(nameof(EndTurn), 2);
    }

    public void OnItemButton()
    {
        attackMessage.ShowMessage("Player uses item!");
        Invoke(nameof(EndTurn), 2);
    }

    public void OnEnemySelected(Character enemy)
    {
        selectedEnemy = enemy;
        enemyNameText.text = enemy.name;
        enemyNameText.gameObject.SetActive(true);
    }

    public void OnEnemyDeselected()
    {
        selectedEnemy = null;
        enemyNameText.gameObject.SetActive(false);
    }

    Character GetRandomPlayerCharacter()
    {
        List<Character> players = characters.FindAll(c => c.isPlayer && c.currentHP > 0);
        if (players.Count > 0)
        {
            return players[Random.Range(0, players.Count)];
        }
        return null;
    }

    Character GetRandomEnemyCharacter()
    {
        List<Character> enemies = characters.FindAll(c => !c.isPlayer && c.currentHP > 0);
        if (enemies.Count > 0)
        {
            return enemies[Random.Range(0, enemies.Count)];
        }
        return null;
    }

    public void EndTurn()
    {
        attackMessage.HideMessage();
        commandsMenu.HideMenu();
        currentTurnIndex = (currentTurnIndex + 1) % characters.Count;
        StartTurn();
    }

    // Method to dynamically add a player to the party
    public void AddPartyMember(PartyMemberUI newPartyMember)
    {
        if (characters.Count >= playerSpawnPoints.Length)
        {
            Debug.LogWarning("No available spawn points for new party member!");
            return;
        }

        int index = characters.Count; // Next available spawn point index
        playerSpawnPoints[index].gameObject.SetActive(true);
        GameObject player = Instantiate(playerPrefab, playerSpawnPoints[index].position, Quaternion.identity);
        PlayerCharacter playerCharacter = player.GetComponent<PlayerCharacter>();
        playerCharacter.Initialize(100, 100, 50, 50, true, newPartyMember.portraitImage.sprite, newPartyMember);
        characters.Add(playerCharacter);
        partyMembers = AppendToArray(partyMembers, newPartyMember);
    }

    // Method to dynamically add an enemy to the party
    public void AddEnemy(EnemyUI newEnemyUI)
    {
        if (characters.Count >= enemySpawnPoints.Length)
        {
            Debug.LogWarning("No available spawn points for new enemy!");
            return;
        }

        int index = characters.Count; // Next available spawn point index
        enemySpawnPoints[index].gameObject.SetActive(true);
        GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoints[index].position, Quaternion.identity);
        EnemyCharacter enemyCharacter = enemy.GetComponent<EnemyCharacter>();
        enemyCharacter.Initialize(100, 100, 0, 0, false, null, newEnemyUI);
        characters.Add(enemyCharacter);
        enemyUIs = AppendToArray(enemyUIs, newEnemyUI);
    }

    // Helper method to append an item to an array
    private T[] AppendToArray<T>(T[] originalArray, T newItem)
    {
        T[] newArray = new T[originalArray.Length + 1];
        System.Array.Copy(originalArray, newArray, originalArray.Length);
        newArray[newArray.Length - 1] = newItem;
        return newArray;
    }
}