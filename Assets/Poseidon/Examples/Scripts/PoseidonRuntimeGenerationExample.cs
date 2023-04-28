using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cinderflame.Poseidon
{
	public class PoseidonRuntimeGenerationExample : MonoBehaviour
	{
		[Header("Generation")]
		public GameObject fastPrefab;
		public GameObject slowPrefab;
		// This example uses hard coded examples instead of randomness
		public Vector3[] positions;

		[Header("UI Settings")]
		// The text that shows the percentage value in the UI
		public UnityEngine.UI.Text percentageText;
		// The UI image we'll be adjusting to give it a semblance of a progress bar.
		public UnityEngine.UI.Image progressBar;
		// We turn off the buttons in the example while it is carving
		public UnityEngine.CanvasGroup canvasGroup;
		
		// How many milliseconds should the Poseidon Operation have 
		// in its budget to be able to do the calculations?
		public float millisecondsPerFrame = 15;


		private List<Poseidon> poseidons = new List<Poseidon>();

		/// <summary>
		/// This method is invoked and called by the button in the scene
		/// to actually generate the carvers "dynamically"
		/// </summary>
		/// <param name="fast">Should we use a fast prefab or a slower one?</param>
		public void GenerateRuntime(bool fast)
		{
			foreach(var poseidon in poseidons)
			{
				Destroy(poseidon.gameObject);
			}

			poseidons.Clear();

			GameObject poseidonPrefab = fast ? fastPrefab : slowPrefab;
			int i = 0;
			foreach(var pos in positions)
			{
				i++;
				var obj = GameObject.Instantiate(poseidonPrefab, pos, Quaternion.identity);
				obj.gameObject.name = $"Object {i}";

				var poseidon = obj.GetComponent<Poseidon>();
				poseidons.Add(poseidon);

				// For the sake of the example let's have them be a bit fatter to intersect each other
				// so that we don't have to figure out exactly where to position them. 
				poseidon.transform.localScale *= 1.5f;
			}
		}

		public void CarveAllKnownPoseidonsTogetherOnSingleFrame()
		{
			// Carve all of the Poseidons together on one frame.
			// This function basically says: 
			// "I want to kick off a carve and don't care how long it takes."
			PoseidonRuntime.CarveEverything(poseidons);
		}

		public void CarveAllKnownPoseidonsTogether()
		{
			var operation = PoseidonRuntime.GetCarveOperation(poseidons, new CarveParameters
			{
				FrameTime = millisecondsPerFrame, // We give each frame 30ms to compute itself
				LogResults = true, // Lets see how long the operation took
				AssembleTiming = CarveParameters.AssemblePhaseTiming.AllTogetherAtEndOfProcess
			});

			StartCoroutine(RunCarveOperation(operation));
			canvasGroup.interactable = false;
		}

		// This is just an example of one way to possibly run
		// this in the background. Here, we're using a coroutine
		// to trigger a frame-update, but in future updates this 
		// will likely become something Poseidon runtime handles by itself.
		private IEnumerator RunCarveOperation(PoseidonOperation operation)
		{
			while(!operation.Finished)
			{
				operation.RunFrame();
				progressBar.fillAmount = operation.Progress;
				percentageText.text = $"{operation.Progress * 100:#,0.00}%";
				yield return null;
			}

			// When the operation is done, let's do some post operation logic:
			canvasGroup.interactable = true;
		}
	}
}