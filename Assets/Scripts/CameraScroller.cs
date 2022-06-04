using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroller : MonoBehaviour
{

    //configs
    [SerializeField] float scrollSpeed = 1f;
    float[] timeBetweenScrolls;
    float currentTimeRemaining;
    bool timerIsRunning = true;
    float smoothTime = 1f;

    Vector3 scrollVector;

    bool canScroll;
    //cycle trackers
    int timesStopped = 0;

    //cached refs
    [SerializeField] GameObject bumper;
    [SerializeField] GameObject loseCollider;
   // [SerializeField] GameObject heightDetector;
    [SerializeField] BoxCollider2D upperBarrier;
    [SerializeField] GameObject joystickOuterCircle;
    [SerializeField] GameObject joystickInnerCircle;
   
    Vector3 currentVelocityOfBumper;

 
    LevelController levelController;



    // Start is called before the first frame update
    void Start()
    {
        scrollVector = new Vector3(0f, scrollSpeed * Time.deltaTime, 0f);
        levelController = FindObjectOfType<LevelController>();
        timeBetweenScrolls = levelController.GetTimeBetweenScrolls();
        currentTimeRemaining = timeBetweenScrolls[timesStopped];
        canScroll = true;
        currentVelocityOfBumper = new Vector3(0, bumper.GetComponent<Rigidbody2D>().velocity.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Countdown();
        Scroll();
    }

    public void SetCanScroll(bool canScroll)
    {
        this.canScroll = canScroll;
    }

    void Countdown()
    {
        if (timerIsRunning)
        {
            if (currentTimeRemaining > 0)
            {
                currentTimeRemaining -= Time.deltaTime;
                canScroll = false;

            }
            else
            {
               // Debug.Log("Timer is at 0");
                currentTimeRemaining = 0;
                timerIsRunning = false;
                canScroll = true;
            }
        }
    }

    public float GetCurrentTimeRemaining()
    {
        return currentTimeRemaining;
    }

    private void Scroll()
    {
        if (canScroll)
        {
            transform.Translate(scrollVector);
            loseCollider.transform.Translate(scrollVector);
           // heightDetector.transform.Translate(scrollVector);
            upperBarrier.transform.Translate(scrollVector);
            joystickInnerCircle.transform.Translate(scrollVector);
            joystickOuterCircle.transform.Translate(scrollVector);


            
            if (!levelController.GetIsBumperStuck())
            {
                
                    bumper.transform.Translate(scrollVector);
                  
   
            }
            

        }
        else
        {
            while ((this.transform.position.y - bumper.transform.position.y) > 10)
            {

                Vector3 targetPosition = new Vector3(this.transform.position.x, this.transform.position.y - 10, this.transform.position.z);
                bumper.transform.Translate(Vector3.SmoothDamp(bumper.transform.position, targetPosition, ref currentVelocityOfBumper, smoothTime));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      //  Debug.Log("Camera Collider triggered");
        if (collision.gameObject.CompareTag("ScrollStopper"))
        {
            timesStopped++;
          //  Debug.Log("Has correct tag");
            canScroll = false;
            currentTimeRemaining = timeBetweenScrolls[timesStopped];
            timerIsRunning = true;
        }
    }


}
