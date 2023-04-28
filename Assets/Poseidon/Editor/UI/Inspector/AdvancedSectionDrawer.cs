using UnityEditor;
using UnityEngine; 
namespace Cinderflame.Poseidon.UI
{
	public class AdvancedSectionDrawer : PoseidonInspectorSection
	{
		protected override void DrawSection(SerializedObject serializedObject, bool showTitle = false)
		{
			if (showTitle)
				Styles.Title("Advanced Properties");

			var config = serializedObject.FindProperty(nameof(PoseidonBase.Configuration));

			PoseidonInspector.Help("Strategy allows you to choose between two different Poseidon algorithms.\n<b>Brutus</b> is a brute force algorithm. It works really really fast on small geometry, but can be slower on very complicated merges. <b>Octresius</b> is an octtree based carve operation, which is very fast for complex geometry, but has a bit of a ramp-up time, which means that it's actually slower for small geometry. <b>Auto</b> means that Poseidon tries to figure out which algorithm to use based on how many verts are in your mesh.");
			EditorGUILayout.PropertyField(config.FindPropertyRelative(nameof(Configuration.Strategy)));
			
			PoseidonInspector.Help("Allows you to mark a Poseidon for Runtime support. At the moment, this is mostly ignored, but in future releases, there may be some automated registering that happens at Runtime, but only for Poseidons that are appropriately marked");
			EditorGUILayout.PropertyField(config.FindPropertyRelative(nameof(Configuration.Scope)), new GUIContent("Scope"));
			
			PoseidonInspector.Help("If, for some reason, Poseidon is generating bad triangles, or there are parts of the merge that are not clean, there may be a situation where you want extremely, small triangles to be removed from the generated mesh. If you specify a very small number here, our algorithm will clean up all triangles with a smaller value during the clean up phase. -1 is the default number to skip that process.");
			EditorGUILayout.PropertyField(config.FindPropertyRelative(nameof(Configuration.SmallestTriangleArea)));
			PoseidonInspector.Help("When two Geometries have Co-planar polygons (imagine two floors carving into one another), the one with the higher priority takes precedence in having its texture shown.");
			EditorGUILayout.PropertyField(config.FindPropertyRelative(nameof(Configuration.Priority)));
			PoseidonInspector.Help("As of Poseidon works on closed, inward facing geometry. But sometimes, you're got an Outward facing mesh and need all of the faces to be flipped, pointing inward. This flips all normals during the generation phase. While we recommend you make a proper mesh in an editor, sometimes it can be easier to do this dynamically. <i>Caution</i> should be used with this, as flipping all faces might be a bit strange.");
			EditorGUILayout.PropertyField(config.FindPropertyRelative(nameof(Configuration.FlipAllFaces)));
			
			PoseidonInspector.Help(
				"Normally, a Poseidon carver exists within a context. For instance, an inward tunnel might carve out of a mountain. " +
				"By default, that tunnel would only exist where its parent exists. But in certain cases, you want to both carve something, but be unaffected by whoever is a higher layer object that is carving you.\n\n" +
				"Checking this means it exists in the void. Use this with caution.\n\n" +
				"Only makes sense in a world where you are carving both inward and outward-facing objects.");
			EditorGUILayout.PropertyField(config.FindPropertyRelative(nameof(Configuration.Protruding)),
					new GUIContent("Protrude Mode"));
		}
	} 
}