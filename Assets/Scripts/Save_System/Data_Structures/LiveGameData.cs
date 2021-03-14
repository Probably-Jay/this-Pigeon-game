using System;
using System.Collections.Generic;
using SaveSystemInternal;

// Jay 13/03

/// <summary>
/// A datastruct to more easily store the state of a game, which will not be serilaised directly. Will be converted into <see cref="SaveGameData"/>, which is made up of only serilisable types
/// </summary>
public class LiveGameData
{
    /// <summary>
    /// Default empty constructor
    /// </summary>
    public LiveGameData() { plants = new List<PlantItem>(); }

    /// <summary>
    /// Constructor for creating this object from a save file
    /// </summary>
    /// <param name="plantDataFromSaveFile">The list of plants that will need to be added to the game</param>
    public LiveGameData(SerialisedPlantData[] plantDataFromSaveFile) 
    { 
        plantsToAdd = plantDataFromSaveFile; 
        plants = new List<PlantItem>(); 
    }

    public string gameID;

    public Dictionary<Player.PlayerEnum, MoodAtributes> gardenGoals; 

    public Dictionary<Player.PlayerEnum, Dictionary<TurnPoints.PointType,int>> actionPoints;
    public List<PlantItem> plants;

    /// <summary>
    /// When a game is loaded, the data about the plants will be added to this array. The plants will need to be instatniated properly.
    /// </summary>
    public readonly SerialisedPlantData[] plantsToAdd;
     
    public int turnNumber;



}

