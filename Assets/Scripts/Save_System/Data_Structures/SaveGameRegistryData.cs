using System;
using UnityEngine;

// Jay 11/03

namespace SaveSystemInternal
{
    [Serializable]
    public class SaveGameRegistryData 
    {

        public GameMetaData[] games;


        public byte[] hash;
        public bool HashIsValid => SaveGameRegistrySerialiser.ValidateHash(this);

    }
}
