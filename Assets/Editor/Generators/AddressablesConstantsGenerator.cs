using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace Editor.Generators
{
    public static class AddressablesConstantsGenerator
    {
        private const string OutputPath = "Assets/Generated/AddressablesKeys.cs";

        [MenuItem("Tools/Generate Addressables Keys")]
        public static void Generate()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            if (settings == null)
            {
                Debug.LogError(
                    "AddressableAssetSettings not found. Make sure that Addressables are configured in the project.");
                return;
            }

            var keys = new HashSet<string>();

            foreach (var group in settings.groups)
            {
                foreach (var entry in group.entries)
                {
                    if (!string.IsNullOrEmpty(entry.address))
                    {
                        keys.Add(entry.address);
                    }
                }
            }

            var classContent = GenerateClass(keys);

            var directory = Path.GetDirectoryName(OutputPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(OutputPath, classContent);
            AssetDatabase.Refresh();

            Debug.Log($"AddressablesKeys.cs сгенерирован в {OutputPath}");
        }

        private static string GenerateClass(IEnumerable<string> keys)
        {
            var keyLines = keys
                .Select(k => $"    public const string {FormatKey(k)} = \"{k}\";")
                .OrderBy(line => line);

            return $@"// This file is auto-generated. Do not modify manually.

public static class AddressablesConstants
{{
{string.Join("\n", keyLines)}
}}";
        }

        private static string FormatKey(string key)
        {
            return key.Replace(" ", "_")
                .Replace("-", "_")
                .Replace(".", "_")
                .Replace("/", "_")
                .Replace("[", "_")
                .Replace("]", "_");
        }
    }
}