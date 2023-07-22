using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckForDanger : Node
{
    private Transform _transform;
    private static int _enemyLayerMask = 1 << 8;
    private int _freezeBombAmmo;

    public CheckForDanger(Transform transform, int freezeAmmo)
    {
        _transform = transform;
        _freezeBombAmmo = freezeAmmo;
    }

    public override NodeState Evaluate()
    {
        if (_freezeBombAmmo > 0)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, Seeker_AI.AttackRange, _enemyLayerMask);

            if (colliders.Length > 0)
            {
                parent.parent.SetData("targetChaser", colliders[0].GetComponent<Chaser_AI>());
                _freezeBombAmmo--;
                _transform.GetComponent<Seeker_AI>().FreezeBombAmmo--;

                state = NodeState.SUCCESS;
                return state;
            }

            if (colliders.Length == 0)
            {
                ClearData("targetChaser");
                state = NodeState.FAILURE;
                return state;
            }
        }
        
        state = NodeState.FAILURE;
        return state;
    }
}
