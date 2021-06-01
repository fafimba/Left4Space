using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Stat
{
    float _max, _actual, _regeneration;
    public float Max
    {
        get
        {
            return _max + +GetModification(ModifierStat.Max);
        }
        set
        {
            _max = value;
            Actual = Actual; //In case the MaxLife goes below the actualLife we refresh it value.
        }
    }
    public float Actual
    {
        get
        {
            return _actual + +GetModification(ModifierStat.Actual);
        }
        set
        {
            _actual = Mathf.Clamp(value, 0, Max);
        }
    }

    public float RegenerationRate
    {
        get
        {
            return _regeneration + GetModification(ModifierStat.RegenerationRate);
        }
        set
        {
            _regeneration = value;
        }
    }


    public List<Modifier> Modifiers;

    float GetModification(ModifierStat stat)
    {

        if (Modifiers == null)
            return 0;

        float _modifier = 0;
        foreach (Modifier modifier in Modifiers)
        {
            if (modifier.stat == stat)
            {
                _modifier += modifier.value * (int)modifier.type;
            }
        }
        return _modifier;
    }
}

public struct Modifier
{
    public string name;
    public ModifierType type;
    public ModifierStat stat;
    public float value;
}

public enum ModifierType
{
    boost = 1,
    debuff = -1
}

public enum ModifierStat
{
    Max,
    Actual,
    RegenerationRate
};

