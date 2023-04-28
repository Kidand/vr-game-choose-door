using UnityEditor;

namespace Cinderflame.Poseidon.UI
{
	public abstract class PoseidonInspectorSection
	{
		public PoseidonInspector inspector;

		public void Draw(PoseidonInspector inspector, bool showTitle = false)
		{
			this.inspector = inspector;
			DrawSection(inspector.serializedObject, showTitle);
		}


		protected abstract void DrawSection(SerializedObject serializedObject, bool showTitle = false);
	}
}