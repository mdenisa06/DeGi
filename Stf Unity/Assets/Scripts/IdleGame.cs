using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class IdleGame : MonoBehaviour
{
    //Text
    public Text dropNumberText;
    public Text dropsPerSecondText;
    public Text bucketUpgradeText;
    public Text rainText;
    public Text cloudText;
    public Text collectText;
    public TMP_Text levelText;
    public Text LevelUpRequirement;

    // Buttons
    public Button bucketUpgradeButton;
    public Button rainButton;
    public Button cloudButton;
    public Button collectButton;

    public double drops;

    // Power-Ups
    public double rainPower; // dropsPerSeconds
    private bool isRainActive = false;
    public double bucketUpgradePower; 
    // Cloud Drops variables
    private double cloudDrops;
    private int cloudDropLimit = 100; // Initial limit for drops in the cloud // I changed it from int to double
    private int cloudDropRate = 1;   // Initial rate at which drops are gathered per second // I changed it from int to double
    private double timeSinceLastGathering; // Time since the last gathering
    private double gatheringInterval = 1; // Time interval for gathering in seconds
    private double lastOnlineTimestamp;

    // Levels
    public int bucketUpgradePowerUpLevel = 0;
    public int rainPowerUpLevel = 0;
    public int cloudDropsPowerUpLevel = 0;
    private int totalPowerUpsUpgradedInLevel = 0;
    public int playerLevel = 1;
    public int dropsRequiredForLevelUp;
    public int initialDropsRequired = 15; // CHANGE HERE BACK TO 85 AFTER YOU ARE DONE TESTING
    public int levelIncreaseAmount = 20;
    // Other
    public string levelUpSceneName = "LevelUpAnimation"; //name of animation scene
    private Vector2 initialSwipePos;

   

    void Start()
    {   
        //PlayerPrefs.DeleteAll();

        initialDropsRequired = 15;    // CHANGE HERE BACK TO 85 AFTER YOU ARE DONE TESTING
        //Debug.Log("global initialDropsRequired: " + initialDropsRequired);
        InvokeRepeating("IncrementDrops", 1.0f, 1.0f); // Calls IncrementDrops every 1 second.
        InvokeRepeating("Save", 1.0f, 1.0f); // Calls IncrementDrops every 1 second.
        Load();
        UpdateUI();
  
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
        
        //lastOnlineTimestamp = double.Parse(PlayerPrefs.GetString("lastOnlineTimestamp", "0"));  //load the last online timestamp

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

            // save the current timestamp
        //lastOnlineTimestamp = GetTimestamp();
        //PlayerPrefs.SetString("lastOnlineTimestamp", lastOnlineTimestamp.ToString());
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
        cloudDropLimit = 100;
        cloudDropRate = 1;
        timeSinceLastGathering = 0;

        Save();
        AdjustCloudDropsPowerUp();
       
    }

    void UpdateUI()
    {
        dropsRequiredForLevelUp = initialDropsRequired + (levelIncreaseAmount * (playerLevel - 1));
        //dropsRequiredForLevelUp = Mathf.RoundToInt(initialDropsRequired * Mathf.Pow(levelIncreaseAmount, playerLevel - 1));
        //Debug.Log("initialDropsRequired: " + initialDropsRequired + "\ndropsRequiredForLevelUp: " + dropsRequiredForLevelUp);

        dropNumberText.text = " " + drops;
        dropsPerSecondText.text = rainPower + "/sec";
        bucketUpgradeText.text = "Bucket Upgrade\n" + bucketUpgradePower + " / tap" + "\n Level: " + bucketUpgradePowerUpLevel;
        rainText.text = "Rain\n" + rainPower + " / sec" + "\n Level: " + rainPowerUpLevel;
        cloudText.text = "Cloud Drops" + "\nLimit: " + cloudDropLimit  + "\nRate: " + cloudDropRate  +"\n Level: " + cloudDropsPowerUpLevel;
        collectText.text = "Collect:\n" + cloudDrops;

        levelText.text = "Lv " + playerLevel; // Update the level text
        LevelUpRequirement.text = "FIRE! FILL UNTIL\n" + dropsRequiredForLevelUp;

        // Check power-up levels and update button interactability
        //bucketUpgradeButton.interactable = (playerLevel >= 2 && bucketUpgradePowerUpLevel >= 1);
        //rainButton.interactable = (playerLevel >= 3 && rainPowerUpLevel >= 1);
        //cloudButton.interactable = (playerLevel >= 4 && cloudDropsPowerUpLevel >= 1);

    }


    void Update()
    {
        // Calculate offline progress
        //CalculateOfflineProgress();

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
/*
        if (playerLevel >= 4 && cloudDropsPowerUpLevel >= 1)
        {
            //Debug.Log("Gathering drops...");
            GatherDrops();
            timeSinceLastGathering = 0; // Reset the timer
        }
        else
        {
            Debug.Log("Not gathering drops. Player Level: " + playerLevel + ", Cloud Power-Up Level: " + cloudDropsPowerUpLevel);
        }
*/
        if (playerLevel >= 4 && cloudDropsPowerUpLevel >= 1)
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
    void TryLevelUp()
    {
        if (drops >= dropsRequiredForLevelUp)
        {
            drops -= dropsRequiredForLevelUp;
            playerLevel++;

            UpdateUI(); // Update the UI after leveling up
            StartCoroutine(LevelUpAnimation());
        }
        // You can add an else statement here for additional feedback if the player doesn't have enough drops
    }

    IEnumerator LevelUpAnimation()
    {
        // Load the level-up scene
        SceneManager.LoadScene(levelUpSceneName, LoadSceneMode.Additive);

        // Wait for a few seconds (adjust this based on your animation length)
        yield return new WaitForSeconds(1.55f); // Example: 3 seconds

        // Unload the level-up scene
        SceneManager.UnloadSceneAsync(levelUpSceneName);

        // Load back the main scene (optional, if you want to return to the main scene)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // For the 2 easy power-up buttons
    public void Clicked()
    {
        drops += bucketUpgradePower;
    }

    private void IncrementDrops()
    {
        drops += rainPower;
    }

    // Power-up buttons
    public void BucketUpgradeClicked()
    {
        if (playerLevel >= 2 && totalPowerUpsUpgradedInLevel < playerLevel - 1)
        {
            bucketUpgradePowerUpLevel++;
            totalPowerUpsUpgradedInLevel++;
            bucketUpgradePower += 1;
        }
    }

    public void RainClicked()
    {
        if (playerLevel >= 3 && totalPowerUpsUpgradedInLevel < playerLevel - 1)
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
        if (playerLevel >= 4 && totalPowerUpsUpgradedInLevel < playerLevel - 1)
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
        if (cloudDropsPowerUpLevel >= 1)
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

    
/*
    // For offline Gathering
    void CalculateOfflineProgress()
    {
        if (lastOnlineTimestamp > 0 && playerLevel >= 4)
        {
            double offlineTime = GetTimestamp() - lastOnlineTimestamp;
            double offlineGatheredDrops = Math.Round(cloudDropRate * offlineTime);


            cloudDrops = Math.Min(cloudDrops + offlineGatheredDrops, cloudDropLimit);
            timeSinceLastGathering = 0; // Reset the timer

            // Update the last online timestamp after calculating offline progress
            lastOnlineTimestamp = GetTimestamp();
            PlayerPrefs.SetString("lastOnlineTimestamp", lastOnlineTimestamp.ToString());
        }
    }

    double GetTimestamp()
    {
        return (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }
   */
}
