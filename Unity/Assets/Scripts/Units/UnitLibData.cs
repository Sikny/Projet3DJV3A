using UnityEngine;

namespace Units
{
    public static class UnitLibData
    {
        public static Camera cam;
        public static LayerMask groundMask;

        public static float rotationSpeed;
        public static float speed;

        public static AbstractUnit[] units = new AbstractUnit[16];

        public static PlayerUnit _selectedUnit;
        
        public static float deltaTime;
    }
}