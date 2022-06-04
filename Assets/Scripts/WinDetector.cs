using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDetector : MonoBehaviour
{
    LevelController levelController;

    private void Start()
    {
        levelController = FindObjectOfType<LevelController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Climber"))
        {
            levelController.SetClimberInWinZone(true);
        }
        if (collision.gameObject.CompareTag("Cannon"))
        {
            levelController.SetBumperInWinZone(true);
        }
    }
}
