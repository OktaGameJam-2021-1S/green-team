using System;

[Serializable]
public struct Challenge
{
    public string Name;
    public int ChallengeType;
    public int Value;
    public int ComparisonType;
    public int RewardType;
    public int RewardAmount;
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
    LesserEqual = 1,
    GreaterEqual = 2,
}

public enum RewardType
{
    None = 0,
    Score = 1,
}
