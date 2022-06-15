using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.onIncreaseScore += spawnNewEnemySequence;
        // spawn two gombaEnemy
        for (int j =  0; j  <  1; j++)
            spawnFromPooler(ObjectType.gombaEnemy);
            spawnFromPooler(ObjectType.greenEnemy);
    }
    void  spawnFromPooler(ObjectType i){
        
        // static method access
        GameObject item =  ObjectPooler.SharedInstance.GetPooledObject(i);
        if (item  !=  null){
            
            if (i == ObjectType.greenEnemy) {
                //set position, and other necessary states
                item.transform.position  =  new  Vector3(Random.Range(-4.5f, 4.5f), -3.04f, 0);
            }
            if (i == ObjectType.gombaEnemy) {
                //set position, and other necessary states
                item.transform.position  =  new  Vector3(Random.Range(-4.5f, 4.5f), -3.56f, 0);
            }
            
            item.SetActive(true);
        }
        else{
            Debug.Log("not enough items in the pool.");
        }
    }
    void spawnNewEnemySequence() {
        StartCoroutine(SpawnTimer());
    }
    IEnumerator SpawnTimer() {
        yield return new WaitForSeconds(2);
        int rand = Random.Range(0,2);
        if (rand == 0) {
            spawnFromPooler(ObjectType.gombaEnemy); 
        }
        if (rand == 1) {
            spawnFromPooler(ObjectType.greenEnemy);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
