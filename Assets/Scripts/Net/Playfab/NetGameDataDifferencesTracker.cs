using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// jay 03/05

namespace NetSystem
{

    public class NetGameDataDifferencesTracker
    {
        NetworkGame owningGame;
        public NetGameDataDifferencesTracker(NetworkGame game)
        {
            owningGame = game;
        }

        NetworkGame.UsableData CurrentGameData => owningGame.CurrentGameData;

        DataDifferences DifferencesInGameData { get; set; } = new DataDifferences();

        /// <summary>
        /// Non-comprehensive class for the differences between the last times the game data was gathered
        /// </summary>
        public class DataDifferences
        {
            public bool AnyDifferences
            {
                get
                {
                    bool diff = false;
                    diff |= turnCompleteIsDifferent;
                    diff |= turnOwnerIsDifferent;
                    diff |= player1HasWonIsDifferent;
                    diff |= player2HasWonIsDifferent;
                    diff |= plantDifferences.Count > 0;
                    return diff;
                }
            }


            public bool turnCompleteIsDifferent = false;
            public bool turnOwnerIsDifferent = false;
            public bool player1HasWonIsDifferent = false;
            public bool player2HasWonIsDifferent = false;

            public List<PlantDifferences> plantDifferences = new List<PlantDifferences>();

            public class PlantDifferences
            {

                public Player.PlayerEnum garden;
                public int slot;

                public bool HasDifferences => planted || removed || tended || grown;

                public bool planted;
                public bool removed;
                /// <summary>
                /// If the plants tendind stat is *at all* different than it was. Does not take into account if the plant has grown
                /// </summary>
                public bool tended;
                public bool grown;

                internal void SetDifferencesAddedPlant((GardenDataPacket.Plant plant, Player.PlayerEnum garden) newPlant)
                {
                    garden = newPlant.garden;
                    slot = newPlant.plant.slotNumber;

                    planted = true;
                    removed = false;
                    tended = false;
                    grown = false;
                }

                internal void SetDifferencesRemovedPlant((GardenDataPacket.Plant plant, Player.PlayerEnum garden) oldPlant)
                {

                    garden = oldPlant.garden;
                    slot = oldPlant.plant.slotNumber;

                    planted = false;
                    removed = true;
                    tended = false;
                    grown = false;
                }



                internal void SetDifferencesComparison((GardenDataPacket.Plant plant, Player.PlayerEnum garden) oldPlant, (GardenDataPacket.Plant plant, Player.PlayerEnum garden) newPlant)
                {
                    garden = oldPlant.garden;
                    slot = oldPlant.plant.slotNumber;

                    planted = false;
                    removed = false;

                    grown = oldPlant.plant.stage != newPlant.plant.stage;

                    tended = oldPlant.plant.watering != newPlant.plant.watering
                             || oldPlant.plant.spraying != newPlant.plant.spraying
                             || oldPlant.plant.trimming != newPlant.plant.trimming;
                }
            }

        }



        /// <summary>
        /// Must be called after applying new data to reset the data differences tracker
        /// </summary>
        public void ResetGameDataDifferences()
        {
            DifferencesInGameData = new DataDifferences();
        }





        public DataDifferences CompareNewGameData(NetworkGame.UsableData newData)
        {
            if (CurrentGameData.turnComplete != newData.turnComplete)
            {
                DifferencesInGameData.turnCompleteIsDifferent = true;
            }

            if (CurrentGameData.turnBelongsTo != newData.turnBelongsTo)
            {
                DifferencesInGameData.turnOwnerIsDifferent = true;
            }

            if (CurrentGameData.playerData.player1MoodAchieved != newData.playerData.player1MoodAchieved)
            {
                DifferencesInGameData.player1HasWonIsDifferent = true;
            }

            if (CurrentGameData.playerData.player2MoodAchieved != newData.playerData.player2MoodAchieved)
            {
                DifferencesInGameData.player1HasWonIsDifferent = true;
            }

            DifferencesInGameData.plantDifferences = GetDifferentPlants(newData);

            return DifferencesInGameData;

        }

        private List<DataDifferences.PlantDifferences> GetDifferentPlants(NetworkGame.UsableData newData)
        {
            var currentGarden1 = CurrentGameData.gardenData.newestGarden1;
            var currentGarden2 = CurrentGameData.gardenData.newestGarden2;
            GetPlants(out List<(GardenDataPacket.Plant, Player.PlayerEnum garden)> currentPlants, currentGarden1, currentGarden2);

            var newGarden1 = CurrentGameData.gardenData.newestGarden1;
            var newGarden2 = CurrentGameData.gardenData.newestGarden2;
            GetPlants(out List<(GardenDataPacket.Plant, Player.PlayerEnum garden)> newPlants, newGarden1, newGarden2);

            MatchPlants(out var matchedPlants, currentPlants, newPlants);

            DetermineDifferences(out var differences, matchedPlants);

            return differences;

        }

        private void DetermineDifferences(out List<DataDifferences.PlantDifferences> differences, List<((GardenDataPacket.Plant plant, Player.PlayerEnum garden) oldPlant, (GardenDataPacket.Plant plant, Player.PlayerEnum garden) newPlant)> matchedPlants)
        {
            differences = new List<DataDifferences.PlantDifferences>();
            foreach (var item in matchedPlants)
            {
                var plantDifferences = new DataDifferences.PlantDifferences();

                if (item.oldPlant.plant == null)
                {
                    plantDifferences.SetDifferencesAddedPlant(item.newPlant);
                    differences.Add(plantDifferences);
                    continue;
                }

                if (item.newPlant.plant == null)
                {
                    plantDifferences.SetDifferencesRemovedPlant(item.oldPlant);
                    differences.Add(plantDifferences);

                    continue;
                }

                plantDifferences.SetDifferencesComparison(item.oldPlant, item.newPlant);
                if (plantDifferences.HasDifferences)
                {
                    differences.Add(plantDifferences);
                }
            }
        }

        private static void GetPlants(out List<(GardenDataPacket.Plant, Player.PlayerEnum garden)> plantsList, GardenDataPacket.Plant[] garden1, GardenDataPacket.Plant[] garden2)
        {
            plantsList = new List<(GardenDataPacket.Plant, Player.PlayerEnum garden)>();
            for (int i = 0; i < garden1.Length; i++)
            {
                var plantG1 = garden1[i];
                var plantG2 = garden2[i];

                if (plantG1.Initilised)
                {
                    plantsList.Add((plantG1, Player.PlayerEnum.Player1));
                }

                if (plantG2.Initilised)
                {
                    plantsList.Add((plantG2, Player.PlayerEnum.Player1));
                }
            }
        }

        private static void MatchPlants(
            out List<((GardenDataPacket.Plant plant, Player.PlayerEnum garden) oldPlants, (GardenDataPacket.Plant plant, Player.PlayerEnum garden) newPlants)> matchedPlants, // plant has no member for which garden it is in, so this has to be stored seperatley
            List<(GardenDataPacket.Plant plant, Player.PlayerEnum garden)> currentPlants,
            List<(GardenDataPacket.Plant plant, Player.PlayerEnum garden)> newPlants)
        {
            matchedPlants = new List<((GardenDataPacket.Plant plant, Player.PlayerEnum garden) oldPlants, (GardenDataPacket.Plant plant, Player.PlayerEnum garden) newPlants)>();

            // catch matches 
            foreach (var currentPlant in currentPlants)
            {
                bool match = false;
                foreach (var newPlant in newPlants)
                {
                    if (PlantsAreSamePlant(currentPlant, newPlant))
                    {
                        match = true;
                        matchedPlants.Add((currentPlant, newPlant));
                        break;
                    }
                }

                // catch removed plants
                if (match == false)
                {
                    matchedPlants.Add((currentPlant, (null, (Player.PlayerEnum)(-1))));
                }
            }

            // catch added plants
            foreach (var newPlant in newPlants)
            {
                bool match = false;
                foreach (var currentPlant in currentPlants)
                {
                    if (PlantsAreSamePlant(currentPlant, newPlant))
                    {
                        match = true;
                        // already added
                        break;
                    }
                }

                if (match == false)
                {
                    matchedPlants.Add(((null, (Player.PlayerEnum)(-1)), newPlant));
                }
            }
        }

        private static bool PlantsAreSamePlant((GardenDataPacket.Plant plant, Player.PlayerEnum garden) currentPlant, (GardenDataPacket.Plant plant, Player.PlayerEnum garden) newPlant)
        {
            return currentPlant.garden == newPlant.garden && currentPlant.plant.slotNumber == newPlant.plant.slotNumber;
        }
    }
}