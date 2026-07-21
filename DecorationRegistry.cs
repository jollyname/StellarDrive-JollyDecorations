using JollyDecorations;
using System.Collections.Generic;
using UnityEngine;

public static class DecorationRegistry
{
    public static List<DecorationItem> Items = new();

    public static void Register()
    {
        Items.Add(new DecorationItem
        {
            Id = 1000,
            Name = "Potted Plant",
            Description = "Potted plant!!!!!!!",
            Size = Vector3.one * 0.25f,
            Mass = 1f,

            PrefabKey = "PottedPlant",
            ThumbnailKey = "PottedPlantThumbnail",

            Build = (prefab) => PrefabFactory.BuildFromPrefab(prefab, Vector3.one * 0.25f, Vector3.zero, Vector3.one, Vector3.zero)
        });
        Items.Add(new DecorationItem
        {
            Id = 1001,
            Name = "Wooden Table",
            Description = "A sturdy table, made out of wood.",
            Size = Vector3.one,
            Mass = 5f,

            PrefabKey = "WoodenTable",
            ThumbnailKey = "WoodenTableThumbnail",

            Build = (prefab) => PrefabFactory.BuildFromPrefab(prefab, new Vector3(1.3f, 0.6f, 0.75f), Vector3.zero, Vector3.one, Vector3.zero)
        });
        Items.Add(new DecorationItem
        {
            Id = 1002,
            Name = "Wooden Chair",
            Description = "A sturdy chair, made out of wood.",
            Size = Vector3.one,
            Mass = 2.5f,

            PrefabKey = "WoodenChair",
            ThumbnailKey = "WoodenChairThumbnail",

            Build = (prefab) => PrefabFactory.BuildFromPrefab(prefab, new Vector3(0.45f, 0.5f, 0.45f), new Vector3(0, 0, 0.0375f), Vector3.one, Vector3.zero)
        });
        Items.Add(new DecorationItem
        {
            Id = 1003,
            Name = "Crate",
            Description = "A crate Ig?",
            Size = Vector3.one,
            Mass = 6f,

            PrefabKey = "Crate",
            ThumbnailKey = "CrateThumbnail",

            Build = (prefab) => PrefabFactory.BuildFromPrefab(prefab, new Vector3(0.85f, 0.7f, 0.85f), new Vector3(0, 0, 0), Vector3.one, Vector3.zero)
        });
        Items.Add(new DecorationItem
        {
            Id = 1004,
            Name = "Computer",
            Description = "A computer terminal.",
            Size = Vector3.one,
            Mass = 3f,

            PrefabKey = "Computer",
            ThumbnailKey = "ComputerThumbnail",

            Build = (prefab) =>
            {
                GameObject computer = PrefabFactory.BuildFromPrefab(prefab, new Vector3(0.4f, 0.425f, 0.3f), new Vector3(0, 0, -0.1f), Vector3.one, Vector3.zero);
                Transform visuals = computer.transform.Find("Visuals");
                TerminalFactory.AddTerminal(visuals.gameObject);

                return computer;
            }
        });
        Items.Add(new DecorationItem
        {
            Id = 1005,
            Name = "Bed",
            Description = "A comfortable bed.",
            Size = Vector3.one,
            Mass = 8f,

            PrefabKey = "Bed",
            ThumbnailKey = "BedThumbnail",

            Build = (prefab) =>
            {
                GameObject bed = PrefabFactory.BuildFromPrefab(prefab, new Vector3(0.85f, 0.5f, 1.9f), Vector3.zero, new Vector3(0.85f, 1, 0.9f), Vector3.zero);
                Transform visuals = bed.transform.Find("Visuals");

                // Get the first child of Visuals
                if (visuals.childCount > 0)
                {
                    Transform meshChild = visuals.GetChild(0);
                    BedFactory.AddBed(meshChild.gameObject, new Vector3(0.85f, 0.5f, 1.9f));
                }

                return bed;
            }
        });
    }
}