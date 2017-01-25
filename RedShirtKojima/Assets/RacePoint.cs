using UnityEngine;
using System.Collections;

public class RacePoint : MonoBehaviour {
    public enum RP_Type
    {
        start, finish, checkpoint
    }
    public RP_Type types;
    
	// Use this for initialization
	void Start () {
        Renderer rend = GetComponent<Renderer>();
        switch (types)
        {
            case RP_Type.start:
                Material matStart = Resources.Load("waypoint 1") as Material;
                rend.material = matStart;
                break;
            case RP_Type.finish:
                Material matFinish = Resources.Load("lights.orange 1") as Material;
                rend.material = matFinish;
                break;
            case RP_Type.checkpoint:
                Material matCheck = Resources.Load("grass_roads 1") as Material;
                rend.material = matCheck;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
      
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == ("Player") && types == RP_Type.finish)
        {
            Debug.Log("finish point");
        }
    }
}
