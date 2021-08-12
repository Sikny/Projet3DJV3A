using System;

namespace LevelEditor.Rule.Math {
    [Serializable]
    public class SerializedVector3
    {
        private float _x, _y, _z;

        public SerializedVector3(float x,float y, float z)
        {
            this._x = x;
            this._y = y;
            this._z = z;
        }

        public float X
        {
            get => _x;
            set => _x = value;
        }
        public float Y
        {
            get => _y;
            set => _y = value;
        }
        public float Z
        {
            get => _z;
            set => _z = value;
        }

        public override string ToString()
        {
            return "vec3(" + _x + ","+_y+"," + _z + ")";
        }

    }
}
