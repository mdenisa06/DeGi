using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{

    public Text dropNumberText;
    public Text dropsPerSecondText;
    public Text bucketUpgradeText;
    public Text rainText;
    public Text cloudText;
    public Text collectText;
    public TMP_Text levelText;
    public Text LevelUpRequirement;

    private Vector2 initialSwipePos;

    public Main main;
    void Awake()
    {
        main = FindObjectOfType<Main>();
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
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

    }

    void UpdateUI()
    {
        main.dropsRequiredForLevelUp = main.initialDropsRequired + (main.levelIncreaseAmount * (main.playerLevel - 1));
        //dropsRequiredForLevelUp = Mathf.RoundToInt(initialDropsRequired * Mathf.Pow(levelIncreaseAmount, playerLevel - 1));
        //Debug.Log("initialDropsRequired: " + initialDropsRequired + "\ndropsRequiredForLevelUp: " + dropsRequiredForLevelUp);

        dropNumberText.text = " " + Math.Floor(main.drops);
        dropsPerSecondText.text = main.rainPower + "/sec";
        bucketUpgradeText.text = "Bucket Upgrade\n" + main.bucketUpgradePower + " / tap" + "\n Level: " + main.bucketUpgradePowerUpLevel;
        rainText.text = "Rain\n" + main.rainPower + " / sec" + "\n Level: " + main.rainPowerUpLevel;
        cloudText.text = "Cloud Drops" + "\nLimit: " + main.cloudDropLimit  + "\nRate: " + main.cloudDropRate  +"\n Level: " + main.cloudDropsPowerUpLevel;
        //collectText.text = "Collect:\n" + cloudDrops;
        collectText.text = "Collect:\n" + Math.Floor(main.cloudDrops).ToString();


        levelText.text = "Lv " + main.playerLevel; // Update the level text
        LevelUpRequirement.text = "FIRE! FILL UNTIL\n" + main.dropsRequiredForLevelUp;

        // Check power-up levels and update button interactability
        //bucketUpgradeButton.interactable = (bucketUpgradePlayerLevelUnlock && bucketUnlock);
        //rainButton.interactable = (rainPlayerLevelUnlock && rainUnlock;
        //cloudButton.interactable = (cloudPlayerLevelUnlock && cloudUnlock);

    }

    void TryLevelUp()
    {
        if (main.drops >= main.dropsRequiredForLevelUp)
        {
            main.drops -= main.dropsRequiredForLevelUp;
            main.playerLevel++;

            UpdateUI(); // Update the UI after leveling up
            StartCoroutine(LevelUpAnimation());
        }
        // You can add an else statement here for additional feedback if the player doesn't have enough drops
    }

    IEnumerator LevelUpAnimation()
    {
        SceneManager.LoadScene("LevelUpAnimation", LoadSceneMode.Additive);
        yield return new WaitForSeconds(1.55f); 
        SceneManager.LoadScene("LevelUpUpgrade");

        //Load back the main scene (optional, if you want to return to the main scene)
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
