using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class CmsEntityPfb : MonoBehaviour
    {
        [SerializeReference, SubclassSelector] public List<EntityComponentDefinition> Components;

        public string GetId()
        {
#if UNITY_EDITOR
            // Get the path of the prefab in the Resources folder
            var path = UnityEditor.AssetDatabase.GetAssetPath(gameObject);

            // Remove the "Assets/Resources/" part and ".prefab" to match the Resources.Load format
            if (path.StartsWith("Assets/GameResources/Resources/") && path.EndsWith(".prefab"))
            {
                path = path["Assets/GameResources/Resources/".Length..];
                path = path[..^".prefab".Length];
            }

            return path;
#endif
            return "";
        }
    }
}