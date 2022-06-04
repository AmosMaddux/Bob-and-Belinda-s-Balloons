using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TargetForSwitching : MonoBehaviour
{
   // [SerializeField] Button switchButton;

    LevelController levelController;

    // Start is called before the first frame update
    void Start()
    {
        //switchButton.gameObject.SetActive(false);
        levelController = FindObjectOfType<LevelController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   /* private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //switchButton.gameObject.SetActive(true);
            //Debug.Log("Player Entered.");
            levelController.SetIsSwitchingModesPossible(true);


        }

    }*/

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //switchButton.gameObject.SetActive(false);
           // levelController.SetIsSwitchingModesPossible(false);



        }
    }
}
