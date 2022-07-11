using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGPUParticleController : MonoBehaviour
{
    [SerializeField] private GPUParticleRootSystem _particleSystem = null;
    [SerializeField] private GPUParticleTargetGroups[] _groups = null;
    [SerializeField] private float _radius = 3f;
    [SerializeField] bool isAnimation = false;
    bool isAnimationAction = false;

    

    private GPUParticleTargetGroups CurrentGroup => _groups[_index];

    private int _index = 0;

    private InitData[] _initData = null;

    
    private IEnumerator Start()
    {
        Initialize();

        yield return new WaitForSeconds(0.5f);

        foreach (var g in _groups)
        {
            g.Initialize(_particleSystem);
        }

        _particleSystem.SetGroup(_groups[0]);
        _particleSystem.Play();

        isAnimationAction = false;
        
        if (isAnimation)
        {
            InvokeRepeating("Animation", 0.2f, 0.2f);
        }
    }

    private void Update()
    {
        //Next();
        //KeepPosition();
        
        if (Input.GetKey(KeyCode.U))
        {
            isAnimationAction = false;
            UpdatePosition();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            isAnimationAction = false;
            Next();
        }
        if (Input.GetKey(KeyCode.K))
        {
            isAnimationAction = false;
            KeepPosition();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            isAnimationAction = true;
            Gravity();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            isAnimationAction = false;
            Explosion();
        }
        if (Input.GetKey(KeyCode.A))
        {
            isAnimationAction = false;
            UpdatePosition();
        }
    }
    private void Initialize()
    {
        _initData = new InitData[_particleSystem.ParticleCount];

        for (int i = 0; i < _initData.Length; i++)
        {
            _initData[i] = new InitData();
        }
    }

    private void Next()
    {
        _index = (_index + 1) % _groups.Length;
        _particleSystem.ChangeUpdateMethod(UpdateMethodType.Target);
        _particleSystem.SetGroup(CurrentGroup);
    }
    private void KeepPosition()
    {
        _particleSystem.ChangeUpdateMethodWithClear(UpdateMethodType.KeepPosition);
        _particleSystem.SetGroup(CurrentGroup);
    }

    private void Gravity()
    {
        _particleSystem.SetOrigin(Vector3.one);
        _particleSystem.ChangeUpdateMethodWithClear(UpdateMethodType.Gravity);
    }
    private void UpdatePosition()
    {
        _particleSystem.ChangeUpdateMethodWithClear(UpdateMethodType.UpdatePosition);
        _particleSystem.SetGroup(CurrentGroup);
    }
    private void Animation()
    {
        if (!isAnimationAction)
        {
            _index = (_index + 1) % _groups.Length;
            _particleSystem.ChangeUpdateMethodWithClear(UpdateMethodType.UpdatePosition);
            _particleSystem.SetGroup(CurrentGroup);
        }
        
    }
    private void Explosion()
    {
        for (int i = 0; i < _initData.Length; i++)
        {
            _initData[i].isActive = 1;
            _initData[i].scale = 2.0f;
            _initData[i].horizontal = Random.onUnitSphere;
            Vector3 v = Vector3.forward;
            float w = Random.Range(1f, 3f);

            float d = Vector3.Dot(v, _initData[i].horizontal);

            if (d < 0)
            {
                v = (v - _initData[i].horizontal);
            }
            else
            {
                v = (v - _initData[i].horizontal);
            }

            _initData[i].velocity = new Vector4(v.x, v.y, v.z, w);
        }

        _particleSystem.SetOrigin(Vector3.one);
        _particleSystem.UpdateInitData(_initData);
        _particleSystem.ChangeUpdateMethodWithClear(UpdateMethodType.Explode);
    }
    private void AnimationSkinnedMesh()
    {
        _particleSystem.ChangeUpdateMethodWithClear(UpdateMethodType.UpdateAnimation);

    }
}

