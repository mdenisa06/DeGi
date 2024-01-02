using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    // UI Text
    public TMP_Text levelUpBucketText;
    public TMP_Text levelUpRainText;
    public TMP_Text levelUpCloudText;
    public Text dropNumberText;
    public Text dropsPerSecondText;
    public Text bucketUpgradeText;
    public Text rainText;
    public Text cloudText;
    public Text collectText;
    public TMP_Text levelText;
    public Text LevelUpRequirement;

    // UI Buttons
    public Button bucketUpgradeButton;
    public Button rainButton;
    public Button cloudButton;
    public Button collectButton;

        // Game variables
    public double drops;
    public double rainPower; // dropsPerSeconds
    public double bucketUpgradePower; 

    // Cloud Drops variables
    public double cloudDrops;
    public int cloudDropLimit = 100; // Initial limit for drops in the cloud // I changed it from int to double
    public int cloudDropRate = 1;   // Initial rate at which drops are gathered per second // I changed it from int to double
    private double gatheringInterval = 1; // Time interval for gathering in seconds
    
    // Levels
    public int bucketUpgradePowerUpLevel = 0;
    public int rainPowerUpLevel = 0;
    public int cloudDropsPowerUpLevel = 0;
    public int totalPowerUpsUpgradedInLevel = 0;
    public int playerLevel = 1;
    public int initialDropsRequired = 15; // CHANGE HERE BACK TO 85 AFTER YOU ARE DONE TESTING
    public int levelIncreaseAmount = 20;
    public int dropsRequiredForLevelUp;

    // For collecting drops offline
    private bool hasCalculatedOfflineProgress = false; 
    private double timeSinceLastGathering; 
    private double lastOnlineTimestamp;
    
    // For faster changes in game
    public int playerLevelAtWhichBucketUnlocks = 2;
    public int playerLevelAtWhichRainUnlocks = 3;
    public int playerLevelAtWhichCloudUnlocks = 4;

    // Other
    public bool isRainActive = false;
    private Vector2 initialSwipePos;
    public CanvasGroup mainScene;
    public CanvasGroup levelUpUpgradesScene;
    public CanvasGroup dailyQuestsScene;

    public DailyQuests dailyQuests;

//__________________________________________________________________________________________________________________________________________
//__________________________________________________________________________________________________________________________________________

    public void CanvasGroupChanger(bool x, CanvasGroup y){
        y.alpha = x ? 1 : 0;
        y.interactable = x;
        y.blocksRaycasts = x;
    }

    public void ChangeTabs(string id){
        DisableAll();
        switch (id)
        {
            case "main":
                mainScene.gameObject.SetActive(true); // Activate it here first
                CanvasGroupChanger(true, mainScene);
                break;
            case "levelUp":
                levelUpUpgradesScene.gameObject.SetActive(true); // Same for others
                CanvasGroupChanger(true, levelUpUpgradesScene);
                break;
            case "DQ":
                dailyQuestsScene.gameObject.SetActive(true);
                CanvasGroupChanger(true, dailyQuestsScene);
                break;
        }

        void DisableAll()
        {
            mainScene.gameObject.SetActive(false);
            levelUpUpgradesScene.gameObject.SetActive(false);
            dailyQuestsScene.gameObject.SetActive(false);
        }
    }

//__________________________________________________________________________________________________________________________________________
//__________________________________________________________________________________________________________________________________________

    void Start()
    {   
        // Change scenes
        ChangeTabs("main");

        // Initialize in start
        initialDropsRequired = 15;    // CHANGE HERE BACK TO 85 AFTER YOU ARE DONE TESTING

        // For rain to add drops per second
        InvokeRepeating("IncrementDrops", 1.0f, 1.0f); // Calls IncrementDrops every 1 second.
        InvokeRepeating("Save", 1.0f, 1.0f); // Calls IncrementDrops every 1 second.

        //Other
        Load();
        UpdateUI();
        //dailyQuests.ActivateTapFastQuest();
    }

    public void Load()
    {
        drops = double.Parse(PlayerPrefs.GetString("drops","0"));
        rainPower = double.Parse(PlayerPrefs.GetString("rainPower","0"));
        bucketUpgradePower = double.Parse(PlayerPrefs.GetString("bucketUpgradePower","1"));
        playerLevel = PlayerPrefs.GetInt("playerLevel", 1);
        bucketUpgradePowerUpLevel = PlayerPrefs.GetInt("bucketUpgradePowerUpLevel", 0);
        rainPowerUpLevel = PlayerPrefs.GetInt("rainPowerUpLevel", 0);
        cloudDropsPowerUpLevel = PlayerPrefs.GetInt("cloudDropsPowerUpLevel", 0);
        totalPowerUpsUpgradedInLevel = PlayerPrefs.GetInt("totalPowerUpsUpgradedInLevel", 0);
        initialDropsRequired = PlayerPrefs.GetInt("initialDropsRequired", 15); // CHANGE HERE BACK TO 85 AFTER YOU ARE DONE TESTING
        cloudDrops = double.Parse(PlayerPrefs.GetString("cloudDrops", "0"));
        cloudDropLimit = int.Parse(PlayerPrefs.GetString("cloudDropLimit", "100"));
        cloudDropRate = int.Parse(PlayerPrefs.GetString("cloudDropRate", "1"));
        timeSinceLastGathering = double.Parse(PlayerPrefs.GetString("timeSinceLastGathering", "0"));
        //lastOnlineTimestamp = double.Parse(PlayerPrefs.GetString("lastOnlineTimestamp", "0"));  //load the last online timestamp
        // For collecting drops offline
        lastOnlineTimestamp = double.Parse(PlayerPrefs.GetString("lastOnlineTimestamp", GetTimestamp().ToString()));
        CalculateOfflineProgress();
    }

    public void Save()
    {
        PlayerPrefs.SetString("drops", drops.ToString());
        PlayerPrefs.SetString("rainPower", rainPower.ToString());
        PlayerPrefs.SetString("bucketUpgradePower", bucketUpgradePower.ToString());
        PlayerPrefs.SetInt("playerLevel", playerLevel);
        PlayerPrefs.SetInt("bucketUpgradePowerUpLevel", bucketUpgradePowerUpLevel);
        PlayerPrefs.SetInt("rainPowerUpLevel", rainPowerUpLevel);
        PlayerPrefs.SetInt("cloudDropsPowerUpLevel", cloudDropsPowerUpLevel);
        PlayerPrefs.SetInt("totalPowerUpsUpgradedInLevel", totalPowerUpsUpgradedInLevel);
        PlayerPrefs.SetInt("initialDropsRequired", initialDropsRequired);
        PlayerPrefs.SetString("cloudDrops", cloudDrops.ToString());
        PlayerPrefs.SetString("cloudDropLimit", cloudDropLimit.ToString());
        PlayerPrefs.SetString("cloudDropRate", cloudDropRate.ToString());
        PlayerPrefs.SetString("timeSinceLastGathering", timeSinceLastGathering.ToString());
    }

    public void UpdateUI()
    {
        dropsRequiredForLevelUp = initialDropsRequired + (levelIncreaseAmount * (playerLevel - 1));
        //dropsRequiredForLevelUp = Mathf.RoundToInt(initialDropsRequired * Mathf.Pow(levelIncreaseAmount, playerLevel - 1));
        dropNumberText.text = " " + Math.Floor(drops);
        dropsPerSecondText.text = rainPower + "/sec";

        if(bucketUpgradePowerUpLevel > 0) bucketUpgradeText.text = "Bucket Upgrade\n" + bucketUpgradePower + " / tap" + "\n Level: " + bucketUpgradePowerUpLevel;
        else bucketUpgradeText.text = "Bucket Upgrade\n" + "Level: " + bucketUpgradePowerUpLevel;

        if(rainPowerUpLevel > 0) rainText.text = "Rain\n" + rainPower + " / sec" + "\n Level: " + rainPowerUpLevel;
        else rainText.text = "Rain\n" + "Level: " + rainPowerUpLevel;

        if(cloudDropsPowerUpLevel > 0) cloudText.text = "Cloud Drops" + "\nLimit: " + cloudDropLimit  + "\nRate: " + cloudDropRate  +"\n Level: " + cloudDropsPowerUpLevel;
        else cloudText.text = "Cloud Drops\n" + "Level: " + cloudDropsPowerUpLevel;

        //collectText.text = "Collect:\n" + cloudDrops;
        collectText.text = "Collect from Cloud:\n" + Math.Floor(cloudDrops).ToString();
        levelText.text = "Lv " + playerLevel; // Update the level text
        LevelUpRequirement.text = "FIRE! FILL UNTIL\n" + dropsRequiredForLevelUp;

        // Check power-up levels and update button interactability
        //bucketUpgradeButton.interactable = (bucketUpgradePlayerLevelUnlock && bucketUnlock);
        //rainButton.interactable = (rainPlayerLevelUnlock && rainUnlock);
        //cloudButton.interactable = (cloudPlayerLevelUnlock && cloudUnlock);

    }

    public void OptionsUI()
    {

        // Update the bucket text based on the level and unlock status
        if(bucketUpgradePowerUpLevel > 0)
        {
            levelUpBucketText.text = "Upgrade Bucket";
        }
        else if(playerLevel >= playerLevelAtWhichBucketUnlocks)
        {
            levelUpBucketText.text = "Unlock Bucket";
        }

        // Update the rain text based on the level and unlock status
        if(rainPowerUpLevel > 0)
        {
            levelUpRainText.text = "Upgrade Rain";
        }
        else if(playerLevel >= playerLevelAtWhichRainUnlocks)
        {
            levelUpRainText.text = "Unlock Rain";
        }
        else
        {
            levelUpRainText.text = "Unlock Rain at level " +playerLevelAtWhichRainUnlocks;
        }

        // Update the cloud text based on the level and unlock status
        if(cloudDropsPowerUpLevel > 0)
        {
            levelUpCloudText.text = "Upgrade Cloud";
        }
        else if(playerLevel >= playerLevelAtWhichCloudUnlocks)
        {
            levelUpCloudText.text = "Unlock Cloud";
        }
        else
        {
            levelUpCloudText.text = "Unlock Cloud at level " + playerLevelAtWhichCloudUnlocks;
        }

    }

    void Update()
    {
        // Calculate offline progress
        //CalculateOfflineProgress();
        OptionsUI();
        UpdateUI();

        if (Input.GetMouseButtonDown(0))
        {
            initialSwipePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 swipeDelta = initialSwipePos - (Vector2)Input.mousePosition;

            // Check if the swipe is downwards and has a minimum distance
            if (swipeDelta.y > 50 && Mathf.Abs(swipeDelta.x) < 50)
            {
                TryLevelUp();
            }
        }

        if (playerLevel >= playerLevelAtWhichCloudUnlocks && cloudDropsPowerUpLevel >= 1)
        {
            // Update the time since the last gathering
            timeSinceLastGathering += Time.deltaTime;

            // Check if it's time to gather drops
            if (timeSinceLastGathering >= gatheringInterval)
            {
                GatherDrops();
                timeSinceLastGathering = 0; // Reset the timer
            }
        }
    }

//__________________________________________________________________________________________________________________________________________
//__________________________________________________________________________________________________________________________________________

    void TryLevelUp()
    {
        if (drops >= dropsRequiredForLevelUp)
        {
            drops -= dropsRequiredForLevelUp;
            playerLevel++;

            UpdateUI(); // Update the UI after leveling up
            dailyQuests.OnPlayerLeveledUp(); // First quest: Put out fire x3
            StartCoroutine(LevelUpAnimation());
        }
        // You can add an else statement here for additional feedback if the player doesn't have enough drops
    }

    IEnumerator LevelUpAnimation()
    {
        SceneManager.LoadScene("LevelUpAnimation", LoadSceneMode.Additive); // Load the animation scene
        yield return new WaitForSeconds(1.55f); // Wait for a few seconds
        SceneManager.UnloadSceneAsync("LevelUpAnimation"); // Unload the animation scene
        ChangeTabs("levelUp");
    }

//__________________________________________________________________________________________________________________________________________
//__________________________________________________________________________________________________________________________________________

    // For collecting drops offline
    void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus) // Game is being paused
        {
            SaveLastOnlineTimestamp();
        }
    }

    void OnApplicationQuit()
    {
        SaveLastOnlineTimestamp();
    }

    void SaveLastOnlineTimestamp()
    {
        double timestamp = GetTimestamp();
        PlayerPrefs.SetString("lastOnlineTimestamp", timestamp.ToString());
        PlayerPrefs.Save();
    }

    void CalculateOfflineProgress()
    {
        if (!hasCalculatedOfflineProgress && lastOnlineTimestamp > 0 && playerLevel >= playerLevelAtWhichCloudUnlocks && cloudDropsPowerUpLevel > 0)
        {
            double currentTime = GetTimestamp();
            double offlineDuration = currentTime - lastOnlineTimestamp;
            double offlineGatheredDrops = cloudDropRate * offlineDuration; 
            double availableSpace = cloudDropLimit - cloudDrops;
            double dropsToAdd = Math.Min(offlineGatheredDrops, availableSpace);
            cloudDrops += Math.Floor(dropsToAdd); // Round the number
            SaveLastOnlineTimestamp(); // Update the last online timestamp to the current time
            hasCalculatedOfflineProgress = true; // Set the flag so this doesn't run again
        } 
    }

    double GetTimestamp()
    {
        return (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }

//__________________________________________________________________________________________________________________________________________
//__________________________________________________________________________________________________________________________________________
    // Buttons

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll(); // This clears all PlayerPrefs data.
        
        drops = 0;
        rainPower = 0;
        bucketUpgradePower = 1;

        initialDropsRequired = 15; // CHANGE HERE BACK TO 85 AFTER YOU ARE DONE TESTING
        playerLevel = 1;
        bucketUpgradePowerUpLevel = 0;
        rainPowerUpLevel = 0;
        cloudDropsPowerUpLevel = 0;
        totalPowerUpsUpgradedInLevel = 0;

        cloudDrops = 0;
        cloudDropLimit = 100;
        cloudDropRate = 1;
        timeSinceLastGathering = 0;

        Save();
        AdjustCloudDropsPowerUp();
       
    }
    // For the main bucket button

    public void Clicked()
    {
        drops += bucketUpgradePower;
        dailyQuests.OnPlayerTapped(); // Second quest: Tap 50 times.
    }

    // For rain mechanism
    private void IncrementDrops()
    {
        drops += rainPower;
    }

    // Power-up buttons
    public void BucketUpgradeClicked()
    {
        if (playerLevel >= playerLevelAtWhichBucketUnlocks && totalPowerUpsUpgradedInLevel < playerLevel - 1)
        {
            bucketUpgradePowerUpLevel++;
            totalPowerUpsUpgradedInLevel++;
            bucketUpgradePower += 1;
        }
    }

    public void RainClicked()
    {
        if (playerLevel >= playerLevelAtWhichRainUnlocks && totalPowerUpsUpgradedInLevel < playerLevel - 1)
        {
            if (!isRainActive)
            {
                rainPowerUpLevel++;
                totalPowerUpsUpgradedInLevel++;
                rainPower += 5;
                isRainActive = false;
            }
        }
    }

    public void CloudClicked()
    {
        if (playerLevel >= playerLevelAtWhichCloudUnlocks && totalPowerUpsUpgradedInLevel < playerLevel - 1)
        {
            cloudDropsPowerUpLevel++;
            totalPowerUpsUpgradedInLevel++;
            
            AdjustCloudDropsPowerUp();
        }
    }

    
    // For Cloud Drop functionality
    public void CollectFromCloud()
    {
        // Collect all drops in the cloud
        drops += cloudDrops;
        cloudDrops = 0; // Reset the drops in the cloud after collection
    }

    void GatherDrops()
    {
        //Debug.Log("Gathering drops. Power-up level: " + cloudDropsPowerUpLevel);
        if (cloudDropsPowerUpLevel > 0)
        {
            double gatheredDrops = cloudDropRate * gatheringInterval;
            cloudDrops = Math.Min(cloudDrops + gatheredDrops, cloudDropLimit);
            //cloudDrops = Math.Floor(cloudDrops);
        }

    }

    void AdjustCloudDropsPowerUp()
    {
        // Define base values and growth factors
        double baseLimit = 100; // Initial limit   // I changed it from int to double
        double baseRate = 1;    // Initial rate    // I changed it from int to double
        double growthFactor = 1.5; // Adjust as needed

        // Use the formula to calculate new values
        cloudDropLimit = (int)(baseLimit * Math.Pow(growthFactor, cloudDropsPowerUpLevel));
        cloudDropRate = (int)(baseRate * Math.Pow(growthFactor, cloudDropsPowerUpLevel));

        //cloudDropLimit = baseLimit * Math.Pow(growthFactor, cloudDropsPowerUpLevel);
        //cloudDropRate = baseRate * Math.Pow(growthFactor, cloudDropsPowerUpLevel);
    }

    // Tab changing buttons
    public void XButton(){
        ChangeTabs("main");
    }

    public void DQButton(){
            ChangeTabs("DQ");
        }
}
