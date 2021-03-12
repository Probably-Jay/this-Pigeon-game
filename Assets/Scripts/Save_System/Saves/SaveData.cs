using System;
using UnityEngine;

// jay 11/03

[Serializable]
public class SaveData 
{
    public SaveData(string localID)
    {
        initialised = true;
        localGameID = localID;
    }

    public SaveData(SaveData copy)
    {
        initialised = copy.initialised;
        localGameID = copy.localGameID;

        hash = copy.hash;
    }

    public bool initialised;
    readonly string localGameID;

    public byte[] hash;

    public bool HashIsValid => SaveDataSerialiser.ValidateHash(this);
}
