using UnityEngine;
using UnityEngine.UI;

public class IdleGame : MonoBehaviour
{
    public Text dropNumber;
    public double drops;
    // Start is called before the first frame update
    void Start()
    {
        drops = 0;
    }

    // Update is called once per frame
    void Update()
    {
        dropNumber.text = " " + drops;
    }

    public void Clicked(){
        drops += 1;
    }
}
