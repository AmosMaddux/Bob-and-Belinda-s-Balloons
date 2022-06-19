using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCollider : MonoBehaviour
{

    [SerializeField] LevelController levelController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("IM TRIGGERED!");
        if (collision.gameObject.CompareTag("Cannonball"))
        {
          //  Debug.Log("LoseCollider Triggered");
            levelController.BallLost();
            Destroy(collision.gameObject);
            Debug.Log("Triggered by balls");


        }

        if (collision.gameObject.CompareTag("Cannon"))
        {
            levelController.ProcessDeath();
            Debug.Log("Triggered by cannon");

        }

        if (collision.gameObject.CompareTag("Climber"))
        {
            levelController.ProcessDeath();
           // Debug.Log("Triggered by climber");
        }
    }
}
