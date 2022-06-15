using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameConstants gameConstants;
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    public float upSpeed = 21;
    private int moveRight = -1;
    private bool collided = false;
    private Vector2 velocity;
    private AudioSource enemyWinAudio;
    private Rigidbody2D enemyBody;
    private BoxCollider2D enemyCollider;
    private bool marioDead = false;
    // private bool jumped = false;
    private bool onGroundState = true;
    // Start is called before the first frame update
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemyWinAudio = GetComponent<AudioSource>();
        enemyCollider = GetComponent<BoxCollider2D>();

        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
        // subscribe to player event
        GameManager.OnPlayerDeath  +=  EnemyRejoice;
    }
    void ComputeVelocity(){
        velocity = new Vector2((moveRight)*maxOffset / enemyPatroltime, 0);
    }
    void MoveGomba(){
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    // animation when player is dead
    void  EnemyRejoice(){
        Debug.Log("Enemy killed Mario");
        enemyBody.bodyType = RigidbodyType2D.Dynamic;
        enemyCollider.isTrigger = false;
        marioDead = true;


        // do whatever you want here, animate etc
        // ...
    }
    void  OnTriggerEnter2D(Collider2D other){
		// check if it collides with Mario
		if (other.gameObject.tag  ==  "Player" && !collided){
            
			// check if collides on top
			float yoffset = (other.transform.position.y  -  this.transform.position.y);
			if (yoffset  >  0.75f){
                collided = true;
				KillSelf();
			}
			else{
				// hurt player
			    CentralManager.centralManagerInstance.damagePlayer();
                enemyWinAudio.PlayOneShot(enemyWinAudio.clip);
			}
		}

	}
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")){
            onGroundState = true;
        }   
    }
   
    void  KillSelf(){
		// enemy dies
		CentralManager.centralManagerInstance.increaseScore();
		StartCoroutine(flatten());
		Debug.Log("Kill sequence ends");
	}
    IEnumerator  flatten(){
		Debug.Log("Flatten starts");
		int steps =  5;
		float stepper =  1.0f/(float) steps;

		for (int i =  0; i  <  steps; i  ++){
			this.transform.localScale  =  new  Vector3(this.transform.localScale.x, this.transform.localScale.y  -  stepper, this.transform.localScale.z);

			// make sure enemy is still above ground
			this.transform.position  =  new  Vector3(this.transform.position.x, gameConstants.groundSurface  +  GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield  return  null;
		}
		Debug.Log("Flatten ends");
		this.gameObject.SetActive(false);
        // Reset local scale for next spawning
        this.transform.localScale = new Vector3(1,1,1);
        collided = false;
		Debug.Log("Enemy returned to pool");
		yield  break;
	}
    void FixedUpdate() {
        
        if (marioDead && onGroundState) {
            Debug.Log("Jump");
            enemyBody.AddForce((Vector2.up * upSpeed) / 3, ForceMode2D.Impulse);
            onGroundState = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (marioDead) {
            
        }
        else if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {// move gomba
            MoveGomba();
        }
        else{
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            MoveGomba();
        }
    }
}
