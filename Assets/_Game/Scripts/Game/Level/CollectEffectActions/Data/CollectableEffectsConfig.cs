using UnityEngine;

namespace Game.Level.Data
{
    [CreateAssetMenu(fileName = "CollectableEffectsConfig")]
    public class CollectableEffectsConfig : ScriptableObject
    {
        [SerializeField] private BaseCollectableEffectAction[] _actions;

        public BaseCollectableEffectAction[] EffectActions => _actions;
    }
}