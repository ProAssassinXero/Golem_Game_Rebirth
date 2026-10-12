using System.Collections;
using UnityEngine;

public class NPC1_Respawn : MonoBehaviour
{
    public GameObject[] npc;
    public bool[] isRespawning;

    void Start()
    {
        npc = GameObject.FindGameObjectsWithTag("NPC1");
        isRespawning = new bool[npc.Length];
    }

    void Update()
    {
        for (int i = 0; i < npc.Length; i++)
        {
            if (npc[i] != null && !npc[i].activeInHierarchy && !isRespawning[i])
            {
                StartCoroutine(RespawnTime(i));
            }
        }
    }
    IEnumerator RespawnTime(int index)
    {
        isRespawning[index] = true;
        yield return new WaitForSeconds(5f);

        npc[index].GetComponent<NPC_1>().ResetNPC();
        npc[index].SetActive(true);

        isRespawning[index] = false;
    }
}