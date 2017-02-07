//Author:       TMS
//Description:  Script that creates skid marks when the attached CarScript reports that it is skidding
//              
//Last edit:    TMS @ 04/02/2017

using UnityEngine;
using System.Collections;


namespace Bam
{
    public class SkidEffectsScript : MonoBehaviour
    {
        public GameObject m_skidSmokePrefab;
        public GameObject m_skidMarkPrefab;

        TrailRenderer[] m_skidTrails;
        ParticleSystem[] m_skidMarkParticles;
        ParticleSystem[] m_skidSmokeParticles;

        bool[] m_trailActive = new bool[4];

        Kojima.CarScript m_myCar;

        void Awake()
        {
            m_myCar = GetComponent<Kojima.CarScript>();

            CreateTrails();
        }

        // Use this for initialization
        void Start()
        {

        }

        void CreateTrails()
        {
            m_skidTrails = new TrailRenderer[4];
            m_skidMarkParticles = new ParticleSystem[4];
            m_skidSmokeParticles = new ParticleSystem[4];

            for (int i=0; i<m_skidTrails.Length; i++)
            {
                m_skidTrails[i] = Instantiate<GameObject>(m_skidMarkPrefab).GetComponent<TrailRenderer>();
                m_skidMarkParticles[i] = m_skidTrails[i].GetComponent<ParticleSystem>();

                m_skidSmokeParticles[i] = Instantiate<GameObject>(m_skidSmokePrefab).GetComponent<ParticleSystem>();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void LateUpdate()
        {
            RaycastHit[] wheelCasts = m_myCar.GetWheelRaycasts;

            for (int i = 0; i < m_skidTrails.Length; i++)
            {               
                if (m_myCar.IsSkidding && m_myCar.m_wheelIsGrounded[i])
                {
                    //Replace all the stuff in here with a better skidmark system if you like (one that builds a mesh etc) 
                    m_skidSmokeParticles[i].transform.position = wheelCasts[i].point;
                    m_skidTrails[i].transform.position = wheelCasts[i].point + wheelCasts[i].normal * 0.05f;

                    if(m_trailActive[i])
                    {

                    }
                    else
                    {
                        m_skidSmokeParticles[i].Play();
                        m_skidMarkParticles[i].Play();
                        m_trailActive[i] = true;
                    }
                }
                else
                {
                    m_skidSmokeParticles[i].Stop();
                    m_skidMarkParticles[i].Pause();
                    m_trailActive[i] = false;
                }

            }
        }
    }
}