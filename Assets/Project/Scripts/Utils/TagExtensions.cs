using UnityEngine;

namespace Project.Scripts.Utils
{
    public static class TagExtensions
    {
        public static bool HasTag(this GameObject obj, string value)
        {
            var tag = obj.GetComponent<Tag>();
            return tag != null && tag.value.Trim().ToLower().Equals(value.Trim().ToLower());
        }
    
        public static bool HasTagThatContains(this GameObject obj, string value)
        {
            var tag = obj.GetComponent<Tag>();
            return tag != null && tag.value.Trim().ToLower().Contains(value.Trim().ToLower());
        }
        
        public static bool HasTag(this Collider2D obj, string value)
        {
            var tag = obj.GetComponent<Tag>();
            return tag != null && tag.value.Trim().ToLower().Equals(value.Trim().ToLower());
        }
    
        public static bool HasTagThatContains(this Collider2D obj, string value)
        {
            var tag = obj.GetComponent<Tag>();
            return tag != null && tag.value.Trim().ToLower().Contains(value.Trim().ToLower());
        }
    }
}