using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created Jay 05/02
// Edited Scott 24/02
// Edited Alexander Purvis 04/03

/// <summary>
/// Script all items will have
/// </summary>
public class PlantItem : MonoBehaviour
{
    [SerializeField] public string objectName;

    [SerializeField, Range(-1, 1)] public int scale1UnpleasantPleasant = 0;
    [SerializeField, Range(-1, 1)] public int scale2PersonalSocial = 0;
    [SerializeField, Range(-1, 1)] public int scale3CalmEnergised = 0;

    public Player plantOwner;
    public bool inLocalGarden;


   // public bool isPlanted = false;
    public Player.PlayerEnum gardenID = Player.PlayerEnum.Unnasigned;


    private void OnEnable()
    {
        // Get current player
        plantOwner = GameManager.Instance.ActivePlayer;

        // Set if in local or other garden
        if (plantOwner.PlayerEnumValue == 0)
        { // = true if local (placed by player 1)
            inLocalGarden = true;
        }
        else
        { // = false if not
            inLocalGarden = false;
        }

    }

   
    public MoodAtributes PlantStats => new MoodAtributes(scale1UnpleasantPleasant, scale2PersonalSocial, scale3CalmEnergised);
}

// added Jay 10/03

/// <summary>
/// A struct that explitly stores the mood atributes in an utterly unambiguous way to prevent the infamous first playable bug from happening again
/// </summary>
public struct MoodAtributes
{
    public enum Scales
    {
        Pleasance
        , Sociability
        , Energy
    }

    /// <summary>
    /// An unambiguous way to store the mood atributes: <see cref="Pleasance"/>, <see cref="Sociability"/>, and <see cref="Energy"/>
    /// </summary>
    /// <param name="plesance">The value on the scale of unpleasant (<see cref="-"/>negative) to pleasant (<see cref="+"/>positive). See <see cref="Pleasance"/></param>
    /// <param name="sociability">The value on the scale of personal (<see cref="-"/>negative) to social (<see cref="+"/>positive). See <see cref="Sociability"/></param>
    /// <param name="energy">The value on the scale of calm (<see cref="-"/>negative) to energised (<see cref="+"/>positive). See <see cref="Energy"/></param>
    public MoodAtributes(int plesance, int sociability, int energy)
    {
        values = new int[System.Enum.GetValues(typeof(Scales)).Length];
        Pleasance = plesance;
        Sociability = sociability;
        Energy = energy;
    }

    private int[] values;

    /// <summary>
    /// The value on the scale of unpleasant (<see cref="-"/>negative) to pleasant (<see cref="+"/>positive) 
    /// </summary>
    public int Pleasance { get => values[(int)Scales.Pleasance]; set => values[(int)Scales.Pleasance] = value; }
    /// <summary>
    /// The value on the scale of personal (<see cref="-"/>negative) to social (<see cref="+"/>positive) 
    /// </summary>
    public int Sociability { get => values[(int)Scales.Sociability]; set => values[(int)Scales.Sociability] = value; }
    /// <summary>
    /// The value on the scale of calm (<see cref="-"/>negative) to energised (<see cref="+"/>positive) 
    /// </summary>
    public int Energy { get => values[(int)Scales.Energy]; set => values[(int)Scales.Energy] = value; }

    /// <summary>
    /// The correct way to index this struct
    /// </summary>
    /// <param name="index">The <see cref="Scales"/> name of the property you wish to access</param>
    /// <returns></returns>
    public int this[Scales index]
    {
        get { return this[(int)index]; }
        set { this[(int)index] = value; }
    }

    /// <summary>
    /// Provided for ease of life, however prefer the exlplicit properties, or <see cref="this[Scales]"/>
    /// </summary>
    /// <param name="index">The index of the value you wish to access</param>
    /// <returns></returns>
    private int this[int index]
    {
        get { return values[index]; }
        set { values[index] = value; }
    }    


    public static MoodAtributes operator -(MoodAtributes a) => new MoodAtributes(-a[0], -a[1], -a[2]);

    public static MoodAtributes operator +(MoodAtributes a, MoodAtributes b) => new MoodAtributes(a[0] + b[0], a[1] + b[1], a[2] + b[2]);

    public static MoodAtributes operator -(MoodAtributes a, MoodAtributes b) => a + (-b);

   // public static bool operator ==(MoodAtributes a, MoodAtributes b) => a[0] == b[0] && a[1] == b[1] && a[2] == b[2];
   // public static bool operator !=(MoodAtributes a, MoodAtributes b) => !(a == b);

}