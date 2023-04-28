using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cinderflame.Poseidon.UI
{ 
	public class ToolsSectionDrawer : PoseidonInspectorSection
	{
		protected override void DrawSection(SerializedObject serializedObject, bool showTitle = false)
		{
			if (showTitle)
				Styles.Title("Tools");

			DrawToolFromPref(PoseidonSettings.UseDeferred, new GUIContent("DEFERRED MODE", Styles.DelayIcon),
				"Only recalculates carves on Mouse Up when moving/rotating/scaling the object around. This way you can move objects about without worrying and carving through everything.");
			
			DrawToolFromPref(PoseidonSettings.LogRebuilds, new GUIContent("LOG REBUILD INFORMATION", Styles.TimingIcon), 
				"When working with many carvers, it can be helpful to see information after the carve about what happened, why the carve was triggered, and how long it took. Turn this off if you find it particularly annoying.");

			DrawToolFromPref(PoseidonSettings.DrawGhost, new GUIContent("DRAW WIREFRAME + REMOVED AREAS", Styles.WireframeIcon),
				"When enabled, draws a wireframe gizmo of the currently selected Poseidon, and causes the areas that been removed to pulsate in and out of existence");
			DrawToolAsButton(() => SettingsService.OpenUserPreferences("Preferences/Poseidon"),
				new GUIContent("SETTINGS", Styles.SettingsIcon),
				"Manage various Poseidon settings and preferences.");
		}

		private void DrawTool(Action toolButtonLambda, GUIContent content, string txt, bool darken = false)
		{
			var old = GUI.backgroundColor;
			if (darken)
			{
				GUI.backgroundColor = old * .75f;
			}

			using (new HorizontalSection(Styles.ToolBackground))
			{
				GUI.backgroundColor = old;
				using (new VerticalSection(GUILayout.ExpandWidth(false), GUILayout.MaxWidth(60)))
				{
					GUILayout.FlexibleSpace();
					toolButtonLambda();
					GUILayout.FlexibleSpace();
				}
				GUILayout.Space(10);
				using (new VerticalSection())
				{
					EditorGUILayout.LabelField(content.text, Styles.LabelBoldBig);

					GUILayout.FlexibleSpace();
					EditorGUILayout.LabelField(new GUIContent(txt), Styles.LabelSmall);
					GUILayout.FlexibleSpace();
				}
			}
		}

		private void DrawPrefButton(BoolPref pref, GUIContent label, GUIStyle style = null, Color selectedTint = default(Color), params GUILayoutOption[] layout)
		{
			if (style == null)
			{
				style = EditorStyles.toggle;
			}

			var value = pref.Get();

			var color = value ? selectedTint : Color.white;

			using (new BackgroundColorScope(color))
			{
				var newValue = GUILayout.Toggle(value, label, style, layout);
				if (value != newValue)
				{
					pref.Set(newValue);
				}
			}
		}

		private void DrawToolFromPref(BoolPref pref, GUIContent content, string txt, bool darken = false)
		{
			DrawTool(() => DrawPrefButton(pref, content, Styles.ToolButton, Styles.yellowIsh, GUILayout.Height(60), GUILayout.Width(60)),
				content, txt, darken);
		}

		private void DrawToolAsButton(Action onClick, GUIContent content, string txt, bool darken = false)
		{
			DrawTool(() =>
			{
				if (GUILayout.Button(content, Styles.ToolButton, GUILayout.Height(60), GUILayout.Width(60)))
				{
					onClick();
				}
			}, content, txt, darken);
		}
	}
}