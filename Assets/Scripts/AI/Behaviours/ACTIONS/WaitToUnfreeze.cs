using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class WaitToUnfreeze : Node
{
    private Transform _transform;
    private Animator _animator;
    private Material _currentMat;
    private Material _freezeMat;
    private Material _normalMat;
    private float _freezeTimer = 0;
    private float _freezeDuration = 30f;

    public WaitToUnfreeze(Transform transform, Material freezeMat, Material normalMat)
    {
        _transform = transform;
        _freezeMat = freezeMat;
        _normalMat = normalMat;
        _animator = transform.GetComponent<Animator>();
        _currentMat = transform.GetComponentInChildren<SkinnedMeshRenderer>().material;
    }
    public override NodeState Evaluate()
    {
        if (_currentMat != _freezeMat)
        {
            _transform.GetComponentInChildren<SkinnedMeshRenderer>().material = _freezeMat;
            _currentMat = _freezeMat;

            //Stop animations
            _animator.SetBool("Walking", false);
            _animator.SetBool("Running", false);
        }

        _freezeTimer += Time.deltaTime;
        if (_freezeTimer > _freezeDuration)
        {
            _transform.GetComponent<Chaser_AI>().IsFrozen = false;
            _transform.GetComponentInChildren<SkinnedMeshRenderer>().material = _normalMat;
            _currentMat = _normalMat;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
