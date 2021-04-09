using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NetSystem
{


    [RequireComponent(typeof(MatchMaker))]
    public class NetworkHandler : MonoBehaviour
    {
        [SerializeField] PlayerClient playerClient;
        MatchMaker matchMaker;

        private void Awake()
        {
            matchMaker = GetComponent<MatchMaker>();
            matchMaker.Init(playerClient);
        }

        public void MatchMake()
        {
            StartCoroutine(matchMaker.JoinNewGame());
        }

    }

}