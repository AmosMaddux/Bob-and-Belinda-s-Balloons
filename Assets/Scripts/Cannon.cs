using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{

    //cached refs
    //[SerializeField] bool cannonMode;
    LevelController levelController;
    private Vector2 launchVector;
    [SerializeField] bool isAiming = false;
   // [SerializeField] bool ballIsFlying = false;
    //[SerializeField] Rigidbody2D ballRigidbody;
    [SerializeField] GameObject cannonball;
    [SerializeField] GameObject bumper;


    //configs
    
    [SerializeField] float launchForceMultiplier = 1;
 



    Vector2 startTouchPosition, endTouchPosition;
    


    // Start is called before the first frame update
    void Start()
    {
        //cannonball = FindObjectOfType<Cannonball>();
        //cannonMode = false;
        levelController = FindObjectOfType<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        CannonClicked();
        /*if (levelController.GetCannonMode())
        {
            if (Input.GetMouseButton(0))
            {
                isAiming = true;
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
                
                launchVector = new Vector2(touchPosition.x - GetComponent<Transform>().position.x, touchPosition.y - GetComponent<Transform>().position.y);


            }
        }

        if(levelController.GetCannonMode() && isAiming)
        {
            if (Input.GetMouseButtonUp(0))
            {
                ballRigidbody.AddForce(launchVector, ForceMode2D.Impulse);
                //cannonMode = false;
                isAiming = false;
                
            }
            
        }*/
        
    }

    void CannonClicked()
    {
        
        //if the mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            
            
            //and if the cursor is above the cannon
            RaycastHit2D hit =  Physics2D.Raycast(new Vector2(
                                                            Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                                            Camera.main.ScreenToWorldPoint(Input.mousePosition).y), //this is the starting position
                                                      Vector2.zero, //this is the direction
                                                      10f);//this is the distance to travel
            //Debug.Log("Clicked on " + hit.collider.gameObject.name);
            
            if (hit.collider.gameObject != null)
            {
                if (hit.collider.gameObject.CompareTag("Cannon"))
                {

                    isAiming = true;

                    GameObject objectHit = hit.transform.gameObject;

                    levelController.SetCanClimberMove(false);
                    levelController.SetCanBumperMove(false);
                    //Debug.Log("Ray hit " + objectHit.name);

                    //set a launch vector (should also change color or something)
                    isAiming = true;
                    startTouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));








                }
            }
            
          
        }

        //is the mouse button is let go of
        if (Input.GetMouseButtonUp(0) && isAiming)
        {
            //create a spawn position and instantiate a clown of the cannonball
            Vector3 cannonballCloneSpawnPosition = new Vector3(bumper.transform.position.x, (bumper.transform.position.y + 1.8f), bumper.transform.position.z);
            GameObject cannonballClone = Instantiate(cannonball, cannonballCloneSpawnPosition, Quaternion.identity);
            cannonballClone.gameObject.SetActive(true);
            cannonballClone.GetComponent<Cannonball>().StartBallCountdown();
            //fire the cannonball clone
            endTouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));


          

            Vector2 rawLaunchVector = new Vector2((endTouchPosition.x - cannonballCloneSpawnPosition.x),
                                                    (endTouchPosition.y - cannonballCloneSpawnPosition.y));

            launchVector = ScaleLaunchVector(rawLaunchVector);
            


            //Debug.Log("Raw launch vector is " + rawLaunchVector);
            //Debug.Log("Launch vector is " + launchVector);


            cannonballClone.GetComponent<Rigidbody2D>().AddForce(launchVector, ForceMode2D.Impulse);
            levelController.UpdateBallsAvailable();
            levelController.SetCanClimberMove(true);
            levelController.SetCanBumperMove(true);
            //cannonMode = false;
            isAiming = false;

            //Debug.Log("Ball launched. Launch vector is " + launchVector);


        }
    }


    private Vector2 ScaleLaunchVector(Vector2 launchVector)
    {
        //convert vecotr coordinates to polar coordinates
        float magnitude = Mathf.Sqrt(Mathf.Pow(launchVector.x, 2) + Mathf.Pow(launchVector.y, 2));
        float angle = Mathf.Acos(launchVector.x / magnitude);
        //convert back to vector coordinates but with a magnitude of 1
        Vector2 newLaunchVector = new Vector2(Mathf.Cos(angle) * launchForceMultiplier, Mathf.Sin(angle) * launchForceMultiplier);
        return newLaunchVector;
    }

    /* public bool GetBallIsFlying()
     {
         return ballIsFlying;
     }*/
}
