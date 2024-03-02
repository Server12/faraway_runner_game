using UnityEngine;

namespace Game.Extensions
{
    public static class ColliderExtensions
    {
        public static bool CheckColliderLayer(this Collider collider, LayerMask layerMask)
        {
            return ((1 << collider.gameObject.layer) | layerMask) == layerMask;
        }

        public static bool CheckTags(this Collider collider, string[] tags, out int tagIndex)
        {
            tagIndex = -1;
            for (var i = 0; i < tags.Length; i++)
            {
                var tag = tags[i];
                if (collider.CompareTag(tag))
                {
                    tagIndex = i;
                    return true;
                }
            }

            return false;
        }

        public static bool CheckTags(this Collider collider, string[] tags)
        {
            for (var i = 0; i < tags.Length; i++)
            {
                var tag = tags[i];
                if (collider.CompareTag(tag))
                {
                    return true;
                }
            }

            return false;
        }
    }
}