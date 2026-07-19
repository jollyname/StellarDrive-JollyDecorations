using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(JollyDecorations.BuildInfo.Description)]
[assembly: AssemblyDescription(JollyDecorations.BuildInfo.Description)]
[assembly: AssemblyCompany(JollyDecorations.BuildInfo.Company)]
[assembly: AssemblyProduct(JollyDecorations.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + JollyDecorations.BuildInfo.Author)]
[assembly: AssemblyTrademark(JollyDecorations.BuildInfo.Company)]
[assembly: AssemblyVersion(JollyDecorations.BuildInfo.Version)]
[assembly: AssemblyFileVersion(JollyDecorations.BuildInfo.Version)]
[assembly: MelonInfo(typeof(JollyDecorations.JollyDecorations), JollyDecorations.BuildInfo.Name, JollyDecorations.BuildInfo.Version, JollyDecorations.BuildInfo.Author, JollyDecorations.BuildInfo.DownloadLink)]
[assembly: MelonColor()]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("CuriousOwlGames", "StellarDrive")]