using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Flamethrower : MonoBehaviour
{
    public UnityEvent attackEvent;
    public GenericWeaponManager gwp;

    public float reloadTime;
    public float realReloadTime;

    bool canAttack = true;
    ParticleSystem ps;

    IEnumerator attackEnum;

    private void Awake()
    {
        gwp.attackEvent = attackEvent;
        gwp.reloadTime = reloadTime;

        ps = GetComponent<ParticleSystem>();
        var emissionModule = ps.emission;
        emissionModule.enabled = false;
    }

    IEnumerator AttackEnum()
    {
        canAttack = false;

        while (true)
        {
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Attack()
    {
        if (canAttack)
        {
            var emissionModule = ps.emission;
            emissionModule.enabled = true;
        }
    }

}
