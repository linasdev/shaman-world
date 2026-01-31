using System.Collections.Generic;
using Exceptions;
using UnityEngine;

namespace Save
{
    public class GameState
    {
        private readonly HashSet<Vector2Int> _unlockedMapNodes = new();

        public bool IsMapNodeLocked(Vector2Int position)
        {
            return !_unlockedMapNodes.Contains(position);
        }

        public void SetMapNodeLocked(Vector2Int position, bool locked)
        {
            if (!locked)
            {
                _unlockedMapNodes.Add(position);
            }
            else
            {
                _unlockedMapNodes.Remove(position);
            }
        }
    }
}
