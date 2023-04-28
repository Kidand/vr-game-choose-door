using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cinderflame.Poseidon
{

	public class PoseidonPreferences : MonoBehaviour
	{
#if UNITY_2018_3_OR_NEWER
		[SettingsProvider]
		static SettingsProvider Settings()
		{
			return new SettingsProvider("Preferences/Poseidon", SettingsScope.User)
			{
				label = "Poseidon",
				guiHandler = (_) =>
				{
					var orig = EditorGUIUtility.labelWidth;
						EditorGUIUtility.labelWidth = 275;

						PoseidonSettings.CarverSettings.Draw();
						PoseidonSettings.Functionality.Draw();
						PoseidonSettings.LookAndFeel.Draw();

						using (new EditorGUILayout.HorizontalScope())
						{
							if (GUILayout.Button(new GUIContent("Reset to Defaults")))
							{
								PoseidonSettings.ResetAll();
							}	
							if (GUILayout.Button(new GUIContent("Mark entire Active Scene as Dirty")))
							{
								if (EditorUtility.DisplayDialog("Mark entire Active Scene as Dirty",
									"Clicking 'Rebuild Everything' will mark every single Poseidon object as dirty (in the current active scene) and might take a very long time to run if your scene is very complicated. Are you sure you want to do this?",
									"Rebuild Everything", "Cancel"))
								{
									var allPoseidons = GameObject.FindObjectsOfType<Poseidon>();
									var activeScene = SceneManager.GetActiveScene();
									foreach (var poseidon in allPoseidons)
									{
										if (poseidon.gameObject.scene == activeScene)
										{
											poseidon.Dirty = true;
										}
									}
								}
							}
						}
						
						

						EditorGUIUtility.labelWidth = orig;
				},
				keywords = PoseidonSettings.GetSearchKeywords()
			};
		}
#endif
	}
}