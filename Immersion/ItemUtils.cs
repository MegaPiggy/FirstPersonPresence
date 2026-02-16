using UnityEngine;

namespace Immersion;

internal static class ItemUtils
{
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

    internal static bool IsTSTAItem(OWItem item)
    {
        if (!ModMain.IsTheStrangerTheyAreInstalled) return false;

        string itemName = item.GetDisplayName();
        return
            itemName == "Mineral" ||
            itemName == " Seal" ||
            itemName == "Skull";
    }

    internal static void OnPickUpItem(OWItem item)
    {
        if (IsTSTAItem(item) && item.GetDisplayName() == "Skull")
        {
            foreach (var renderer in item.GetComponentsInChildren<SkinnedMeshRenderer>())
                renderer.updateWhenOffscreen = true;
        }
    }

    internal static void OnDropItem(OWItem item)
    {
        if (IsTSTAItem(item) && item.GetDisplayName() == "Skull")
        {
            foreach (var renderer in item.GetComponentsInChildren<SkinnedMeshRenderer>())
                renderer.updateWhenOffscreen = false;
        }
    }
}
