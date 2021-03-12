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

    // keep this up to date
    public SaveData(SaveData copy)
    {
        initialised = copy.initialised;
        localGameID = copy.localGameID;

        value = copy.value;

        hash = copy.hash;
    }

    public bool initialised;
    public string localGameID;

    public int value;


    public byte[] hash;
    public bool HashIsValid => SaveDataSerialiser.ValidateHash(this);

}
