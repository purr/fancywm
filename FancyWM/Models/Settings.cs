using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows.Media;

using FancyWM.Utilities;

namespace FancyWM.Models
{
    public enum OverflowPlacementStrategy
    {
        Vertical,
        Horizontal,
        Stack,
    }

    public interface ITilingServiceSettings
    {
        bool AllocateNewPanelSpace { get; }
        bool AnimateWindowMovement { get; }
        int WindowPadding { get; }
        int PanelHeight { get; }
        int AutoSplitCount { get; }
        OverflowPlacementStrategy OverflowPlacementStrategy { get; }
        bool ShowFocus { get; }
        bool AutoCollapsePanels { get; }
        bool DelayReposition { get; }
        bool AutoFloatNewWindows { get; }
        bool PreserveWindowPositionsOnExit { get; }
        bool EnableDragDropAutoPanelCreation { get; }
        bool HideWindowActionMenuOnHover { get; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class DefaultKeybindingAttribute(params KeyCode[] keys) : Attribute
    {
        public readonly KeyCode[] Keys = keys;
    }

    public record class Settings : IEquatable<Settings>, ITilingServiceSettings
    {
        public Settings()
        {

        }

        [JsonConverter(typeof(Converters.ActivationHotkeyConverter))]
        public ActivationHotkey ActivationHotkey { get; init; } = ActivationHotkey.Default;

        public bool ActivateOnCapsLock { get; init; } = false;

        public bool ShowStartupWindow { get; init; } = true;

        public bool NotifyVirtualDesktopServiceIncompatibility { get; init; } = true;

        public bool AllocateNewPanelSpace { get; init; } = true;

        public bool AutoCollapsePanels { get; init; } = false;

        public int AutoSplitCount { get; init; } = 2;
        public OverflowPlacementStrategy OverflowPlacementStrategy { get; init; } = OverflowPlacementStrategy.Stack;

        public bool DelayReposition { get; init; } = true;
        public bool AutoFloatNewWindows { get; init; } = false;
        // Keep current tiled positions when FancyWM is stopped/closed.
        // If false, stop() restores pre-tiling window positions.
        public bool PreserveWindowPositionsOnExit { get; init; } = true;

        // Governs drag-drop split/stack auto-creation from drop zones.
        // Default on to preserve the interactive tiling flow.
        public bool EnableDragDropAutoPanelCreation { get; init; } = true;
        public bool HideWindowActionMenuOnHover { get; init; } = false;

        public bool AnimateWindowMovement { get; init; } = true;

        public bool ModifierMoveWindow { get; init; } = false;

        public bool ModifierMoveWindowAutoFocus { get; init; } = false;

        public int WindowPadding { get; init; } = 4;

        public int PanelHeight { get; init; } = 18;

        public int PanelFontSize { get; init; } = 12;

        public bool ShowFocus { get; init; } = false;

        public bool ShowFocusDuringAction { get; init; } = true;

        public bool OverrideAccentColor { get; init; } = false;

        [JsonConverter(typeof(Converters.ColorConverter))]
        public Color CustomAccentColor { get; init; } = Color.FromRgb(0, 100, 255);

        [JsonConverter(typeof(Converters.KeybindingConverter))]
        public KeybindingDictionary Keybindings { get; init; } = [];

        public List<string> ProcessIgnoreList { get; init; } =
        [
            "Taskmgr"
        ];

        public List<string> ClassIgnoreList { get; init; } =
        [
            "OperationStatusWindow",
            "RAIL_WINDOW",
        ];

        public bool RemindToRateReview { get; init; } = true;

        public bool ShowContextHints { get; init; } = true;

        public bool MultiMonitorSupport { get; init; } = true;

        public bool SoundOnFailure { get; init; } = true;

        public bool CheckForUpdates { get; init; } = true;
    }
}
