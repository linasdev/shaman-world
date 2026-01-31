using UnityEngine;
using UnityEngine.InputSystem;

namespace Map
{
    public class MapPlayerBehavior : MonoBehaviour
    {
        private MapNode _selectedMapNode;
        private InputAction _moveAction;
        private InputAction _enterAreaAction;

        public void Start()
        {
            var actionMap = InputSystem.actions.FindActionMap("Map");

            _selectedMapNode = MapProvider.MainMap.GetRootNode();
            _moveAction = actionMap.FindAction("Move");
            _enterAreaAction = actionMap.FindAction("EnterArea");
        }

        public void Update()
        {
            if (_enterAreaAction.triggered)
            {
                LoadSelectedArea();
                return;
            }

            if (!_moveAction.triggered)
            {
                return;
            }

            var movementDirection = Vector2Int.RoundToInt(_moveAction.ReadValue<Vector2>());

            if (movementDirection.sqrMagnitude == 0)
            {
                return;
            }

            var direction = MapDirectionMethods.FromVector2Int(movementDirection);

            if (direction == null)
            {
                return;
            }

            var nextMapNode =  _selectedMapNode.GetNeighbor((MapDirection)direction);

            if (nextMapNode == null || nextMapNode.IsLocked())
            {
                return;
            }

            _selectedMapNode = nextMapNode;
            transform.position = new Vector3(_selectedMapNode.GetPosition().x, _selectedMapNode.GetPosition().y, 0);
        }

        private void LoadSelectedArea() {
            MapProvider.MainMap.UnloadGameObjects();
        }
    }
}
