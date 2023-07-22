using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureManager : MonoBehaviour
{
    [SerializeField] private GameObject _seeker;
    [SerializeField] private GameObject _player;

    private GameObject[] _tokens;

    private void Start()
    {
        _tokens = GameObject.FindGameObjectsWithTag("Token");
    }

    public bool CaptureEnemy(Transform target)
    {
        if (target == _seeker.transform || target == _player.transform)
        {
            if (target == _seeker.transform)
            {
                Destroy(_seeker);
                Debug.Log("Seeker successfully destroyed");
            }

            return true;
        }
        else return false;
    }

    public bool CaptureToken(GameObject target)
    {
        Debug.Log("Attempting to destroy token");
        if (target != null)
        {
            Destroy(target);
            Seeker_AI.tokenCounter++;
            Debug.Log("Token successfully destroyed");
            return true;
        }
        else return false;
    }
}
