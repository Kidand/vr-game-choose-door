using UnityEditor;


namespace Cinderflame.Poseidon
{
	/// <summary>
	/// This class registers the PoseidonMouseGrabber with the SceneView 
	/// so that we can detect "Mouse Up" in deferred mode. It exists here
	/// so we can register using different Unity APIs based on the you're using.
	/// </summary>
	[InitializeOnLoad]
	public static class PoseidonMouseInspector
	{
		static PoseidonMouseInspector()
		{
#if UNITY_2019_1_OR_NEWER
			SceneView.duringSceneGui += PoseidonMouseGrabber.OnSceneGUI;
#else
			SceneView.onSceneGUIDelegate += PoseidonMouseGrabber.OnSceneGUI;
#endif
		}
	}
}