using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarDraft : MonoBehaviour {

    public GameObject triggerPrefrab;
    List<GameObject> carDraftTriggers;
    float timer;
    float spawnRate;

	// Use this for initialization
	void Start () {
        timer = 0.0f;
        spawnRate = 0.2f;

        carDraftTriggers = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

        if(timer >= spawnRate)
        {
            timer = 0.0f;
            GameObject newTrigger = Instantiate(triggerPrefrab);
            newTrigger.transform.position = transform.position;
            newTrigger.GetComponent<DraftTrigger>().parentName = transform.name;
            carDraftTriggers.Add(newTrigger);

        }

        /*
        foreach (GameObject trigger in carDraftTriggers)
        {
            DraftTrigger triggerScript = trigger.GetComponent<DraftTrigger>();
            
            if (triggerScript.getDead())
            {
                
                carDraftTriggers.Remove(trigger);
                GameObject.Destroy(trigger);
                
            }
        }*/
    }
}
