using UnityEngine;
using UnityEngine.UI;

public class IdleGame : MonoBehaviour
{
    public Text dropNumberText;
    public Text dropsPerSecondText;
    public Text rainText;
    public Text bucketUpgradeText;

    public double drops;
    // what changes in the buttons' text
    public double rainPower; // dropsPerSeconds
    public double bucketUpgradePower; 
    private bool isRainActive = false;


    //public int upgradeLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        InvokeRepeating("IncrementDrops", 1.0f, 1.0f); // Calls IncrementDrops every 1 second.
        InvokeRepeating("Save", 1.0f, 1.0f); // Calls IncrementDrops every 1 second.
        Load();
    }

    public void Load()
    {
        drops = double.Parse(PlayerPrefs.GetString("drops","0"));
        rainPower = double.Parse(PlayerPrefs.GetString("rainPower","0"));
        bucketUpgradePower = double.Parse(PlayerPrefs.GetString("bucketUpgradePower","1"));
    }

    public void Save()
    {
        PlayerPrefs.SetString("drops", drops.ToString());
        PlayerPrefs.SetString("rainPower", rainPower.ToString());
        PlayerPrefs.SetString("bucketUpgradePower", bucketUpgradePower.ToString());
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll(); // This clears all PlayerPrefs data.
    }

    // Update is called once per frame
    void Update()
    {
        dropNumberText.text = " " + drops;
        dropsPerSecondText.text = rainPower + "/sec";
        rainText.text = "Rain\n" + rainPower + " / sec";
        bucketUpgradeText.text = "Bucket Upgrade\n" + bucketUpgradePower + " / tap";
        
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
            rainPower += 5;
            isRainActive = false;
        }
    }

    private void IncrementDrops()
    {
        drops += rainPower;
    }

    public void BucketUpgradeClicked()
    {
        bucketUpgradePower += 1;
    }

   
}
