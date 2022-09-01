using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraRotation _cameraRotation;
    [SerializeField] private Game _game;
    [SerializeField] private GameObjectToggler _gridToggler;
    [SerializeField] private ObstaclesManager _obstaclesManager;

    void Update()
    {
        if (_game.IsLevelSet)
        {
            if (Input.GetKeyDown(KeyCode.W)) { _cameraRotation.Rotate(MovingDirections.Forward); }
            if (Input.GetKeyDown(KeyCode.D)) { _cameraRotation.Rotate(MovingDirections.Right); }
            if (Input.GetKeyDown(KeyCode.S)) { _cameraRotation.Rotate(MovingDirections.Back); }
            if (Input.GetKeyDown(KeyCode.A)) { _cameraRotation.Rotate(MovingDirections.Left); }
            if (Input.GetKeyDown(KeyCode.Mouse0)) { _obstaclesManager.TryToToggleObstacle(_game.IsPassing); }
            if (Input.GetKeyDown(KeyCode.R)) { _game.ChangePassing(); }
            if (Input.GetKeyDown(KeyCode.G)) { _gridToggler.Toggle(); }
        }
    }
}
