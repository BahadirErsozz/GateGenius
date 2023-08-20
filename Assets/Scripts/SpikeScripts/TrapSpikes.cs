using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapSpikes : MonoBehaviour
{
    public List<Player> ListCharacters = new List<Player>();
    public List<Spike>  ListSpikes = new List<Spike>();
    Coroutine SpikeTriggerRoutine;
    bool SpikesReloaded;


    private void Start()
    {
        SpikeTriggerRoutine = null;
        SpikesReloaded = true;
        ListCharacters.Clear();
        ListSpikes.Clear();

        Spike[] arr = this.gameObject.GetComponentsInChildren<Spike>();
        foreach(Spike s in arr)
        {
            ListSpikes.Add(s);
        }
    }

    private void Update()
    {
        if(ListCharacters.Count != 0)
        {
            foreach(Player player in ListCharacters)
            {
                if(player.currentHealth != 0)
                {
                    if(SpikeTriggerRoutine == null && SpikesReloaded)
                    {
                        SpikeTriggerRoutine = StartCoroutine(_TriggerSpikes());
                    }
                }
            }
        }
    }

    IEnumerator _TriggerSpikes()
    {

        SpikesReloaded = false;
        yield return new WaitForSeconds(0.3f);

        foreach (Spike s in ListSpikes)
        {
            s.Shoot();

        }
        
        yield return new WaitForSeconds(1f);

        foreach (Spike s in ListSpikes)
        {
            s.Retract();
        }

        yield return new WaitForSeconds(1f);

        SpikeTriggerRoutine = null;
        SpikesReloaded = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player control= other.gameObject.transform.root.gameObject.GetComponent<Player>();

        if(control != null)
        {
            if (!ListCharacters.Contains(control))
            {
                ListCharacters.Add(control);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Player control = other.gameObject.transform.root.gameObject.GetComponent<Player>();

        if (control != null)
        {
            if (ListCharacters.Contains(control))
            {
                ListCharacters.Remove(control);
            }
        }
    }
}
