using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DailyQuests : MonoBehaviour
{
    // UI
    public TMP_Text treesNumberText;
    public TMP_Text Quest1Text;
    public TMP_Text Quest2Text;
    public TMP_Text Quest3Text;
    public Text progressQuest1Text;
    public Text progressQuest2Text;
    public Text progressQuest3Text;

    private DateTime lastQuestCompletionTime;
    private Quest currentQuest;
    private bool isCooldownActive;
    public int treesFromQuests;

    // First quest: Put out fire x3
    private Quest putOutFireQuest;
    private int levelUpCount = 0;

    // Second quest: Tap 50 times.
    private Quest tapQuest;
    private int tapCount = 0;

    // Third quest: Tap 15 times in 5 seconds.
    private Quest tapFastQuest;
    private int tapFastCount = 0;
    private float tapFastTimer = 0f;
    private bool isTapFastQuestActive = false;
    void Start()
    {
        ActivateTapFastQuest();
        putOutFireQuest = new Quest
        {
            Description = "Put Out Fire x3",
            IsCompleted = false,
            Reward = 4 // Trees
        };
        tapQuest = new Quest
        {
            Description = "Tap 50 times",
            IsCompleted = false,
            Reward = 1 // Tree
        };
        tapFastQuest = new Quest
        {
            Description = "Tap 15 times in 5 seconds",
            IsCompleted = false,
            Reward = 1 // Tree
        };
        
        //LoadQuestState(); // Load saved quest state
        //InitializeQuest(); // Initialize the current quest
    }

    void Update()
    {/*
        // Update quest progress and check for completion
        UpdateQuestProgress();

        // Check if cooldown is over
        if (isCooldownActive && (DateTime.Now - lastQuestCompletionTime).TotalHours >= 8)
        {
            isCooldownActive = false;
            InitializeNextQuest();
        }
        */
        if (isTapFastQuestActive)
        {
            tapFastTimer += Time.deltaTime;
            if (tapFastTimer > 5f)
            {
                // The quest failed to be completed within the time limit
                // Resetting the quest variables
                tapFastCount = 0;
                tapFastTimer = 0f;
                isTapFastQuestActive = false;
            }
        }
    }

    public void OnPlayerLeveledUp() // First quest: Put out fire x3
    {
        if (!putOutFireQuest.IsCompleted)
        {
            levelUpCount++;
            if (levelUpCount >= 3)
            {
                //putOutFireQuest.IsCompleted = true;
                //treesFromQuests = putOutFireQuest.Reward; // Award the reward
                
                CompleteQuest(putOutFireQuest); // isComplete = false;
                levelUpCount = 0; // Reset the counter for the "Put Out x3 Fires" quest
                Debug.Log("Trees earned from first quest: " + treesFromQuests);
                // Update UI  
            }
        }
    }

    public void OnPlayerTapped() // Second quest: Tap 50 times.
    {
        if (!tapQuest.IsCompleted)
        {
            tapCount++;
            if (tapCount >= 50)
            {
                CompleteQuest(tapQuest);
                tapCount = 0; // Reset the counter for the "Tap 50 times" quest
                Debug.Log("Trees earned from second quest: " + treesFromQuests);
            }
        }
    }

    public void OnPlayerTappedFast() // Third quest: Tap 15 times in 5 seconds.
    {
        if (!tapFastQuest.IsCompleted && isTapFastQuestActive)
        {
            tapFastCount++;
            if (tapFastCount == 1)
            {
                tapFastTimer = 0f; // Reset timer on first tap
            }
            if (tapFastCount >= 15)
            {
                CompleteQuest(tapFastQuest);
                tapFastCount = 0; // Reset the counter for the "Tap 15 times in 5 seconds" quest
                isTapFastQuestActive = false; // Deactivate the quest
                Debug.Log("Trees earned from third quest: " + treesFromQuests);
            }
        }
    }

    public void ActivateTapFastQuest()
    {
        isTapFastQuestActive = true;
    }

    public void CompleteQuest(Quest quest)
    {
        quest.IsCompleted = true;
        treesFromQuests = quest.Reward; // Award the reward

        // Give reward and start cooldown
        lastQuestCompletionTime = DateTime.Now;
        isCooldownActive = true;
        // Save quest state
        ResetQuest(quest);

        // TESTING
        // Reactivate the quest for immediate retesting
        if (quest == tapFastQuest)
        {
            isTapFastQuestActive = true;
        }
    }

    private void ResetQuest(Quest quest)
    {
        quest.IsCompleted = false;
    }
/*
    public void SkipQuest()
    {
        if (!isCooldownActive)
        {
            // Start cooldown
            lastQuestCompletionTime = DateTime.Now;
            isCooldownActive = true;

            // Initialize the next quest
            InitializeNextQuest();
        }
    }

    // Other methods for handling specific quests
    private void LoadQuestState()
    {
        // Load the last quest completion time and current quest state
    }

    private void InitializeQuest()
    {
        // Initialize the quest based on saved state or start a new quest
    }

    private void UpdateQuestProgress()
    {
        // Update the progress of the current quest
    }

    private void InitializeNextQuest()
    {
        // Initialize the next quest
    }
    */
}

public class Quest
{
    public string Description;
    public bool IsCompleted;
    public int Reward;
    // Other quest-related properties and methods...
}