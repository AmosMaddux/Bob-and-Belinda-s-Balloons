using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideGroundDetector : MonoBehaviour
{
    //public RuleTile groundTile;
    //public Tilemap groundTileMap;





    Joystick joystick;
    float directionOfJoystickWhenBumperHitsWall = 0;
    float currentDirectionOfJoystick = 0;
    bool isTouchingWall = false;

    LevelController levelController;
    PolygonCollider2D sideCollider;
    //[SerializeField] Transform bumper;
    // Start is called before the first frame update
    void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        joystick = FindObjectOfType<Joystick>();
        sideCollider = GetComponent<PolygonCollider2D>();

       
    }

    // Update is called once per frame
    void Update()
    {
        currentDirectionOfJoystick = (Mathf.Sign(joystick.getBumperDirection().x));

        DetectIfMovingAwayFromGround();
        // DetectGroundBurst(groundLayer);
        //  DetectGroundBurst(unbreakableLayer);
        //  DetectGroundBurst(switchableLayer);
      /*  Debug.Log("currentDirection = " + currentDirectionOfJoystick + " oldDirection " + directionOfJoystickWhenBumperHitsWall);
        Debug.Log("CanBumperMove = " + levelController.GetCanBumperMove());*/

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            directionOfJoystickWhenBumperHitsWall = (Mathf.Sign(joystick.getBumperDirection().x));
            isTouchingWall = true;
            levelController.SetCanBumperMove(false);
           // Debug.Log("CanBumperMove " + levelController.GetCanBumperMove());
           // Debug.Log("IsBumperStuck " + levelController.GetIsBumperStuck());



        }
    }

   /* private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            levelController.SetCanBumperMove(true);


        }
    }*/

    private void DetectIfMovingAwayFromGround()
    {
        if (currentDirectionOfJoystick != directionOfJoystickWhenBumperHitsWall && isTouchingWall == true)
        {
            levelController.SetCanBumperMove(true);
            isTouchingWall = false;
           // Debug.Log("CanBumperMove " + levelController.GetCanBumperMove());

           // Debug.Log("IsBumperStuck " + levelController.GetIsBumperStuck());

        }

    }

   /* private void OnTriggerExit2D(Collider2D collision)
    {
        levelController.SetIsBumperStuck(false);
    }*/


}
