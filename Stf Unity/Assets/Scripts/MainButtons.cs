using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MainButtons : MonoBehaviour
{
    public Button bucketUpgradeButton;
    public Button rainButton;
    public Button cloudButton;
    public Button collectButton;

    public Main main;
    
    void Awake()
    {
        main = FindObjectOfType<Main>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked()
    {
        main.drops += main.bucketUpgradePower;
    }

    public void BucketUpgradeClicked()
    {
        if (main.playerLevel >= main.playerLevelAtWhichBucketUnlocks && main.totalPowerUpsUpgradedInLevel < main.playerLevel - 1)
        {
            main.bucketUpgradePowerUpLevel++;
            main.totalPowerUpsUpgradedInLevel++;
            main.bucketUpgradePower += 1;
        }
    }

    public void RainClicked()
    {
        if (main.playerLevel >= main.playerLevelAtWhichRainUnlocks && main.totalPowerUpsUpgradedInLevel < main.playerLevel - 1)
        {
            if (!main.isRainActive)
            {
                main.rainPowerUpLevel++;
                main.totalPowerUpsUpgradedInLevel++;
                main.rainPower += 5;
                main.isRainActive = false;
            }
        }
    }

    public void CloudClicked()
    {
        if (main.playerLevel >= main.playerLevelAtWhichCloudUnlocks && main.totalPowerUpsUpgradedInLevel < main.playerLevel - 1)
        {
            main.cloudDropsPowerUpLevel++;
            main.totalPowerUpsUpgradedInLevel++;
            
            main.AdjustCloudDropsPowerUp();
        }
    }

    
    // For Cloud Drop functionality
    public void CollectFromCloud()
    {
        // Collect all drops in the cloud
        main.drops += main.cloudDrops;
        main.cloudDrops = 0; // Reset the drops in the cloud after collection
    }
}
