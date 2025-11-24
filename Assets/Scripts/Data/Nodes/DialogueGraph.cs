using System;
using UnityEngine;
using XNode;

namespace Data.Nodes
{
	[Serializable]
	[CreateAssetMenu]
	public class DialogueGraph : NodeGraph
	{
		[SerializeField] private BaseNode _startNode;
		[SerializeField] private int _dialogueId;

		public int DialogueId => _dialogueId;
		public BaseNode StartNode => _startNode;
		public BaseNode Current { get; set; }
	}
}