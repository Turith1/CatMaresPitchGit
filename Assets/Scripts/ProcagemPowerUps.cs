using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcagemPowerUps : MonoBehaviour
{
    [SerializeField] private GameObject[] _powerUps;
    [SerializeField] private Transform[] _powerUpsTransform;
    // Start is called before the first frame update
    [SerializeField] private List<Transform> _usePositions;
    private void Start()
    {
        for (int i = 0; i <= 4; i++)
        {
            int randomPosition = Random.Range(0, _powerUpsTransform.Length);
            while (_usePositions.Contains(_powerUpsTransform[randomPosition]))
            {
                randomPosition = Random.Range(0, _powerUpsTransform.Length);
            }
            GameObject powerUps = Instantiate(_powerUps[i], _powerUpsTransform[randomPosition].position, Quaternion.identity);
 
        }
    }

    private GameObject Instantiate(GameObject gameObject, Transform[] randomPosition, Quaternion identity)
    {
        throw new System.NotImplementedException();
    }


}
