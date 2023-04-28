using UnityEditor;
using UnityEngine;

namespace Cinderflame.Poseidon
{
	/// <summary>
	/// A very helpful class that allows you to easily create your own Poseidons
	/// in a 3D modeler, and import them. If your mesh has an object prefixed with
	/// _POSEIDON, it will automatically make it a Poseidon upon import. 
	/// </summary>
	public class ModelImporter : AssetPostprocessor
	{
		void OnPostprocessModel(GameObject obj)
		{
			if (PoseidonSettings.AutoImport.Get())
			{
				SearchForPoseidons(obj);
			}
		}

		private void SearchForPoseidons(GameObject obj)
		{
			StripObjectName(obj);
			
			for (int i = 0; i < obj.transform.childCount; i++)
			{
				var child = obj.transform.GetChild(i);

				StripObjectName(child.gameObject);

				if (HandlePoseidon(child.gameObject))
				{
					Object.DestroyImmediate(child.gameObject);
					i--;
					continue;
				}

				SearchForPoseidons(child.gameObject);
			}
		}

		private bool HandlePoseidon(GameObject go)
		{
			if (!go.transform.parent) return false; // A Poseidon can't be a top level object

			var name = go.name;
			var parent = go.transform.parent.gameObject;

			if (name.EndsWith("_POSEIDON") || name.EndsWith("_POSEIDON_OUT"))
			{
				Debug.Log($"[Poseidon] Importer found a new Carver on {go.name} | Root: {go.transform.root.name}");

				parent.AddComponent<MeshCollider>();
				parent.isStatic = true;

				var poseidon = parent.AddComponent<Poseidon>();

				poseidon.MeshFilter = parent.GetComponent<MeshFilter>();
				poseidon.BaseMesh = parent.GetComponent<MeshFilter>().sharedMesh;

				var renderer = parent.GetComponent<MeshRenderer>();
				if (renderer)
				{
					renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
				}

				if(name.EndsWith("_POSEIDON_OUT"))
				{
					poseidon.Configuration.Facing = Facing.Outward;
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// We strip off parentheses because many editors will only let you have 
		/// one object named the same thing. If there were multiple versions of 
		/// things labeled as "_POSEIDON" - sometimes those might come in as "_POSEIDON(2)"
		/// </summary>
		private void StripObjectName(GameObject obj)
		{
			if (obj.name.EndsWith(")"))
			{
				obj.name = obj.name.Substring(0, obj.name.LastIndexOf(" ("));
			}
		}
	}
}