using UnityEditor;
using UnityEngine;

namespace Cinderflame.Poseidon.UI
{

	public class MeshSectionDrawer : PoseidonInspectorSection
	{
		protected override void DrawSection(SerializedObject serializedObject, bool showTitle = false)
		{
			if (showTitle)
				Styles.Title("Mesh Reference");

			PoseidonInspector.Help("The <b>Base Mesh</b> is the most important part of Poseidon. Since we override the MeshFilter's mesh with our generated mesh, we keep a reference here to the original mesh that is used for all of the carving operations. Most of the time, this property should be the only thing you're ever configuring on a Poseidon.");
			EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(PoseidonBase.BaseMesh)));
			
			
			var config = serializedObject.FindProperty(nameof(PoseidonBase.Configuration));
			
			PoseidonInspector.Help(
				"Which way are the vertices in this object facing? Inward (Room) or Outward.");
				EditorGUILayout.PropertyField(config.FindPropertyRelative(nameof(Configuration.Facing)));
			
			if (PoseidonSettings.EditorCarveMode == PoseidonMode.Layered)
			{
				PoseidonInspector.Help(
					"For the most part, objects in a specific layer do not intersect with objects in a different layer. However, objects in layer 1 may contain (and cut off) those in layer 2, which may contain those in layer 3, etc. This can allow you to create nested Poseidons.");
				EditorGUILayout.PropertyField(config.FindPropertyRelative(nameof(Configuration.Layer)));
			}
		}
	}
}