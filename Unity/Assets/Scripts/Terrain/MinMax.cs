namespace Terrain {
    public class MinMax {
        public float Min { get; private set; }
        public float Max { get; private set; }

        public MinMax() {
            Min = float.MaxValue;
            Max = float.MinValue;
        }

        public void HandleValue(float value) {
            Max = value > Max ? value + 0.1f : Max;
            Min = value < Min ? value - 0.1f : Min;
        }
    }
}
