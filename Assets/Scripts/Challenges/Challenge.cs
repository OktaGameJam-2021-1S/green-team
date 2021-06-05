using System;

[Serializable]
public struct Challenge
{
    public string Name;
    public int ChallengeType;
    public int Value;
    public int ComparisonType;
}

public enum ChallengeType
{
    Demolish = 0,
    Naturalize = 1,
    Damage = 2,
    GrowPlant = 3,
}

public enum ComparisonType
{
    Equal = 0,
    Lesser = 1,
    Greater = 2,
}
