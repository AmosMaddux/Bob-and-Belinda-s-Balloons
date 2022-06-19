using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    //cached refs
    Rigidbody2D myRigidbody;
    BoxCollider2D myBoxCollider;
    [SerializeField] Transform climber;
    [SerializeField] GameObject bomb;
    [SerializeField] public LayerMask groundLayer; //the layer the rays will pick from
    [SerializeField] public LayerMask unbreakableLayer; //the layer the rays will pick from
    [SerializeField] public LayerMask climberLayer;
    [SerializeField] public LayerMask explodableLayer;
    [SerializeField] GameObject explosionVFX;


    //configs
    [SerializeField] float raycastDistance = 1.0f;
   // [SerializeField] float secondRaycastDistance = 2.0f;
    [SerializeField] float explosionDelay = 3f;
    [SerializeField] float yOffset = 0.5f;
    [SerializeField] float xOffset = 0.5f;
    [SerializeField] float angleIncrement = 2;
    [SerializeField] int cycles = 1;
    [SerializeField] float secondExplosionVerticalOffset = 0.5f;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBomb()
    {
        Vector3 bombCloneSpawnPosition = new Vector3(climber.position.x, climber.position.y + yOffset, climber.position.z);
        GameObject bombClone = Instantiate(bomb, bombCloneSpawnPosition, Quaternion.identity);
        bombClone.gameObject.SetActive(true);
        //return bombClone; //so its going to spawn the bomb in the game AND return a reference to the bomb for other methods
        

        StartCoroutine(BombDestroy(bombClone));


    }

    IEnumerator BombDestroy(GameObject bombClone)
    {


        yield return new WaitForSeconds(explosionDelay);
        GetComponent<DestroyBlock>().Explosion(explodableLayer, bombClone.GetComponent<Transform>().position, raycastDistance, angleIncrement, cycles);
/*
        GetComponent<DestroyBlock>().Explosion(groundLayer, bombClone.GetComponent<Transform>().position, raycastDistance, angleIncrement, cycles);
        GetComponent<DestroyBlock>().Explosion(unbreakableLayer, bombClone.GetComponent<Transform>().position, raycastDistance, angleIncrement, cycles);
        GetComponent<DestroyBlock>().Explosion(climberLayer, bombClone.GetComponent<Transform>().position, raycastDistance, angleIncrement, cycles);
*/
        Vector3 secondExplosionSource = new Vector3(bombClone.GetComponent<Transform>().position.x,
                                                    bombClone.GetComponent<Transform>().position.y + secondExplosionVerticalOffset,
                                                    bombClone.GetComponent<Transform>().position.z);
        GetComponent<DestroyBlock>().Explosion(explodableLayer, secondExplosionSource, raycastDistance, angleIncrement, cycles);

        /*GetComponent<DestroyBlock>().Explosion(groundLayer, secondExplosionSource, raycastDistance, angleIncrement, cycles);
        GetComponent<DestroyBlock>().Explosion(unbreakableLayer, secondExplosionSource, raycastDistance, angleIncrement, cycles);
        GetComponent<DestroyBlock>().Explosion(climberLayer, secondExplosionSource, raycastDistance, angleIncrement, cycles);*/
        /* GetComponent<DestroyBlock>().Explosion(groundLayer, bombClone.GetComponent<Transform>().position, secondRaycastDistance);
         /* GetComponent<DestroyBlock>().Explosion(groundLayer, bombClone.GetComponent<Transform>().position, secondRaycastDistance);
         GetComponent<DestroyBlock>().Explosion(unbreakableLayer, bombClone.GetComponent<Transform>().position, secondRaycastDistance);
         GetComponent<DestroyBlock>().Explosion(climberLayer, bombClone.GetComponent<Transform>().position, secondRaycastDistance);*/
        GameObject explosionVFXClone = Instantiate(explosionVFX, bombClone.transform.position, Quaternion.identity);
        Destroy(explosionVFXClone, 3f);

        Destroy(bombClone);
    }




    /*public void ExplodeBomb()
    {
        FindObjectOfType<DestroyBlock>().Explosion(groundLayer, this.GetComponent<Transform>().position, explosionDelay);
        FindObjectOfType<DestroyBlock>().Explosion(unbreakableLayer, this.GetComponent<Transform>().position, explosionDelay);

    }*/
}
