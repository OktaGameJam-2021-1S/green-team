using System;

[Serializable]
public struct Challenge
{
    public string Name;
    public int ChallengeType;
    public int Value;
    // Only for RangeComparisonType
    public int MaxValue;
    public int ComparisonType;
}

public enum ChallengeType
{
    Demolish = 0,
    Naturalize = 1,
}

public enum ComparisonType
{
    Equal = 0,
    Lesser = 1,
    Greater = 2,
    Range = 3
}
