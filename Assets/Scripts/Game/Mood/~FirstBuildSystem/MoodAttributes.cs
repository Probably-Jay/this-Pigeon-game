using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// added Jay 10/03
// depraceated Jay 26/03 due to redesign of mood system

/// <summary>
/// A datastructure that explitly stores the mood atributes in an utterly unambiguous way to prevent the infamous first playable bug from happening again
/// </summary>
[System.Obsolete("This class has been replaced by " + nameof(Mood.TraitValue),false)]
public class MoodAttributes
{
    /// <summary>
    /// The number of attribute scales. Important to be const for serialisation
    /// </summary>
    public const int NumberOfAtributeScales = 3;
    /// <summary>
    /// Enum representing each atribute scale. For the trinary states of these scales see <see cref="AttributeValues"/>
    /// </summary>
    public enum Scales
    {
        Pleasance
        , Sociability
        , Energy
    }

    /// <summary>
    /// Enum representing the trinary states of each of the attribute <see cref="Scales"/>
    /// </summary>
    public enum AttributeValues
    {
        Unpleasant
        , NeutPleasance
        , Pleasant
        , Personal
        , NeutSociability
        , Social
        , Calm
        , NeutEnergy
        , Energetic
    }

    #region Utility Dictionaries

    /// Mood Index Sprites (this is terrible)
    /// 0 = Energetic
    /// 1 = NeutPleasant
    /// 2 = Pleasant
    /// 3 = Sad (TEMP, ONLY FOR TESTING)
    /// 4 = NeutSocial
    /// 5 = Social
    /// 6 = Calm
    /// 7 = Unpleasant
    /// 8 = Personal
    /// 9 = NeutEnergy
    private static Dictionary<AttributeValues, int> attributeToSpriteIndex = new Dictionary<AttributeValues, int>
    {
        {AttributeValues.Unpleasant, 7}
        , {AttributeValues.NeutPleasance, 1}
        , {AttributeValues.Pleasant, 2}
        , {AttributeValues.Personal, 8}
        , {AttributeValues.NeutSociability, 4}
        , {AttributeValues.Social, 5 }
        , {AttributeValues.Calm, 6 }
        , {AttributeValues.NeutEnergy, 9 }
        , {AttributeValues.Energetic, 0 }
    };



    private static Dictionary<Scales, AttributeValues[]> atributes = new Dictionary<Scales, AttributeValues[]>
    {
        {Scales.Pleasance,      new AttributeValues[NumberOfAtributeScales] {AttributeValues.Unpleasant, AttributeValues.NeutPleasance,      AttributeValues.Pleasant } }
        ,{Scales.Sociability,   new AttributeValues[NumberOfAtributeScales] {AttributeValues.Personal,   AttributeValues.NeutSociability,    AttributeValues.Social }}
        ,{Scales.Energy,        new AttributeValues[NumberOfAtributeScales] {AttributeValues.Calm,       AttributeValues.NeutEnergy,         AttributeValues.Energetic }}

    };

    private static string[,] names = new string[,]
    {
        {"Unpleasant","Neutral","Pleasant"}
        ,  { "Personal", "Neutral","Social" }
        , {"Calm","Neutral","Energised" }
    };

    #endregion

    #region Helper Functions

    /// <summary>
    /// Get a string representation of the mood for use in rich text in <see cref="TMPro.TextMeshPro"/>
    /// </summary>
    /// <param name="scale">The atribute scale you want to display</param>
    /// <returns>Returns the printable name along with the absolute value for that scale and the correct image for the provided scale at the current value of that scale, 
    /// in the format “<b>{Name}</b>: {absolute value of scale}<sprite={sprite index}>”</returns>
    public string GetDisplayWithImage(Scales scale) => $"<b>{GetName(scale)}</b>: {Mathf.Abs(this[scale])}{GetImage(scale)}";

    /// <summary>
    /// Returns the printable names for this scale at the current value
    /// </summary>
    /// <param name="scale">The scale the atribute is in</param>
    /// <returns>Returns the printable names for the provided scale at the current value of that scale. 
    /// If the Pleasance scale holds a value of -3, it will return “Unpleasant”. However, if it held a value of 0 it would return “Neutral”.</returns>
    public string GetName(Scales scale) => names[(int)scale, GetIndexFromValue(this[scale])];

    /// <summary>
    /// Returns image for this scale at the current value as rich text markup
    /// </summary>
    /// <param name="scale">The scale the atribute is in</param>
    /// <returns>Returns the image for the provided scale at the current value of that scale as rich text markup, in the format “<sprite={sprite index}></returns>
    public string GetImage(Scales scale) => $"<sprite={attributeToSpriteIndex[GetAttributeValue(scale, this[scale])]}>";

    /// <summary>
    /// Creat a deepcopy of the values, returned in <see cref="Pleasance"/>, <see cref="Sociability"/>, and <see cref="Energy"/> 
    /// </summary>
    /// <returns>A new <c>int[]</c> containing a copy of the underlying values</returns>
    public int[] DeepCopyValues() => (int[])values.Clone();

    private static AttributeValues GetAttributeValue(Scales scale, int value) => atributes[scale][GetIndexFromValue(value)]; // get the AttributeValue enum of a scale and value
    private static int GetIndexFromValue(int value) => (value < 0) ? 0 : (value == 0 ? 1 :/*value > 0*/ 2); // 0 if negative, 1 if 0, 2 if poitive

    #endregion



    /// <summary>
    /// An unambiguous way to store the mood atributes: <see cref="Pleasance"/>, <see cref="Sociability"/>, and <see cref="Energy"/>
    /// </summary>
    /// <param name="plesance">The value on the scale of unpleasant (<see cref="-"/>negative) to pleasant (<see cref="+"/>positive). See <see cref="Pleasance"/></param>
    /// <param name="sociability">The value on the scale of personal (<see cref="-"/>negative) to social (<see cref="+"/>positive). See <see cref="Sociability"/></param>
    /// <param name="energy">The value on the scale of calm (<see cref="-"/>negative) to energised (<see cref="+"/>positive). See <see cref="Energy"/></param>
    public MoodAttributes(int plesance, int sociability, int energy)
    {
        values = new int[NumberOfAtributeScales];
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
    public int this[Scales index]
    {
        get { return this[(int)index]; }
        set { this[(int)index] = value; }
    }

    /// <summary>
    /// Provided for ease of life, however prefer the explicit properties, or indexing with <see cref="this[Scales]"/>
    /// </summary>
    /// <param name="index">The index of the value you wish to access</param>
    private int this[int index]
    {
        get { return values[index]; }
        set { values[index] = value; }
    }

    #region Operators
    public static MoodAttributes operator -(MoodAttributes a) => new MoodAttributes(-a[0], -a[1], -a[2]);

    public static MoodAttributes operator +(MoodAttributes a, MoodAttributes b) => new MoodAttributes(a[0] + b[0], a[1] + b[1], a[2] + b[2]);

    public static MoodAttributes operator -(MoodAttributes a, MoodAttributes b) => a + (-b);


    public static bool operator ==(MoodAttributes a, MoodAttributes b) => a.Equals(b);
    public static bool operator !=(MoodAttributes a, MoodAttributes b) => !(a == b);


    
    /// <summary>
    /// Requirement of implimenting <see cref="=="/> equality
    /// </summary>
    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            MoodAttributes o = (MoodAttributes)obj;
            return this[0] == o[0] && this[1] == o[1] && this[2] == o[2];
        }
    }
    /// <summary>
    /// Requirement of implimenting <see cref="=="/> equality
    /// </summary>
    public override int GetHashCode()
    {
        return values.GetHashCode();
    }

    #endregion

}