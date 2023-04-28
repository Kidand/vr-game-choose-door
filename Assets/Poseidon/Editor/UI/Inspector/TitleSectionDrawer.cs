using UnityEditor;
using UnityEngine;

namespace Cinderflame.Poseidon.UI
{

	public class TitleSectionDrawer : PoseidonInspectorSection
	{
		protected override void DrawSection(SerializedObject serializedObject, bool showTitle = false)
		{
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();

			using (new HorizontalSection())
			{
				EditorGUILayout.LabelField(new GUIContent("<b>POSEIDON</b>"), new GUIStyle(Styles.LabelCenteredBig) { fontSize = 24 });

				var showHelp = PoseidonSettings.ShowHelp.Get();
				var newShowHelp = GUILayout.Toggle(showHelp, new GUIContent("?", "Show Help"), EditorStyles.toolbarButton, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true),
					GUILayout.MaxWidth(20),
					GUILayout.MinWidth(20),
					GUILayout.MinHeight(25),
					GUILayout.MaxHeight(25));
				if (showHelp != newShowHelp)
				{
					PoseidonSettings.ShowHelp.Set(newShowHelp);
				}

				var showAsTabs = PoseidonSettings.Tabbed.Get();
				var newShowAsTabs = GUILayout.Toggle(showAsTabs, new GUIContent("T", "Tabbed"), EditorStyles.toolbarButton, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true),
					GUILayout.MaxWidth(20),
					GUILayout.MinWidth(20),
					GUILayout.MinHeight(25),
					GUILayout.MaxHeight(25));
				if (showAsTabs != newShowAsTabs)
				{
					PoseidonSettings.Tabbed.Set(newShowAsTabs);
				}
			}
		}

	}
}