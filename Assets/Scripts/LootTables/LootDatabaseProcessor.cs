#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;

namespace Assets.Scripts.LootTables
{
    public struct AssetWatcher
    {
        public string path;
        public Action function;
    }

    public class LootDatabaseProcessor : UnityEditor.AssetModificationProcessor
    {
        public static List<AssetWatcher> flagFileAddresses = new List<AssetWatcher>();

        private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            foreach (var assetWatcher in flagFileAddresses)
            {
                if (assetPath == assetWatcher.path)
                {
                    assetWatcher.function();
                }
            }

            return AssetDeleteResult.DidNotDelete;
        }
    }
}
#endif