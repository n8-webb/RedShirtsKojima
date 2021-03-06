﻿using UnityEngine;
using System.Collections;

namespace Bam
{
    public class MainHUDScript : MonoBehaviour
    {
        public static MainHUDScript singleton;
        public PlayerHUDScript[] playerHUDs;
        public GameObject playerHUDPrefab;



        void Awake()
        {
            singleton = this;
            playerHUDs = new PlayerHUDScript[4];

            
        }

        // Use this for initialization
        void Start()
        {
            for (int i = 0; i < Kojima.GameController.s_ncurrentPlayers; i++)
            {
                GameObject newPlayerHUD = Instantiate(playerHUDPrefab) as GameObject;
                playerHUDs[i] = newPlayerHUD.GetComponent<PlayerHUDScript>();
            }

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateTimer(int mins, int seconds)
        {
            for (int i = 0; i < 4; i++)
            {
                playerHUDs[i].DisplayTimer(mins, seconds);
            }
        }

        public void UpdateScore(int score, int playerNum)
        {
            playerHUDs[playerNum - 1].DisplayScore(score);
        }

        public void ToggleHUDLights(bool lightsOn)
        {
            for (int i = 0; i < playerHUDs.Length; i++)
            {
                if (playerHUDs[i])
                {
                    playerHUDs[i].ToggleLights(lightsOn);
                }
            }
        }

        public void ShowNextItem(int playerNum, string itemName)
        {
            playerHUDs[playerNum - 1].ShowNextItem(itemName);
        }
    }
}