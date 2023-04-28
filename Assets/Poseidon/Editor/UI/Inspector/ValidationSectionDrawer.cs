using System;
using UnityEditor;
using UnityEngine;

namespace Cinderflame.Poseidon.UI
{
	public class ValidationSectionDrawer : PoseidonInspectorSection
	{
		protected override void DrawSection(SerializedObject serializedObject, bool showTitle = false)
		{
			if (serializedObject.isEditingMultipleObjects) return;
			var poseidon = inspector.target as Poseidon;
			if (!poseidon) return;
			
			PerformColliderChecks(poseidon);
			PerformMeshReadabilityCheck(poseidon);
		}

		private void PerformMeshReadabilityCheck(Poseidon poseidon)
		{
			if (poseidon.Configuration.Scope != CarveScope.EditorAndRuntime)
				return;

			// If we have a mesh... but that mesh is not readable, we should warn
			// them if they try to read it at runtime.
			if (poseidon.BaseMesh && !poseidon.BaseMesh.isReadable)
			{
				if (Styles.HelpBoxWithButton($"The mesh you are using is not marked as read-write enabled, and will not be readable for carving at Runtime. \n", "Fix Now", MessageType.Warning))
				{
					var path = AssetDatabase.GetAssetOrScenePath(poseidon.BaseMesh);
					var importer = AssetImporter.GetAtPath(path);
					if (importer is UnityEditor.ModelImporter modelImporter)
					{
						Undo.RegisterCompleteObjectUndo(importer, "Set Mesh Read-Write Enabled");
						modelImporter.isReadable = true;
						importer.SaveAndReimport();
					}
					else
					{
						Debug.LogWarning("Could not automatically get the ModelImporter for the mesh used. Please manually set the Mesh's Read-Write Enabled property to 'True'");
					}
				}
			}
		}

		private void PerformColliderChecks(Poseidon poseidon)
		{
			var colliders = poseidon.GetComponents<Collider>();
			
			if (colliders.Length == 0)
			{
				if (Styles.HelpBoxWithButton($"Poseidons are almost always used with a collider unless doing something purely aesthetic.\n", "Fix Now", MessageType.Warning))
				{
					Action("Add Mesh Collider", (p) =>
					{
						p.gameObject.AddComponent<MeshCollider>();
					});
				}
			}
			else if (colliders.Length > 1)
			{
				EditorGUILayout.HelpBox("This Poseidon has more than one Collider. This may be intentional, but will likely cause issues.", MessageType.Warning);
			}
			else
			{
				var collider = colliders[0];
				if (!(collider is MeshCollider))
				{
					if (Styles.HelpBoxWithButton($"Poseidon only works with MeshColliders, and this object has a {collider.GetType()}. This is probably not intended. Click fix now to delete the collider and replace it with a MeshCollider.", "Fix Now", MessageType.Warning))
					{
						Action("Fix Collider Type", (p) =>
						{
							UnityEngine.Object.DestroyImmediate(collider);
							p.gameObject.AddComponent<MeshCollider>();
						});
					}
				}
			}
		}

		private void Action(string name, Action<Poseidon> action)
		{

			var poseidon = inspector.target as Poseidon;
			if (!poseidon) return;

			Undo.RegisterCompleteObjectUndo(inspector.target, "Poseidon - " + name);
			action(poseidon);
		}
	}
}