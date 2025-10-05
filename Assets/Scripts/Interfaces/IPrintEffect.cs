using System.Threading.Tasks;
using TMPro;

namespace Interfaces
{
    public interface IPrintEffect
    {
        public Task<bool> StartEffect(TMP_Text text);
        public void SpeedUpEffect();
    }
}