using Zenject;

namespace Interfaces
{
    public interface IAbility : IFixedTickable
    {
        public bool Enabled { get; set; }
    }
}