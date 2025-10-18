using UnityEngine;
using XNode;

namespace Data.Nodes
{
	[CreateAssetMenu]
	public class DialogueGraph : NodeGraph
	{
		[SerializeField] private BaseNode _startNode;
		public BaseNode StartNode => _startNode;
		public BaseNode Current { get; set; }
	}
}