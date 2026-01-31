using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
                nextMapNode?.Unlock(); // TODO: Remove this
                return;
            }

            _selectedMapNode = nextMapNode;

            var worldPosition = _selectedMapNode.GetWorldPosition();
            if (!worldPosition.HasValue)
            {
                return;
            }

            var worldPositionValue = worldPosition.Value;
            transform.position = new Vector3(worldPositionValue.x, worldPositionValue.y, 0);
        }

        private void LoadSelectedArea() {
            var sceneName = _selectedMapNode.GetSceneName();
            if (sceneName == null)
            {
                return;
            }

            MapProvider.MainMap.UnloadGameObjects();
            SceneManager.LoadScene(sceneName);
        }
    }
}
