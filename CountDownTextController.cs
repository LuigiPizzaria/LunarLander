using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTextController : MonoBehaviour {

    [SerializeField]
    private Text CountDownText;

    //[SerializeField]
    //private Text SpeedText;

    [SerializeField]
    private GameObject lunaLander;

    private float countDownTime = 5;

   

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        countDownTime -= 1* Time.deltaTime;
        
        
        if (countDownTime >= 0)
        {
            lunaLander.GetComponent<Rigidbody>().Sleep();
            lunaLander.GetComponent<LunaController>().setBewegen(false);
            CountDownText.text = countDownTime.ToString("0");
            //SpeedText.text = "";
        }

        else
        {
            lunaLander.GetComponent<Rigidbody>().WakeUp();
            lunaLander.GetComponent<LunaController>().setBewegen(true);
            CountDownText.text = "";
        }
    }
    
}
