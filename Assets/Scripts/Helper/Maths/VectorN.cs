using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// created Jay 07/02

[System.Serializable]
public class VectorN : IEnumerable
{

    [HideInInspector] public float[] values;
    public float[] Values { get => values; protected set => values = value; }

    public int Length => Values.Length;
    public float Magnitude 
    {
        get
        {
            float total = 0;
            foreach (var value in values)
            {
                total += value*value;
            }
            return Mathf.Sqrt(total);
        }
    }

    public VectorN Normalised
    {
        get
        {
            VectorN temp = new VectorN(this.Length);
            temp = this;
            temp.Normalise();
            return temp;
        }
    }


    public VectorN(int N)
    {
        Values = new float[N];
    }

    public float this[int i]
    {
        get
        {
            return Values[i];
        }
        set
        {
            Values[i] = value;
        }
    }

    public void Normalise()
    {
       // var v = new VectorN(this.Length);
        for (int i = 0; i < Length; i++)
        {
            this[i] = this[i] / Magnitude;
        }
    }

    public static VectorN operator +(VectorN left, VectorN right)
    {
        if (left.Length != right.Length)
        {
            throw new System.InvalidOperationException();
        }

        var v = new VectorN(left.Length);
        for (int i = 0; i < left.Length; i++)
        {
            v[i] = left[i] + right[i];
        }
        return v;
    } 
    
    public static VectorN operator /(VectorN left, float right)
    {
        var v = new VectorN(left.Length);
        for (int i = 0; i < left.Length; i++)
        {
            v[i] = left[i] / right;
        }
        return v;
    }




    // Implementation for the GetEnumerator method.
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public VectorNEnumerator GetEnumerator()
    {
        return new VectorNEnumerator(Values);
    }

}





/// <summary>
/// Taken from https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable.getenumerator?view=net-5.0, standard Ienumerator implimentation
/// </summary>
public class VectorNEnumerator : IEnumerator
{
    public  float[] values;

    // Enumerators are positioned before the first element
    // until the first MoveNext() call.
    int position = -1;

    public VectorNEnumerator(float[] v)
    {
        values = v;
    }


    public bool MoveNext()
    {
        position++;
        return (position < values.Length);
    }

    public void Reset()
    {
        position = -1;
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public float Current
    {
        get
        {
            try
            {
                return values[position];
            }
            catch (System.IndexOutOfRangeException)
            {
                throw new System.InvalidOperationException();
            }
        }
    }
}