using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    private float mushroomPatrolTime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;
    private Rigidbody2D mushroomBody;
    public float speed = 20;
    public float upSpeed = 15;
    public SpriteRenderer mushroomSprite;
    // Start is called before the first frame update
    void Start()
    {
        mushroomBody = GetComponent<Rigidbody2D>();
        mushroomSprite = GetComponent<SpriteRenderer>();
        ComputeVelocity();
        mushroomBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
    }
    void  OnCollisionEnter2D(Collision2D col) {

        if (col.gameObject.CompareTag("Pipe")){
            Debug.Log("Hit pipe");
            moveRight *= -1;
            ComputeVelocity();
        }
        if (col.gameObject.CompareTag("Player")){
            velocity = new Vector2(0,0);
            StartCoroutine(enlarge());
        }
    }
    IEnumerator enlarge() {
        int steps = 4;
		float stepper =  1.0f/(float) (steps + 1);
        for (int i =  0; i  <  steps; i  ++){
			this.transform.localScale  =  new  Vector3(this.transform.localScale.x + stepper, this.transform.localScale.y  +  stepper, this.transform.localScale.z);
			yield  return  null;
		}
        mushroomSprite.enabled = false;
        yield break;
    }
    // void  OnBecameInvisible(){
    //     Destroy(gameObject);	
    // }
    void ComputeVelocity() {
        velocity = new Vector2((moveRight * speed) / mushroomPatrolTime, 0);
    }
    void MoveMushroom() {
        mushroomBody.MovePosition(mushroomBody.position + velocity * Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        MoveMushroom();
    }
}
