using System;
using UnityEngine;

// Jay 11/03

namespace SaveSystemInternal
{
    /// <summary>
    /// Datastructure representing the game registry file, the file that holds the names of all active games so they can be found by filepath
    /// </summary>
    [Serializable]
    public class SaveGameRegistryData 
    {
        /// <summary>
        /// The list of existing games. The filepath of a game is given by passing the value of <see cref="GameMetaData.gameID"/> to <see cref="SaveDataSerialiser.GetFilePath(string)"/>
        /// </summary>
        public GameMetaData[] games;

        /// <summary>
        /// A hash of the data of this struct (excluting the <see cref="hash"/> variable), to detect corruption in loading data
        /// </summary>
        public byte[] hash;
        /// <summary>
        /// Calls <see cref="SaveDataUtility.ValidateHash(SaveGameRegistryData)"/> to compare the data to it's hash
        /// </summary>
        public bool HashIsValid => SaveDataUtility.ValidateHash(this);

    }
}
