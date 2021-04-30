﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace NetSystem
{
    public class NetworkGame
    {
        public PlayFab.CloudScriptModels.EntityKey GroupEntityKey => new PlayFab.CloudScriptModels.EntityKey() { Id = group.Group.Id, Type = group.Group.Type };
        public string GroupName => group.GroupName;
        public bool GameOpenToJoin => metaData.Open;

        public bool NewGameJustCreated { get; private set; }

        public List<PlayFab.CloudScriptModels.EntityKey> playerEntities => new List<PlayFab.CloudScriptModels.EntityKey>() { metaData.Player1, metaData.Player2 };


        readonly PlayFab.GroupsModels.GroupWithRoles group;
        readonly NetworkGameMetadata metaData;

        public RawData rawData;
        public UsableData usableData;

        public EnterGameContext EnteredGameContext { get; private set; }

        public NetworkGame(PlayFab.GroupsModels.GroupWithRoles group, NetworkGame.NetworkGameMetadata gameMetaData, bool newGame = false)
        {
            this.group = group;
            this.metaData = gameMetaData;
            NewGameJustCreated = newGame;
        }


        public class NetworkGameMetadata
        {
            public bool Open;
            public PlayFab.CloudScriptModels.EntityKey Player1;
            public PlayFab.CloudScriptModels.EntityKey Player2;
          //  public string SharedDataID;
        }

        public class GameSharedData 
        {
            public string Value;
            public string LastUpdated;

        }

        public class RawData
        {
            public string gameBegun;
            public string gameStartedBy;
            public string turnBelongsTo;
            public string turnComplete;

            public string time;

            public string gardenData;
            public string playerData;            
        }

        public class UsableData
        {
            public bool gameBegun;
            public string gameStartedBy;
            public string turnBelongsTo;

            public bool turnComplete;

            public string time;

            public GardenDataPacket gardenData;
            public PlayerDataPacket playerData;

            public bool NewTurn = false;

         
        }


        public bool DeserilaiseRawData(RawData rawData)
        {
            usableData = new UsableData();

            {
                bool? begun = ParseBool(rawData.gameBegun);
                if (!begun.HasValue)
                {
                    return false;
                }
                usableData.gameBegun = begun.Value;
            }

            {
                var turnComplete = ParseBool(rawData.turnComplete);
                if (!turnComplete.HasValue)
                {
                    return false;
                }
                usableData.turnComplete = turnComplete.Value;
            }

            usableData.gameStartedBy = rawData.gameStartedBy;
            usableData.turnBelongsTo = rawData.turnBelongsTo;

         
            
            
            try
            {
                usableData.playerData = JsonUtility.FromJson<PlayerDataPacket>(rawData.playerData);
                usableData.gardenData = JsonUtility.FromJson<GardenDataPacket>(rawData.gardenData);
            }
            catch (Exception e)
            {
                Debug.LogError($"Deserilisation error: {e}");
                return false;
            }

            return true;
        }

        private static bool? ParseBool(string data)
        {
            if(data == "true")
            {
                return true;
            }
            if(data == "false")
            {
                return false;
            }
            return null;
        }


        /// <summary>
        /// Takes a collection of games and sorts them into active ongoing games and open unstarted games
        /// </summary>
        /// <param name="games"></param>
        /// <param name="openGames"></param>
        /// <param name="activeGames"></param>
        public static void SeperateOpenAndClosedGames(ReadOnlyCollection<NetworkGame> games, out List<NetworkGame> openGames, out List<NetworkGame> activeGames)
        {
            openGames = new List<NetworkGame>();
            activeGames = new List<NetworkGame>();
            foreach (var game in games)
            {
                if (game.GameOpenToJoin)
                {
                    openGames.Add(game);
                }
                else
                {
                    activeGames.Add(game);
                }
            }
        }

        public void DetermineGameContext()
        {
            var playerClient = NetworkHandler.Instance.PlayerClient;

            NetUtility.EnterGameContext context = NetUtility.DetermineEnterGameContext(usableData, playerClient);

            bool createNewGame = false;
            bool initialisePlayer = false;
            bool claimingTurn = false;
            EnterGameContext.InteractionState interactionState;
            Player.PlayerEnum playerWeAre;
            switch (context)
            {
                case NetUtility.EnterGameContext.CreateNewGamePlayer1:
                    playerWeAre = Player.PlayerEnum.Player1;
                    createNewGame = true;
                    initialisePlayer = true;
                    interactionState = EnterGameContext.InteractionState.Playing;
                    break;

                case NetUtility.EnterGameContext.JoinGamePlayer2:
                    playerWeAre = Player.PlayerEnum.Player2;
                    initialisePlayer = true;
                    interactionState = EnterGameContext.InteractionState.Playing;
                    break;

                case NetUtility.EnterGameContext.ResumePlayingPlayer1:
                    playerWeAre = Player.PlayerEnum.Player1;
                    interactionState = EnterGameContext.InteractionState.Playing;
                    break;

                case NetUtility.EnterGameContext.ResumePlayingPlayer2:
                    playerWeAre = Player.PlayerEnum.Player2;
                    interactionState = EnterGameContext.InteractionState.Playing;
                    break;

                case NetUtility.EnterGameContext.ResumeClaimTurnPlayer1:
                    playerWeAre = Player.PlayerEnum.Player1;
                    claimingTurn = true;
                    interactionState = EnterGameContext.InteractionState.Spectating;
                    break;

                case NetUtility.EnterGameContext.ResumeClaimTurnPlayer2:
                    playerWeAre = Player.PlayerEnum.Player2;
                    claimingTurn = true;
                    interactionState = EnterGameContext.InteractionState.Spectating;
                    break;

                case NetUtility.EnterGameContext.ResumeSpectatingPlayer1:
                    playerWeAre = Player.PlayerEnum.Player1;
                    interactionState = EnterGameContext.InteractionState.Spectating;
                    break;

                case NetUtility.EnterGameContext.ResumeSpectatingPlayer2:
                    playerWeAre = Player.PlayerEnum.Player2;
                    interactionState = EnterGameContext.InteractionState.Spectating;
                    break;

                case NetUtility.EnterGameContext.CannotEnterUnilitilasedGamePlayer2:
                    playerWeAre = Player.PlayerEnum.Player2;
                    interactionState = EnterGameContext.InteractionState.NotEntering;
                    break;

                case NetUtility.EnterGameContext.Error:
                    return;
                default:
                    return;
            }

           EnteredGameContext = new EnterGameContext(
                createNewGame: createNewGame,
                initialisePlayer: initialisePlayer,
                interactionState: interactionState,
                claimingTurn: claimingTurn,
                playerWeAre: playerWeAre,
                enumValue: context
                );
        }

        public class EnterGameContext
        {
            public bool createNewGame;
            public bool initialisePlayer;
            public InteractionState interactionState;
            public Player.PlayerEnum playerWeAre;
            public bool claimingTurn;
            public Mood.Emotion.Emotions emotion;
            public NetUtility.EnterGameContext enumValue;

            public bool AllowedIntoGame => interactionState!= InteractionState.NotEntering;
            public SceneChangeController.Scenes SceneEntering => initialisePlayer ? SceneChangeController.Scenes.MoodSelectScreen : SceneChangeController.Scenes.Game;


            public EnterGameContext(
                bool createNewGame
                ,bool initialisePlayer
                ,bool claimingTurn
                , InteractionState interactionState
                ,Player.PlayerEnum playerWeAre
                , NetUtility.EnterGameContext enumValue
                )
            {
                this.createNewGame      = createNewGame;
                this.initialisePlayer   = initialisePlayer;
                this.interactionState   = interactionState;
                this.claimingTurn       = claimingTurn;
                this.playerWeAre        = playerWeAre;
                this.enumValue          = enumValue;
            }
            public enum InteractionState { 
                NotEntering,
                Playing,
                Spectating
            }

            public void SaveSelectedEmotion(Mood.Emotion.Emotions emotion)
            {
                this.emotion = emotion;
            }

        }



        
    }
}