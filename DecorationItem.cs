using System;
using UnityEngine;

namespace JollyDecorations
{
    public enum DecorationSnappingStyle
    {
        Any,
        Floor,
        Wall
    }

    public class DecorationItem
    {
        public ushort Id;
        public string Name;
        public string Description;

        public Vector3 Size = Vector3.one;
        public float Mass;

        public DecorationSnappingStyle SnappingStyle = DecorationSnappingStyle.Any;

        // Prefab key in AssetBundle
        public string PrefabKey;
        public string ThumbnailKey;

        public Func<GameObject, GameObject> Build;
        public Action<GameObject> PostBuild;
    }
}