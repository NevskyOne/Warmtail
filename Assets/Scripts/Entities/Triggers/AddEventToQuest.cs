using Data;
using Entities.NPC;
using UnityEngine;
using UnityEngine.Events;

namespace Entities.Triggers
{
    public class AddEventToQuest : MonoBehaviour
    {
        [SerializeField] private QuestSystem _system;
        [SerializeField] private QuestData _quest;
        [SerializeField] private UnityEvent _event;

        public void Add() => _system.AddEvent(_quest, _event);
    }
}