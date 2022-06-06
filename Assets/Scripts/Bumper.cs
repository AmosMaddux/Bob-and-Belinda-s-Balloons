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
    RaycastHit2D hit;
    Joystick joystick;
   

    //configs
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float heightCorrectionSpeed = 1f;
    [SerializeField] float minXPos;
    [SerializeField] float maxXPos;
    [SerializeField] float raycastDistance;
    [SerializeField] LayerMask targetLayer;
    bool canCameraMakeBumperScroll = true;
    bool catchUpMovementEnabled = false;



    // Start is called before the first frame update
    void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        mainCameraPosition = FindObjectOfType<Camera>().transform.position;
        bottomOfBumper = GetComponent<BoxCollider2D>();
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
        Vector2 direction = new Vector2(currentDirectionOfJoystick, 0);
        GameObject detectedObject = castRays(direction);

        if (detectedObject != null)
        {
            if (detectedObject.CompareTag("Ground"))
            {
                Debug.Log("Collided with " + hit.collider.gameObject.name + " at position " + hit.collider.gameObject.transform.position);
                return false;
            }
            else
            {
                Debug.Log("Not ground. Tag is " + detectedObject.tag 
                             + ". Name is " + detectedObject.name 
                             + ". Position is " + detectedObject.transform.position);
                return true;
            }
        }
        else
        {
            Debug.Log("No collision");
            return true;
        }

            
    }

    
       


    private GameObject castRays(Vector2 direction)
    {
        
        hit = Physics2D.Raycast(transform.position, direction, 1, targetLayer); //starts the raycast with an origin, direction, distance, and layer covnerted to an integer vlaue

        Vector2 endPos = new Vector2(transform.position.x + direction.x, transform.position.y); //saves the endpoint of the raycast. For debugging? noy only that

        Debug.DrawLine(transform.position, endPos, Color.blue);

        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    public void MoveToCorrectHeight()
    {
        /*float xMove = GetComponent<Joystick>().getBumperDirection().x * Time.deltaTime * movementSpeed; //multiply only the componenet you want


        Vector3 catchUpVector = new Vector3(this.transform.position.x, heightCorrectionSpeed * direction * Time.deltaTime, this.transform.position.z);
        this.transform.Translate(catchUpVector);

        float targetPosition = Camera.main.transform.position.y - 10;
        Vector3 newMovementVector = new Vector3(this.transform.position.x, targetPosition, this.transform.position.z);
        this.transform.position = Vector3.MoveTowards(this.transform.position, newMovementVector, heightCorrectionSpeed);*/

        transform.position = new Vector2(transform.position.x, (transform.position.y + 0.1f));



    }
}










