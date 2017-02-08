//Author:       TMS
//Description:  Script that handles a player's camera.
//Last Edit:    Yams @ 14/01/2017  

using UnityEngine;
using System.Collections;
using Rewired;

namespace Bam
{
    public class PlayerCameraScript : MonoBehaviour
    {

        [System.Serializable]
        public struct CameraInfo
        {
            public viewStyles_e m_viewStyle;
            public screenPositions_e m_positionOnScreen;

            public int m_nfollowThisPlayer;
            public bool[] m_followThesePlayers;
        }

        [System.Serializable]
        public enum screenPositions_e { topLeft, topRight, bottomLeft, bottomRight, topHalf, bottomHalf, fullScreen }

        public Camera Cam { get { return m_cam; } }
        Camera m_cam, m_UICam;

        public enum viewStyles_e { thirdPerson, overhead, driving, bonnet }
        public viewStyles_e m_currentViewStyle = viewStyles_e.thirdPerson;

        //public CarScript[] myPlayers;
        public Kojima.CarScript m_mainPlayer;
        public bool[] m_followingThesePlayers;

        float m_turnSpeed = 90;
        float m_distanceFromPlayer = 7.5f;




        //Overhead
        public Vector3 m_curPos;

        //Third person
        [SerializeField]
        float m_freeCamTimer = 0;


        float m_targetFOV = 65;

        // Use this for initialization
        void Awake()
        {

            m_followingThesePlayers = new bool[4];
            m_cam = GetComponent<Camera>();
            m_UICam = transform.FindChild("UICamera").GetComponent<Camera>();
        }

        void Start()
        {
            if (m_mainPlayer)
            {
                m_curPos = -m_mainPlayer.transform.eulerAngles;
                transform.position = m_mainPlayer.transform.position;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public Camera GetCameraComponent()
        {
            return m_cam;
        }

        public Camera GetUICameraComponent()
        {
            return m_UICam;
        }

        public void SetupCamera(CameraInfo newInfo)
        {
            m_mainPlayer = null;

            if (newInfo.m_nfollowThisPlayer > 0)
                m_mainPlayer = Kojima.GameController.s_singleton.m_players[newInfo.m_nfollowThisPlayer - 1];

            m_followingThesePlayers = newInfo.m_followThesePlayers;
            SwitchViewStyle(newInfo.m_viewStyle);

            switch (newInfo.m_positionOnScreen)
            {
                case screenPositions_e.bottomLeft:
                    MoveScreenToHere(new Vector2(0, 0), new Vector2(0.5f, 0.5f));
                    break;
                case screenPositions_e.bottomRight:
                    MoveScreenToHere(new Vector2(0.5f, 0), new Vector2(0.5f, 0.5f));
                    break;
                case screenPositions_e.topLeft:
                    MoveScreenToHere(new Vector2(0, 0.5f), new Vector2(0.5f, 0.5f));
                    break;
                case screenPositions_e.topRight:
                    MoveScreenToHere(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
                    break;

                case screenPositions_e.topHalf:
                    MoveScreenToHere(new Vector2(0.0f, 0.5f), new Vector2(1f, 0.5f));
                    break;
                case screenPositions_e.bottomHalf:
                    MoveScreenToHere(new Vector2(0.0f, 0.0f), new Vector2(1f, 0.5f));
                    break;

                case screenPositions_e.fullScreen:
                    MoveScreenToHere(new Vector2(0.0f, 0.0f), new Vector2(1f, 1f));
                    break;
            }
        }

        public void SwitchViewStyle(viewStyles_e newViewStyle)
        {
            m_currentViewStyle = newViewStyle;

            switch (newViewStyle)
            {
                case viewStyles_e.overhead:
                    m_cam.orthographic = true;
                    break;
                case viewStyles_e.thirdPerson:

                    m_cam.orthographic = false;
                    break;
                case viewStyles_e.driving:
                    m_cam.orthographic = false;

                    break;
            }

            if (m_mainPlayer == null)
            {
                for (int i = 0; i < m_followingThesePlayers.Length; i++)
                {
                    if (m_followingThesePlayers[i])
                    {
                        m_mainPlayer = Kojima.GameController.s_singleton.m_players[i];
                        StartCoroutine("ResetThirdPersonAngle");
                        i = 99;
                    }
                }
            }
            else
            {
                StartCoroutine("ResetThirdPersonAngle");
            }
        }

        IEnumerator ResetThirdPersonAngle()
        {
            float targetY = m_mainPlayer.transform.eulerAngles.y - 180;
            float amount = Mathf.Abs(targetY - m_curPos.y);
            float speed = 5;

            for (float timer = 0; timer < amount - 0.15f; timer += Time.deltaTime)
            {
                m_curPos.x = Mathf.LerpAngle(m_curPos.x, 15, speed * Time.deltaTime);
                m_curPos.y = Mathf.LerpAngle(m_curPos.y, targetY, speed * Time.deltaTime);

                timer = Mathf.Lerp(timer, amount, speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            //Debug.Log("DONE");
        }

        Vector3 GetTargetPos(Quaternion rot, Vector3 backwards)
        {
            return m_mainPlayer.transform.position + (rot * backwards);
        }

        void DriveCam(Vector3 input)
        {
            float camSpeed = 12, camRotationSpeed = 7.5f;
            float backDistance = 5.25f;
            float upDistance = 2.5f;

            Vector3 offset = input;
            Quaternion finalRotation = transform.rotation;
            Vector3 forward = Vector3.forward;

            Vector3 target = m_mainPlayer.transform.position + m_mainPlayer.transform.forward * backDistance + m_mainPlayer.transform.up * upDistance;

            target = DetectWalls(target);

            if (m_mainPlayer.AllWheelsGrounded)
            {
                forward = Quaternion.LookRotation(-m_mainPlayer.transform.forward, Vector3.up).eulerAngles;
            }
            else
            {
                target =  m_mainPlayer.transform.position + m_mainPlayer.transform.forward * backDistance + Vector3.up * upDistance;
                forward = Quaternion.LookRotation(-m_mainPlayer.transform.forward, Vector3.up).eulerAngles;
            }

            finalRotation = Quaternion.Euler(forward - input * 45);
            transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, camRotationSpeed * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, target, camSpeed * Time.deltaTime);
            AllowBehindCam();
        }

        Vector3 DetectWalls(Vector3 targetPos)
        {
            Vector3 returnVector = targetPos;

            RaycastHit rH;
            if (Physics.Linecast(m_mainPlayer.transform.position + Vector3.up, targetPos, out rH, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
            {
                //Debug.Log(rH.collider.gameObject.name);
                //Debug.Log(mainPlayer.transform.forward);

                m_curPos.x += m_mainPlayer.transform.forward.y * Time.deltaTime * 10;

                //Debug.Log(Vector3.Distance(transform.position, mainPlayer.transform.position));
                if (Vector3.Distance(transform.position, m_mainPlayer.transform.position) < 2)
                {
                    m_curPos.x += 45 * Time.deltaTime;
                }

                if (Vector3.Distance(transform.position, m_mainPlayer.transform.position) < 6)
                {
                    m_curPos.x += 15 * Time.deltaTime;
                }

                returnVector = rH.point + rH.normal * 0.35f;
            }

            return returnVector;
        }

        void AllowBehindCam()
        {
            Vector3 target;
            //Looking behind
            if (m_mainPlayer.GetRewiredPlayer().GetButton("Look Behind"))
            {
                StopCoroutine("ResetThirdPersonAngle");
                m_freeCamTimer = 1;
                target = m_mainPlayer.transform.position - m_mainPlayer.transform.forward * 9.5f + m_mainPlayer.transform.up * 3;

                transform.position = DetectWalls(target);
                transform.rotation = Quaternion.LookRotation(m_mainPlayer.transform.forward + Vector3.up*0.155f);
                //m_curPos.y = Mathf.LerpAngle(m_curPos.y, m_mainPlayer.transform.eulerAngles.y, 25 * Time.deltaTime);
            }
            if (m_mainPlayer.GetRewiredPlayer().GetButtonUp("Look Behind"))
            {
                StartCoroutine("ResetThirdPersonAngle");
                transform.position = m_mainPlayer.transform.position + m_mainPlayer.transform.forward * 5 + Vector3.up * 2;
                transform.rotation = Quaternion.LookRotation(-m_mainPlayer.transform.forward, m_mainPlayer.transform.up);
            }
        }

        void ThirdPerson(Vector3 input)
        {
            if (!m_mainPlayer)
                return;

            m_targetFOV = 60 + m_mainPlayer.m_fcurrentWheelSpeed * 0.15f;
            m_curPos += -input * m_turnSpeed * Time.deltaTime;
            m_curPos.x = Mathf.Clamp(m_curPos.x, 10, 60);

            if (input.magnitude > 0.1f)
            {
                StopCoroutine("ResetThirdPersonAngle");
                m_freeCamTimer = 1.2f;
            }

            //Handles the "automatic" third person camera
            if (m_freeCamTimer <= 0 && !m_mainPlayer.InMidAir)
            {
                Vector3 targetCurPos = m_mainPlayer.transform.eulerAngles;
                targetCurPos.y -= 180;

                if (m_mainPlayer.GetDrifting)
                {
                    Vector3 veloDir = Quaternion.LookRotation(m_mainPlayer.GetVelocity()).eulerAngles;
                    m_curPos.y = Mathf.LerpAngle(m_curPos.y, veloDir.y, 0.1f * Time.deltaTime);
                }
                else
                {
                    m_curPos.y = Mathf.LerpAngle(m_curPos.y, Quaternion.Euler(targetCurPos).eulerAngles.y, 2 * Time.deltaTime);
                }

                m_curPos.x = Mathf.LerpAngle(m_curPos.x, 25, 1 * Time.deltaTime);
            }
            else
            {
                m_freeCamTimer -= Time.deltaTime;
            }

            float distanceMultiplier = 1;

            if (m_mainPlayer.AllWheelsGrounded)
            {
                //distanceMultiplier += Mathf.Abs(m_mainPlayer.transform.up.x * 1.25f);
            }

            Vector3 backwards = new Vector3(0, 0, -(m_distanceFromPlayer * distanceMultiplier));
            Quaternion rot = Quaternion.Euler(m_curPos);

            Vector3 targetPos = Vector3.zero;

            if (m_mainPlayer)
            {
                targetPos = m_mainPlayer.transform.position + (rot * backwards);
            }

            //TODO: Rewired
            if (m_mainPlayer.GetRewiredPlayer().GetButtonDown("Reset Camera"))
            {
                StartCoroutine("ResetThirdPersonAngle");
            }

            targetPos = DetectWalls(targetPos);

            transform.position = Vector3.Lerp(transform.position, targetPos + m_mainPlayer.GetVelocity() * Time.deltaTime, 8 * Time.deltaTime);
            transform.LookAt(m_mainPlayer.transform.position);

            AllowBehindCam();
        }

        Vector3 GetAveragePosition(Kojima.CarScript[] players)
        {
            Vector3 pos = Vector3.zero;

            Vector3[] playerPos = new Vector3[players.Length];
            int playersToFollow = 0;

            for (int i = 0; i < playerPos.Length; i++)
            {
                if (players[i] && m_followingThesePlayers[i])
                {
                    playerPos[i] = players[i].transform.position;
                    playersToFollow++;
                }
            }

            pos = GetAveragePosition(playerPos, playersToFollow);

            return pos;
        }

        Vector3 GetAveragePosition(Vector3[] positions, float divide = 0)
        {
            Vector3 pos = Vector3.zero;

            if (divide == 0)
                divide = positions.Length;

            float multiplier = 1 / (float)divide;

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] *= multiplier;
                pos += positions[i];
            }

            return pos;
        }

        void Overhead(Vector3 input)
        {
            bool orthographicMode = true;

            Vector3 pos = Vector3.zero;

            if (!m_mainPlayer)
                pos = GetAveragePosition(Kojima.GameController.s_singleton.m_players);
            else
                pos = m_mainPlayer.transform.position;

            float height = 15;
            float distance = 0;

            for (int i = 0; i < Kojima.GameController.s_singleton.m_players.Length - 1; i++)
            {
                if (i < Kojima.GameController.s_singleton.m_players.Length)
                {
                    if (Kojima.GameController.s_singleton.m_players[i] && Kojima.GameController.s_singleton.m_players[i + 1] && m_followingThesePlayers[i])
                    {
                        distance += Vector3.Distance(Kojima.GameController.s_singleton.m_players[i].transform.position, Kojima.GameController.s_singleton.m_players[i + 1].transform.position);
                    }
                }
            }

            m_targetFOV = 100 + distance * 0.35f;
            m_targetFOV = Mathf.Clamp(m_targetFOV, 80, 120);

            if (orthographicMode)
            {
                //cam.orthographic = true;
                m_cam.orthographicSize = 10 + distance * 0.5f;
                distance = 100;
            }
            else
            {
                //cam.orthographic = false;
            }

            height += distance * 0.1f;

            pos += Vector3.up * height;

            transform.position = Vector3.Lerp(transform.position, pos, 8 * Time.deltaTime);

            transform.rotation = Quaternion.LookRotation(-Vector3.up);
        }

        void BonnetCam()
        {
            transform.position = Vector3.Lerp(transform.position, m_mainPlayer.transform.position - m_mainPlayer.transform.forward * 3 + m_mainPlayer.transform.up * 1.5f, 10 * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(-m_mainPlayer.transform.forward, m_mainPlayer.transform.up);
        }

        void TakeInput(out Vector3 stickInput)
        {
            stickInput = Vector3.zero;

            if (m_mainPlayer)
            {
                Rewired.Player player = ReInput.players.GetPlayer(m_mainPlayer.m_nplayerIndex - 1);
                stickInput = new Vector3(player.GetAxisRaw("Look Vertical"), -player.GetAxisRaw("Look Horizontal"), 0);

                if(player.GetButtonDown("Change Camera"))
                {
                    switch(m_currentViewStyle)
                    {
                        case viewStyles_e.bonnet:
                            SwitchViewStyle(viewStyles_e.driving);
                            break;
                        case viewStyles_e.driving:
                            SwitchViewStyle(viewStyles_e.thirdPerson);
                            break;
                        case viewStyles_e.thirdPerson:
                            SwitchViewStyle(viewStyles_e.bonnet);
                            break;
                    }
                }
            }
        }

        void FixedUpdate()
        {
            Vector3 stickInput = Vector3.zero;

            TakeInput(out stickInput);

            //TODO:Add rewired
            switch (m_currentViewStyle)
            {
                case viewStyles_e.thirdPerson:
                    ThirdPerson(stickInput);
                    break;
                case viewStyles_e.overhead:
                    Overhead(stickInput);
                    break;
                case viewStyles_e.driving:
                    DriveCam(stickInput);
                    break;
                case viewStyles_e.bonnet:
                    BonnetCam();
                    break;
            }

            m_cam.fieldOfView = Mathf.Lerp(m_cam.fieldOfView, m_targetFOV, 4 * Time.deltaTime);
            m_UICam.fieldOfView = m_cam.fieldOfView;
        }

        public void MoveScreenToHere(Vector2 _newPos, Vector2 _size)
        {
            Rect newRect = m_cam.rect;
            newRect.position = _newPos;
            newRect.size = _size;

            m_cam.rect = newRect;
            m_UICam.rect = m_cam.rect;
        }
    }
}