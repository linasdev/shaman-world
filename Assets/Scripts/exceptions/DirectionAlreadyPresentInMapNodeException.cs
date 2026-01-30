using System;
using map;

namespace exceptions
{
    public class DirectionAlreadyPresentInMapNodeException : Exception
    {
        private MapDirection Direction { get; }

        public DirectionAlreadyPresentInMapNodeException(MapDirection direction)
        {
            this.Direction = direction;
        }
    }
}