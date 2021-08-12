using System;

namespace LevelEditor.Rule.Math {
    [Serializable]
    public class SerializedVector2
    {
        private float _x, _z;

        public SerializedVector2(float x, float z)
        {
            _x = x;
            _z = z;
        }

        public float X {
            get => _x;
            set => _x = value;
        }
        public float Z {
            get => _z;
            set => _z = value;
        }

        public override string ToString()
        {
            return "vec2(" + _x + "," + _z + ")";
        }

    }
}
