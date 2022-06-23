using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystals : MonoBehaviour
{

    private CompositeCollider2D myCollider;
    [SerializeField] private GameObject shatterVFX;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<CompositeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Climber") ||
               collider.gameObject.CompareTag("Cannon") ||
               collider.gameObject.CompareTag("Cannonball"))
        {
            float xPos = Mathf.Floor(collider.gameObject.transform.position.x);
            float yPos = Mathf.Floor(collider.gameObject.transform.position.y);

        }

    }
    

}
