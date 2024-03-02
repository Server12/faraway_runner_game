using Game.Game.Level.Views.Factory;
using Game.Level;
using UnityEngine;

namespace Game.Data
{
    /// <summary>
    /// All Pool Factories Container
    /// Registers each in LevelScope
    /// </summary>
    [CreateAssetMenu(fileName = "GamePoolFactories", menuName = "Create/GamePoolFactories", order = 0)]
    public class GamePoolFactories : ScriptableObject
    {
        [SerializeField] private CollectablesFactory _collectablesFactory;
        
        [SerializeField] private ObstacleFactory _obstacleFactory;

        public ObstacleFactory ObstaclesFactory => _obstacleFactory;

        public CollectablesFactory CollectablesFactory => _collectablesFactory;
        
    } 
}