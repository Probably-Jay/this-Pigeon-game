using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Created Jay 05/02
/// <summary>
/// Script all items will have
/// </summary>
/// 
/// Edited Scott 24/02
/// 
public class Item : MonoBehaviour
{
    [SerializeField] public string objectName;
 
    [SerializeField, Range(-1, 1)] public int moodEnergy = 0;
    [SerializeField, Range(-1, 1)] public int moodPride  = 0;
    [SerializeField, Range(-1, 1)] public int moodSocial = 0;

    public Player plantOwner;
    public bool inLocalGarden;

    private void OnEnable()
    {
        // Get current player
        plantOwner = GameManager.Instance.ActivePlayer;

        // Set if in local or other garden
        // TODO: Change later - if I understand correctly, players can place plants in each other's gardens too
        if (plantOwner.PlayerEnumValue == 0)
        { // = true if local (placed by player 1)
            inLocalGarden = true;
        }
        else
        { // = false if not
            inLocalGarden = false;
        }

        Debug.Log("Plant is in Garden #" + inLocalGarden);
    }
}
