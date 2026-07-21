using MelonLoader;
using Ship.Interface.Settings;
using StellarModdingToolkit.Assets;
using StellarModdingToolkit.StellarDriveIntegration;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JollyDecorations
{
    public static class BuildInfo
    {
        public const string Name = "Jolly's Decorations";
        public const string Description = "Adds decorative parts to the game.";
        public const string Author = "Jollyname";
        public const string Company = null;
        public const string Version = "1.0.2";
        public const string DownloadLink = null;
    }

    public class JollyDecorations : MelonMod
    {
        public static AssetLoader Loader;

        public override void OnLateInitializeMelon()
        {
            Loader = new AssetLoader(
                MelonAssembly.Assembly,
                LoggerInstance,
                new[]
                {
                    "PottedPlant", "PottedPlantThumbnail",
                    "WoodenTable", "WoodenTableThumbnail",
                    "WoodenChair", "WoodenChairThumbnail",
                    "Crate", "CrateThumbnail",
                    "Computer", "ComputerThumbnail",
                    "Bed", "BedThumbnail"
                }
            );

            PrefabFactory.Loader = Loader;
            DecorationRegistry.Register();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != 1) return;

            GameObject debug = new("Collider Debugger");
            Object.DontDestroyOnLoad(debug);
            debug.AddComponent<ColliderDebugger>();

            RegisterDecorationParts();
        }

        private void RegisterDecorationParts()
        {
            foreach (var item in DecorationRegistry.Items)
            {
                var part = ScriptableObject.CreateInstance<PartSettings>();

                part.fullLabel = item.Name;
                part.name = item.Name;
                part.description = item.Description;
                part.size = item.Size;
                part.id = item.Id;
                part.mass = item.Mass;

                Texture2D thumb = string.IsNullOrEmpty(item.ThumbnailKey) ? null : Loader.GetAsset<Texture2D>(item.ThumbnailKey);
                part.thumbnailTex = thumb != null ? thumb : Texture2D.blackTexture;

                part.snappingStyle = item.SnappingStyle switch
                {
                    DecorationSnappingStyle.Any => Ship.Interface.Model.SnappingStyle.PreciseOnAny,
                    DecorationSnappingStyle.Floor => Ship.Interface.Model.SnappingStyle.PreciseOnFloor,
                    _ => Ship.Interface.Model.SnappingStyle.PreciseOnWall,
                };

                part.internalStateType = Ship.Interface.Model.Parts.StateTypes.PartInternalStateType.None;
                part.buildingCost = [];

                GameObject prefab = Loader.GetAsset<GameObject>(item.PrefabKey);

                GameObject built = item.Build(prefab);
                item.PostBuild?.Invoke(built);
                part.part = built;

                IntegrationUtilities.AddPart(part);
            }
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            if (buildIndex != 1) return;

            foreach (var item in Object.FindObjectsOfType<PartSettings>()
                .Where(p => DecorationRegistry.Items.Any(i => i.Id == p.id))
                .ToArray())
            {
                Object.Destroy(item);
            }
        }
    }
}