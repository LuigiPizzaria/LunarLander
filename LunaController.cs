using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LunaController : MonoBehaviour {

    //moi
    [SerializeField]
    GameObject Bottom;
    //moi
    [SerializeField]
    GameObject Crawler;
    //moi
    [SerializeField]
    Text WinText;
    //moi
    [SerializeField]
    Text InfoText;
    //moi
    [SerializeField]
    Text SpeedText;
    //[SerializeField]
    //GameObject Trigger;

    //moi
    private int x = 0;

    //moi
    private int speedX = 0;
    private int speedY = 0;

    //moi
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float maxSpeed = 1;

    //moi
    private bool bewegen = false;
    private bool isTooFast = false;
    //ma
    private bool check;
    private bool play = true;
    //übung lb
    private float amount; //pvel
    //ma
    private bool psound = true;

    //rigidbody moi
    private Rigidbody rb;
    float pvel;

    private CountDownTextController countDown;
    //sound für rakete
    private FMOD.Studio.EventInstance rocket;
    //die velocity
    private FMOD.Studio.ParameterInstance rocketvel;
    //sound crash
    private FMOD.Studio.EventInstance hit;
    //fallen sound
    private FMOD.Studio.EventInstance sink;
    private FMOD.Studio.ParameterInstance svel;



    // Use this for initialization
    void Start () {
        x = Random.Range(-15, 15);
        transform.position = new Vector3(x, 10, 11);
        rb = GetComponent<Rigidbody>();
        Time.timeScale = 1;

        countDown = GameObject.FindObjectOfType<CountDownTextController>();

        rocket = FMODUnity.RuntimeManager.CreateInstance("event:/rocket");
        hit = FMODUnity.RuntimeManager.CreateInstance("event:/hit");
        sink = FMODUnity.RuntimeManager.CreateInstance("event:/sink");
    }

	
	// Update is called once per frame
	void Update () {

        //Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //pvel = rb.velocity.y * -1;
        amount = rb.velocity.magnitude;

        float vel = transform.position.y * -1;
        Debug.Log(vel);

        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");

        /*   if (Input.GetAxis("Vertical") > 0) EventManager.Particles() += ParticleController.FlameParticles();
           else EventManager.Particles() -= ParticleController.FlameParticles();
        */
        //if (Input.GetKeyDown(KeyCode.W)) moveUp();
        if (Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.W))
        {
            rocket.start();
            sink.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            rocket.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            sink.start();
        }
        //if (Input.GetKeyDown(KeyCode.S)) moveDown();
        //if (Input.GetKeyDown(KeyCode.D)) moveRight();
        //if (Input.GetKeyDown(KeyCode.A)) moveLeft();

        //sounds wie im tutorial
        if (rocket.getParameter("rocketvel", out rocketvel)!= FMOD.RESULT.OK)
        {
            Debug.LogError("velocity parameter not found on rocket event");
            return;
        }
        rocketvel.setValue(amount);

        if (sink.getParameter("svel", out svel) != FMOD.RESULT.OK)
        {
            Debug.LogError("fall parameter not found on falling event");
            return;
        }
        svel.setValue(vel);

        Vector3 velocity = new Vector3(moveH,moveV,0);

        if(!rb.IsSleeping() || bewegen) rb.AddForce(velocity * speed);

        //Debug.Log(rb.velocity.magnitude);
        //Debug.Log(pvel);

        if (rb.velocity.y < maxSpeed)
        {
            SpeedText.text = "Zu schnell um zu landen!";
            SpeedText.color = Color.red;
            
            //FMODUnity.RuntimeManager.PlayOneShot("event:/bad", new Vector3(0, 0, 0));
        }
        else
        {
            if (psound == true)
            {
                SpeedText.text = "Gute Landegeschwindigkeit!";
                SpeedText.color = Color.green;
            }
            isTooFast = false;
            //FMODUnity.RuntimeManager.PlayOneShot("event:/good", new Vector3(0, 0, 0));
           
        }

        if (isTooFast != check)
        {
            check = isTooFast;
        }
        if(isTooFast == false && psound == true)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/good", new Vector3(0, 0, 0));
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject == Bottom)
        {
            sink.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            LoseGame();
            Time.timeScale = 0;
        }
        if (isTooFast && collision.gameObject == Crawler) {
            sink.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            LoseGame();
            Time.timeScale = 0;
        }
        if (!isTooFast && collision.gameObject == Crawler) {
            sink.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            WinGame();
            Time.timeScale = 0;
        }
        
    }

    public void setBewegen(bool bewegen) {
        this.bewegen = bewegen;
    }

    void LoseGame() {

        //FMODUnity.RuntimeManager.PlayOneShot("event:/loose", new Vector3(0, 0, 0));
        WinText.text = "You lose!";
        InfoText.text = "Press 'R' to Restart. Press 'ESC' to leave Game.";
        SpeedText.text = "";

        if (hit.getParameter("rocketvel", out rocketvel) != FMOD.RESULT.OK)
        {
            Debug.LogError("velocity parameter not found on crash event");
            return;
        }
        rocketvel.setValue(amount);
        hit.start();
    }

    void WinGame() {

        //FMODUnity.RuntimeManager.PlayOneShot("event:/win", new Vector3(0, 0, 0));
        WinText.text = "Great Landing!";
        InfoText.text = "Press 'R' to Restart. Press 'ESC' to leave Game.";
        SpeedText.text = "";
        //FMODUnity.RuntimeManager.PlayOneShot("event:/wins", new Vector3(0, 0, 0));
        if (hit.getParameter("rocketvel", out rocketvel) != FMOD.RESULT.OK)
        {
            Debug.LogError("velocity parameter not found on crash event");
            return;
        }
        rocketvel.setValue(amount);
        hit.start();
    }
    


}
