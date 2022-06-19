using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightSideCollider : MonoBehaviour
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
            bumper.setCollisionOnRight(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            bumper.setCollisionOnRight(false);
        }
    }
}
