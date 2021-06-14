using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float maxSpeed = 10;
    public float upSpeed = 10;

    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    // Start is called before the first frame update
    private float moveHorizontal;
    private bool onGroundState = true;

    public Transform enemyLocation;
    public Text scoreText;
    private int score = 0;
    private bool countScoreState = false;

    private Animator marioAnimator;
    private AudioSource marioAudio;

    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator = GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // toggle state
        if (Input.GetKeyDown("a") && faceRightState){
            faceRightState = false;
            marioSprite.flipX = true;

            if (Mathf.Abs(marioBody.velocity.x) > 1.0) {
                marioAnimator.SetTrigger("onSkid");
            }
        }

        if (Input.GetKeyDown("d") && !faceRightState){
            faceRightState = true;
            marioSprite.flipX = false;

            if (Mathf.Abs(marioBody.velocity.x) > 1.0) {
                marioAnimator.SetTrigger("onSkid");
            }
        }

        // when jumping, and Gomba is near Mario and we haven't registered our score
        if (!onGroundState && countScoreState)
        {
          if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
          {
              countScoreState = false;
              score++;
              Debug.Log(score);
          }
        }

        // Animation handling
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
        marioAnimator.SetBool("onGround", onGroundState);
      
    }

    // FixedUpdate may be called once per frame. See documentation for details.
    // ? 60 fps --> function called 60 times per second
    void FixedUpdate(){
        // dynamic rigidbody
        moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
        }
      
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            // stop
            marioBody.velocity = Vector2.zero;
        }
        
        if (Input.GetKeyDown("space") && onGroundState){
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            marioAnimator.SetBool("onGround", onGroundState);
            countScoreState = true; //check if Gomba is underneath
        }
    }
  
    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) {
            onGroundState = true; // back on ground
            marioAnimator.SetBool("onGround", onGroundState);
            countScoreState = false; // reset score state
            scoreText.text = "Score: " + score.ToString();
        }

        if (col.gameObject.CompareTag("Obstacles") && Mathf.Abs(marioBody.velocity.y) <0.01f) {
            onGroundState = true; // back on ground
            marioAnimator.SetBool("onGround", onGroundState);
            countScoreState = false; // reset score state
        }
    }
        
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Gomba!");
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void PlayJumpSound(){
	    marioAudio.PlayOneShot(marioAudio.clip);
    }

}
