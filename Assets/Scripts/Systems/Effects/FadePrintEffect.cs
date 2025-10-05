using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Interfaces;
using TMPro;
using UnityEngine;

namespace Systems.Effects
{
    public class FadePrintEffect : IPrintEffect
    {
        [SerializeField] private float _speed = 1.5f;
        [SerializeField] private float _gradientWidth = 0.2f;
        [SerializeField] private float _fastMultiplier = 4f;
        private bool _speedUp;

        public async Task<bool> StartEffect(TMP_Text text)
        {
            _speedUp = false;
            var info = text.textInfo;
            int totalChars = info.characterCount;
            if (totalChars == 0)
            {
                text.alpha = 1;
                return true;
            }
            int meshCount = info.meshInfo.Length;
            Color32[][] meshColors = new Color32[meshCount][];
            for (int m = 0; m < meshCount; m++)
            {
                meshColors[m] = info.meshInfo[m].colors32;
                for (int k = 0; k < meshColors[m].Length; k++) meshColors[m][k].a = 0;
                info.meshInfo[m].mesh.colors32 = meshColors[m];
                text.UpdateGeometry(info.meshInfo[m].mesh, m);
            }
            text.alpha = 1;
            text.ForceMeshUpdate();

            float head = -_gradientWidth;
            float target = 1f + _gradientWidth;
            while (head < target)
            {
                float dt = Time.deltaTime;
                head += dt * _speed * (_speedUp ? _fastMultiplier : 1f);
                text.ForceMeshUpdate();
                info = text.textInfo;
                totalChars = info.characterCount;
                for (int m = 0; m < meshCount; m++) meshColors[m] = info.meshInfo[m].colors32;
                for (int i = 0; i < totalChars; i++)
                {
                    var ch = info.characterInfo[i];
                    if (!ch.isVisible) continue;
                    int mat = ch.materialReferenceIndex;
                    int v = ch.vertexIndex;
                    float pos = totalChars == 1 ? 0f : (float)i / (totalChars - 1);
                    float raw = (head - pos) / _gradientWidth;
                    float a = Mathf.Clamp01(raw);
                    a = Mathf.SmoothStep(0f, 1f, a);
                    byte ab = (byte)Mathf.RoundToInt(a * 255f);
                    var cols = meshColors[mat];
                    cols[v + 0].a = ab;
                    cols[v + 1].a = ab;
                    cols[v + 2].a = ab;
                    cols[v + 3].a = ab;
                }

                for (int m = 0; m < meshCount; m++)
                {
                    info.meshInfo[m].mesh.colors32 = meshColors[m];
                    text.UpdateGeometry(info.meshInfo[m].mesh, m);
                }

                await UniTask.Yield();
            }

            for (int m = 0; m < meshCount; m++)
            {
                var cols = info.meshInfo[m].colors32;
                for (int k = 0; k < cols.Length; k++) cols[k].a = 255;
                info.meshInfo[m].mesh.colors32 = cols;
                text.UpdateGeometry(info.meshInfo[m].mesh, m);
            }
            return true;
        }

        public void SpeedUpEffect()
        {
            _speedUp = true;
        }
    }
}