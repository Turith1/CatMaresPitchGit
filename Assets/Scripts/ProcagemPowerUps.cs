using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcagemPowerUps : MonoBehaviour
{
    [SerializeField] private GameObject[] _powerUps;
    [SerializeField] private Transform[] _powerUpsTransform;
    // Start is called before the first frame update
    [SerializeField] private List<Transform> _usePositions;
    [SerializeField] private ProcagemPowerUps _procScript;
    private int _randomPos;
    private void Start()
    {
        for (int i = 0; i < _powerUps.Length; i++)
        {
            int randomPosition = Random.Range(0, _powerUpsTransform.Length);
            if (!_usePositions.Contains(_powerUpsTransform[randomPosition]))
            {
                GameObject pickUp = Instantiate(_powerUps[i], _powerUpsTransform[randomPosition].position, Quaternion.identity);
                GunPickup gun = pickUp.GetComponent<GunPickup>();
                if (gun != null)
                {
                    gun._proc = _procScript;
                }
                else if (gun == null)
                {
                    pickUp.GetComponent<PickupItem>()._proc = _procScript;
                }
                _usePositions.Add(_powerUpsTransform[randomPosition]);
            }
            else
                i--;
        }
    }

    public void RespawnItem(Transform pos, GameObject powerup)
    {
        powerup.SetActive(false);
        StartCoroutine(RespawnDelay(pos, powerup));
    }

    private IEnumerator RespawnDelay(Transform pos, GameObject powerup)
    {
        yield return new WaitForSeconds(2);
        for(int i = 0; i < _usePositions.Count; i++)
        {
            if(_usePositions[i].position == pos.position)
            {
                _usePositions.Remove(_usePositions[i]);
                break;
            }
        }

        for (int i = 0; i < _powerUpsTransform.Length; i++)
        {
            int randomPosition = Random.Range(0, _powerUpsTransform.Length);
            if (!_usePositions.Contains(_powerUpsTransform[randomPosition]))
            {
                pos.position = _powerUpsTransform[randomPosition].position;
                _usePositions.Add(_powerUpsTransform[randomPosition]);
                break;
            }
            else
                i--;
        }

        powerup.SetActive(true);
    }
}
