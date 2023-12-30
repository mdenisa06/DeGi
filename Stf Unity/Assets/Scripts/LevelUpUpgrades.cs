using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUpUpgrades : MonoBehaviour
{
    public Main main;
    public TMP_Text level_up_bucket_text;
    public TMP_Text level_up_rain_text;
    public TMP_Text level_up_cloud_text;

    void Awake()
    {
        main = FindObjectOfType<Main>();
    }
    // Start is called before the first frame update
    void Start()
    {
        /*
        bool test1 = main.playerLevel >= main.playerLevelAtWhichBucketUnlocks;
        bool test2 = main.playerLevel >= main.playerLevelAtWhichRainUnlocks;
        bool test3 = main.playerLevel >= main.playerLevelAtWhichCloudUnlocks;

        Debug.Log("bucketUnlocked: " + test1 + "player level: " + main.playerLevel + "playerLevelAtWhichBucketUnlocks: " + main.playerLevelAtWhichBucketUnlocks);
        Debug.Log("rainUnlocked: " + test2 + "player level: " + main.playerLevel + "playerLevelAtWhichRainUnlocks: " + main.playerLevelAtWhichRainUnlocks);
        Debug.Log("cloudUnlocked: " + test3 + "player level: " + main.playerLevel + "playerLevelAtWhichCloudUnlocks: " +  main.playerLevelAtWhichCloudUnlocks);
    */
    }

    // Update is called once per frame
    void Update()
    {
        OptionsUI();
    }

    public void OptionsUI()
    {

        // Update the bucket text based on the level and unlock status
        if(main.bucketUnlocked)
        {
            level_up_bucket_text.text = "Upgrade Bucket";
        }
        else if(main.playerLevel >= main.playerLevelAtWhichBucketUnlocks)
        {
            level_up_bucket_text.text = "Unlock Bucket";
        }

        // Update the rain text based on the level and unlock status
        if(main.rainUnlocked)
        {
            level_up_rain_text.text = "Upgrade Rain";
        }
        else if(main.playerLevel >= main.playerLevelAtWhichRainUnlocks)
        {
            level_up_rain_text.text = "Unlock Rain";
        }
        else
        {
            level_up_rain_text.text = "Unlock Rain at level " + main.playerLevelAtWhichRainUnlocks;
        }

        // Update the cloud text based on the level and unlock status
        if(main.cloudUnlocked)
        {
            level_up_cloud_text.text = "Upgrade Cloud";
        }
        else if(main.playerLevel >= main.playerLevelAtWhichCloudUnlocks)
        {
            level_up_cloud_text.text = "Unlock Cloud";
        }
        else
        {
            level_up_cloud_text.text = "Unlock Cloud at level " + main.playerLevelAtWhichCloudUnlocks;
        }

    }

    public void LevelUpBucket(){

    }

    public void LevelUpRain(){
        
    }

    public void LevelUpCloud(){
        
    }

    public void XButton(){
        SceneManager.LoadScene("Main");
    }
}
