using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    private PlayerSpriteRenderer activeRenderer;

    public CapsuleCollider2D capsuleCollider { get; private set; }
    public DeathAnimation deathAnimation { get; private set; }

    public FirebaseManager firebaseManager;
    public PointCounter pointCounter;
    public LanguageManager1 languageManager1;
    public GameObject CanvasScore;
    public ScoreManager scoreManager;

    public bool big => bigRenderer.enabled;
    public bool dead => deathAnimation.enabled;
    public bool starpower { get; private set; }

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        deathAnimation = GetComponent<DeathAnimation>();
        activeRenderer = smallRenderer;
    }

    public void Hit()
    {
        if (!dead && !starpower)
        {
            if (big) {
                Shrink();
            } else {
                Death();
            }
        }
    }

    public void Death()
    {
        var levelEs=1;
        var levelEn=1;
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;
        if(languageManager1.Language.Equals("es"))
        {
            if(firebaseManager.userData.scoreEs>=50)
          {
            levelEs=2;
          }
          firebaseManager.WriteUserData(
          firebaseManager.userData.username, // replace with actual username
          firebaseManager.userData.password, // replace with actual password
          firebaseManager.userData.levelEn, // replace with actual level
          levelEs,
          firebaseManager.userData.scoreEn, // replace with actual score in English
          pointCounter.GetCoin()); // replace with actual score in Spanish
        }
         else if(languageManager1.Language.Equals("en"))
        {
            if(firebaseManager.userData.scoreEn>=50)
          {
            levelEn=2;
          }
          firebaseManager.WriteUserData(
          firebaseManager.userData.username, // replace with actual username
          firebaseManager.userData.password, // replace with actual password
          levelEn, // replace with actual level
          firebaseManager.userData.levelEs,
          pointCounter.GetCoin(),
          firebaseManager.userData.scoreEs // replace with actual score in English
          ); 
        }
          // Save the user score to Firebase
        CanvasScore.SetActive(true);
        scoreManager.StartAgain();
        //GameManager.Instance.ResetLevel(3f);
    }

    public void Grow()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

        StartCoroutine(ScaleAnimation());
    }

    public void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        activeRenderer = smallRenderer;

        capsuleCollider.size = new Vector2(1f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);

        StartCoroutine(ScaleAnimation());
    }

    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }

            yield return null;
        }

        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
    }

    public void Starpower()
    {
        StartCoroutine(StarpowerAnimation());
    }

    private IEnumerator StarpowerAnimation()
    {
        starpower = true;

        float elapsed = 0f;
        float duration = 10f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0) {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;
        starpower = false;
    }

}
