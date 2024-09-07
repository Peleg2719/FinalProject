using System.Collections;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Transform flag;
    public Transform poleBottom;
    public Transform castle;
    public float speed = 6f;
    public int nextWorld = 1;
    public int nextStage = 1;
    public FirebaseManager firebaseManager;
    public LanguageManager1 languageManager1;
    public PointCounter pointCounter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MoveTo(flag, poleBottom.position));
            StartCoroutine(LevelCompleteSequence(other.transform));
        
        var levelEs=1;
        var levelEn=1;
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
      }
    }

    private IEnumerator LevelCompleteSequence(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;

        yield return MoveTo(player, poleBottom.position);
        yield return MoveTo(player, player.position + Vector3.right);
        yield return MoveTo(player, player.position + Vector3.right + Vector3.down);
        yield return MoveTo(player, castle.position);

        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        GameManager.Instance.LoadLevel(nextWorld, nextStage);
    }

    private IEnumerator MoveTo(Transform subject, Vector3 position)
    {
        while (Vector3.Distance(subject.position, position) > 0.125f)
        {
            subject.position = Vector3.MoveTowards(subject.position, position, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = position;
    }

}
