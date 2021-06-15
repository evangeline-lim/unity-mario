using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBoxController : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public SpringJoint2D springJoint;
    public GameObject consummablePrefab; // the spawned mushroom prefab
    public SpriteRenderer spriteRenderer;
    public Sprite usedQuestionBox; // the sprite that indicates empty box instead of a question mark
    private bool hit =  false; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && !hit){
            Debug.Log("Question box hit");
            hit = true;

            // ensure that the box will move by this amt minimally when hit. so that ObjectMovedAndStopped will work
            rigidBody.AddForce(new Vector2(0, rigidBody.mass*20), ForceMode2D.Impulse);
            // rigidBody.AddForce(Vector2.up*20, ForceMode2D.Impulse);
            // ? Force: will need to be applied over a time frame. To produce the same impulse, need to call this function 50 times (0.02s per call)
            // rigidBody.AddForce(Vector2.up*20, ForceMode2D.Force);

            // spawn the mushroom prefab slightly above the box
            // ? this: script that is attached to the box.
            // ? this.transform.position.x refers to the world position
            Instantiate(consummablePrefab, new Vector3(this.transform.position.x, this.transform.position.y + 1.0f, this.transform.position.z), Quaternion.identity);

            StartCoroutine(DisableHittable());

        }
    }

    // this will return true when the object is stationary
    // we need to be sure that the object has moved before
    bool ObjectMovedAndStopped(){
        return Mathf.Abs(rigidBody.velocity.magnitude) < 0.01;
    }

    IEnumerator DisableHittable(){
        if (!ObjectMovedAndStopped()){
            yield return new WaitUntil(() => ObjectMovedAndStopped());
        }

        //continues here when the ObjectMovedAndStopped() returns true
        spriteRenderer.sprite = usedQuestionBox; // change sprite to be "used-box" sprite
        rigidBody.bodyType = RigidbodyType2D.Static; // make the box unaffected by Physics

        //reset box position
        this.transform.localPosition = Vector3.zero;
        springJoint.enabled = false; // disable spring
    }
}
