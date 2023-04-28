using UnityEditor;
using UnityEngine;

namespace Cinderflame.Poseidon.UI
{
	/// <summary>
	/// This mess of a file is a rather horrid concoction of all of the Cinderflame
	/// themes for inspectors, random icons, GUISkins, shared colors, and apparently 
	/// extensions for various drawing operations. 
	/// </summary>
	public static class Styles
	{

		#region Styles

		public static GUIStyle OuterContainer { get; private set; }
		public static GUIStyle InnerContainer { get; private set; }
		public static GUIStyle SubmeshSettingContainer { get; private set; }
		public static GUIStyle Toolbar { get; private set; }
		public static GUIStyle HelpBox { get; private set; }
		public static GUIStyle ToolButton { get; private set; }
		public static GUIStyle ToolBackground { get; private set; }

		#endregion

		#region Icons

		public static Texture HelpIcon { get; private set; }
		public static Texture DelayIcon { get; private set; }
		public static Texture TimingIcon { get; private set; }
		public static Texture WireframeIcon { get; private set; }
		public static Texture ProgressBarIcon { get; private set; }
		public static Texture SettingsIcon { get; private set; }

		#endregion

		#region Labels

		public static GUIStyle Label { get; private set; }
		public static GUIStyle LabelCentered { get; private set; }
		public static GUIStyle LabelBold { 	get; private set; }
		public static GUIStyle LabelBoldCentered { get; private set; }
		public static GUIStyle LabelBig { get; private set; }
		public static GUIStyle LabelBoldBig { get; private set; }
		public static GUIStyle LabelCenteredBig { get; private set; }
		public static GUIStyle LabelBoldCenteredBig { get; private set; }
		public static GUIStyle LabelSmall { get; private set; }
		public static GUIStyle LabelBoldSmall { get; private set; }
		public static GUIStyle LabelCenteredSmall { get; private set; }
		public static GUIStyle LabelBoldCenteredSmall { get; private set; }
		#endregion


		#region Constants

		private static readonly Color fontColor = Color.white * 0.85f;
		public static readonly Color yellowIsh = new Color(1, 1, 0.6f, 1);
		public static readonly Color grayBorder = new Color(0.19f, 0.19f, 0.19f, 1);


		/// <summary>
		/// The font size of big texts
		/// </summary>
		private const int fontBigSize = 13;
		/// <summary>
		/// The font size of small texts
		/// </summary>
		private const int fontSmallSize = 9;
		/// <summary>
		/// The default margins size
		/// </summary>
		private const int defaultPadding = 8;
		/// <summary>
		/// The default margin rectoffset
		/// </summary>
		private static readonly RectOffset PADDING = new RectOffset(defaultPadding, defaultPadding, defaultPadding, defaultPadding);
		private static readonly RectOffset ZERO = new RectOffset(0, 0, 0, 0);
		#endregion

		static Styles()
		{
			CreateBackgrounds();
			CreateLabels();

			HelpIcon = Resources.Load<Texture2D>("Poseidon.Help");
			DelayIcon = Resources.Load<Texture2D>("Poseidon.Delay");
			WireframeIcon = Resources.Load<Texture2D>("Poseidon.Wireframe");
			ProgressBarIcon = Resources.Load<Texture2D>("Poseidon.Progress");
			SettingsIcon = Resources.Load<Texture2D>("Poseidon.Settings");
			TimingIcon = Resources.Load<Texture2D>("Poseidon.Time");

		}

		private static void CreateBackgrounds()
		{
#if UNITY_2019_3_OR_NEWER
			OuterContainer = new GUIStyle(EditorStyles.helpBox)
#else
			OuterContainer = new GUIStyle
#endif

			{
				alignment = TextAnchor.MiddleCenter,
				border = new RectOffset(2, 2, 2, 2),
				imagePosition = ImagePosition.ImageOnly,
				padding = ZERO,
				normal = new GUIStyleState
				{
					textColor = fontColor,
					background = EditorStyles.helpBox.normal.background
				}
			};
#if UNITY_2019_3_OR_NEWER
			SubmeshSettingContainer = new GUIStyle(EditorStyles.helpBox)
#else
			SubmeshSettingContainer = new GUIStyle
#endif
			{
				alignment = TextAnchor.MiddleCenter,
				border = new RectOffset(2, 2, 2, 2),
				imagePosition = ImagePosition.ImageOnly,
				padding = PADDING,
				normal = new GUIStyleState
				{
					textColor = fontColor,
					background = EditorStyles.helpBox.normal.background
				}
			};

			InnerContainer = new GUIStyle(EditorStyles.inspectorDefaultMargins) { padding = PADDING };

			Toolbar = new GUIStyle(EditorStyles.toolbarButton)
			{
				fixedHeight = 24f,
				fontSize = 12
			};

			if (EditorGUIUtility.isProSkin)
				Toolbar.onNormal.textColor = new Color(1, 1, 0.75f, 1);

			HelpBox = new GUIStyle(EditorStyles.helpBox) { richText = true };


			ToolButton = new GUIStyle("Button")
			{
				fontSize = 11,
				alignment = TextAnchor.MiddleCenter,
				fixedHeight = 0,
				fontStyle = FontStyle.Bold,
				imagePosition = ImagePosition.ImageOnly,
				padding = new RectOffset(6, 6, 6, 6),
			};


			ToolBackground = new GUIStyle(SubmeshSettingContainer);
			ToolBackground.normal.background = EditorStyles.textArea.normal.background;
		}

		private static void CreateLabels()
		{
			Label = new GUIStyle
			{
				alignment = TextAnchor.MiddleLeft,
				padding = new RectOffset(3, 3, 3, 3),
				richText = true,
				wordWrap = true
			};

			if (EditorGUIUtility.isProSkin)
			{
				Label.normal.textColor = fontColor;
			}

			// Weeee!
			LabelBig = new GUIStyle(Label)					{ fontSize = fontBigSize };
			LabelBold = new GUIStyle(Label)					{ fontStyle = FontStyle.Bold };
			LabelSmall = new GUIStyle(Label)				{ fontSize = fontSmallSize };
			LabelBoldBig = new GUIStyle(LabelBold)			{ fontSize = LabelBig.fontSize };
			LabelCentered = new GUIStyle(Label)				{ alignment = TextAnchor.UpperCenter };
			LabelBoldSmall = new GUIStyle(LabelBold)		{ fontSize = LabelSmall.fontSize };
			LabelCenteredBig = new GUIStyle(LabelCentered)	{ fontSize = LabelBig.fontSize };
			LabelBoldCentered = new GUIStyle(LabelBold)		{ alignment = LabelCentered.alignment };
			LabelCenteredSmall = new GUIStyle(LabelCentered)			{ fontSize = LabelSmall.fontSize };
			LabelBoldCenteredBig = new GUIStyle(LabelBoldCentered)		{ fontSize = LabelBig.fontSize };
			LabelBoldCenteredSmall = new GUIStyle(LabelBoldCentered)	{ fontSize = LabelSmall.fontSize };
		}

		public static void DrawLine(Color color, int thickness = 2, int padding = 10)
		{
			var rect = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
			rect.height = thickness;
			rect.y += padding / 2;
			rect.x -= 2;
			rect.width += 6;
			EditorGUI.DrawRect(rect, color);
		}

		public static void DrawHorizontalLine(int thickness = 2, int padding = 10)
		{
			DrawLine(Label.normal.textColor, thickness, padding);
		}

		public static void Title(string s)
		{
			EditorGUILayout.Separator();
			EditorGUILayout.LabelField(s, LabelBoldBig);
			DrawHorizontalLine(1, 1);
			EditorGUILayout.Separator();
		}

		public static bool HelpBoxWithButton(string message, string button, MessageType type = MessageType.None)
		{
			Rect rect = GUILayoutUtility.GetRect(new GUIContent(message), HelpBox);
			GUILayoutUtility.GetRect(1f, 25f);
			rect.height += 25f;
			EditorGUI.HelpBox(rect, message, type);
			//GUI.Label(rect, message, EditorStyles.helpBox);
			Rect position = new Rect(rect.xMax - 80f - 4f, rect.yMax - 20f - 4f, 80f, 20f);
			return GUI.Button(position, button);
		}

		public static void DrawProperty(this SerializedProperty prop, string relative)
		{
			EditorGUILayout.PropertyField(prop.FindPropertyRelative(relative));
		}

		public static Rect Shift(this Rect r, float x, float y)
		{
			r.x += x;
			r.y += y;
			return r;
		}
	}
}