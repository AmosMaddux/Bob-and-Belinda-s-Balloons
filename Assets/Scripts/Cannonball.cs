using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    //cached refs
    [SerializeField] Rigidbody2D myRigidbody;
    [SerializeField] Bumper bumper;

    LevelController levelController;


    //configs
    [SerializeField] float randomFactorMax = 1;
    [SerializeField] float raycastDistance = 1.0f;
    [SerializeField] float ballLifeTime = 15;

    float randomFactor;

    [SerializeField] public LayerMask groundLayer; //the layer the rays will pick from

    //These two values will be used to detect if the ball is bouncing without any y offset, i.e. back and forth horizontally
    float oldCollisionYValue = 0;
    float newCollisionYValue = 0;
    [SerializeField] float tweakThreshhold = 0.5f;



    [SerializeField] float startPositionYOffset;
    [SerializeField] float angleIncrement = 45;
    [SerializeField] int cycles = 1;

    //private bool ballIsFlying;

    // Start is called before the first frame update
    void Start()
    {
        // ballIsFlying = false;
        levelController = FindObjectOfType<LevelController>();
        randomFactor = Random.Range(-randomFactorMax, randomFactorMax);
    }

    // Update is called once per frame
    void Update()
    {
        // IsBallFlying();
    }

    public void StartBallCountdown()
    {
        StartCoroutine(CountdownTilBallIsDestroyed());
    }
    IEnumerator CountdownTilBallIsDestroyed()
    {
        yield return new WaitForSeconds(ballLifeTime);
        Destroy(gameObject);
    }

    


  

    void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<DestroyBlock>().Explosion(groundLayer, this.GetComponent<Transform>().position, raycastDistance, angleIncrement, cycles);
        //GetComponent<DestroyBlock>().Explosion((LayerMask.GetMask("Crystals")), this.GetComponent<Transform>().position, 0.6f, angleIncrement, cycles);

        //This will compare tha last collision with the current collision and tweak the velocity if the y values are too similar
        newCollisionYValue = this.transform.position.y;
        if (oldCollisionYValue != 0)
        {
        //    Debug.Log("Difference between values is " + (Mathf.Abs(Mathf.Abs(newCollisionYValue) - Mathf.Abs(oldCollisionYValue))));

            if (Mathf.Abs(Mathf.Abs(newCollisionYValue) - Mathf.Abs(oldCollisionYValue)) < tweakThreshhold)
            {
                Vector2 velocityTweak = new Vector2(Random.Range(-randomFactorMax, randomFactorMax), Random.Range(-randomFactorMax, randomFactorMax));
                myRigidbody.velocity += velocityTweak;
                //Debug.Log("Ball collided with " + collision.gameObject.name);
               // Debug.Log("Velocity tweak is " + velocityTweak);
            }
        }
        oldCollisionYValue = newCollisionYValue;
        //Debug.Log("Collision");
        

    }

        
}


