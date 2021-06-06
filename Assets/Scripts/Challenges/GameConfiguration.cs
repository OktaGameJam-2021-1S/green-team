using System.Collections.Generic;
using UnityEngine;

public class GameConfiguration : ScriptableObject
{
    public List<Challenge> Challenges;

    public List<Score> Scores;

    public int WinThreshold;

}
