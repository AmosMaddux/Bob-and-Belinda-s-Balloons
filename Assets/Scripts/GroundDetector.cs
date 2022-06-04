using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; //so we can use all the tilemap stuff like get the blocks in the enumerator


public class GroundDetector : MonoBehaviour
{


    LevelController levelController;
    PolygonCollider2D upperCollider;
 
    void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        upperCollider = GetComponent<PolygonCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            levelController.SetIsBumperStuck(true);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        levelController.SetIsBumperStuck(false);
    }



   
}
