using UnityEngine;
using UnityEngine.UI;

public class IdleGame : MonoBehaviour
{
    public Text dropNumberText;
    public double drops;

    public Text dropsPerSecondText;
    public Text rainText;
    public Text bucketUpgradeText;

    //public double dropsPerSecondRain; // for button Rain
    //public double bucketUpgradeDrop; // for button bucket update
    
    // what changes in the buttons' text
    public double rainPower; // dropsPerSeconds
    public double bucketUpgradePower; 

    //public int upgradeLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        drops = 0;
        //dropsPerSecondRain = 0;
        rainPower = 0;
        bucketUpgradePower = 1;


    }

    // Update is called once per frame
    void Update()
    {
        dropNumberText.text = " " + drops;
        dropsPerSecondText.text = rainPower + "/sec";
        rainText.text = "Rain\n" + rainPower + " / sec";
        bucketUpgradeText.text = "Bucket Upgrade\n" + bucketUpgradePower + " / tap";
        drops += rainPower * Time.deltaTime;
    }

    //Buttons
    public void Clicked(){
        drops += bucketUpgradePower;
    }

    public void RainClicked(){
        rainPower += 0.2;
    }

    public void BucketUpgradeClicked(){
        bucketUpgradePower += 1;
    }
}
