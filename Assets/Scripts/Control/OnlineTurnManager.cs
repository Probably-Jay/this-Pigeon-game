using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetSystem;

namespace GameCore
{
    /// <summary>
    /// Manages a turn in the networked game, replaces <see cref="HotSeatManager"/>
    /// </summary>
    public class OnlineTurnManager : MonoBehaviour
    {
        [SerializeField] GameObject playerPrefab;




        ActiveGame Game => NetworkHandler.Instance.NetGame;
        PlayerClient PlayerClient => NetworkHandler.Instance.PlayerClient;
        public Player LocalPlayer { get; private set; }

        public OnlineTurnTracker TurnTracker { get; private set; } = new OnlineTurnTracker();

 


        private void Initialise()
        {
            LocalPlayer = InstantiatePlayer();
            LocalPlayer.Init(PlayerClient);
        //    players.Add(LocalPlayer.EnumID, LocalPlayer);
        }

        

        private Player InstantiatePlayer() 
        {
            GameObject playerObject = Instantiate(playerPrefab);
            playerObject.transform.SetParent(transform);
            Player player = playerObject.GetComponent<Player>();
            return player;
        }
    }
}