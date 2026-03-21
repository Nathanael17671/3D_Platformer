using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic_AS;
    [SerializeField] private AudioSource potionInsertSFX;
    [SerializeField] private AudioSource recipeFailSFX;
    [SerializeField] private AudioSource buttonDownSFX;
    [SerializeField] private AudioSource buttonUpSFX;
    [SerializeField] private AudioSource smallButtonDownSFX;
    [SerializeField] private AudioSource smallButtonUpSFX;

    public void Start()
    {
        backgroundMusic_AS.Play();
    }
    void LateUpdate()
    {
        backgroundMusic_AS.volume = DataManager.Instance.musicVolume / 100f;
    }
    public void PlayPotionInsertSFX()
    {
        potionInsertSFX.Play();
    }
    public void PlayRecipeFailSFX()
    {
        recipeFailSFX.Play();
    }
    public void PlayButtonDownSFX()
    {
        buttonDownSFX.Play();
    }
    public void PlayButtonUpSFX()
    {
        buttonUpSFX.Play();
    }
    public void PlaySmallButtonDownSFX()
    {
        smallButtonDownSFX.Play();
    }
    public void PlaySmallButtonUpSFX()
    {
        smallButtonUpSFX.Play();
    }
}
