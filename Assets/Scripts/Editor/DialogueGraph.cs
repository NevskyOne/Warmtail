using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace Editor
{
    [Serializable, Graph(AssetExtension)]
    public class DialogueGraph : Graph
    {
        public const string AssetExtension = "dg";
        [MenuItem("Assets/Create/Dialogue Graph", false)]
        private static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>();
		}
    }
}