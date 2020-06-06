using System;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain {
    [Serializable]
    public class UnitList {
        public readonly List<Vector2Int> _list = new List<Vector2Int>();

        public void Add(int x, int y) {
            _list.Add(new Vector2Int(x, y));
        }

        public Vector2Int Get(int i) {
            return _list[i];
        }

        public int Count() {
            return _list.Count;
        }

        public Vector2Int Last() {
            return _list[_list.Count - 1];
        }
        
        public bool Contains(int x, int y) {
            int listSize = _list.Count;
            for (int i = 0; i < listSize; i++) {
                if (_list[i].x == x && _list[i].y == y)
                    return true;
            }
            return false;
        }

        public bool HasNeighbour(int x, int y) {
            int listSize = _list.Count;
            if (listSize == 0) return true;
            for (int i = 0; i < listSize; i++) {
                int curX = _list[i].x;
                int curY = _list[i].y;
                if (curY == y && (curX == x + 1 || curX == x - 1)
                    || curX == x && (curY == y + 1 || curY == y - 1))
                    return true;
            }
            return false;
        }
    }
}
