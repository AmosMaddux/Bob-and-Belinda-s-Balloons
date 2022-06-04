using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] GameObject[] blockExploders;
    [SerializeField] public LayerMask switchableBlockLayer;
    [SerializeField] float timeBetweenExplosions;
    [SerializeField] Sprite leverSwitched;
    [SerializeField] float raycastDistance = 1.0f;
    [SerializeField] float angleIncrement = 90;
    [SerializeField] int cycles = 1;
    bool isTriggered = false;
    
    SpriteRenderer mySpriteRenderer;
    int explosionNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BeginExplosions()
    {
        StartCoroutine(ExplodeDownTheLine());


        /*while(explosionNumber < blockExploders.Length)
        {
            StartCoroutine(ExplodeDownTheLine());

        }*/
        /*  for (int explosionNumber = 0; explosionNumber < blockExploders.Length; explosionNumber++)
          {


              //Debug.Log("Explosion Number " + explosionNumber + " completed.");
          }*/

        /*foreach (GameObject blockExploder in blockExploders)
        {

            StartCoroutine(ExplodeDownTheLine(blockExploder));
        }*/
    }

    //I put the incrememnet (explosionNumnber++) int the coroutine so that it will only go up once the alloted time has passed
    IEnumerator ExplodeDownTheLine()
    {
        foreach(GameObject blockExploder in blockExploders)
        {
            yield return new WaitForSeconds(timeBetweenExplosions);
            DestroyBlock destroyBlock = blockExploder.GetComponent<DestroyBlock>();
            Vector3 explosionPosition = blockExploder.GetComponent<Transform>().position;

            destroyBlock.Explosion(switchableBlockLayer, explosionPosition, raycastDistance, angleIncrement, cycles);
        }
        
        //explosionNumber++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered)
        {
            Debug.Log("Triggered by " + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Climber"))
            {
                mySpriteRenderer.sprite = leverSwitched;
                BeginExplosions();
                Debug.Log("Collided with climber");
                isTriggered = true;
            }
        }
        

    }

    

    
}
