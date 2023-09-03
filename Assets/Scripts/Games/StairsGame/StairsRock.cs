using TMPro;
using UnityEngine;

namespace KendaGame.Games {
  public class StairsRock : MonoBehaviour {
    public int id = 0;
    [SerializeField]
    TextMeshPro title;

    public void InitRock(int id, string title) {
      this.id = id;
      this.title.text = title;
    }

    public int GetId() {
      return id;
    }

    public string GetLetter() {
      return title.text.ToString();
    }

    private void OnMouseDown() {
      StairsLevelManager.Instance.OnRockSelected(this);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
      StairsCharacter stairsCharacter = collision.gameObject.GetComponent<StairsCharacter>();

      if(stairsCharacter != null) {
        stairsCharacter.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        stairsCharacter.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        StairsLevelManager.Instance.OnRockTouched(this);
      }
    }

    public void HideTitle(){
      title.enabled = false;
    }

    public void ShowTitle() {
      title.enabled = true;
    }
  }
}