using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUParticleTargetSubGroup : MonoBehaviour
{
    public string Label = "";
    private List<GPU3DTarget> _targets = new List<GPU3DTarget>();

    private int _allCount = -1;
    private uint[] _indices = null;

    public void ClearTargets()
    {
        _targets.Clear();
    }

    public void AddTarget(GPU3DTarget target)
    {
        if (_targets.Contains(target))
        {
            return;
        }

        _allCount = -1;
        _targets.Add(target);
    }

    public void AddTargets(List<GPU3DTarget> targets)
    {
        _targets.AddRange(targets);
    }

    public void RemoveTarget(GPU3DTarget target)
    {
        if (!_targets.Contains(target))
        {
            _allCount = -1;
            _targets.Remove(target);
        }
    }

    public int GetCount()
    {
        if (_allCount != -1)
        {
            return _allCount;
        }

        int total = 0;

        foreach (var t in _targets)
        {
            total += t.VertexCount;
        }

        _allCount = total;

        return _allCount;
    }

    private void AllocateArrayIfNeeded()
    {
        if (_indices == null)
        {
            _indices = new uint[GetCount()];
        }
        else
        {
            if (_indices.Length < GetCount())
            {
                System.Array.Resize(ref _indices, GetCount());
            }
        }
    }

    public uint[] GetIndices()
    {
        AllocateArrayIfNeeded();

        int index = 0;

        foreach (var t in _targets)
        {
            if (_indices.Length <= index + t.SubGroupIndices.Length)
            {
                int len = _indices.Length - index;
                System.Array.Copy(t.SubGroupIndices, 0, _indices, index, len);
                break;
            }

            System.Array.Copy(t.SubGroupIndices, 0, _indices, index, t.SubGroupIndices.Length);
            index += t.SubGroupIndices.Length;
        }

        return _indices;
    }
}
