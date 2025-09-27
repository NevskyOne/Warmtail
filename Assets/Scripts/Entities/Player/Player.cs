using Data.Player;
using Interfaces;
using TriInspector;
using UnityEngine;
using Zenject;
using R3;
using Systems.DataSystems;

namespace Entities.Player
{
    public class Player : MonoBehaviour
    {
        private GlobalDataSystem _globalDataSystem;
        
        [Inject]
        private void Construct(GlobalDataSystem globalDataSystem)
        {
            _globalDataSystem = globalDataSystem;
            _globalDataSystem.SubscribeTo<SavablePlayerData>(Notify);
            _globalDataSystem.SubscribeTo<RuntimePlayerData>(HP);
        }
        

        public void Damage(int value)
        {
            _globalDataSystem.Edit<RuntimePlayerData>(p =>
            {
                p.HP -= value;
            });
            
            _globalDataSystem.Edit<SavablePlayerData>(p => {
                p.Coins += 50;
            });
        }

        public void Notify()
        {
            Debug.Log($"Coins updated: {_globalDataSystem.Get<SavablePlayerData>().Coins}");
        }
        
        public void HP()
        {
            Debug.Log($"Player HP: {_globalDataSystem.Get<RuntimePlayerData>().HP}");
        }
    }
}