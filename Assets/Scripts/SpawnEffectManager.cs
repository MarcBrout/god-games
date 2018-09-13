using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffectManager : MonoBehaviour {

    public List<GameObject> pool;
    public Transform player;

	void Start () {

        StartCoroutine(SpawnEffect(0));
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
