namespace Units.utils {
    public struct EquipmentEffect
    {
        public EquipmentEffect(int idEffect, int level)
        {
            this.IdEffect = idEffect;
            LevelEffect = level;
        }

        public int IdEffect { get; set; }
        public int LevelEffect { get; set; }
    }
}