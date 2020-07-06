
namespace Units.utils {
    public struct Effect
    {
        public Effect(int idEffect, int level, float timeout)
        {
            this.IdEffect = idEffect;
            LevelEffect = level;
            this.Timeout = timeout;
        }

        public int IdEffect { get; set; }
        public int LevelEffect { get; set; }
        public float Timeout { get; set; }
    }
}