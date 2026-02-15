using HarmonyLib;
using Immersion.Components;

namespace Immersion.Utils;

[HarmonyPatch]
internal static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerCameraController), nameof(PlayerCameraController.Start))]
    private static void PlayerCameraController_Start_Postfix(PlayerCameraController __instance) =>
        OffsetManager.AddToPlayerCamera(__instance);

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerTool), nameof(PlayerTool.EquipTool))]
    private static void PlayerTool_EquipTool_Postfix(PlayerTool __instance) =>
        ViewmodelArmUtils.OnEquipTool(__instance);

    [HarmonyPostfix]
    [HarmonyPatch(typeof(OWItem), nameof(OWItem.PickUpItem))]
    private static void OWItem_PickUpItem_Postfix(OWItem __instance) =>
        ViewmodelArmUtils.OnPickUpItem(__instance);
}