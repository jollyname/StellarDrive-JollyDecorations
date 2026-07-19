using Ship.Interface.Model.Parts;
using Ship.Parts.Common;
using StellarModdingToolkit.Assets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JollyDecorations
{
    public static class BedFactory
    {
        public static void AddBed(GameObject parent, Vector3 size)
        {
            GameObject interactions = new("Interactions");

            interactions.transform.SetParent(parent.transform);
            interactions.layer = LayerMask.NameToLayer("Interactable");
            interactions.tag = "Interactable";

            interactions.AddComponent<DefaultShipPartInteractions>();


            GameObject bedInteraction = new("Bed Interaction");

            bedInteraction.transform.SetParent(interactions.transform);
            bedInteraction.layer = interactions.layer;
            bedInteraction.tag = interactions.tag;

            bedInteraction.AddComponent<Ship.Parts.Bed.BedInteraction>();


            BoxCollider collider = bedInteraction.AddComponent<BoxCollider>();
            collider.center = new Vector3(0, size.y / 2, 0);
            collider.size = size;
        }
    }

    public static class TerminalFactory
    {
        public static void AddTerminal(GameObject parent)
        {
            GameObject canvasObject = new("Terminal Canvas");
            canvasObject.transform.SetParent(parent.transform);

            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(240, 185);
            canvasRect.localPosition = new Vector3(0, 0.3f, 0.035f);
            canvasRect.localRotation = Quaternion.Euler(0, 180, 0);
            canvasRect.localScale = Vector3.one * 0.001f;


            GameObject backgroundObject = new("Terminal Background");
            backgroundObject.transform.SetParent(canvasObject.transform);

            RectTransform backgroundRect = backgroundObject.AddComponent<RectTransform>();
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;
            backgroundRect.localPosition = Vector3.zero;
            backgroundRect.localRotation = Quaternion.identity;
            backgroundRect.localScale = Vector3.one * 1.1f;

            Image background = backgroundObject.AddComponent<Image>();
            background.color = Color.black;


            GameObject textObject = new("Terminal Text");
            textObject.transform.SetParent(canvasObject.transform);

            RectTransform textRect = textObject.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            textRect.localPosition = Vector3.zero;
            textRect.localRotation = Quaternion.identity;
            textRect.localScale = Vector3.one;


            TMP_Text text = textObject.AddComponent<TextMeshProUGUI>();

            text.fontSize = 10;
            text.alignment = TextAlignmentOptions.TopLeft;
            text.color = Color.green;
            text.enableWordWrapping = true;


            ComputerTerminal terminal = canvasObject.AddComponent<ComputerTerminal>();
            terminal.TerminalText = text;
        }
    }

    public static class PrefabFactory
    {
        public static AssetLoader Loader;

        private static Material ConvertMaterial(Shader shader, Material original)
        {
            Material material = new(shader);

            if (original != null)
            {
                if (original.HasProperty("_MainTex"))
                    material.SetTexture("_AlbedoTex", original.GetTexture("_MainTex"));

                if (original.HasProperty("_BumpMap"))
                    material.SetTexture("_NormalMap", original.GetTexture("_BumpMap"));

                if (original.HasProperty("_EmissionMap"))
                    material.SetTexture("_EmissionTex", original.GetTexture("_EmissionMap"));

                if (original.HasProperty("_EmissionColor"))
                    material.SetColor("_EmissionColor", original.GetColor("_EmissionColor"));
            }

            return material;
        }

        public static void ApplyGameShader(GameObject instance)
        {
            Shader shader = Shader.Find("Shader Graphs/PlanetObjectDefaultLighting");

            foreach (var renderer in instance.GetComponentsInChildren<Renderer>())
            {
                Material[] originals = renderer.sharedMaterials;
                Material[] converted = new Material[originals.Length];

                for (int i = 0; i < originals.Length; i++)
                    converted[i] = ConvertMaterial(shader, originals[i]);

                renderer.materials = converted;
            }
        }

        public static GameObject BuildFromPrefab(GameObject prefab, Vector3 size, Vector3 additionalOffset, Vector3 visualScale, Vector3 additionalRotation)
        {
            Vector3 offset = Vector3.up * (size.y / 2f);

            GameObject partObject = new(prefab.name);
            var bounds = partObject.AddComponent<ShipPartBounds>();
            bounds.bounds = size;
            bounds.center = offset + additionalOffset;

            GameObject visualContainer = new("Visuals");
            visualContainer.AddComponent<DefaultShipPartVisuals>();
            visualContainer.transform.parent = partObject.transform;

            GameObject meshInstance = Object.Instantiate(prefab);
            ApplyGameShader(meshInstance);

            meshInstance.transform.parent = visualContainer.transform;
            meshInstance.transform.localPosition = Vector3.zero;
            meshInstance.transform.localRotation = Quaternion.Euler(additionalRotation);
            meshInstance.transform.localScale = visualScale;

            partObject.SetActive(false);

            return partObject;
        }
    }
}