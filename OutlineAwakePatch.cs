using System.Reflection;
using UnityEngine;
using HarmonyLib;

[HarmonyPatch(typeof(Outline), "OnEnable")]
public static class OutlineOnEnablePatch
{
    public static void Prefix(Outline __instance)
    {
        // Force find the correct renderers
        Renderer[] newRenderers = __instance.GetComponentsInChildren<Renderer>();
        if (newRenderers.Length == 0)
        {
            Transform parent = __instance.transform.parent;
            for (int i = 0; i < 3 && parent != null; i++)
            {
                newRenderers = parent.GetComponentsInChildren<Renderer>();
                if (newRenderers.Length > 0) break;
                parent = parent.parent;
            }
        }

        // Override the internal 'renderers' field BEFORE OnEnable runs
        var field = typeof(Outline).GetField("renderers", BindingFlags.NonPublic | BindingFlags.Instance);
        field.SetValue(__instance, newRenderers);
    }
}