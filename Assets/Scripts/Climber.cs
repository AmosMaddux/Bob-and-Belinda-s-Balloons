using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Climber : MonoBehaviour
{

    //cached refs
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private CapsuleCollider2D myFeet;
    [SerializeField] private BoxCollider2D myBody;
    [SerializeField] private Animator myAnimator;
    [SerializeField] GameObject bomb;
    [SerializeField] Transform myTransform;
    [SerializeField] private Tilemap ladder;
    LevelController levelController;

    //configs
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float climbSpeed = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float jumpThreshold = 0.3f;
    [SerializeField] private float timeBetweenJumps = 1f;
    [SerializeField] [Range(0, 1)] private float ladderWaitToGrabTime = 0.5f;
    private float gravityScaleAtStart;
    private bool initialLadderTouch = true;

    [Header("Bomb")]
    [SerializeField] private float spawnBombThreshold = -0.8f;
    [SerializeField] private float timeBetweenBombSpawns = 1f;
    [SerializeField] public LayerMask groundLayer; //the layer the rays will pick from
    [SerializeField] public LayerMask unbreakableLayer; //the layer the rays will pick from
    [SerializeField] float explosionDelay = 3f;

    [Header("Deathkick")]
    [SerializeField] private float deathkickLowValue = 20f;
    [SerializeField] private float deathkickHighValue = 100f;



    bool bombSpawnPeriodOver = true;
    bool jumpPeriodOver = true;
    bool isAlive = true;
    



    

    // Start is called before the first frame update
    void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        CheckJumpingOrFalling();
   
      
    }

    private void FixedUpdate()
    {
       if(levelController.GetCanClimberMove())
        {
            if(isAlive)
            {
                FlipSprite();

                Move();
                Jump();
                Climb();
                SetBomb();
                //BreakCrystal();
            }

        }

        Debug.Log("Gravity: " + myRigidbody.gravityScale);


    }

    


    public void Die()
    {
        if(isAlive)
        {
            isAlive = false;

            Vector2 kickVector = new Vector2(0, 0);

            int directionOfFlight = Random.Range(0, 2);
            if (directionOfFlight == 0)
            {
                 kickVector = new Vector2(Random.Range(-deathkickHighValue, -deathkickLowValue), Random.Range(deathkickLowValue, deathkickHighValue));

            }
            else if (directionOfFlight == 1)
            {
                 kickVector = new Vector2(Random.Range(deathkickLowValue, deathkickHighValue), Random.Range(deathkickLowValue, deathkickHighValue));

            }
            else
            {
                Debug.Log("Direction out of range");
            }

            //myRigidbody.freezeRotation = false; //for 2d rigidbodys it automatically know i am talking about the z axis. 
            myRigidbody.velocity = kickVector;

            myAnimator.SetTrigger("isDying");
           // Debug.Log("Deathkick called. ");
            levelController.ProcessDeath();
        }
      

    }

    //for some reason i could never get this to work? It always said the scale was either 0 or was already negative when it shouldn't have been.
    //I would try to mul;tiply the sign of the x velocity by the x scale, but when i did, the scale was already negative, hence, a 
    //scale that was always positive. When i removed the sign of the x velocity from the equation, it said the scale was 0???????
    //Okay okay okay, so it turns out A. when you change scale in ANY animation, even if you don't acutally change, if it is mentioned as 
    //one of the properties in an animation that is in the controller, the sontroller always overrides anythign you do in your script
    //B. I had it mutliplying the X scale EVERY FRAME, which meant that it would become negative, then become multiplied b y the negative direction
    //in the3 very next frame. So i fixed this by multiplying the direction by the absolute value of the x scale. Which fis always positive, so it fixed it
    private void FlipSprite()

    {
        bool playerHasHorizontalInput = Mathf.Abs(GetComponent<Joystick>().getClimberDirection().x) > Mathf.Epsilon;
        if (playerHasHorizontalInput)
        {
            

            transform.localScale = new Vector2(Mathf.Sign(GetComponent<Joystick>().getClimberDirection().x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
            

       
        }


    }

    public void Move()
    {

        Vector2 direction = GetComponent<Joystick>().getClimberDirection();
        Vector2 currentVelocity = myRigidbody.velocity;
        currentVelocity.x = direction.x * movementSpeed;
        myRigidbody.velocity = currentVelocity;
       // Debug.Log("Current velocity is " + currentVelocity);
        //Debug.Log("Velocity is supposedly " + myRigidbody.velocity);


        //set the animator to the run animations IF the climber has horizontal speed
        //will eventually need to change this so that it only runs if touching the ground
        bool playerHasHorizontalInput = Mathf.Abs(GetComponent<Joystick>().getClimberDirection().x) > Mathf.Epsilon;
        //Debug.Log("Horizontal Input is " + GetComponent<Joystick>().getDirection().x);
        if (playerHasHorizontalInput)
        {
           // Debug.Log("Horizontal Input Check triggered");
            if (IsOnGround())
            {
                myAnimator.SetBool("isRunning", true);

            }
            else
            {
                myAnimator.SetBool("isRunning", false);
                //Debug.Log("isRunning set to " + myAnimator.GetBool("isRunning"));

            }
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
           //Debug.Log("isRunning set to " + myAnimator.GetBool("isRunning"));

        }

    }

    public void Jump()
    {
        Vector2 direction = GetComponent<Joystick>().getClimberDirection();
        if (direction.y > jumpThreshold)
        {
            if(IsOnGround())
            {
                if (!IsTouchingLadder())
                {
                    if (jumpPeriodOver)
                    {
                        Vector2 jumpVelocity = myRigidbody.velocity;
                        jumpVelocity.y = jumpForce;
                        myRigidbody.velocity = jumpVelocity;
                        jumpPeriodOver = false;
                        StartCoroutine(NoJumpPeriodTimer());
                    }
                }
                
            }
            

            
        }
    }

    private void Climb()
    {
        if (IsTouchingLadder())
        {
            float yMovementInput = GetComponent<Joystick>().getClimberDirection().y; // this gets the whatever axis we are using for the current device. It is scaleable. So pushing the joystick a little or turning the phone a little pushes a little.
            Vector2 climbingVelocity = new Vector2(myRigidbody.velocity.x, 0);
            myRigidbody.gravityScale = 0;
            if (yMovementInput > 0.5 || yMovementInput < -0.5)
            {
                climbingVelocity = new Vector2(myRigidbody.velocity.x, yMovementInput * climbSpeed);
            }
            myRigidbody.velocity = climbingVelocity;
            

            // bool playerVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon; //couldn't we use a collision with the ladder instead?
            // myAnimator.SetBool("isClimbing", playerVerticalSpeed);
        }
        else
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            //myAnimator.SetBool("isClimbing", false); // takes fo the bug where he keeps climbing if you hold up after leaving a ladder
        }




    }

   /* private void BreakCrystal()
    {
        if (myBody.IsTouchingLayers(LayerMask.GetMask("Crystals")))
        {
            bomb.GetComponent<DestroyBlock>().Explosion(LayerMask.GetMask("Crystals"), myTransform.position, 0.8f, 45, 1);
        }
    }*/

    private void SetBomb()
    {
        if (levelController.GetBombsAvailable() > 0)
        {
            Vector2 direction = GetComponent<Joystick>().getClimberDirection();

            if (direction.y < spawnBombThreshold)
            {
                if (IsOnGround())
                {
                    if (bombSpawnPeriodOver)
                    {
                        levelController.UpdateBombsAvailable();

                        bomb.GetComponent<Bomb>().SpawnBomb();
                        bombSpawnPeriodOver = false;
                        StartCoroutine(NoBombSpawnPeriodTimer());


                    }
                }
            }
        }
       
    }

  

    IEnumerator NoJumpPeriodTimer()
    {
        yield return new WaitForSeconds(timeBetweenJumps);
        jumpPeriodOver = true;
    }

    IEnumerator NoBombSpawnPeriodTimer()
    {
        yield return new WaitForSeconds(timeBetweenBombSpawns);
        bombSpawnPeriodOver = true;
    }

    public void CheckJumpingOrFalling()
    {
        if (!IsOnGround())
        {
            if (myRigidbody.velocity.y > Mathf.Epsilon)
            {

                //myAnimator.SetBool("isRunning", false);
                myAnimator.SetBool("isJumping", true);
                // myAnimator.SetBool("isRunning", false);




            }
            else if (myRigidbody.velocity.y < -Mathf.Epsilon)
            {
                //Debug.Log("Velocity is negative");
                //Debug.Log(" Velocity is " + myRigidbody.velocity.y);
                myAnimator.SetBool("isFalling", true);
                myAnimator.SetBool("isJumping", false);
                //myAnimator.SetBool("isRunning", false);

            }
            
        }
        else
        {
            myAnimator.SetBool("isFalling", false);
            myAnimator.SetBool("isJumping", false);
        }
       // Debug.Log("Vertical velocity is " + myRigidbody.velocity.y);
        
    }

    public bool IsOnGround()
    {
        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) || myFeet.IsTouchingLayers(LayerMask.GetMask("Unbreakable")) ||
            myFeet.IsTouchingLayers(LayerMask.GetMask("Switchables")) || myFeet.IsTouchingLayers(LayerMask.GetMask("Bumper"))
            || myFeet.IsTouchingLayers(LayerMask.GetMask("Barrier"))) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

/*    IEnumerator LadderWaitToGrabTime()
    {
        yield return new WaitForSeconds(ladderWaitToGrabTime);
    }*/

    public bool IsTouchingLadder()
    {
        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            return true;
        }
        else
        {
            return false;
        }
        /* Vector3Int ladderCellPosition = new Vector3Int(ladder.WorldToCell(myTransform.position).x, ladder.WorldToCell(myTransform.position).y, 0);

         if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder")))
         {
             if (Mathf.Abs(myTransform.position.x - ladder.GetCellCenterWorld(ladderCellPosition).x) < 0.3f)
             {
                 initialLadderTouch = false;
                 return true;
             }
             else
             {
                 return false;
             }

         }
         else
         {
             initialLadderTouch = true;
             return false;
         }*/
        /*  if (initialLadderTouch)
          {
              StartCoroutine(LadderWaitToGrabTime());
              if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder")))
              {
                  initialLadderTouch = false;
                  return true;
              }
              else
              {
                  initialLadderTouch = true;
                  return false;
              }

          }
          else
          {
              if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder")))
              {
                  initialLadderTouch = false;
                  return true;
              }
              else
              {
                  initialLadderTouch = true;
                  return false;
              }
          }*/

    }
}
