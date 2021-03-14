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

        public GameMetaData[] games;

        /// <summary>
        /// A hash of the data of this struct (excluting the <see cref="hash"/> variable), to detect corruption in loading data
        /// </summary>
        public byte[] hash;
        /// <summary>
        /// Calls <see cref="SaveSystemInternal.SaveDataSerialiser.ValidateHash"/> to compare the data to it's hash
        /// </summary>
        public bool HashIsValid => SaveGameRegistrySerialiser.ValidateHash(this);

    }
}
