using System;
using Game.Core.FactoryPool;
using UnityEngine;

namespace Game.Game.Level.Views.Factory
{
    /// <summary>
    /// Pool factory for create/get Obstacle views, and setup material by index.
    /// </summary>
    [Serializable]
    public class ObstacleFactory : BaseFactoryPool<Obstacle>
    {
        [SerializeField] private Material[] _materials;

        public int TotalMaterials => _materials.Length;

        protected override void ActionOnGet(Obstacle poolObject)
        {
            poolObject.isVisible = true;
            base.ActionOnGet(poolObject);
        }


        public Obstacle Spawn(int materialIndex, Vector3 position)
        {
            materialIndex = Mathf.Clamp(materialIndex, 0, TotalMaterials);
            var obstacle = GetOrCreate();
            obstacle.Renderer.material = _materials[materialIndex];
            obstacle.transform.position = position;
            return obstacle;
        }
    }
}