using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; //so we can use all the tilemap stuff like get the blocks in the enumerator

public class DestroyBlock : MonoBehaviour
{

    public RuleTile groundTile;
    public Tilemap groundTileMap;
    [SerializeField] Climber climber;
    

    
    //[SerializeField] public float castDistance = 1.0f; //the cast distance of the ray
    //public Transform raycastPoint; //the origing of the ray. will create a child on the object to use this.
    //[SerializeField] public LayerMask groundLayer; //the layer the rays will pick from


    //float blockDestroyTime = 0f; //set to zero for no delay between input and destrtuction

    Vector3 direction; //the direction of the raycast.
    RaycastHit2D hit;

    bool destroyingBlock = false;
    bool placingBlock = false;
    // Start is called before the first frame update
    




    public void Explosion(LayerMask targetLayer, Vector3 raycastPoint, float raycastDistance, float angleIncrement, int cycles)
    {


        for (int cycle = 1; cycle <= cycles; cycle++)
        {
            for (float angle = 0; angle < 360; angle += angleIncrement) //this should cast rays radially around the center of the ball
            {
                //Debug.Log("Angle in degrees is " + angle);
                float angleInRadians = angle * Mathf.Deg2Rad; //convert angle to radians
                                                              // Debug.Log("Angle in radians is " + angle);


                direction.x = raycastDistance * Mathf.Cos(angleInRadians) * cycle; //get the x rectangular coordinate from the radius and the polar coordinate angle.
                direction.y = raycastDistance * Mathf.Sin(angleInRadians) * cycle; //get the y rectangular coordinate from the radius and the polar coordinate angle.

                //Debug.Log("direction is " + direction);
                hit = Physics2D.Raycast(raycastPoint, direction, raycastDistance, targetLayer); //starts the raycast with an origin, direction, distance, and layer covnerted to an integer vlaue

                Vector2 endPos = raycastPoint + direction; //saves the endpoint of the raycast. For debugging? noy only that

                Debug.DrawLine(raycastPoint, endPos, Color.red);

                if (hit.collider) //returns true if the ray hit a collider 
                {
                    if (hit.collider.gameObject.CompareTag("Climber"))
                    {
                        climber.Die();
                    }

                    if (hit.collider.gameObject.CompareTag("Ground"))
                    {
                        //StartCoroutine(BreakBlock(hit.collider.gameObject.GetComponent<Tilemap>(), endPos)); //gets the tilemap to be fed into the function 
                        BreakBlock(hit.collider.gameObject.GetComponent<Tilemap>(), endPos);    
                    }
                }
            }
        } 
    }
    void BreakBlock(Tilemap map, Vector2 position) //is an enumerator so we can wait a certaion amount of seconds before executing if we want
    {
        
        //yield return new WaitForSeconds(explosionDelay);
        position.x = Mathf.Floor(position.x); //gets the closest integer value to x. used for raycasting. maybe not necessary for collsions only
        position.y = Mathf.Floor(position.y);//gets the closest integer value to x. used for raycasting. maybe not necessary for collsions only

        //I can add an explosion particle effect here too
        map.SetTile(new Vector3Int((int)position.x, (int)position.y, 0), null); //sets a null tile at the indicated position

        
    }

}
