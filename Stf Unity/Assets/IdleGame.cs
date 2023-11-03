using UnityEngine;
using UnityEngine.UI;

public class IdleGame : MonoBehaviour
{
    public Text dropNumberText;
    

    public Text dropsPerSecondText;
    public Text rainText;
    public Text bucketUpgradeText;

    //public double dropsPerSecondRain; // for button Rain
    //public double bucketUpgradeDrop; // for button bucket update
    public double drops;
    // what changes in the buttons' text
    public double rainPower; // dropsPerSeconds
    public double bucketUpgradePower; 
    private bool isRainActive = false;


    //public int upgradeLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        drops = 0;
        rainPower = 0;
        bucketUpgradePower = 1;
        InvokeRepeating("IncrementDrops", 1.0f, 1.0f); // Calls IncrementDrops every 1 second.

    }

    // Update is called once per frame
    void Update()
    {
        dropNumberText.text = " " + drops;
        dropsPerSecondText.text = rainPower + "/sec";
        rainText.text = "Rain\n" + rainPower + " / sec";
        bucketUpgradeText.text = "Bucket Upgrade\n" + bucketUpgradePower + " / tap";
        //drops += rainPower * Time.deltaTime;
        //InvokeRepeating("IncrementDrops", 10.0f, 100.0f); // Calls IncrementDrops every 1 second.
    }

    //Buttons
    public void Clicked(){
        drops += bucketUpgradePower;
    }

    public void RainClicked(){
        if (!isRainActive){
            rainPower += 5;
            isRainActive = true;
        }
    }

    private void IncrementDrops(){
        drops += rainPower;
    }

    public void BucketUpgradeClicked(){
        bucketUpgradePower += 1;
    }
}
