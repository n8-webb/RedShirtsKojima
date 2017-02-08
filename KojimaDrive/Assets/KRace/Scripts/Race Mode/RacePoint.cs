using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


namespace KRace
{
	public class RacePoint : MonoBehaviour
	{
		//sets up our enum for ease of use when making races
		public enum RP_Type
		{
			START, FINISH, CHECKPOINT
		}
		public RP_Type types;

		public Transform[] startGrid;
		public GameObject[] playerList;

		public Vector3[] gridPositions;
        public List<GameObject> checkPointList = new List<GameObject>();
        public int size;
        public Text checkpointsText;
		
		
		 

		Renderer rend;

		// Use this for initialization
		void Start()
		{

            Material matStart = Resources.Load("wpStart") as Material;
            Material matCheck = Resources.Load("wpCheckPoint") as Material;
            Material matFinish = Resources.Load("wpFinish") as Material;
            rend = GetComponent<Renderer>();
			switch (types)
			{
				case RP_Type.START:
					rend.material = matStart;
					InitialiseGrid(playerList.Length);
                    gameObject.tag = "StartPoint";
					SpawnAtGrid();
					break;
				case RP_Type.FINISH:
					rend.material = matFinish;
                    gameObject.tag = "FinishPoint";
                    break;
				case RP_Type.CHECKPOINT:
					rend.material = matCheck;
                    gameObject.tag = "CheckPoint";
                    
					break;
			}

            foreach (GameObject racePoint in GameObject.FindGameObjectsWithTag("CheckPoint"))
            {

                checkPointList.Insert(checkPointList.Count,racePoint);
            }


        }

		// Update is called once per frame
		void Update()
		{
           
		}

		void SpawnAtGrid()
		{


			for (int i = 0; i < playerList.Length; i++)
			{
				playerList [i].transform.position = gridPositions[i];
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
				GameObject gp = Instantiate(Resources.Load("GridPoint") as GameObject, gridPositions[i], this.transform.rotation) as GameObject;
				gp.transform.parent = this.transform;
			}

		}

		void OnTriggerEnter(Collider col)
		{
			if (col.gameObject.tag == ("Player") && types == RP_Type.FINISH)
			{
				Debug.Log("finish point");
			}

            if (col.gameObject.tag == ("Player") && types == RP_Type.CHECKPOINT)
            {
                Debug.Log("passed checkpoint");
               
                CheckPointCount(checkPointList.IndexOf(this.gameObject) +1);
            }
        }


        void CheckPointCount(int checkNum)
        {

            checkpointsText.text = checkNum.ToString();
        }
	}

}