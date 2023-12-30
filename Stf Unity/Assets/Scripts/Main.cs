using UnityEngine;
using System.Collections;
using System;

public class Main : MonoBehaviour
{
    //UI Text
    

    //UI Buttons
  

    //Game variables
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

    public bool isRainActive = false;
    private bool hasCalculatedOfflineProgress = false; // for collecting drops offline
    private double timeSinceLastGathering; // Time since the last gathering
    private double lastOnlineTimestamp;
    

    public int playerLevelAtWhichBucketUnlocks = 2;
    public int playerLevelAtWhichRainUnlocks = 3;
    public int playerLevelAtWhichCloudUnlocks = 4;
    public bool bucketUnlocked;
    public bool rainUnlocked;
    public bool cloudUnlocked;
/*
    private bool bucketUpgradePlayerLevelUnlock;
    private bool rainPlayerLevelUnlock;
    private bool cloudPlayerLevelUnlock;
   */ 
    // Other
    //public OfflineGatheringManager offlineGatheringManager;

   
    

    void Start()
    {   
        //PlayerPrefs.DeleteAll();

        initialDropsRequired = 15;    // CHANGE HERE BACK TO 85 AFTER YOU ARE DONE TESTING
        //Debug.Log("global initialDropsRequired: " + initialDropsRequired);
        InvokeRepeating("IncrementDrops", 1.0f, 1.0f); // Calls IncrementDrops every 1 second.
        InvokeRepeating("Save", 1.0f, 1.0f); // Calls IncrementDrops every 1 second.
        Load();
        //UpdateUI();

        bucketUnlocked = bucketUpgradePowerUpLevel > 0;
        rainUnlocked = rainPowerUpLevel > 0;
        cloudUnlocked = cloudDropsPowerUpLevel > 0;
/*
        bucketUpgradePlayerLevelUnlock = playerLevel >= 2;
        rainPlayerLevelUnlock = playerLevel >= 3;
        cloudPlayerLevelUnlock = playerLevel >= 4;
*/
        //for collecting drops offline
        CalculateOfflineProgress(); // Calculate any offline progress
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

        initialDropsRequired = PlayerPrefs.GetInt("initialDropsRequired", 15);
            // CHANGE HERE BACK TO 85 AFTER YOU ARE DONE TESTING

        cloudDrops = double.Parse(PlayerPrefs.GetString("cloudDrops", "0"));
        cloudDropLimit = int.Parse(PlayerPrefs.GetString("cloudDropLimit", "100"));
        cloudDropRate = int.Parse(PlayerPrefs.GetString("cloudDropRate", "1"));
        timeSinceLastGathering = double.Parse(PlayerPrefs.GetString("timeSinceLastGathering", "0"));
        
        //for collecting drops offline
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
//for collecting drops offline
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
        //Debug.Log("Saving last online timestamp: " + timestamp);
        PlayerPrefs.SetString("lastOnlineTimestamp", timestamp.ToString());
        PlayerPrefs.Save();
    }

    void CalculateOfflineProgress()
    {
        if (!hasCalculatedOfflineProgress && lastOnlineTimestamp > 0 && playerLevel >= 4 && cloudDropsPowerUpLevel >= 1)
        {
            double currentTime = GetTimestamp();
            //Debug.Log("Current time: " + currentTime);
            
            double offlineDuration = currentTime - lastOnlineTimestamp;
            //Debug.Log("Offline duration (in seconds): " + offlineDuration);
            
            double offlineGatheredDrops = cloudDropRate * offlineDuration; 
            //Debug.Log("Expected offline gathered drops: " + offlineGatheredDrops);

            double availableSpace = cloudDropLimit - cloudDrops;
            double dropsToAdd = Math.Min(offlineGatheredDrops, availableSpace);
            
            //Debug.Log("Drops to add (after considering available space): " + dropsToAdd);

            cloudDrops += dropsToAdd; 

            SaveLastOnlineTimestamp(); // Update the last online timestamp to the current time
            hasCalculatedOfflineProgress = true; // Set the flag so this doesn't run again
        }
    }



    double GetTimestamp()
    {
        return (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }


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
        cloudDropLimit = 0;
        cloudDropRate = 0;
        timeSinceLastGathering = 0;

        Save();
        AdjustCloudDropsPowerUp();
       
    }


    void Update()
    {
        // Calculate offline progress
        CalculateOfflineProgress();
        

        
        if (playerLevel >= playerLevelAtWhichCloudUnlocks && cloudUnlocked)
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
/*
    void ResetGatheringTimer()
    {
        timeSinceLastGathering = 0;
    }
*/
    

    // For the 2 easy power-up buttons
    

    private void IncrementDrops()
    {
        drops += rainPower;
    }

    // Power-up buttons
    

    void GatherDrops()
    {
        //Debug.Log("Gathering drops. Power-up level: " + cloudDropsPowerUpLevel);
        if (cloudUnlocked)
        {
            double gatheredDrops = cloudDropRate * gatheringInterval;
            cloudDrops = Math.Min(cloudDrops + gatheredDrops, cloudDropLimit);
            //cloudDrops = Math.Floor(cloudDrops);
        }

        
    }

    public void AdjustCloudDropsPowerUp()
    {
        // Define base values and growth factors
        double baseLimit = 100; // Initial limit   // I changed it from int to double
        double baseRate = 1;    // Initial rate    // I changed it from int to double
        double growthFactor = 1.5; // Adjust as needed

        // Use the formula to calculate new values
        cloudDropLimit = (int)(baseLimit * Math.Pow(growthFactor, cloudDropsPowerUpLevel));
        cloudDropRate = (int)(baseRate * Math.Pow(growthFactor, cloudDropsPowerUpLevel));  
    }

    

}
