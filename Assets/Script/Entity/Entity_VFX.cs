using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("On Damaged VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = 0.2f;
    private Material orginalMaterial;
    private Coroutine onDamageVfxCo;

    [Header("On Hit VFX")]
    [SerializeField] private Color onHitVfxColor = Color.white;
    [SerializeField] private GameObject hitVfx;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        orginalMaterial = sr.material;
    }

    public void CreateOnHItVFX(Transform target)
    {
       GameObject vfx = Instantiate(hitVfx, target.position, Quaternion.identity);
       vfx.GetComponentInChildren<SpriteRenderer>().color = onHitVfxColor;
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
