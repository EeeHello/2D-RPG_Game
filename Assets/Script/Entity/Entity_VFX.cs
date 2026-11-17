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

    [Header("Element Colors")]
    [SerializeField] private Color chillVfx = Color.cyan;
    private Color orignalHitVfxColor;
 
    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        orginalMaterial = sr.material;
        orignalHitVfxColor = onHitVfxColor;
    }

    public void PlayOnStatusVfx(float duration, ElementType element)
    {
        if (element == ElementType.Ice)
        {
            StartCoroutine(PlayStatusVfxCo(duration, chillVfx));
        }
    }

    private IEnumerator PlayStatusVfxCo(float duration, Color effectColor) 
    {
        float tickInterval = 0.25f;
        float timeHasPasssed = 0;

        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * 0.8f;

        bool toggle = false;

        while (timeHasPasssed < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle; 

            yield return new WaitForSeconds(tickInterval);

            timeHasPasssed = timeHasPasssed + tickInterval;
        }

        sr.color = Color.white;
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

    public void UpdateOnHitColor(ElementType element)
    {
        if (element == ElementType.Ice)
        {
            onHitVfxColor = chillVfx;
        }

        if (element == ElementType.None)
        {
            onHitVfxColor = orignalHitVfxColor;
        }
    }

    public void PlayOnDamageVfx()
    {
        if (onDamageVfxCo != null)
            StopCoroutine(onDamageVfxCo);

        onDamageVfxCo = StartCoroutine(OnDamageVfxCo());
    }

    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onDamageMaterial;

        yield return new WaitForSeconds(onDamageVfxDuration);
        sr.material = orginalMaterial;
    }

    // sr.material = onDmgMat
    // wait for onDmgVfxDur
    // sr.mat = ogMat
}
