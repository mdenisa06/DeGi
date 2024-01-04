using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;

public class DailyQuests : MonoBehaviour
{
    // UI
    
    public TMP_Text treesNumberText;/*
    public TMP_Text Quest1Text;
    public TMP_Text Quest2Text;
    public TMP_Text Quest3Text;
    public Text progressQuest1Text;
    public Text progressQuest2Text;
    public Text progressQuest3Text;
    */
    public TMP_Text[] questTexts; 
    public Text[] progressTexts; 
    private Dictionary<string, Quest> allQuests;
    private List<Quest> activeQuests = new List<Quest>();
    private const int ActiveQuestCount = 3;

    //private DateTime lastQuestCompletionTime;
    //private Quest currentQuest;
    private bool isCooldownActive;
    public int treesFromQuests;

    // First quest: Put out fire x3
    //private Quest putOutFireQuest;
    private int levelUpCount = 0;
    private bool isPutOutFireQuestActive = false;

    // Second quest: Tap 50 times.
    //private Quest tapQuest;
    private int tapCount = 0;
    private bool isTapQuestActive = false;

    // Third quest: Tap 15 times in 5 seconds.
    //private Quest tapFastQuest;
    private int tapFastCount = 0;
    private float tapFastTimer = 0f;
    private bool isTapFastQuestActive = false;

    // Quests 4 5 and 6:
    private bool isUpgradeBucketQuestActive = false;
    private bool isUpgradeRainQuestActive = false;
    private bool isUpgradeCloudQuestActive = false;
    //private Quest bucketPowerUpUpgradeQuest; // Quest 4: Unlock/Upgrade your bucket to the next level.
    //private Quest rainPowerUpUpgradeQuest; // Quest 5: Unlock/Upgrade your rain to the next level.
    //private Quest cloudPowerUpUpgradeQuest;  // Quest 6: Unlock/Upgrade your cloud to the next level.
    void Start()
    {
        InitializeAllQuests();
        PickRandomActiveQuests();
        UpdateQuestUI();

       
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
        //ActivateTapFastQuest();
        UpdateQuestUI();
        if (isTapFastQuestActive) // Third quest: Tap 15 times in 5 seconds.
        {
            tapFastTimer += Time.deltaTime;
            if (tapFastTimer > 5.0f)
            {
                // The quest failed to be completed within the time limit
                // Resetting the quest variables
                tapFastCount = 0;
                tapFastTimer = 0f;
                isTapFastQuestActive = false;
            }
        }
    }

    void UpdateQuestUI(){
        treesNumberText.text = treesFromQuests.ToString() + " Trees";
        for (int i = 0; i < activeQuests.Count; i++)
        {
            questTexts[i].text = activeQuests[i].Description;
            progressTexts[i].text = GetProgressForQuest(activeQuests[i]);
        }
    }

    private void InitializeAllQuests()
    {
        allQuests = new Dictionary<string, Quest>
        {
            {"Put Out Fire x3", new Quest{ Description = "Put Out Fire x3", Reward = 4 }},
            {"Tap 50 times", new Quest{ Description = "Tap 50 times", Reward = 1 }},
            {"Tap 15 times in 5 seconds", new Quest{ Description = "Tap 15 times in 5 seconds", Reward = 1 }},
            //{"Unlock/Upgrade your bucket to the next level", new Quest{ Description = "Unlock/Upgrade your bucket to the next level", Reward = 2}},
            //{"Unlock/Upgrade your rain to the next level", new Quest{ Description = "Unlock/Upgrade your rain to the next level", Reward = 2}},
            //{"Unlock/Upgrade your cloud to the next level", new Quest{ Description = "Unlock/Upgrade your cloud to the next level", Reward = 2}}
            
            // Add other quests here
        };
    }

    private void PickRandomActiveQuests()
    {
        List<Quest> questPool = new List<Quest>(allQuests.Values);
        for (int i = 0; i < ActiveQuestCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, questPool.Count);
            activeQuests.Add(questPool[randomIndex]);
            questPool.RemoveAt(randomIndex); // Remove the picked quest to avoid duplicates
            UpdateQuestActivationFlags();
        }
    }

    

    string GetProgressForQuest(Quest quest)
    {
        // Based on the quest, return a string representing the progress
        // For example:
        if (quest.Description == "Put Out Fire x3")
        {
            return levelUpCount + "/3";
        }
        else if (quest.Description == "Tap 50 times")
        {
            return tapCount + "/50";
        }
        else if(quest.Description == "Tap 15 times in 5 seconds"){
            return tapFastCount + "/15";
        }
        else if(quest.Description == "Unlock/Upgrade your bucket to the next level"){
            if(!quest.IsCompleted)
                return "0/1";
            else return "1/1";
        }
        else if(quest.Description == "Unlock/Upgrade your rain to the next level"){
            if(!quest.IsCompleted)
                return "0/1";
            else return "1/1";
        }
        else if(quest.Description == "Unlock/Upgrade your cloud to the next level"){
            if(!quest.IsCompleted)
                return "0/1";
            else return "1/1";
        }
        // ... handle other quests
        return "0/0"; // Default if quest not recognized
    }

    private void UpdateQuestActivationFlags()
    {
        
        isPutOutFireQuestActive = false;
        isTapQuestActive = false;
        isTapFastQuestActive = false;
        isUpgradeBucketQuestActive = false;
        isUpgradeRainQuestActive = false;
        isUpgradeCloudQuestActive = false;
        
        foreach (var quest in activeQuests)
        {
            switch (quest.Description)
            {
                case "Put Out Fire x3":
                    isPutOutFireQuestActive = true;
                    break;
                case "Tap 50 times":
                    isTapQuestActive = true;
                    break;
                case "Tap 15 times in 5 seconds":
                    isTapFastQuestActive = true;
                    break;
                case "Unlock/Upgrade your bucket to the next level":
                    isUpgradeBucketQuestActive = true;
                    break;
                case "Unlock/Upgrade your rain to the next level":
                    isUpgradeRainQuestActive = true;
                    break;
                case "Unlock/Upgrade your cloud to the next level":
                    isUpgradeCloudQuestActive = true;
                    break;
            }
        }
    }

    
//__________________________________________________________________________________________________________________________________________
//__________________________________________________________________________________________________________________________________________

    // Quests LOGIC

    public void OnPlayerLeveledUp() // First quest: Put out fire x3
    {
        var putOutFireQuest = activeQuests.Find(quest => quest.Description == "Put Out Fire x3");
        if (putOutFireQuest != null && !putOutFireQuest.IsCompleted && isPutOutFireQuestActive)
        {
            levelUpCount++;
            if (levelUpCount >= 3)
            {   
                CompleteQuest(putOutFireQuest); // isComplete = false;
                levelUpCount = 0; // Reset the counter for the "Put Out x3 Fires" quest
                Debug.Log("Trees earned from first quest: " + treesFromQuests);
                // Update UI  
            }
        }
    }

    public void OnPlayerTapped() // Second quest: Tap 50 times.
    {
        Quest questToCheck = activeQuests.Find(quest => quest.Description == "Tap 50 times");
        if (!questToCheck.IsCompleted && isTapQuestActive)
        {
            tapCount++;
            if (tapCount >= 50)
            {
                CompleteQuest(questToCheck);
                tapCount = 0; // Reset the counter for the "Tap 50 times" quest
                Debug.Log("Trees earned from second quest: " + treesFromQuests);
            }
        }
    }

    public void OnPlayerTappedFast() // Third quest: Tap 15 times in 5 seconds.
    {
        Quest questToCheck = activeQuests.Find(quest => quest.Description == "Tap 15 times in 5 seconds");
        if (!questToCheck.IsCompleted && isTapFastQuestActive)
        {
            tapFastCount++;
            if (tapFastCount == 1)
            {
                tapFastTimer = 0f; // Reset timer on first tap
            }
            if (tapFastCount >= 15)
            {
                CompleteQuest(questToCheck);
                tapFastCount = 0; // Reset the counter for the "Tap 15 times in 5 seconds" quest
                isTapFastQuestActive = false; // Deactivate the quest
                Debug.Log("Trees earned from third quest: " + treesFromQuests);
            }
        }
    }

/*
    public void ActivateTapFastQuest()
    {
        isTapFastQuestActive = true;
    }

*/


    public void OnBucketUpgraded() // Quest 4: Unlock/Upgrade your bucket to the next level.
    {
        Quest questToCheck = activeQuests.Find(quest => quest.Description == "Unlock/Upgrade your bucket to the next level");
        if (!questToCheck.IsCompleted && isUpgradeBucketQuestActive)
        {
            CompleteQuest(questToCheck);
            Debug.Log("Trees earned from 4th quest: " + treesFromQuests);
        }
    }

    public void OnRainUpgraded() // Quest 5: Unlock/Upgrade your rain to the next level.
    {
        Quest questToCheck = activeQuests.Find(quest => quest.Description == "Unlock/Upgrade your rain to the next level");
        if (!questToCheck.IsCompleted && isUpgradeRainQuestActive)
        {
            CompleteQuest(questToCheck);
            Debug.Log("Trees earned from 5th quest: " + treesFromQuests);
        }
    }

    public void OnCloudUpgraded() // Quest 6: Unlock/Upgrade your cloud to the next level.
    {
        Quest questToCheck = activeQuests.Find(quest => quest.Description == "Unlock/Upgrade your cloud to the next level");
        if (!questToCheck.IsCompleted && isUpgradeCloudQuestActive)
        {
            CompleteQuest(questToCheck);
            Debug.Log("Trees earned from 6th quest: " + treesFromQuests);
        }
    }

//__________________________________________________________________________________________________________________________________________
//__________________________________________________________________________________________________________________________________________


    public void CompleteQuest(Quest quest)
    {
        if (activeQuests.Contains(quest) && !quest.IsCompleted)
        {
            quest.IsCompleted = true;
            treesFromQuests += quest.Reward; // Award the reward
            UpdateQuestUI();

            // Start cooldown
            StartCoroutine(CooldownQuest(quest));
        }
        else
        {
            Debug.Log("Attempted to complete a quest that is not active: " + quest.Description);
        }
/*
        // Give reward and start cooldown
        lastQuestCompletionTime = DateTime.Now;
        isCooldownActive = true;
        // Save quest state
        ResetQuest(quest);
*/
        // TESTING
        // Reactivate the quest for immediate retesting
        /*
        if (quest == tapFastQuest)
        {
            isTapFastQuestActive = true;
        }
        */
    }

    private IEnumerator CooldownQuest(Quest quest)
    {
        // Set a flag or disable quest interactions
        isCooldownActive = true;
        
        // Wait for cooldown period (8 hours here)
        yield return new WaitForSeconds(30); // 30 secs for testing, in rest: 8 * 3600

        // Reset the quest and pick a new random one
        ResetQuest(quest);
        ReplaceCompletedQuest(quest);
        UpdateQuestUI();
        
        // Remove the flag or enable quest interactions
        isCooldownActive = false;
    }

    private void ResetQuest(Quest quest)
    {
        quest.IsCompleted = false;
    }

    private void ReplaceCompletedQuest(Quest quest)
    {
        int index = activeQuests.IndexOf(quest);
        if (index != -1)
        {
            // Pick a new quest from the remaining ones in the pool
            List<Quest> remainingQuests = new List<Quest>(allQuests.Values);
            remainingQuests.RemoveAll(q => activeQuests.Contains(q)); // Remove already active quests
            if (remainingQuests.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, remainingQuests.Count);
                activeQuests[index] = remainingQuests[randomIndex];
                UpdateQuestActivationFlags();
            }
        }
    }

}

public class Quest
{
    public string Description;
    public bool IsCompleted;
    public int Reward;
    // Other quest-related properties and methods...
}

     /*
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
        bucketPowerUpUpgradeQuest = new Quest
        {
            Description = "Unlock/Upgrade your bucket to the next level",
            IsCompleted = false,
            Reward = 2 // Trees
        };
        rainPowerUpUpgradeQuest = new Quest
        {
            Description = "Unlock/Upgrade your rain to the next level",
            IsCompleted = false,
            Reward = 2 // Trees
        };
        cloudPowerUpUpgradeQuest = new Quest
        {
            Description = "Unlock/Upgrade your cloud to the next level",
            IsCompleted = false,
            Reward = 2 // Trees
        };*/

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