﻿using System;
using UnityEngine;

// Jay 11/03

[Serializable]
public class SaveGameRegistryData 
{
    public bool initialised = false;

    public GameMetaData[] games;

    



    public byte[] hash;
    public bool HashIsValid => SaveGameRegistrySerialiser.ValidateHash(this);

}
