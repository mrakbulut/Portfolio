using UnityEngine;
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _countToSpawn = 100;

    private void Start()
    {
        for (int i = 0; i < _countToSpawn; i++)
        {
            Instantiate(_prefab, transform);
        }
    }
}
