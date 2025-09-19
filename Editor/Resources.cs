using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    public static class Resources
    {
        public static async UniTask<T> GetResource<T>(string path, LoadType resourceType = LoadType.InBuilt)
            where T : Object
        {
            switch (resourceType)
            {
                case LoadType.InBuilt:
                    return (T)await UnityEngine.Resources.LoadAsync<T>(path);
            }

            await UniTask.Yield();
            return null;
        }


        public static T[] FindObjectsOfType<T>(string type)
        {
            var foundPrefabs = new List<T>();

            var prefabGUIDs = AssetDatabase.FindAssets(type);
            foreach (var guid in prefabGUIDs)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T));
                switch (asset)
                {
                    case T typedAsset:
                        foundPrefabs.Add(typedAsset);
                        continue;
                    case Component component:
                    {
                        var targetComponent = component.GetComponent<T>();
                        if (targetComponent is null) continue;
                        foundPrefabs.Add(targetComponent);
                        break;
                    }
                }
            }

            return foundPrefabs.ToArray();
        }
    }

    public enum LoadType
    {
        None,
        InBuilt,
        Addressable,
    }

    public static class ResourceType
    {
        public const string Prefab = "t:Prefab";
        public const string ScriptableObject = "t:ScriptableObject";
    }
}
