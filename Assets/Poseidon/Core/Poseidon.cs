using UnityEngine;

namespace Cinderflame.Poseidon
{
	/// <summary>
	/// Attach this script onto a GameObject with a MeshRenderer 
	/// (and optionally, a MeshCollider) to perform boolean/CSG
	/// carving operations automatically whenever the GameObject 
	/// intersects with other GameObjects containing a Poseidon script.
	/// 
	/// We include this as a subclass of PoseidonBase in order to 
	/// ensure that GUIDs don't update when upgrading DLLs to newer 
	/// versions. This instance exists outside of the main DLLs in
	/// order to keep forward compatibility.
	/// 
	/// Also, we have a few #defines which need to be in the actual
	/// raw compiled code so that it is specific to your editor version.
	/// </summary>

	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter))]
	public sealed class Poseidon : PoseidonBase
	{
		public override string DisplayName 
		{
			get
			{
				var name = gameObject.name;
#if UNITY_EDITOR
				// Seeing Mesh and Carver show up all the time can be extremely boring. 
				// Feel free to customize these if your 3D modeler of choice keeps 
				// naming your default meshes "Mesh" or whatever, and you're too lazy to rename things
				//
				// Either way we find that naming things "Carver" helps because then you can see the 
				// custom name you've given your parent. "Courtyard Door" (which has a child "Carver")
				// makes more sense than "Carver" - which one? 
				if (name.Equals("Mesh") || name.Equals("Carver"))
				{
#if UNITY_2018_3_OR_NEWER
					name = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject).name;
#else
					name = UnityEditor.PrefabUtility.FindPrefabRoot(gameObject).name;
#endif
				}
#endif

				return name;
			}
		}

		private void OnValidate()
		{
			MeshFilter = GetComponent<MeshFilter>();
			if(MeshFilter)
				MeshFilter.hideFlags |= HideFlags.NotEditable;
		}

		private void OnDisable()
		{
			MeshFilter = GetComponent<MeshFilter>();
			if(MeshFilter)
				MeshFilter.hideFlags &= ~HideFlags.NotEditable;
		}

		private void OnEnable()
		{
			RegisterPoseidon();
		}

		[ContextMenu("Rebuild Poseidon")]
		void RebuildPoseidon() => Dirty = true;

		[ContextMenu("Remove and Revert Mesh")]
		void RemoveCompletely()
		{
			#if UNITY_EDITOR
			ResetToBaseMesh();
			UnityEditor.EditorUtility.SetDirty(gameObject);
			DestroyImmediate(this);
			#endif
		}

		public void ResetToBaseMesh()
		{
			var meshFilter = GetComponent<MeshFilter>();
			if (meshFilter && BaseMesh)
			{
				meshFilter.sharedMesh = BaseMesh;
			}
		}
		
		/// <summary>
		/// Poseidon does not work in Prefab mode - if for some reason you want to do some 
		/// crazy prefab mode related stuff... you can change this at your own risk.
		/// </summary>
		public override bool IsPrefabMode()
		{
#if UNITY_2018_3_OR_NEWER && UNITY_EDITOR
			var currentStage = UnityEditor.SceneManagement.StageUtility.GetStageHandle(gameObject);
			var mainStage = UnityEditor.SceneManagement.StageUtility.GetMainStageHandle();
			return currentStage != mainStage;
#else
			return false;
#endif
		}
	}
}