using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cinderflame.Poseidon.UI
{

	public class VerticalSection : IDisposable
	{
		public VerticalSection(params GUILayoutOption[] options)
		{
			EditorGUILayout.BeginVertical(options);
		}

		public VerticalSection(GUIStyle style, params GUILayoutOption[] options)
		{
			EditorGUILayout.BeginVertical(style, options);
		}

		public void Dispose()
		{
			EditorGUILayout.EndVertical();
		}
	}

	public class HorizontalSection : IDisposable
	{
		public Rect rect { get; set; }

		public HorizontalSection(params GUILayoutOption[] options)
		{
			rect = EditorGUILayout.BeginHorizontal(options);
		}

		public HorizontalSection(GUIStyle style, params GUILayoutOption[] options)
		{
			EditorGUILayout.BeginHorizontal(style, options);
		}

		public void Dispose()
		{
			EditorGUILayout.EndHorizontal();
		}
	}

	public class BackgroundColorScope : GUI.Scope
	{
		private Color oldColor;
		public BackgroundColorScope(Color color)
		{
			oldColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
		}

		protected override void CloseScope()
		{
			GUI.backgroundColor = oldColor;
		}
	}
}