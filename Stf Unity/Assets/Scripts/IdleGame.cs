using UnityEngine;
using UnityEngine.UI;

public class IdleGame : MonoBehaviour
{
    public Text dropNumberText;
    public Text dropsPerSecondText;
    public Text rainText;
    public Text bucketUpgradeText;
    public Text levelText;
    public Text LevelUpRequirement;


    public double drops;
    // what changes in the buttons' text
    public double rainPower; // dropsPerSeconds
    public double bucketUpgradePower; 
    private bool isRainActive = false;


    public int playerLevel = 1;
    public int dropsRequiredForLevelUp;
    private Vector2 initialSwipePos;
    
    public int initialDropsRequired = 85;
    public int levelIncreaseAmount = 20;
    
    void Start()
    {
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
    }

    public void Save()
    {
        PlayerPrefs.SetString("drops", drops.ToString());
        PlayerPrefs.SetString("rainPower", rainPower.ToString());
        PlayerPrefs.SetString("bucketUpgradePower", bucketUpgradePower.ToString());
        PlayerPrefs.SetInt("playerLevel", playerLevel);
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll(); // This clears all PlayerPrefs data.
    }

    void UpdateUI()
    {
        dropNumberText.text = " " + drops;
        dropsPerSecondText.text = rainPower + "/sec";
        rainText.text = "Rain\n" + rainPower + " / sec";
        bucketUpgradeText.text = "Bucket Upgrade\n" + bucketUpgradePower + " / tap";
        dropsRequiredForLevelUp = initialDropsRequired + (levelIncreaseAmount * (playerLevel - 1));
        levelText.text = "Lv " + playerLevel; // Update the level text
        LevelUpRequirement.text = "FIRE! FILL UNTIL\n" + dropsRequiredForLevelUp;

    }

    void Update()
    {
        //dropNumberText.text = " " + drops;
        //dropsPerSecondText.text = rainPower + "/sec";
        //rainText.text = "Rain\n" + rainPower + " / sec";
        //bucketUpgradeText.text = "Bucket Upgrade\n" + bucketUpgradePower + " / tap";

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
        
    }

    void TryLevelUp()
    {
        if (drops >= dropsRequiredForLevelUp)
        {
            drops -= dropsRequiredForLevelUp;
            playerLevel++;
            UpdateUI(); // Update the UI after leveling up
        }
        // You can add an else statement here for additional feedback if the player doesn't have enough drops
    }

    //Buttons
    public void Clicked()
    {
        drops += bucketUpgradePower;
    }

    public void RainClicked()
    {
        if (!isRainActive)
        {
            if (playerLevel >= 2)
            {
                rainPower += 5;
                isRainActive = false;
                if (playerLevel == 2) playerLevel = 3;
            }
        }
    }

    private void IncrementDrops()
    {
        drops += rainPower;
    }

    public void BucketUpgradeClicked()
    {
        if (playerLevel >= 1)
        {
            bucketUpgradePower += 1;
            if (playerLevel == 1) playerLevel = 2;
        }
    }

    public void CloudClicked()
    {
        if (playerLevel >= 3)
        {
            // Implement functionality for the third power-up
            // You can add specific upgrades or actions here
            if (playerLevel == 3) playerLevel = 4;
        }
    }

   
}
