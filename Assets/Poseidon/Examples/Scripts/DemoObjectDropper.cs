using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cinderflame.Poseidon.Examples
{
	/// <summary>
	/// This is just a sample "object dropper" that lets you
	/// intersect some mesh at runtime with a base mesh in the middle
	/// of the scene in order to showcase more dynamic carving.
	///
	/// This is rather simplified/fragile in order to demonstrate
	/// how the product works, rather than to be copied/reused somewhere else.
	///
	/// Use the scroll wheel to swap between multiple meshes.
	/// </summary>
	public class DemoObjectDropper : MonoBehaviour
	{
		public Camera mainCamera;
		
		public Poseidon baseObject;
		public Poseidon carveObject;
		
		public float depth = .1f;

		[Header("Floating Gizmo")]
		public Transform wireframeGizmo;
		private MeshFilter wireframeMeshFilter;
		
		[Header("UI Section")] 
		public Image progressMeter;
		public Text percentageText;

		private PoseidonOperation operation;
		public List<Mesh> carvingMeshes = new List<Mesh>();
		private int currentMeshIndex = 0;

		void Start()
		{
			wireframeMeshFilter = wireframeGizmo.GetComponent<MeshFilter>();
			
			// Set an initial mesh to carve with
			if (currentMeshIndex < carvingMeshes.Count && currentMeshIndex >= 0)
			{
				Debug.Log("Setting Base Mesh of Carver to " + carvingMeshes[currentMeshIndex].name);
				carveObject.BaseMesh = carvingMeshes[currentMeshIndex];
				wireframeMeshFilter.sharedMesh = carvingMeshes[currentMeshIndex];
			}
		}
		
		void Update()
		{
			// If we have a running operation, and it's not done,
			// let's run a bit of it every frame.
			if (operation != null && !operation.Finished)
			{
				operation.RunFrame();
				progressMeter.fillAmount = 1 - operation.Progress;
				percentageText.text = $"{operation.Progress * 100:#,0.00}%";
				
				// We won't allow any other clicks/hovers to happen in this demo
				// until the operation is over.
				return;
			}

			HandleScrollingToChangeCarvingMesh();

			Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit)){

				if (hit.collider.gameObject == baseObject.gameObject)
				{
					wireframeGizmo.position = hit.point;
					wireframeGizmo.LookAt(hit.point + hit.normal);
					wireframeGizmo.position = wireframeGizmo.position - wireframeGizmo.forward * depth;
					
					// if left button pressed, find out what we clicked on
					if (Input.GetMouseButtonDown(0))
					{
						carveObject.transform.position = hit.point;
						carveObject.transform.LookAt(hit.point + hit.normal * 1);
						
						// Shift the object back a bit to embed it.
						carveObject.transform.position = carveObject.transform.position - carveObject.transform.forward * depth;
						
						// Get a handle to a new operation to run. We'll let it start running on next frame.
						operation = PoseidonRuntime.GetCarveOperation(new []{ carveObject, baseObject }, new CarveParameters
						{
							FrameTime = 30, // We give each frame 30ms to compute itself
							LogResults = true, // Lets see how long the operation took,
							AssembleTiming = CarveParameters.AssemblePhaseTiming.AllTogetherAtEndOfProcess
						});
						
						// In order to make things pretty, let's remove the current hole by resetting 
						// the base object using ResetToBaseMesh();
						baseObject.ResetToBaseMesh();
					}
				}
			}
		}

		private void HandleScrollingToChangeCarvingMesh()
		{
			if (carvingMeshes.Count == 0) return;
			bool changed = false;
			
			if (Input.mouseScrollDelta.y < 0)
			{
				currentMeshIndex--;
				if (currentMeshIndex < 0)
				{
					currentMeshIndex = carvingMeshes.Count - 1;
				}

				changed = true;
			}
			else if (Input.mouseScrollDelta.y > 0)
			{
				currentMeshIndex++;
				if (currentMeshIndex >= carvingMeshes.Count)
				{
					currentMeshIndex = 0;
				}

				changed = true;
			}

			if (changed)
			{
				carveObject.BaseMesh = carvingMeshes[currentMeshIndex];
				wireframeMeshFilter.sharedMesh = carvingMeshes[currentMeshIndex];
			}
		}
	}
}