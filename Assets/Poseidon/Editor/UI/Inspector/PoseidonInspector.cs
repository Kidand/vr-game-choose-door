using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cinderflame.Poseidon.UI
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Poseidon))]
	public class PoseidonInspector : UnityEditor.Editor
	{
		private int selectedTab = 0;

		private PoseidonInspectorSection titleSection = new TitleSectionDrawer();
		private PoseidonInspectorSection validationSection = new ValidationSectionDrawer();

		private List<PoseidonInspectorSection> sections = new List<PoseidonInspectorSection>
		{
			new MeshSectionDrawer(),
			new UVSectionDrawer(),
			new AdvancedSectionDrawer(),
			new ToolsSectionDrawer(), 
		};

		[MenuItem("CONTEXT/" + nameof(Poseidon) + "/Switch Tabbed-Linear")]
		private static void ToggleTabbed() => PoseidonSettings.Tabbed.Toggle();

		[MenuItem("CONTEXT/" + nameof(Poseidon) + "/Toggle Help Bubbles")]
		private static void ToggleHelp() => PoseidonSettings.ShowHelp.Toggle();

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			titleSection.Draw(this);
			validationSection.Draw(this);

			if (PoseidonSettings.Tabbed.Get())
				DrawTabView();
			else
				DrawLinearView();
			

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawLinearView()
		{
			foreach(var section in sections)
				section.Draw(this, true);
		}

		internal static void Help(string s)
		{
			if (PoseidonSettings.ShowHelp)
			{
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.LabelField(new GUIContent(
					s, Styles.HelpIcon, "In order to hide Poseidon Help Text, click the Question Mark Button in the top right of the inspector."), 
					Styles.HelpBox);
			}
		}

		private void DrawTabView()
		{
			string[] tabs = { "Geometry", "UV", "Advanced", "Tools" };

			using (new VerticalSection(Styles.OuterContainer))
			{
#if UNITY_2019_3_OR_NEWER
				// TODO: This should be moved to Styles.
				using (new HorizontalSection(new GUIStyle(EditorStyles.helpBox) {
					padding = new RectOffset(2,1,1,1), 
					margin = new RectOffset()
				}))
				{
					selectedTab = GUILayout.Toolbar(selectedTab, tabs, EditorStyles.toolbarButton);
				}
#else
				selectedTab = GUILayout.Toolbar(selectedTab, tabs, Styles.Toolbar);
#endif
				using (new VerticalSection(Styles.InnerContainer))
				{
					sections[selectedTab].Draw(this, false);
				}
			}
		}
	}
}