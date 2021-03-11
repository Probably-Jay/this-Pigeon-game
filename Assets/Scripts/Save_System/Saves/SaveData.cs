using System;
using UnityEngine;

// jay 11/03

[Serializable]
public class SaveData 
{
    public bool initialised = false;
    string localGameID;

    public byte[] hash;

    public bool HashIsValid => SaveDataSerialiser.ValidateHash(this);
}
