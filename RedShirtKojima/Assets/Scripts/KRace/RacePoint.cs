using UnityEngine;
using System.Collections;

public class RacePoint : MonoBehaviour {
	//sets up our enum for ease of use when making races
    public enum RP_Type
    {
        start, finish, checkpoint
    }
    public RP_Type types;

	public Transform[] startGrid; 
	public GameObject[] playerList;

	public Vector3[] gridPositions;
		

    
	// Use this for initialization
	void Start () {
		

        Renderer rend = GetComponent<Renderer>();
        switch (types)
        {
		case RP_Type.start:
			Material matStart = Resources.Load ("waypoint 1") as Material;
			rend.material = matStart;
			InitialiseGrid(playerList.Length);
			SpawnAtGrid();
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

	void SpawnAtGrid(){


		for (int i = 0; i < playerList.Length; i++)
		{
			//playerList [i].transform.position = startGrid[i].position;
			//playerList [i].transform.rotation = startGrid[i].rotation;


		}
	}

	void InitialiseGrid(int gridSize)
	{
		//this sets up where the starting grid positions are from our start point
		gridPositions[0] = (this.transform.position - new Vector3(2.0f, 0.3f, 0.0f));
		gridPositions[1] = (this.transform.position - new Vector3(4.0f, 0.3f, 0.0f));
		gridPositions[2] = (this.transform.position - new Vector3(6.0f, 0.3f, 0.0f));
		gridPositions[3] = (this.transform.position - new Vector3(8.0f, 0.3f, 0.0f));



		//this loop creates the starting grid as children of the start point so it inherits its transform
		for (int i = 0; i < gridSize; i++)
		{
			GameObject gp = Instantiate (Resources.Load ("GridPoint") as GameObject, gridPositions [i], this.transform.rotation) as GameObject;
			gp.transform.parent = this.transform;
		}

	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == ("Player") && types == RP_Type.finish)
        {
            Debug.Log("finish point");
        }
    }
}
