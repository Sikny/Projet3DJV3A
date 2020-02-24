using System.Collections.Generic;
using Units.UnitSystem;
using UnityEngine;

namespace Units.proto
{
    public static class UnitLibData
    {
        public static Camera cam;
        public static LayerMask groundMask;

        public static Interactable focus;

        public static float rotationSpeed;
        public static float speed;

        public static AbstractUnit[] units = new AbstractUnit[16];

        public static float deltaTime;
    }
}