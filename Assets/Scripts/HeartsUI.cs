using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    public SpriteRenderer[] heartImages;     
    public Sprite fullHeart;
    public Sprite emptyHeart;

    
    public void UpdateHearts(int current, int max)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            bool filled = i < current;
            heartImages[i].sprite = filled ? fullHeart : emptyHeart;
            heartImages[i].enabled = i < max; 
        }
    }
}
