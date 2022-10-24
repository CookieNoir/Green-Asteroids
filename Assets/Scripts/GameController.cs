using UnityEngine;
using Asteroids;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraRotation _cameraRotation;
    [SerializeField] private Game _game;
    [SerializeField] private GameObjectToggler _gridToggler;
    [SerializeField] private ObstaclesManager _obstaclesManager;
    private bool _controllable;

    public void AllowControls() { _controllable = true; }
    public void RestrictControls() { _controllable = false; }

    void Update()
    {
        if (_controllable)
        {
            if (Input.GetKeyDown(KeyCode.W)) { _cameraRotation.RotateForward(); }
            if (Input.GetKeyDown(KeyCode.D)) { _cameraRotation.RotateRight(); }
            if (Input.GetKeyDown(KeyCode.S)) { _cameraRotation.RotateBack(); }
            if (Input.GetKeyDown(KeyCode.A)) { _cameraRotation.RotateLeft(); }
            if (Input.GetKeyDown(KeyCode.Mouse0)) { _obstaclesManager.TryToToggleObstacle(); }
            if (Input.GetKeyDown(KeyCode.R)) { _game.ChangePassing(); }
            if (Input.GetKeyDown(KeyCode.G)) { _gridToggler.Toggle(); }
        }
    }
}