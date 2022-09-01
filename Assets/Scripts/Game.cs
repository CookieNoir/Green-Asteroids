using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private WorldGenerator _worldGenerator;
    [SerializeField] private TilePainter _tilePainter;
    [SerializeField] private LevelProperties _levelProperties;
    [SerializeField] private EntitiesManager _entitiesManager;
    [SerializeField] private ObstaclesManager _obstaclesManager;
    [SerializeField] private GlobalValues _globalValues;
    [SerializeField] private CameraRotation _cameraRotation;
    public bool IsLevelSet { get; private set; }
    public bool IsPassing { get; private set; }

    private void StartLevel(LevelProperties levelProperties)
    {
        _globalValues.SetValues(levelProperties.LevelGraphics);
        CubicGrid<ChangeableObstacle> cubicGrid = _worldGenerator.GenerateWorld(levelProperties);
        _entitiesManager.SetEntities(cubicGrid, levelProperties.MovingEntitiesProperties);
        _obstaclesManager.SetValues(cubicGrid, levelProperties.MaxObstaclesCount);
        _tilePainter.SetValues(cubicGrid.CellsCount);
        _cameraRotation.SetCamera(levelProperties.CameraStartAngle, levelProperties.CanMoveCamera);
        IsPassing = false;
        IsLevelSet = true;
    }

    private void Start()
    {
        StartLevel(_levelProperties);
    }

    public void ChangePassing()
    {
        if (IsPassing) _StopPassing();
        else _StartPassing();
    }

    private void _StartPassing()
    {
        _entitiesManager.StartMoving();
        _tilePainter.StartPainting();
        IsPassing = true;
    }

    private void _StopPassing()
    {
        IsPassing = false;
        _entitiesManager.StopMoving();
        _tilePainter.StopPainting();
    }
}
