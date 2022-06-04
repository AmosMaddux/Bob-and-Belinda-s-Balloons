using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightDetector : MonoBehaviour
{
    LevelController levelController;
    Bumper bumper;
    // Start is called before the first frame update
    void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        bumper = FindObjectOfType<Bumper>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cannon"))
        {
            Debug.Log("Triggered by cannon");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!levelController.GetIsBumperStuck())
        {
            Debug.Log("Moving back to desired height");
            Debug.Log("isBumperStuck = " + levelController.GetIsBumperStuck());
           // bumper.MoveToCorrectHeight();
        }
    }
}
