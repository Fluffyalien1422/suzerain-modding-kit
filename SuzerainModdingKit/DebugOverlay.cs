using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppPixelCrushers.DialogueSystem;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;

namespace SuzerainModdingKit;

internal static class DebugOverlay
{
    public const int OverlayWidthDefault = 250;
    public const int OverlayWidthVarsList = 700;

    private static bool _show;
    private static VariableSearchOverlay _varsList;

    public static bool IsShowing()
    {
        return _show;
    }

    public static void SetVisibility(bool value)
    {
        _show = value;
        _varsList = null;

        // Show/hide the debug console panel. This is not strictly required, so we can ignore
        // if null.
        if (value)
        {
            Panels.Instance?.DebugConsolePanel?.Show();
        }
        else
        {
            Panels.Instance?.DebugConsolePanel?.Hide();
        }
    }

    public static void Update()
    {
        if (!IsShowing())
        {
            return;
        }

        string stepName = GameState.CurrentStepName;

        int overlayWidth = _varsList == null ? OverlayWidthDefault : OverlayWidthVarsList;
        GUILayout.BeginArea(new Rect(10, 10, overlayWidth, overlayWidth));
        GUILayout.BeginVertical(GUI.skin.box);

        GUILayout.Label("Debug Overlay");
        GUILayout.Label(stepName ?? "GameFlowManager not loaded!");

        if (_varsList == null)
        {
            if (GUILayout.Button("Hide (Ctrl+D)"))
            {
                SetVisibility(value: false);
            }
            // Only show these buttons if the game is loaded.
            if (stepName != null)
            {
                if (GUILayout.Button("Next Step"))
                {
                    Managers.Instance.GameFlowManager.EndStep();
                    string newStepName = GameState.CurrentStepName;
                    Melon<Core>.Logger.Msg($"Debug overlay: Skipped step '{stepName}'. " +
                        $"New step: '{newStepName}'.");
                }
                if (GUILayout.Button("Next Turn"))
                {
                    Melon<Core>.Logger.Msg("Debug overlay: Showing next turn button.");
                    Managers.Instance.GameFlowManager.ShowEndTurnButton();
                }
                if (GUILayout.Button("Reevaluate Step"))
                {
                    Melon<Core>.Logger.Msg("Debug overlay: Reevaluating current step.");
                    Managers.Instance.GameFlowManager.EvaluateCurrentStep();
                }
                if (GUILayout.Button("Variables"))
                {
                    _varsList = new();
                }
            }
        }
        else
        {
            // Auto hide the vars list if stepName is null (meaning the game is not loaded).
            if (GUILayout.Button("Back") || stepName == null)
            {
                _varsList = null;
            }
            else
            {
                _varsList.Update();
            }
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    public sealed class VariableSearchOverlay
    {
        private const float _itemHeight = 25f;
        private const float _listHeight = 200f;

        private string _searchQuery = "";
        private Vector2 _scrollPos;
        private List<string> _filteredItems;
        private readonly GUIStyle _buttonStyle = new(GUI.skin.button)
        {
            fontSize = 11,
            alignment = TextAnchor.MiddleLeft,
        };

        private void ComputeFilteredItems()
        {
            Il2CppStringArray items = DialogueLua.GetAllVariables();

            IEnumerable<string> filtered = string.IsNullOrEmpty(_searchQuery) ? items :
                items.Where(s => s.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase));

            _filteredItems = [.. filtered];
        }

        private void DrawList()
        {
            if (_filteredItems == null)
            {
                throw new InvalidOperationException(
                    "Cannot draw variables list while '_filteredItems' is null");
            }

            int itemCount = _filteredItems.Count;
            float totalHeight = itemCount * _itemHeight;

            _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Height(_listHeight));

            int firstVisible = Mathf.Max(0, Mathf.FloorToInt(_scrollPos.y / _itemHeight));
            int lastVisible = Mathf.Min(itemCount - 1,
                                   Mathf.CeilToInt((_scrollPos.y + _listHeight) / _itemHeight));

            // Render only visible items using absolute Rect positioning inside the scroll view.
            for (int i = firstVisible; i <= lastVisible; i++)
            {
                Rect rect = new(0, i * _itemHeight, OverlayWidthVarsList, _itemHeight);
                string varName = _filteredItems[i];
                if (GUI.Button(rect, varName, _buttonStyle))
                {
                    TMP_InputField field = Panels.Instance?.DebugConsolePanel?.inputField;
                    if (field != null)
                    {
                        field.Append($"Variable['{varName}']");
                        field.ActivateInputField();
                    }
                }
            }

            // Reserve the full scroll area height without GUILayout.Space
            // (GUILayout.Space is stripped by Il2Cpp in Suzerain, so we can't use it).
            GUILayout.Label("", GUILayout.Height(totalHeight), GUILayout.Width(0));

            GUILayout.EndScrollView();
        }

        public void Update()
        {
            GUILayout.Label("Variables");

            GUILayout.Label("Search:");
            string newSearchQuery = GUILayout.TextField(_searchQuery,
                GUILayout.Width(OverlayWidthVarsList - 50));

            // Update the filtered items if the search query has changed.
            if (_filteredItems == null ||
                !newSearchQuery.Equals(_searchQuery, StringComparison.OrdinalIgnoreCase))
            {
                ComputeFilteredItems();
            }
            _searchQuery = newSearchQuery;

            DrawList();
        }
    }
}
