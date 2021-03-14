using System;
using System.Collections.Generic;

// Jay 13/03

/// <summary>
/// A datastruct to more easily store the state of a game, which will not be serilaised directly. Will be converted into <see cref="SaveGameData"/>, which is made up of only serilisable types
/// </summary>
public class LiveGameData
{
    public Dictionary<Player.PlayerEnum, MoodAtributes> gardenGoals; 

    public Dictionary<Player.PlayerEnum, Dictionary<TurnPoints.PointType,int>> actionPoints;
    public List<PlantItem> plants;
     
    public int turnNumber;



}

