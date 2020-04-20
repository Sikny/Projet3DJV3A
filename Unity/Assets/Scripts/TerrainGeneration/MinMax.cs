namespace TerrainGeneration {
    public class MinMax {
        public float Min { get; private set; }
        public float Max { get; private set; }

        public MinMax() {
            Min = float.MaxValue;
            Max = float.MinValue;
        }

        public void HandleValue(float value) {
            Max = value > Max ? value : Max;
            Min = value < Min ? value : Min;
        }
    }
}
