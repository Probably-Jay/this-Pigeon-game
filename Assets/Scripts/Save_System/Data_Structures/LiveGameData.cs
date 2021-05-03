using System;
using System.Collections.Generic;
using SaveSystemInternal;
using Plants;

// Jay 13/03

/// <summary>
/// A datastruct to more easily store the state of a game, which will not be serialised directly. Will be converted into <see cref="SaveSystemInternal.SaveGameData"/> before being saved to file
/// </summary>
public class LiveGameData
{
    ///// <summary>
    ///// Default empty constructor
    ///// </summary>
    //public LiveGameData() { plants = new List<Plant>(); }

    ///// <summary>
    ///// Constructor for creating this object from a save file
    ///// </summary>
    ///// <param name="plantDataFromSaveFile">The list of plants that will need to be added to the game</param>
    //public LiveGameData(SerialisedPlantData[] plantDataFromSaveFile) 
    //{ 
    //    plantsToAdd = plantDataFromSaveFile; 
    //    plants = new List<Plant>(); 
    //}

    ///// <summary>
    ///// The ID of a game, set here so the associated <see cref="GameMetaData"/> structure can be recovered if  lost
    ///// </summary>
    //public string gameID;

    ///// <summary>
    ///// The goals of the garden. Stored by <see cref="Player.PlayerEnum"/> and <see cref="MoodAttributes"/>
    ///// </summary>
    //public Dictionary<Player.PlayerEnum, MoodAttributes> gardenGoals;

    ///// <summary>
    ///// The current number of points that each player has left to perform this turn. Stored by <see cref="Player.PlayerEnum"/> and <see cref="TurnPoints.PointType"/>
    ///// </summary>
    //public Dictionary<Player.PlayerEnum, Dictionary<TurnPoints.PointType,int>> actionPoints;

    ///// <summary>
    ///// List of all of the plants in the game
    ///// </summary>
    //public List<Plant> plants;

    ///// <summary>
    ///// When a game is loaded, the data about the plants will be added to this array. The plants will need to be instatniated properly
    ///// </summary>
    //public readonly SerialisedPlantData[] plantsToAdd;
     
    ///// <summary>
    ///// The current turn number. Who's turn it is can be calculated from this information
    ///// </summary>
    //public int turnNumber;



}

