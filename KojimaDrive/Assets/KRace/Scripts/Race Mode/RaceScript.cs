using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KRace {
	public class RaceScript : MonoBehaviour {
        //public List<Vector3> m_aRacePointPos = new List<Vector3>();
        //public List<float> m_aRacePointYRot = new List<float>();
        //List<Transform> m_aRacePointsTransform = new List<Transform>(); //Array prefix for list? ask during session
        //List<GameObject> m_aRacePoints = new List<GameObject>();
        //List<GameObject> m_aRacePointClones = new List<GameObject>();
        //List<RacePoint> m_aRacePointScripts = new List<RacePoint>();

        public List<GameObject> racePointList = new List<GameObject>();
        public RacePoint pointPrefab;

		bool m_bRaceReady = true;
		int m_nCurrentCheckpoint = 0;

		// Use this for initialization
		void Start() {
            foreach (GameObject racePoint in GameObject.FindGameObjectsWithTag("RacePoint"))
            {

               racePointList.Insert(0, racePoint);
            }

            for(int i = 0; i < racePointList.Count; i++)
            {
                object newPoint = Instantiate(pointPrefab, racePointList[i].transform.position, racePointList[i].transform.rotation);
                if(i == 0)
                {
                    
                    //Andy figure out how to change the types of the pointPrefabs here! :D
                }
            }
            

            
            //if (m_aRacePointPos[1] == Vector3.zero) {
            //	Debug.Log("List of racepoint's tranforms not set! You need to set both the rotation and the position in the prefab.");
            //	m_bRaceReady = false;
            //} else {
            //	for (int i = 0; i < m_aRacePointsTransform.Count; i++) {
            //		m_aRacePointsTransform[i].position = m_aRacePointPos[i];
            //		Quaternion tempQuaternion = Quaternion.Euler(0.0f, 0.0f, m_aRacePointYRot[i]);
            //		m_aRacePointsTransform[i].rotation = tempQuaternion;

            //		m_aRacePointClones[i] = Instantiate(m_aRacePoints[i], m_aRacePointsTransform[i].parent.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            //		m_aRacePointClones[i].transform.SetParent(m_aRacePointsTransform[i].parent, false);

            //		m_aRacePointScripts[i] = (RacePoint)m_aRacePointClones[i].GetComponent(typeof(RacePoint));
            //	}
            //	m_aRacePointScripts[0].types = RacePoint.RP_Type.START;
            //	for (int i = 1; i < m_aRacePointScripts.Count - 1; i++) {
            //		m_aRacePointScripts[i].types = RacePoint.RP_Type.CHECKPOINT;
            //	}
            //	m_aRacePointScripts[m_aRacePointScripts.Count].types = RacePoint.RP_Type.FINISH;
            //}
        }

		// Update is called once per frame
		void Update() {

		}
	}
}