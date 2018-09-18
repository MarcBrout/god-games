using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffectManager : MonoBehaviour
{

    public List<GameObject> pool;
    public Transform player;

    bool check = false;
    void Start()
    {

        //StartCoroutine(SpawnEffect(0));
    }

    private void Update()
    {
        if (!pool[0].activeSelf && !check)
        {
            check = true;
            StartCoroutine(ActivateIn(pool[0], 0.5f));
        }
    }

    public IEnumerator ActivateIn(GameObject go, float s = 1f)
    {
        yield return new WaitForSeconds(s);
        go.SetActive(true);
        check = false;
    }

    public IEnumerator SpawnEffect(int nb)
    {
        while (true)
        {
            Instantiate(pool[nb], new Vector3(player.position.x, pool[nb].transform.position.y, player.position.z), pool[nb].transform.rotation);
            yield return new WaitForSeconds(1f);
        }
    }
}
