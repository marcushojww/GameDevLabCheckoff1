using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMushroom : MonoBehaviour, ConsumableInterface
{
    public  Texture t;
    private SpriteRenderer redMushroomSprite;

    void Start()
    {
        redMushroomSprite = GetComponent<SpriteRenderer>();
    }
	public  void  consumedBy(GameObject player){
		// give player jump boost
		player.GetComponent<PlayerController>().upSpeed  +=  10;
		StartCoroutine(removeEffect(player));
	}

    void  OnCollisionEnter2D(Collision2D col) {

        if (col.gameObject.CompareTag("Player")){
            // update UI
            CentralManager.centralManagerInstance.addPowerup(t, 1, this);
            GetComponent<Collider2D>().enabled  =  false;
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
        redMushroomSprite.enabled = false;
        yield break;
    }

	IEnumerator  removeEffect(GameObject player){
		yield  return  new  WaitForSeconds(5.0f);
		player.GetComponent<PlayerController>().upSpeed  -=  10;
	}
}
