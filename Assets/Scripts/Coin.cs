using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpriteRenderer coinSprite;

    // Start is called before the first frame update
    void Start()
    {
        coinSprite = GetComponent<SpriteRenderer>();
    }

    void  OnCollisionEnter2D(Collision2D col) {

        if (col.gameObject.CompareTag("Player")){
            GetComponent<Collider2D>().enabled  =  false;
            CentralManager.centralManagerInstance.increaseScore();
            StartCoroutine(enlarge());
        }
    }

    IEnumerator enlarge() {
        int steps = 2;
        float stepper =  1.0f/(float) (steps + 8);
        for (int i =  0; i  <  steps; i  ++){
            this.transform.localScale  =  new  Vector3(this.transform.localScale.x + stepper, this.transform.localScale.y  +  stepper, this.transform.localScale.z);
            yield  return  null;
        }
        coinSprite.enabled = false;
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
