using System;
using UnityEngine;
using System.Linq;

// jay 11/03
namespace SaveSystemInternal
{
    /// <summary>
    /// Datastructure for holding the full data from a game in a serialisable form. This is the final form of data before it is serialised, and it is deserialised to this structure.
    /// This will not be the direct interface for collecting data from objects during the game. For that, see <see cref="LiveGameData"/>
    /// </summary>
    [Serializable]
    public class SaveGameData
    {

        /// <summary>
        /// Default constructor of the serialisable save data structure. Requires <paramref name="localID"/> so that <see cref="localGameID"/> will never be lost
        /// </summary>
        /// <param name="localID">The ID of the game this data describes</param>
        public SaveGameData(string localID)
        {
            initialised = true;
            localGameID = localID;

            // these need to be initialised as they are a constant size
            p1gardenGoals   = new int[Player.NumberOfPlayers * MoodAttributes.NumberOfAtributeScales];
            p2gardenGoals   = new int[Player.NumberOfPlayers * MoodAttributes.NumberOfAtributeScales];

            p1actionPoints  = new int[Player.NumberOfPlayers * TurnPoints.NumberOfPointTypes];
            p2actionPoints  = new int[Player.NumberOfPlayers * TurnPoints.NumberOfPointTypes];

            // hash gurenteed to be initialised before serialisation

        }

        /// <summary>
        /// Copy constructor, deep-copies/clones all memebers from <paramref name="other"/> 
        /// </summary>
        /// <param name="other">A <see cref="SaveGameData"/> that should be copied</param>
        public SaveGameData(SaveGameData other)
        {
            // keep this up to date, deepcopy non-value types (any no literal, any collection)

            initialised = other.initialised;
            localGameID = other.localGameID;

            p1gardenGoals = (int[])other.p1gardenGoals.Clone();
            p2gardenGoals = (int[])other.p2gardenGoals.Clone();

            p1actionPoints = (int[])other.p1actionPoints.Clone();
            p2actionPoints = (int[])other.p2actionPoints.Clone();

            plants = (SerialisedPlantData[])other.plants.Clone();

            turnNumber = other.turnNumber;

            testValue = other.testValue;
            hash = (byte[])other.hash.Clone();
        }

        public bool initialised;
        /// <summary>
        /// The ID of a game, set here so the associated <see cref="GameMetaData"/> structure can be recovered if  lost
        /// </summary>
        public string localGameID;

        /// <summary>
        /// The mood goal for <see cref="Player.PlayerEnum.Player1"/>. This has to be a 1D array due to serialisation limitations (hence the breach of this Pigeon best-practices regarding diferent conceptual-type seperation)
        /// </summary>
        public int[] p1gardenGoals;
        /// <summary>
        /// See <see cref="p1gardenGoals"/>. For <see cref="Player.PlayerEnum.Player2"/>
        /// </summary>
        public int[] p2gardenGoals;

        /// <summary>
        /// The action points for <see cref="Player.PlayerEnum.Player1"/>. In the order defined by <see cref="TurnPoints.PointType"/>. This has to be a 1D array due to serialisation limitations
        /// </summary>
        public int[] p1actionPoints;
        /// <summary>
        /// See <see cref="p1actionPoints"/>. For <see cref="Player.PlayerEnum.Player2"/>
        /// </summary>
        public int[] p2actionPoints;

        /// <summary>
        /// Strucure of <see cref="SerialisedPlantData"/> to store all of the plants in the game
        /// </summary>
        public SerialisedPlantData[] plants;

        public int turnNumber;


        public int testValue;

        /// <summary>
        /// A hash of the data of this struct (excluting the <see cref="hash"/> variable), to detect corruption in loading data. Gurenteed to be initialised before serialisation
        /// </summary>
        public byte[] hash;
        /// <summary>
        /// Calls <see cref="SaveDataUtility.ValidateHash(SaveGameData)"/> to compare the data to it's hash
        /// </summary>
        public bool HashIsValid => SaveDataUtility.ValidateHash(this);

    }


    /// <summary>
    /// A serialisable representation of a plant
    /// </summary>
    [Serializable]
    public class SerialisedPlantData
    {
        /// <summary>
        /// ID enum value of plant, cast to an <c>int</c>. Will be replaced when new plant system is implimented
        /// </summary>
        public int plantNameType;
        /// <summary>
        /// The <see cref="Transform.position"/> of the plant, cast to a <c>float[]</c>
        /// </summary>
        public float[] position;
        /// <summary>
        /// The <see cref="Player.PlayerEnum"/> who owns the plant, cast to an <c>int</c>
        /// </summary>
        public int owner;
    }
}