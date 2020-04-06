
public struct Effect
{
    public Effect(int idEffect, int level, float timeout)
    {
        IdEffect = idEffect;
        LevelEffect = level;
        Timeout = timeout;
    }

    public int IdEffect { get; set; }
    public int LevelEffect { get; set; }
    public float Timeout { get; set; }
}