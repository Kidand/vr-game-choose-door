using System;
using UnityEditor;
using UnityEngine;

namespace Cinderflame.Poseidon.UI
{
	public class UVSectionDrawer : PoseidonInspectorSection
	{
		protected override void DrawSection(SerializedObject serializedObject, bool showTitle = false)
		{
			if (showTitle)
				Styles.Title("Material / Submesh Properties");

			PoseidonInspector.Help(Help);

			var configuration = serializedObject.FindProperty(nameof(PoseidonBase.Configuration));
			var settings = configuration.FindPropertyRelative(nameof(Configuration.SubMeshSettings));

			Renderer renderer;
			PerformUVSettingsCheck(settings, out renderer);

			// TODO: We should be more serious about multiple 
			// object selection and not rely on inspector.target.
			var poseidon = inspector.target as Poseidon;
			if (poseidon == null)
			{
				EditorGUILayout.HelpBox("Something went wrong in trying to render UV Settings Overrides. Please try in Prefab Mode, selecting only one object at a time, or reopen the current scene.", MessageType.Error);
			}

			var sharedMaterials = renderer == null ? null : renderer.sharedMaterials;

			for (int i = 0; i < settings.arraySize; i++)
			{
				var setting = settings.GetArrayElementAtIndex(i);

				using (var section = new HorizontalSection(Styles.SubmeshSettingContainer))
				{
					// try to get the material that matches it. 
					Material mat = null;
					Texture2D tex = null;
					if (sharedMaterials != null && sharedMaterials.Length > i)
					{
						mat = sharedMaterials[i];
					}

					if (mat != null)
					{
						tex = AssetPreview.GetAssetPreview(mat);
					}

					var rect = GUILayoutUtility.GetRect(50, 50, GUILayout.ExpandWidth(false));
					DrawMaterialTexture(rect, tex);

					GUILayout.Space(20);
					using (new VerticalSection())
					{
						EditorGUILayout.LabelField(mat == null ? "Submesh " + i : mat.name, Styles.LabelBoldBig);

						var space = setting.FindPropertyRelative(nameof(SubmeshSettings.UVSpace));
						EditorGUILayout.PropertyField(space);

						if ((SubmeshSettings.UVStrategy)space.enumValueIndex == SubmeshSettings.UVStrategy.WorldSpace)
						{
							setting.DrawProperty(nameof(SubmeshSettings.ProjectionAxis));
							setting.DrawProperty(nameof(SubmeshSettings.Scale));
							setting.DrawProperty(nameof(SubmeshSettings.Offset));
							setting.DrawProperty(nameof(SubmeshSettings.Rotation));
							setting.DrawProperty(nameof(SubmeshSettings.OffsetIgnoresRotation));
						}
					}

				}
			}

			var ans = GUILayout.Toolbar(-1, new string[] { "Clear All", "World Space On All", "Reset" });
			if (ans != -1)
			{
				switch (ans)
				{
					case 0:
						UVAction("Clear Submesh Settings", (config) =>
						{
							config.SubMeshSettings.Clear();
						});
						break;
					case 1:
						UVAction("Force World Space", (config) =>
						{
							if (config.SubMeshSettings.Count == 0)
							{
								RecreateSubmeshSettings(poseidon);
							}

							for (int i = 0; i < config.SubMeshSettings.Count; i++)
							{
								var sms = config.SubMeshSettings[i];
								sms.UVSpace = SubmeshSettings.UVStrategy.WorldSpace;
								config.SubMeshSettings[i] = sms;
							}
						});
						break;
					case 2:
						UVAction("Reset Submesh Settings", (config) =>
						{
							RecreateSubmeshSettings(poseidon);
						});
						break;
				}
			}
		}

		private void RecreateSubmeshSettings(Poseidon p)
		{
			foreach (var target in inspector.targets)
			{
				var poseidon = target as Poseidon;

				if (poseidon == null)
				{
					Debug.LogError("Something went horribly wrong with recreating the UV settings. Please reselect your object and try again.", target);
				}

				var actualSubMeshCount = poseidon == null ? 0 : (poseidon.BaseMesh == null ? 0 : poseidon.BaseMesh.subMeshCount);
				poseidon.Configuration.SubMeshSettings.Clear();
				for (var i = 0; i < actualSubMeshCount; i++)
				{
					poseidon.Configuration.SubMeshSettings.Add(new SubmeshSettings(true));
				}

			}

		}

		/// <summary>
		/// Returns whether or not we should actually render UV settings
		/// </summary>
		/// <returns></returns>
		private void PerformUVSettingsCheck(SerializedProperty settings, out Renderer renderer)
		{
			if (inspector.serializedObject.isEditingMultipleObjects)
			{
				EditorGUILayout.HelpBox("Trying to edit multiple Poseidons at once... kind of works, but may have extremely unexpected results. Be careful.", MessageType.Warning);
			}

			renderer = null;

			var poseidon = inspector.target as Poseidon;
			if (poseidon != null)
			{
				renderer = poseidon.GetComponent<Renderer>();
			}

			if (renderer == null)
			{
				EditorGUILayout.HelpBox("No mesh renderer could be found - technically, things will still work for carving a mesh, but there's no materials visible. We'd highly recommend adding one.", MessageType.Warning);
			}
			else if (!(renderer is MeshRenderer))
			{
				EditorGUILayout.HelpBox("Poseidon really probably should only work with MeshRenderers..., not " + renderer.GetType(), MessageType.Warning);
			}

			var actualSubMeshCount = poseidon == null ? 0 : (poseidon.BaseMesh == null ? 0 : poseidon.BaseMesh.subMeshCount);
			var overrideCount = settings.arraySize;

			if (actualSubMeshCount != 0 && overrideCount == 0)
			{
				if (Styles.HelpBoxWithButton("You have not yet initialized your UV Overrides", "Generate", MessageType.Info))
				{
					Undo.RegisterCompleteObjectUndo(inspector.target, "Poseidon - Generate UV Settings");
					poseidon.Configuration.SubMeshSettings.Clear();
					for (int i = 0; i < actualSubMeshCount; i++)
					{
						poseidon.Configuration.SubMeshSettings.Add(new SubmeshSettings(true));
					}
				}
			}
			else if (actualSubMeshCount != overrideCount)
			{
				if (Styles.HelpBoxWithButton($"The number of UV overrides does not match the number of submeshes in your base mesh. This is technically fine, since Poseidon uses default values and ignores extra fields, but you can regenerate them if you like. This can happen if a mesh is edited so that we can dynamically pick up changes. \n\nIt is recommended to do this on a prefab if possible.", "Regenerate", MessageType.Warning))
				{
					Undo.RegisterCompleteObjectUndo(inspector.target, "Poseidon - Regenerate UV Settings");
					poseidon.Configuration.SubMeshSettings.Clear();
					for (int i = 0; i < actualSubMeshCount; i++)
					{
						poseidon.Configuration.SubMeshSettings.Add(new SubmeshSettings(true));
					}
				}

			}
		}

		private void DrawMaterialTexture(Rect rect, Texture2D tex)
		{
			var border = new Rect(rect.x - 1, rect.y - 1, rect.width + 2, rect.height + 2);
			EditorGUI.DrawRect(border, Styles.grayBorder);

			if (tex)
			{
				EditorGUI.DrawPreviewTexture(rect, tex);
			}
			else
			{
				EditorGUI.DrawRect(border, EditorStyles.centeredGreyMiniLabel.normal.textColor);
			}
		}

		private void UVAction(string name, Action<Configuration> action)
		{
			if(inspector.serializedObject.isEditingMultipleObjects)
			{
				Undo.RegisterCompleteObjectUndo(inspector.targets, "Poseidon (MULTIPLE) - " + name);

				foreach (var target in inspector.targets)
				{
					var poseidon = target as Poseidon;
					if(!poseidon)
					{
						Debug.LogError($"When performing action {name}, one of the Poseidons was null. Please investigate.", target);
						continue;
					}
					action(poseidon.Configuration);
				}
			}
			else
			{
				var poseidon = inspector.target as Poseidon;
				if (!poseidon) return;

				Undo.RegisterCompleteObjectUndo(inspector.target, "Poseidon - " + name);
				action(poseidon.Configuration);
			}
		}




		const string Help = @"Since Poseidon deals with merging objects together, you can choose to have Poseidon recalculate your UVs for the generated mesh to use World Space UVs. 

The process is fully reversible, and can have different settings for each submesh/material on your object. <b>World Space UVs</b> mean that the texture is applied based on where the object is in the world, which can minimize seams when combining Poseidons with identical textures.

It is recommended that most changes to the UV Settings be done at the Prefab level, if necessary.

<b>- Projection Axis:</b> Cardinal means that it uses all 3 axes, XYZ and applies the texture flattened onto it. If for some reason you need to only have it on one axis, you can choose that axis. That might matter only for something like, floors, or walls, etc.
<b>- Scale:</b> How big and small the texture for that material becomes.
<b>- Offset:</b> Slides the texture across the X and Y - useful in order to line up specific things. 
<b>- Rotation:</b> Adds an angular rotation to the texture itself. Ranges from -90 to 90 degrees.
<b>- Offset ignores Rotation:</b> If checked, applies offset before rotating. Otherwise, offset is applied to the rotated UV texture.";

	}
}