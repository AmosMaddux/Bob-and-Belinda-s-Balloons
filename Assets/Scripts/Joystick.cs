using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    //Cached Refs
   // [SerializeField] private Transform player;
    private bool touchStart = false;
    private Vector2 touchBeginning;
    private Vector2 touchEnding;
    //private Vector2 direction;
    private Vector2 climberDirection;
    private Vector2 bumperDirection;

    [SerializeField] private Transform innerCircle;
    [SerializeField] private Transform outerCircle;

    //Configs
    //[SerializeField] private float speed = 5.0f;
    [SerializeField] private float offsetClampMagnitude = 1.0f;
    float bumperPlayerInputFraction = 3f;
    float bumperPlayerInputBarrier;

    // Start is called before the first frame update
    void Start()
    {
        //this will create a maximum y pixel that the bumper input can be. If the input is below this, the input goes to the bumper
        // if the input is above this, the input goes to the climber.
        //but it also converts that pixel to a world point
        //bumperPlayerInputBarrier = Screen.height / bumperPlayerInputFraction;
        bumperPlayerInputBarrier = Camera.main.ScreenToWorldPoint(new Vector3(0f,
                                                                (Screen.height / bumperPlayerInputFraction),
                                                                 Camera.main.transform.position.z)).y;
        //Debug.Log("bumperPlayerInputBarrier is " + bumperPlayerInputBarrier);
    }

    // Update is called once per frame
    void Update()
    {
        bumperPlayerInputBarrier = Camera.main.ScreenToWorldPoint(new Vector3(0f,
                                                                (Screen.height / bumperPlayerInputFraction),
                                                                 Camera.main.transform.position.z)).y;
        touchTracker();
        touchMovement();




        // moveCharacter(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
    }

    void touchTracker()
    {

        if (Input.GetMouseButtonDown(0))
        {
            touchBeginning = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));

            //set the joysticks position to the touch
            innerCircle.transform.position = touchBeginning;
            outerCircle.transform.position = touchBeginning;

            //make the joystick visible when touch starts
            innerCircle.GetComponent<SpriteRenderer>().enabled = true;
            outerCircle.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (Input.GetMouseButton(0))
        {
            touchStart = true;
            touchEnding = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        }
        else
        {
            touchStart = false;
            //lets see if this changes how both climber and bumper move when only one section has input...success!!!
            climberDirection = Vector2.zero;
            bumperDirection = Vector2.zero;
        }

    }

    private void touchMovement()
    {
        if (touchStart)
        {
            //right now the inner circle is not scrolling correctly because it is locked to the touchBeginning vector by 
            // the clampmagnitude and offsetclampmagnitude. I am going to try to make it use the outer circle's
            //position as a reference, this shoyudl be fine initially because the outer circle
            //spawns where the touchbeginning is, but then moves with the scroller. IT WORKS!!!!!!!!

            if (touchBeginning.y <= bumperPlayerInputBarrier)
            {
                Vector2 offset = new Vector2((touchEnding.x - outerCircle.transform.position.x),
                                                (touchEnding.y - outerCircle.transform.position.y));// touchBeginning;
                bumperDirection = Vector2.ClampMagnitude(offset, 1.0f);
                innerCircle.transform.position = new Vector2(outerCircle.transform.position.x + (bumperDirection.x * offsetClampMagnitude),
                                                                            outerCircle.transform.position.y + (bumperDirection.y * offsetClampMagnitude));
               // Debug.Log("Log Condition 1: Barrier is at " + bumperPlayerInputBarrier + " and touchPosition is at " + touchBeginning.y);
            }
            if (touchBeginning.y > bumperPlayerInputBarrier)
            {
                Vector2 offset = new Vector2((touchEnding.x - outerCircle.transform.position.x),
                                               (touchEnding.y - outerCircle.transform.position.y));
                //Vector2 offset = touchEnding - touchBeginning;
                climberDirection = Vector2.ClampMagnitude(offset, 1.0f);
                innerCircle.transform.position = new Vector2(outerCircle.transform.position.x + (climberDirection.x * offsetClampMagnitude),
                                                                            outerCircle.transform.position.y + (climberDirection.y * offsetClampMagnitude));
           // Debug.Log("Log Condition 2: Barrier is at " + bumperPlayerInputBarrier + " and touchPosition is at " + touchBeginning.y);
            }
                                                                        

        }
        else
        {

            //make the joystick invisible when touch ends
            innerCircle.GetComponent<SpriteRenderer>().enabled = false;
            outerCircle.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    

    public Vector2 getBumperDirection()
    {
        if(touchStart)
        {
           // Debug.Log("Get Direction called. touchStart is true");
            return bumperDirection;
        }
        else
        {
           // Debug.Log("Get Direction called. touchStart is false");
            return new Vector2(0f, 0f);
        }
    }

    public Vector2 getClimberDirection()
    {
        if (touchStart)
        {
            // Debug.Log("Get Direction called. touchStart is true");
            return climberDirection;
        }
        else
        {
            // Debug.Log("Get Direction called. touchStart is false");
            return new Vector2(0f, 0f);
        }
    }

   
}
