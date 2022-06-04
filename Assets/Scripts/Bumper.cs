using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{

    //cached refs
    [SerializeField] private Rigidbody2D myRigidbody;
    LevelController levelController;
    Vector3 mainCameraPosition;
    BoxCollider2D bottomOfBumper;
   

    //configs
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float heightCorrectionSpeed = 1f;
    [SerializeField] float minXPos;
    [SerializeField] float maxXPos;
    bool canCameraMakeBumperScroll = true;
    bool catchUpMovementEnabled = false;



    // Start is called before the first frame update
    void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        mainCameraPosition = FindObjectOfType<Camera>().transform.position;
        bottomOfBumper = GetComponent<BoxCollider2D>();
   
    }

    // Update is called once per frame
    void Update()
    {
       
      if (levelController.GetCanBumperMove())
        {
            Move();
        }
        //MoveToCorrectHeight(); //I should set a another collider to check this. Instead of having it compute all the time.

       // DetectHeight();
        
    }

    public void Move()
    {

       if (!catchUpMovementEnabled)
        {
            float xMove = GetComponent<Joystick>().getBumperDirection().x * Time.deltaTime * movementSpeed; //multiply only the componenet you want

            if (transform.position.x + xMove >= minXPos && transform.position.x + xMove <= maxXPos)
            {
                transform.Translate(xMove, 0f, 0f); //move object

            }
        }

       

    }

    public bool GetCanCameraMakeBumperScroll()
    {
        return canCameraMakeBumperScroll;
    }

   /* private void DetectHeight()
    {
        if(levelController.GetIsBumperStuck())
        {
            canCameraMakeBumperScroll = false;
        }
        else
        {
            float differenceInHeight = Camera.main.transform.position.y - this.transform.position.y;

            if (differenceInHeight < 11.5 && differenceInHeight > 8.5)
            {
                canCameraMakeBumperScroll = true;
                catchUpMovementEnabled = false;
                Debug.Log("Difference in height between camera and bumper is " + (Camera.main.transform.position.y - this.transform.position.y));
            }
            else if (differenceInHeight > 11.5f)
            {
                catchUpMovementEnabled = true;
                MoveToCorrectHeight(1);
                Debug.Log("Difference in height between camera and bumper is " + (Camera.main.transform.position.y - this.transform.position.y));

            }
            else if (differenceInHeight < 8.5)
            {
                catchUpMovementEnabled = true;
                MoveToCorrectHeight(-1);
                Debug.Log("Difference in height between camera and bumper is " + (Camera.main.transform.position.y - this.transform.position.y));

            }
        }
        
    }*/

    

   /* public void MoveToCorrectHeight(int direction)
    {
        float xMove = GetComponent<Joystick>().getBumperDirection().x * Time.deltaTime * movementSpeed; //multiply only the componenet you want

        
        Vector3 catchUpVector = new Vector3(this.transform.position.x, heightCorrectionSpeed * direction * Time.deltaTime, this.transform.position.z);
        this.transform.Translate(catchUpVector);
*//*
        float targetPosition = Camera.main.transform.position.y - 10;
        Vector3 newMovementVector = new Vector3(this.transform.position.x, targetPosition, this.transform.position.z);
        this.transform.position = Vector3.MoveTowards(this.transform.position, newMovementVector, heightCorrectionSpeed);*//*
            
        
        
    }*/

   
}
