using Immersion.Components;
using OWML.Utils;
using UnityEngine;

namespace Immersion.Utils;

internal static class ViewmodelArmUtils
{
    internal static AssetBundle ViewmodelArmAssetBundle;

    internal static void LoadAssetBundle()
    {
        if (ViewmodelArmAssetBundle == null)
            ViewmodelArmAssetBundle = ModMain.Instance.ModHelper.Assets.LoadBundle("AssetBundles/viewmodelarm");
    }

    internal static bool IsBaseGameItem(OWItem item)
    {
        return item.GetItemType() switch
        {
            ItemType.SharedStone => item is SharedStone,
            ItemType.Scroll => item is ScrollItem,
            ItemType.ConversationStone => item is NomaiConversationStone,
            ItemType.WarpCore => item is WarpCoreItem,
            ItemType.Lantern => item is SimpleLanternItem,
            ItemType.SlideReel => item is SlideReelItem,
            ItemType.DreamLantern => item is DreamLanternItem,
            ItemType.VisionTorch => item is VisionTorchItem,
            _ => false
        };
    }

    internal static string TryGetArmDataID(OWItem owItem)
    {
        if (IsBaseGameItem(owItem))
        {
            var itemType = owItem.GetItemType();
            switch (itemType)
            {
                // some items have variants
                case ItemType.Scroll:
                    return owItem.name switch
                    {
                        "Prefab_NOM_Scroll_egg" => "Scroll_Egg",
                        "Prefab_NOM_Scroll_Jeff" => "Scroll_Jeff",
                        _ => "Scroll"
                    };
                case ItemType.ConversationStone:
                    var word = (owItem as NomaiConversationStone).GetWord();
                    if (word == NomaiWord.Identify || word == NomaiWord.Explain)
                        return "ConversationStone_Big";
                    else
                        return "ConversationStone";

                case ItemType.WarpCore:
                    var warpCoreType = (owItem as WarpCoreItem).GetWarpCoreType();
                    if (warpCoreType == WarpCoreType.Vessel || warpCoreType == WarpCoreType.VesselBroken)
                        return "WarpCore";
                    else
                        return "WarpCore_Simple";

                case ItemType.DreamLantern:
                    return (owItem as DreamLanternItem).GetLanternType() switch
                    {
                        DreamLanternType.Nonfunctioning => "DreamLantern_Nonfunctioning",
                        DreamLanternType.Malfunctioning => "DreamLantern_Malfunctioning",
                        _ => "DreamLantern"
                    };

                // for the rest, their arm data identifier is simply their OWItem type
                default:
                    return itemType.GetName();
            }
        }

        return null;
    }

    internal static void OnEquipTool(PlayerTool tool)
    {
        // don't try to add viewmodel arm if disabled in config, if there is no armdata for it, or if this tool already has one
        if (!Config.EnableViewmodelArms || ArmData.GetArmData(tool.name) == null) return;

        // check for existing arm and enable if found (PlayerTool has no event for tool being equipped, so this is required)
        var existingArm = tool.transform.Find("ViewmodelArm");
        if (existingArm != null)
        {
            existingArm.gameObject.SetActive(true);
            return;
        }

        ViewmodelArm.NewViewmodelArm(tool);
    }

    internal static void OnPickUpItem(OWItem item)
    {
        if (!Config.EnableViewmodelArms) return;

        var isBaseGameItem = IsBaseGameItem(item);

        // rotate lantern to put it in better position for viewmodel arm
        if (isBaseGameItem && item.GetItemType() == ItemType.Lantern)
            item.transform.localEulerAngles = new Vector3(0f, 327f, 0f);

        // don't try to add viewmodel arm if disabled in config or if this item already has one
        if (item.transform.Find("ViewmodelArm")) return;

        if (isBaseGameItem)
            ViewmodelArm.NewViewmodelArm(item);
    }
}