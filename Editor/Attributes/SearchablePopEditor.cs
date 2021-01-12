using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Monogum.BricksBucket.Core.Editor.Attributes
{
    // ReSharper disable CommentTypo
    /// <!-- SearchablePopup -->
    /// 
    /// <summary>
    ///
    /// <para>
    /// A popup window that displays a list of options and may use a search
    /// string to filter the displayed content.
    /// </para>
    ///
    /// <para>
    /// Based in the UnityEditorJunkie project by @roboryantron.
    /// https://github.com/roboryantron/UnityEditorJunkie
    /// </para>
    ///
    /// </summary>
    /// 
    /// <!-- By Javier García | @jvrgms | 2020 -->
    // ReSharper restore CommentTypo
    public class SearchablePopup : PopupWindowContent
    {

        #region Fields

        /// <summary>
        /// How far to indent list entries.
        /// </summary>
        private const float Indent = 8.0f;

        /// <summary>
        /// Name to use for the text field for search.
        /// </summary>
        private const string SearchControlName = "EnumSearchText";

        /// <summary>
        /// GUIStyles implicitly cast from a string.
        /// </summary>
        private static readonly GUIStyle Selection = "SelectionRect";
        
        /// <summary>
        /// Callback to trigger when an item is selected.
        /// </summary>
        private readonly Action<int> _onSelectionMade;

        /// <summary>
        /// Index of the item that was selected when the list was opened.
        /// </summary>
        private readonly int _currentIndex;

        /// <summary>
        /// Container for all available options that does the actual string
        /// filtering of the content.
        /// </summary>
        private readonly FilteredList _list;

        /// <summary>
        /// Scroll offset for the vertical scroll area.
        /// </summary>
        private Vector2 _scroll;

        /// <summary>
        /// Index of the item under the mouse or selected with the keyboard.
        /// </summary>
        private int _hoverIndex;

        /// <summary>
        /// An item index to scroll to on the next draw.
        /// </summary>
        private int _scrollToIndex;

        /// <summary>
        /// An offset to apply after scrolling to scrollToIndex. This can be
        /// used to control if the selection appears at the top, bottom, or
        /// center of the popup.
        /// </summary>
        private float _scrollOffset;

        #endregion



        #region Constructor

        /// <summary> Creates a new instance of searchable popup. </summary>
        /// <param name="names"> Collection for popup filling. </param>
        /// <param name="currentIndex"> Current selected index. </param>
        /// <param name="onSelectionCallback">Action called on selected.</param>
        private SearchablePopup (
            string[] names,
            int currentIndex,
            Action<int> onSelectionCallback
        ) {
            _list = new FilteredList (names);
            this._currentIndex = currentIndex;
            this._onSelectionMade = onSelectionCallback;

            _hoverIndex = currentIndex;
            _scrollToIndex = currentIndex;
            _scrollOffset = GetWindowSize ().y -
                EditorGUIUtility.singleLineHeight * 2;
        }

        #endregion



        #region PopupWindowContent Overrides

        /// <summary> Callback when the popup window is open. </summary>
        public override void OnOpen ()
        {
            base.OnOpen ();
            // Force a repaint every frame to be responsive to mouse hover.
            EditorApplication.update += Repaint;
        }

        /// <summary> Callback when the popup window is close. </summary>
        public override void OnClose ()
        {
            base.OnClose ();
            // ReSharper disable once DelegateSubtraction
            EditorApplication.update -= Repaint;
        }

        /// <summary> Returns the size of the popup window. </summary>
        /// <returns> Size of the popup window. </returns>
        public sealed override Vector2 GetWindowSize ()
        {
            var size = _list.MaxLength * EditorGUIUtility.singleLineHeight +
                EditorStyles.toolbar.fixedHeight;
            return new Vector2 (base.GetWindowSize ().x, Mathf.Min (600, size));
        }

        /// <summary> Draws the popup window options. </summary>
        /// <param name="rect"> Rect where to draw popup. </param>
        public override void OnGUI (Rect rect)
        {
            var searchRect = new Rect (
                x: 0, y: 0,
                width: rect.width,
                height: EditorStyles.toolbar.fixedHeight
            );

            var scrollRect = Rect.MinMaxRect (
                xmin: 0, ymin: searchRect.yMax,
                xmax: rect.xMax, ymax: rect.yMax
            );

            HandleKeyboard ();
            DrawSearch (searchRect);
            DrawSelectionArea (scrollRect);
        }

        #endregion



        #region Static Methods

        /// <summary> Show a new SearchablePopup. </summary>
        /// <param name="activatorRect">
        /// Rectangle of the button that triggered the popup.
        /// </param>
        /// <param name="options">List of strings to choose from.</param>
        /// <param name="current">
        /// Index of the currently selected string.
        /// </param>
        /// <param name="onSelectionCallback">
        /// Callback to trigger when a choice is made.
        /// </param>
        public static void Show (
            Rect activatorRect,
            string[] options,
            int current,
            Action<int> onSelectionCallback
        ) {
            var win = new SearchablePopup (
                names: options,
                currentIndex: current,
                onSelectionCallback: onSelectionCallback
            );
            PopupWindow.Show (activatorRect, win);
        }

        /// <summary>
        /// Force the focused window to redraw. This can be used to make the
        /// popup more responsive to mouse movement.
        /// </summary>
        private static void Repaint ()
        {
            EditorWindow.focusedWindow.Repaint ();
        }

        #endregion



        #region Class Implementation

        /// <summary> Draws Search Input Field. </summary>
        /// <param name="rect"> Rect to draw search input field. </param>
        private void DrawSearch (Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
                EditorStyles.toolbar.Draw (rect, false, false, false, false);

            var searchRect = new Rect (rect);
            searchRect.xMin += 6;
            searchRect.xMax -= 6;
            searchRect.y += 2;
            searchRect.height = EditorGUIUtility.singleLineHeight - 2;

            GUI.FocusControl (SearchControlName);
            GUI.SetNextControlName (SearchControlName);
            string newText = GUI.TextField (searchRect, _list.Filter);

            if (_list.UpdateFilter (newText))
            {
                _hoverIndex = 0;
                _scroll = Vector2.zero;
            }

            searchRect.x = searchRect.xMax;

            if (string.IsNullOrEmpty (_list.Filter))
                GUI.Box (searchRect, GUIContent.none);
        }

        /// <summary> Draws selection area with options. </summary>
        /// <param name="scrollRect"> Rect to scroll. </param>
        private void DrawSelectionArea (Rect scrollRect)
        {
            var contentRect = new Rect (0, 0,
                scrollRect.width - GUI.skin.verticalScrollbar.fixedWidth,
                _list.Entries.Count * EditorGUIUtility.singleLineHeight);

            _scroll = GUI.BeginScrollView (scrollRect, _scroll, contentRect);

            var rowRect = new Rect (
                x: 0, y: 0,
                width: scrollRect.width,
                height: EditorGUIUtility.singleLineHeight
            );

            for (int i = 0; i < _list.Entries.Count; i++)
            {
                if (_scrollToIndex == i &&
                    (Event.current.type == EventType.Repaint
                     || Event.current.type == EventType.Layout))
                {
                    var r = new Rect (rowRect);
                    r.y += _scrollOffset;
                    GUI.ScrollTo (r);
                    _scrollToIndex = -1;
                    _scroll.x = 0;
                }

                if (rowRect.Contains (Event.current.mousePosition))
                {
                    switch (Event.current.type)
                    {
                        case EventType.MouseMove:
                        case EventType.ScrollWheel:
                            _hoverIndex = i;
                            break;
                        case EventType.MouseDown:
                            _onSelectionMade (_list.Entries[i].index);
                            EditorWindow.focusedWindow.Close ();
                            break;
                    }
                }

                DrawRow (rowRect, i);

                rowRect.y = rowRect.yMax;
            }

            GUI.EndScrollView ();
        }

        /// <summary> Draws a single option. </summary>
        /// <param name="rowRect"> Rect to draw option. </param>
        /// <param name="i"> Index of the option. </param>
        private void DrawRow (Rect rowRect, int i)
        {
            if (_list.Entries[i].index == _currentIndex || i == _hoverIndex)
                GUI.Box (rowRect, "", Selection);

            Rect labelRect = new Rect (rowRect);
            labelRect.xMin += Indent;

            EditorGUI.indentLevel++;
            GUI.Label (labelRect, _list.Entries[i].text);
            EditorGUI.indentLevel--;
        }

        /// <summary>
        /// Process keyboard input to navigate the choices or make a selection.
        /// </summary>
        private void HandleKeyboard ()
        {
            if (Event.current.type != EventType.KeyDown) return;
            if (Event.current.keyCode == KeyCode.DownArrow)
            {
                _hoverIndex = Mathf.Min (
                    a: _list.Entries.Count - 1,
                    b: _hoverIndex + 1
                );
                Event.current.Use ();
                _scrollToIndex = _hoverIndex;
                _scrollOffset = EditorGUIUtility.singleLineHeight;
            }

            if (Event.current.keyCode == KeyCode.UpArrow)
            {
                _hoverIndex = Mathf.Max (0, _hoverIndex - 1);
                Event.current.Use ();
                _scrollToIndex = _hoverIndex;
                _scrollOffset = -EditorGUIUtility.singleLineHeight;
            }

            switch (Event.current.keyCode)
            {
                case KeyCode.Return:
                {
                    if (_hoverIndex >= 0 && _hoverIndex < _list.Entries.Count)
                    {
                        _onSelectionMade (_list.Entries[_hoverIndex].index);
                        EditorWindow.focusedWindow.Close ();
                    }

                    break;
                }
                case KeyCode.Escape:
                    EditorWindow.focusedWindow.Close ();
                    break;
                
                default: return;
            }
        }

        #endregion



        #region Nested Classes

        /// <summary>
        /// Stores a list of strings and can return a subset of that list that
        /// matches a given filter string.
        /// </summary>
        private class FilteredList
        {
            #region Fields

            /// <summary> All possible items in the list. </summary>
            private readonly string[] _allItems;

            #endregion

            
            #region Properties

            /// <summary> The current string filtering the list. </summary>
            public string Filter { get; private set; }

            /// <summary> All valid entries for the current filter. </summary>
            public List<Entry> Entries { get; private set; }

            /// <summary> Total possible entries in the list. </summary>
            public int MaxLength => _allItems.Length;

            #endregion

            
            #region Methods

            /// <summary> Create a new filtered list. </summary>
            /// <param name="items">All The items to filter.</param>
            public FilteredList (string[] items)
            {
                _allItems = items;
                Entries = new List<Entry> ();
                UpdateFilter ("");
            }

            /// <summary>
            /// Sets a new filter string and updates the Entries that match the
            /// new filter if it has changed.
            /// </summary>
            /// <param name="filter">String to use to filter the list.</param>
            /// <returns>
            /// True if the filter is updated, false if newFilter is the same
            /// as the current Filter and no update is necessary.
            /// </returns>
            public bool UpdateFilter (string filter)
            {
                if (Filter == filter)
                    return false;

                Filter = filter;
                Entries.Clear ();

                for (int i = 0; i < _allItems.Length; i++)
                {
                    if (!string.IsNullOrEmpty (Filter) && !_allItems[i].
                        ToLower ().Contains (Filter.ToLower ()))
                        continue;
                    var entry = new Entry
                    {
                        index = i,
                        text = _allItems[i]
                    };
                    if (string.Equals (
                        a: _allItems[i],
                        b: Filter,
                        comparisonType:
                        StringComparison.CurrentCultureIgnoreCase
                    ))
                        Entries.Insert (0, entry);
                    else
                        Entries.Add (entry);
                }

                return true;
            }

            #endregion

            
            #region Nested Classes

            /// <summary>
            /// An entry in the filtered list, mapping the text to the
            /// original index.
            /// </summary>
            public struct Entry
            {
                /// <summary> Entry Index Reference. </summary>
                public int index;

                /// <summary> Entry Text Reference. </summary>
                public string text;
            }

            #endregion
        }

        #endregion

    }
}
