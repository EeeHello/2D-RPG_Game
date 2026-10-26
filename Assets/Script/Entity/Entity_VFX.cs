using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity entity;

    [Header("On Damaged VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = 0.2f;
    private Material orginalMaterial;
    private Coroutine onDamageVfxCo;

    [Header("On Hit VFX")]
    [SerializeField] private Color onHitVfxColor = Color.white;
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private GameObject critVfx;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        orginalMaterial = sr.material;
    }

    public void CreateOnHItVFX(Transform target, bool isCrit)
    {
        GameObject hitPrefabs = isCrit ? critVfx : hitVfx;
        GameObject vfx = Instantiate(hitPrefabs, target.position, Quaternion.identity);

        if (!isCrit)
        {
            vfx.GetComponentInChildren<SpriteRenderer>().color = onHitVfxColor;
        }

        if (entity.facingDir == -1 && isCrit)
        {
            vfx.transform.Rotate(0, 180, 0);
        }

    }

    public void PlayOnDamageVfx()
    {
        if (onDamageVfxCo != null)
            StopCoroutine(onDamageVfxCo);

        onDamageVfxCo = StartCoroutine(OnDamageVfxCoroutine());
    }

    private IEnumerator OnDamageVfxCoroutine()
    {
        sr.material = onDamageMaterial;

        yield return new WaitForSeconds(onDamageVfxDuration);
        sr.material = orginalMaterial;
    }

    // sr.material = onDmgMat
    // wait for onDmgVfxDur
    // sr.mat = ogMat
}
