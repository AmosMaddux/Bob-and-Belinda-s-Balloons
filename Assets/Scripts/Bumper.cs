using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{

    //cached refs
    LevelController levelController;
    RaycastHit2D hit;
    Joystick joystick;
   

    //configs
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] float minXPos;
    [SerializeField] float maxXPos;


    //variables
    private bool collisionOnLeft;
    private bool collisionOnRight;


    // Start is called before the first frame update
    void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        joystick = FindObjectOfType<Joystick>();
   
    }

    // Update is called once per frame
    void Update()
    {
       
      if (levelController.GetCanBumperMove())
        {
            Move();
        } 
    }

    public void Move()
    {

       if (canMoveSideways())
        {
            float xMove = GetComponent<Joystick>().getBumperDirection().x * Time.deltaTime * movementSpeed; //multiply only the componenet you want

            if (transform.position.x + xMove >= minXPos && transform.position.x + xMove <= maxXPos)
            {
                transform.Translate(xMove, 0f, 0f); //move object

            }
        }
    }

    private bool canMoveSideways()
    {
        float currentDirectionOfJoystick = Mathf.Sign(joystick.getBumperDirection().x);
        /*Vector2 direction = new Vector2(currentDirectionOfJoystick, 0);
        GameObject detectedObject = castRays(direction);*/

        if (currentDirectionOfJoystick < Mathf.Epsilon) //pointed left
        {
            if (!collisionOnLeft)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (currentDirectionOfJoystick > Mathf.Epsilon) //pointed right
        {
            if (!collisionOnRight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }      
    }

    public void setCollisionOnLeft(bool isCollision)
    {
        collisionOnLeft = isCollision;
    }

    public void setCollisionOnRight(bool isCollision)
    {
        collisionOnRight = isCollision;
    }


}










