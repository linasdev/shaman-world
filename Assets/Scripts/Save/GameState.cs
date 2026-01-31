using System;
using System.Collections.Generic;
using Exceptions;
using UnityEngine;

namespace Save
{
    [Serializable]
    public class GameState
    {
        public List<Vector2Int> unlockedMapNodes = new();

        public bool IsMapNodeLocked(Vector2Int position)
        {
            return !unlockedMapNodes.Contains(position);
        }

        public void SetMapNodeLocked(Vector2Int position, bool locked)
        {
            if (!locked)
            {
                if (unlockedMapNodes.Contains(position))
                {
                    return;
                }

                unlockedMapNodes.Add(position);
                return;
            }

            unlockedMapNodes.Remove(position);
        }
    }
}
