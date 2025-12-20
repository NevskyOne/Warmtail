// SpriteShapeHoleCutter.cs
// Помести на любой GameObject (можно на сам SpriteShapeController)
using UnityEngine;
using UnityEngine.U2D;
using System.Collections.Generic;
using TriInspector;
using Spline = UnityEngine.U2D.Spline; // не обязателен, но для совместимости

[RequireComponent(typeof(SpriteShapeController))]
[ExecuteInEditMode]
public class SpriteShapeCutter : MonoBehaviour
{
    public SpriteShapeController MainShape;
    public List<SpriteShapeController> HoleShapes = new();
    [Range(0.01f, 5f)] public float UpdateInterval = 0.2f; // как часто обновлять (для производительности)
    public bool UpdateInPlayMode = true;
    public bool UpdateInEditMode = true;

    private float _timer;
    private SpriteShapeController _cachedMain;

    private void Reset()
    {
        MainShape = GetComponent<SpriteShapeController>();
        _cachedMain = MainShape;
    }

    private void OnEnable()
    {
        if (MainShape == null) MainShape = GetComponent<SpriteShapeController>();
        _cachedMain = MainShape;
        RebuildAll();
    }

    private void Update()
    {
        if (!UpdateInPlayMode && Application.isPlaying) return;
        if (!UpdateInEditMode && !Application.isPlaying) return;

        _timer += Time.deltaTime;
        if (_timer >= UpdateInterval)
        {
            _timer = 0f;
            if (HasChanged()) RebuildAll();
        }
    }

    private bool HasChanged()
    {
        if (MainShape != _cachedMain || MainShape == null) return true;

        // проверяем, изменились ли точки хотя бы у одного сплайна
        if (MainShape.spline.GetHashCode() != _cachedMain.spline.GetHashCode()) return true;
        foreach (var hole in HoleShapes)
        {
            if (hole != null && hole.spline.GetHashCode() != hole.spline.GetHashCode())
                return true;
        }
        return false;
    }

    [Button("Rebuild Holes Now")]
    public void RebuildAll()
    {
        if (!MainShape) return;

        _cachedMain = MainShape;
        var spline = MainShape.spline;
        spline.Clear();

        // 1. Добавляем внешний контур
        AddShapeToSpline(MainShape, spline, false);

        // 2. Добавляем все отверстия (с флагом isOpen = false и reverse order)
        foreach (var hole in HoleShapes)
        {
            if (hole == null) continue;
            AddShapeToSpline(hole, spline, true);
        }

        MainShape.RefreshSpriteShape();
    }

    private void AddShapeToSpline(SpriteShapeController source, Spline target, bool isHole)
    {
        var src = source.spline;
        int pointCount = src.GetPointCount();

        // Если это отверстие — делаем его закрытым и в обратном порядке (по часовой против)
        bool reverse = isHole;
        int start = reverse ? pointCount - 1 : 0;
        int end = reverse ? -1 : pointCount;
        int step = reverse ? -1 : 1;

        for (int i = start; i != end; i += step)
        {
            int idx = (i + pointCount) % pointCount;
            var pos = src.GetPosition(idx);
            var lt = src.GetLeftTangent(idx);
            var rt = src.GetRightTangent(idx);
            var mode = src.GetTangentMode(idx);
            var height = src.GetHeight(idx);
            var spriteIndex = src.GetSpriteIndex(idx);

            // Преобразуем локальные координаты hole → мировые → обратно в локальные mainShape
            Vector3 worldPos = source.transform.TransformPoint(pos);
            Vector3 localPos = MainShape.transform.InverseTransformPoint(worldPos);

            Vector3 worldLT = source.transform.TransformPoint(pos + lt) - worldPos;
            Vector3 worldRT = source.transform.TransformPoint(pos + rt) - worldPos;
            Vector3 localLT = MainShape.transform.InverseTransformVector(worldLT);
            Vector3 localRT = MainShape.transform.InverseTransformVector(worldRT);

            int newIndex = target.GetPointCount();
            target.InsertPointAt(newIndex, localPos);
            target.SetLeftTangent(newIndex, localLT);
            target.SetRightTangent(newIndex, localRT);
            target.SetTangentMode(newIndex, mode);
            target.SetHeight(newIndex, height);
            target.SetSpriteIndex(newIndex, spriteIndex);
        }

        // Для отверстий обязательно делаем его закрытым (замыкаем первую и последнюю точку)
        if (isHole && pointCount > 2)
        {
            target.SetTangentMode(target.GetPointCount() - 1, ShapeTangentMode.Continuous);
        }
    }

    // Кнопка в инспекторе
    [Button("Clear All Holes")]
    public void ClearHoles()
    {
        HoleShapes.Clear();
        RebuildAll();
    }
}