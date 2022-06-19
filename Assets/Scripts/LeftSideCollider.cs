using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftSideCollider : MonoBehaviour
{

    Bumper bumper;
    // Start is called before the first frame update
    void Start()
    {
        bumper = FindObjectOfType<Bumper>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            bumper.setCollisionOnLeft(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            bumper.setCollisionOnLeft(false);
        }
    }
}
