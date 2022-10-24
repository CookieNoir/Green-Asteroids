using UnityEngine;
using Asteroids.Grid;
using Asteroids.Levels;
using Asteroids.Tiles.Painting;
using Asteroids.Entities;
using Asteroids.Math;

namespace Asteroids
{
    public class LevelCreator : MonoBehaviour
    {
        [SerializeField] private WorldGenerator _worldGenerator;
        [SerializeField] private TilePainter _tilePainter;
        [SerializeField] private EntitiesManager _entitiesManager;
        [SerializeField] private ObstaclesManager _obstaclesManager;
        [SerializeField] private GraphicsValues _globalValues;
        [SerializeField] private CameraRotation _cameraRotation;
        [SerializeField] private NoiseHandler _noiseHandler;

        public void CreateLevel(LevelProperties levelProperties)
        {
            _noiseHandler.SetNoise(levelProperties.LevelGraphics.Noise);
            _globalValues.SetValues(levelProperties.LevelGraphics);
            CubicGrid<ChangeableObstacle> cubicGrid = _worldGenerator.GenerateWorld(levelProperties);
            _entitiesManager.SetEntities(cubicGrid, levelProperties.MovingEntitiesProperties);
            _obstaclesManager.SetValues(cubicGrid, levelProperties.MaxObstaclesCount);
            _tilePainter.SetValues(cubicGrid.CellsCount);
            _cameraRotation.SetCamera(levelProperties.CameraStartAngle, levelProperties.CanMoveCamera);
        }
    }
}