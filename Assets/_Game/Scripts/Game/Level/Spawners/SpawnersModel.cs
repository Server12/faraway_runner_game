using System;
using UnityEngine;

namespace Game.Level.Spawners
{
    public class SpawnersModel : MonoBehaviour
    {
        [SerializeField] private int _maxToGenerate;
        [SerializeField] private Spawner[] _spawners = Array.Empty<Spawner>();

        public Spawner[] Spawners => _spawners;

        public int MaxToGenerate => _maxToGenerate;

#if UNITY_EDITOR

        [SerializeField] private Color _color = Color.black;

        [SerializeField] private Vector3 _boundsSize;
        [SerializeField] private bool _fillSpawners;

        private void OnValidate()
        {
            if (_fillSpawners)
            {
                _fillSpawners = false;

                _spawners = new Spawner[transform.childCount];

                for (int i = 0; i < transform.childCount; i++)
                {
                    _spawners[i].localPos = transform.GetChild(i).localPosition;
                }
            }
        }

        private void OnTransformChildrenChanged()
        {
            _fillSpawners = true;
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < _spawners.Length; i++)
            {
                Gizmos.color = _color;
                Gizmos.DrawCube(transform.position + _spawners[i].localPos, _boundsSize);
            }
        }

#endif
    }
}