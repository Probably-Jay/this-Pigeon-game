using System;
using UnityEngine;
using System.Linq;

// jay 11/03

/// <summary>
/// Datastructure for holding the full data from a game in a serilaisable form. This is the final form of data before it is serialised, and it is deserialised to this structure.
/// This will not be the direct interface for collecting data from objects during the game - this will be performed by <see cref="LiveGameData"/>
/// </summary>
[Serializable]
public class SaveGameData
{
    const int NUMBEROFPLAYERS = 2;
    const int NUMBEROFATTRIBUTESCALES = 2;
    const int NUMBEROFACTIONPOINTTYPES = 2;


    public SaveGameData(string localID)
    {
        initialised = true;
        localGameID = localID;

    }

    /// <summary>
    /// Copy constructor, deep-copies/clones all memebers from <paramref name="other"/> 
    /// </summary>
    /// <param name="other">A <see cref="SaveGameData"/> that should be copied</param>
    public SaveGameData(SaveGameData other)
    {
        // keep this up to date, deepcopy non-value types (any no literal, any collection)

        initialised     = other.initialised;
        localGameID     = other.localGameID;

        p1gardenGoals   = (int[])other.p1gardenGoals.Clone();
        p2gardenGoals   = (int[])other.p2gardenGoals.Clone();

        p1actionPoints  = (int[])other.p1actionPoints.Clone();
        p2actionPoints  = (int[])other.p2actionPoints.Clone();

        plants          = (SerialisedPlantData[])other.plants.Clone();

        testValue       = other.testValue;
        hash            = (byte[])other.hash.Clone() ;
    }

    public bool initialised;
    public string localGameID;

    public int[] p1gardenGoals;
    public int[] p2gardenGoals;

    public int[] p1actionPoints;
    public int[] p2actionPoints;

    public SerialisedPlantData[] plants;


    public int testValue;

    /// <summary>
    /// A hash of the data of this struct (excluting the <see cref="hash"/> variable), to detect corruption in loading data
    /// </summary>
    public byte[] hash;
    /// <summary>
    /// Calls <see cref="SaveSystemInternal.SaveDataSerialiser.ValidateHash"/> to compare the data to it's hash
    /// </summary>
    public bool HashIsValid => SaveSystemInternal.SaveDataSerialiser.ValidateHash(this);

}


/// <summary>
/// 
/// </summary>
[Serializable]
public class SerialisedPlantData
{

}