using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 40;
    public float maxSpeed = 15;
    public float upSpeed = 21;
    private Rigidbody2D marioBody;
    private bool onGroundState = true;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    // public Transform enemyLocation;
    // public Text scoreText;
    // private int score = 0;
    // private bool countScoreState = false;
    private Animator marioAnimator;
    private AudioSource marioAudio;

    void PlayJumpSound(){
        marioAudio.PlayOneShot(marioAudio.clip);
    }
  
    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
	    Application.targetFrameRate =  30;
	    marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator = GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();
    }

    void FixedUpdate() 
    {
      // dynamic rigidbody
      float moveHorizontal = Input.GetAxisRaw("Horizontal");
      if (Mathf.Abs(moveHorizontal) > 0){
          Vector2 movement = new Vector2(moveHorizontal, 0);
          if (marioBody.velocity.magnitude < maxSpeed) {
            marioBody.AddForce(movement * speed);
          }
      }
      if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
          // stop
          marioBody.velocity = new Vector2(0,0);
          // marioBody.velocity = new Vector2(0f, marioBody.velocity.y);
          // marioBody.isKinematic = false;
      }
      if (Input.GetKeyDown("space") && onGroundState){
          marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
          onGroundState = false;
        //   countScoreState = true; //check if Gomba is underneath
          marioAnimator.SetBool("onGround", onGroundState);
      }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")){
            onGroundState = true;
            // countScoreState = false; // reset score state
            // scoreText.text = "SCORE: " + score.ToString();
            marioAnimator.SetBool("onGround", onGroundState);
        }

        if (col.gameObject.CompareTag("Obstacle")) {
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);
        }
            
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy")){
            Debug.Log("Collided with Gomba");
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
    }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
        
        // toggle state
        if (Input.GetKeyDown("a") && faceRightState){
            if (Mathf.Abs(marioBody.velocity.x) >  1.0) {
                marioAnimator.SetTrigger("onSkid");
            }
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if (Input.GetKeyDown("d") && !faceRightState){
            if (Mathf.Abs(marioBody.velocity.x) >  1.0) {
                marioAnimator.SetTrigger("onSkid");
            }
            faceRightState = true;
            marioSprite.flipX = false;
        }
        // when jumping, and Gomba is near Mario and we haven't registered our score
        // if (!onGroundState && countScoreState)
        // {
        //     //   if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
        //     //   {
        //     //       countScoreState = false;
        //     //       score++;
        //     //       Debug.Log(score);
        //     //   }
        // }
    }
    
}