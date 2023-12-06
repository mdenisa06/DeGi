using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class IdleGame : MonoBehaviour
{
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

    public string levelUpSceneName = "LevelUpAnimation"; //name of animation scene

    public double drops;
    // what changes in the buttons' text
    public double rainPower; // dropsPerSeconds
    public double bucketUpgradePower; 

     // Cloud Drops variables
    private double cloudDrops;
    private double cloudDropLimit = 100; // Initial limit for drops in the cloud
    private double cloudDropRate = 1;   // Initial rate at which drops are gathered per second
    private double timeSinceLastGathering; // Time since the last gathering
    private double gatheringInterval = 1; // Time interval for gathering in seconds

    private bool isRainActive = false;



    public int bucketUpgradePowerUpLevel = 0;
    public int rainPowerUpLevel = 0;
    public int cloudDropsPowerUpLevel = 0;
    private int totalPowerUpsUpgradedInLevel = 0;
    public int playerLevel = 1;
    public int dropsRequiredForLevelUp;
    public int initialDropsRequired = 15; // CHANGE HERE BACK TO 85 AFTER YOU ARE DONE TESTING
    public int levelIncreaseAmount = 20;
    private Vector2 initialSwipePos;
    
    void Start()
    {   
        initialDropsRequired = 15;    // CHANGE HERE BACK TO 85 AFTER YOU ARE DONE TESTING
        //Debug.Log("global initialDropsRequired: " + initialDropsRequired);
        //PlayerPrefs.DeleteAll();
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

        UpdateUI();  // Update the UI after resetting player preferences
        //dropsRequiredForLevelUp = Mathf.RoundToInt(initialDropsRequired * Mathf.Pow(levelIncreaseAmount, playerLevel - 1));
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

        if (playerLevel >= 4)
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

    //Buttons
    public void Clicked()
    {
        drops += bucketUpgradePower;
    }

    private void IncrementDrops()
    {
        drops += rainPower;
    }


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
            // Implement functionality for the third power-up
            // You can add specific upgrades or actions here
            AdjustCloudDropsPowerUp();
        }
    }

    void GatherDrops()
    {
        double gatheredDrops = cloudDropRate * gatheringInterval;
        cloudDrops = Math.Min(cloudDrops + gatheredDrops, cloudDropLimit);
    }

    public void CollectFromCloud()
    {
        // Collect all drops in the cloud
        drops += cloudDrops;
        cloudDrops = 0; // Reset the drops in the cloud after collection
    }

    void AdjustCloudDropsPowerUp()
    {
        // Define base values and growth factors
        double baseLimit = 100; // Initial limit
        double baseRate = 1;    // Initial rate
        double growthFactor = 1.2; // Adjust as needed

        // Use the formula to calculate new values
        cloudDropLimit = baseLimit * Math.Pow(growthFactor, cloudDropsPowerUpLevel);
        cloudDropRate = baseRate * Math.Pow(growthFactor, cloudDropsPowerUpLevel);
    }

   
}
