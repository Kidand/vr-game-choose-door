//#define USE_POSEIDON_TICK

using System;
using UnityEditor;
using UnityEngine;

namespace Cinderflame.Poseidon.UI
{
	[InitializeOnLoad]
	public class PoseidonSceneOverlay
	{
		static SceneView view;

		private static Texture2D Pause;
		private static Texture2D Resume;
		private static Texture2D Pause_Hover;
		private static Texture2D Resume_Hover;

		static PoseidonSceneOverlay()
		{
#if UNITY_2019_1_OR_NEWER
			SceneView.duringSceneGui -= OnSceneGUI;
			SceneView.duringSceneGui += OnSceneGUI;
#else
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
			SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
			LoadTextures();
		}

		private static void LoadTextures()
		{
			try
			{
				Pause = Resources.Load<Texture2D>("Poseidon_Pause");
				Pause_Hover = Resources.Load<Texture2D>("Poseidon_Pause_Hover");

				Resume = Resources.Load<Texture2D>("Poseidon_Resume");
				Resume_Hover = Resources.Load<Texture2D>("Poseidon_Resume_Hover");
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

		private static void OnSceneGUI(SceneView sceneView)
		{
			view = sceneView;

			if (!PoseidonSettings.ShowSceneOverlay.Get()) return;

			Handles.BeginGUI();
			HandleUI();
#if USE_POSEIDON_TICK
			HandleTick();
#endif
			Handles.EndGUI();
		}

		public static void HandleUI()
		{
			const float W = 32, H = 32;
			const float X = 8, Y = 30;

			var frozen = PoseidonSettings.FreezeAllRebuilds;

			var bottomContent = new GUIContent(frozen ? "Paused" : "Running");
			var buttonIcon = frozen ? Resume : Pause;
			var hoverIcon = frozen ? Resume_Hover : Pause_Hover;

			if(buttonIcon == null || hoverIcon == null) LoadTextures();

			var style = new GUIStyle(Styles.LabelCenteredSmall) { alignment = TextAnchor.UpperCenter };
			style.fontSize -= 2;
			style.padding = new RectOffset();
			GUI.Label(BottomRightOut(new Rect(X, Y - 2, W, 4)), bottomContent, style);


			var textPos = BottomRightOut(new Rect(X, Y + H, W + 10, 10));
			if (textPos.Contains(Event.current.mousePosition))
			{
				style.fontStyle = FontStyle.Bold;
			}
			style.alignment = TextAnchor.MiddleCenter;
			style.fontSize += 1;
			style.wordWrap = false;
			GUI.Label(textPos, new GUIContent("Poseidon"), style);

			if (GUI.Button(textPos, new GUIContent(Texture2D.blackTexture), GUIStyle.none))
			{
				SettingsService.OpenUserPreferences("Preferences/Poseidon");
			}

			var pos = BottomRightOut(new Rect(X, Y, W, H));
			if (pos.Contains(Event.current.mousePosition))
			{
				GUI.DrawTexture(pos.Shift(1, 1), hoverIcon);
			}

			if (GUI.Button(pos, buttonIcon, GUIStyle.none))
			{
				PoseidonSettings.FreezeAllRebuilds.Toggle();
			}
		}

#if USE_POSEIDON_TICK
		public static void HandleTick()
		{
			// This is mostly used as a debugging tool for debugging 
			// individual frames of Poseidon as it runs in Editor. 
			// If you're actually enabling this, you will likely find
			// that the logic might not complete if you're doing frame 
			// operations. This works best if the whole system is paused.

			const float W = 32, H = 32;
			const float X = 56, Y = 30;
			
			var pos = BottomRightOut(new Rect(X, Y, W, H));
			if (pos.Contains(Event.current.mousePosition))
			{
				GUI.DrawTexture(pos.Shift(1, 1), Resume_Hover);
			}
			
			if (GUI.Button(pos, Resume, GUIStyle.none))
			{
				Zeus.TickRunner();
			}
		}
#endif

		public static Rect BottomOut(Rect rect) =>
			new Rect(rect.x, view.position.height - rect.y - rect.height, rect.width, rect.height);
		public static Rect BottomRightOut(Rect rect) =>
			new Rect(view.position.width - rect.x - rect.width, view.position.height - rect.y - rect.height, rect.width, rect.height);
	}
}