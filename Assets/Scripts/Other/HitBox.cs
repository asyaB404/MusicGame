using UnityEngine;


public class HitBox : MonoBehaviour
{
    public bool isTouching;
    [SerializeField] private GameObject touching;
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private Color perfect;
    [SerializeField] private Color great;
    [SerializeField] private int colorState = 0;

    private void Update()
    {
        if (touching && isTouching != touching.activeSelf)
            touching.SetActive(isTouching);
    }

    private void LateUpdate()
    {
        isTouching = false;
    }

    public void PlayEffect(int color = 0)
    {
        if (color != colorState)
        {
            UpdateParticleColor(color);
            colorState = color;
        }

        effect.Play();
    }

    private void UpdateParticleColor(int color)
    {
        var effectMain = effect.main;
        effectMain.startColor = color switch
        {
            0 => new ParticleSystem.MinMaxGradient(perfect),
            1 => new ParticleSystem.MinMaxGradient(great),
            _ => effectMain.startColor
        };
    }

    #region Debug

    [Space(20)]
    [ContextMenuItem(nameof(UpdateParticleColorWithoutParams), nameof(UpdateParticleColorWithoutParams))]
    [SerializeField]
    private int debugI;

    private void UpdateParticleColorWithoutParams()
    {
        UpdateParticleColor(debugI);
    }

    #endregion
}