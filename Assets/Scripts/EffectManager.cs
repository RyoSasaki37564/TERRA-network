using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Card,
    Role
}

public class EffectManager : MonoBehaviour
{
    private static InitData[] _initDatas;

    private static EffectManager _instance = null;
    /// <summary>
    /// エフェクトマネージャーインスタンス
    /// </summary>
    public EffectManager _EffectInstance => _instance;
    private void Start()
    {
        if (_instance == null)
        {
            _instance = new EffectManager();
        }
    }
    /// <summary>
    /// 形を変える
    /// </summary>
    /// <param name="system">使うオブジェクトのルートシステム</param>
    /// <param name="target">変更される対象のオブジェクト</param>
    public static void UpdateParticle(GPUParticleRootSystem system, GPUParticleTargetGroup target)
    {
        system.ChangeUpdateMethod(UpdateMethodType.Target);
        system.SetGroup(target);
    }
    /// <summary>
    /// 演出後に差し替えなしに位置を更新したい場合この関数を呼ぶ。
    /// </summary>
    /// <param name="system">使うオブジェクトのルートシステム</param>
    /// <param name="target">変更される対象のオブジェクト</param>
    public static void KeepParticlelocalPosition(GPUParticleRootSystem system, GPUParticleTargetGroup target)
    {
        system.ChangeUpdateMethodWithClear(UpdateMethodType.KeepPosition);
        system.SetGroup(target);
    }
    /// <summary>
    /// トレイルしながら移動させたい場合にこれをUpdateで呼ぶ。
    /// </summary>
    /// <param name="system">使うオブジェクトのルートシステム</param>
    /// <param name="target">変更される対象のオブジェクト</param>
    public static void TrailParticlelocalPosition(GPUParticleRootSystem system, GPUParticleTargetGroup target)
    {
        system.ChangeUpdateMethodWithClear(UpdateMethodType.UpdatePosition);
        system.SetGroup(target);
    }
    /// <summary>
    /// 色保持しながらオブジェクトをはじけさせたいとき
    /// </summary>
    /// <param name="system">使うオブジェクトのルートシステム</param>
    /// <param name="target">変更される対象のオブジェクト</param>
    public static void Gravity(GPUParticleRootSystem system, GPUParticleTargetGroup target)
    {
        system.ChangeUpdateMethodWithClear(UpdateMethodType.Gravity);
        system.SetGroup(target);
    }
    /// <summary>
    /// 色保持しながらオブジェクトをはじけさせたいとき
    /// </summary>
    /// <param name="system">使うオブジェクトのルートシステム</param>
    /// <param name="target">変更される対象のオブジェクト</param>
    public static void Explode(GPUParticleRootSystem system, GPUParticleTargetGroup target)
    {
        ParticleInitialize(system);
        for (int i = 0; i < _initDatas.Length; i++)
        {
            _initDatas[i].isActive = 1;
            _initDatas[i].scale = 2.0f;
            _initDatas[i].horizontal = Random.onUnitSphere;
            Vector3 v = Vector3.forward;
            float w = Random.Range(1f, 3f);

            float d = Vector3.Dot(v, _initDatas[i].horizontal);

            if (d < 0)
            {
                v = (v - _initDatas[i].horizontal);
            }
            else
            {
                v = (v - _initDatas[i].horizontal);
            }

            _initDatas[i].velocity = new Vector4(v.x, v.y, v.z, w);
        }
        system.SetOrigin(Vector3.one);
        system.UpdateInitData(_initDatas);
        system.ChangeUpdateMethodWithClear(UpdateMethodType.Explode);
    }
    private static void ParticleInitialize(GPUParticleRootSystem system)
    {
        _initDatas = new InitData[system.ParticleCount];
        for (int i = 0; i < _initDatas.Length; i++)
        {
            _initDatas[i] = new InitData();
        }
    }
}
