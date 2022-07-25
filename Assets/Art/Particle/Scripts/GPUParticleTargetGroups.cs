using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct IndexData
{
    public int ID;
    public uint[] Indices;
}

public class GPUParticleTargetGroups : MonoBehaviour, GPUParticleTargetGroup
{
    [SerializeField] private bool _autoDetection = true;
    [SerializeField] private GameObject[] _targets = null;
    [SerializeField] private bool _avoidAllocateIndices = false;

    private GPUParticleTarget[] _particleTargets = null;
    private Vector3[] _allVertices = null;
    private InitData[] _allInitData = null;
    private Vector2[] _allUV = null;
    private Matrix4x4[] _matrixData = null;
    private uint[] _indices = null;

    private Texture2DArray _textureArray = null;

    private int _allCount = -1;

    public Texture2DArray TextureArray => _textureArray;
    public Matrix4x4[] MatrixData => _matrixData;
    public InitData[] AllInitData => _allInitData;
    public uint[] Indices => _indices;

    private void OnDestroy()
    {
        if (_textureArray != null)
        {
            Destroy(_textureArray);
        }
        
    }
    public void Initialize(GPUParticleRootSystem system)
    {
        if (_autoDetection)
        {
            _particleTargets = GetComponentsInChildren<GPUParticleTarget>(true);
        }
        else
        {
            _particleTargets = _targets.Select(t => t.GetComponent<GPUParticleTarget>()).ToArray();
        }

        foreach (var t in _particleTargets)
        {
            t.Initialize();
        }

        Debug.Log(_particleTargets.Length);

        _allInitData = new InitData[GetCount()];

        if (!_avoidAllocateIndices)
        {
            AllocateIndices(Mathf.Min(system.ParticleCount, _allInitData.Length));
        }

        CreateTextureArray();

        CollectAllData();

        CreateAllInitData();
    }

    private void CreateTextureArray()
    {
        Debug.Log("Create all textures as array. : " + _particleTargets.Length);

        int count = _particleTargets.Length;
        int width = _particleTargets[0].Texture.width;
        int height = _particleTargets[0].Texture.height;
        _textureArray = new Texture2DArray(width, height, count, TextureFormat.RGBA32, false, true);
        _textureArray.filterMode = FilterMode.Bilinear;
        _textureArray.wrapMode = TextureWrapMode.Repeat;

        for (int i = 0; i < _particleTargets.Length; i++)
        {
            _textureArray.SetPixels(_particleTargets[i].Texture.GetPixels(0), i, 0);
        }

        _textureArray.Apply();
    }

    private void CollectAllData()
    {
       

        int count = GetCount();

        _allVertices = new Vector3[count];
        _allUV = new Vector2[count];
        _matrixData = new Matrix4x4[_particleTargets.Length];

        int idx = 0;

        for (int i = 0; i < _particleTargets.Length; i++)
        {
            GPUParticleTarget t = _particleTargets[i];

            System.Array.Copy(t.Vertices, 0, _allVertices, idx, t.Vertices.Length);
            System.Array.Copy(t.UV, 0, _allUV, idx, t.UV.Length);

            _matrixData[i] = t.WorldMatrix;

            t.SetStartIndex(idx);

            idx += t.Vertices.Length;
        }

    }

    private void AllocateIndices(int count)
    {
        _indices = new uint[count];

        for (int i = 0; i < _indices.Length; i++)
        {
            _indices[i] = (uint)i;
        }
    }
    private int GetCount()
    {
        if (_allCount != -1)
        {
            return _allCount;
        }

        int total = 0;

        foreach (var t in _particleTargets)
        {
            total += t.VertexCount;
        }

        _allCount = total;

        return _allCount;
    }

    
    private void CreateAllInitData()
    {
        

        int idx = 0;
        int total = _particleTargets[0].VertexCount;

        for (int i = 0; i < _allInitData.Length; i++)
        {
            if (i >= total)
            {
                idx++;
                total += _particleTargets[idx].VertexCount;
            }

            _allInitData[i].isActive = 1;
            _allInitData[i].targetPosition = _allVertices[i];
            _allInitData[i].uv = _allUV[i];
            _allInitData[i].targetId = idx;
            _allInitData[i].scale = Random.Range(_particleTargets[idx].MinScale, _particleTargets[idx].MaxScale);
            _allInitData[i].horizontal = Random.onUnitSphere;
            Vector3 v = (_allVertices[i] + Random.insideUnitSphere).normalized;
            float w = Random.Range(1f, 3f);
            _allInitData[i].velocity = new Vector4(v.x, v.y, v.z, w);
        }
    }

    public void UpdateMatrices()
    {
        for (int i = 0; i < _particleTargets.Length; i++)
        {
            _matrixData[i] = _particleTargets[i].WorldMatrix;
        }
    }
}
