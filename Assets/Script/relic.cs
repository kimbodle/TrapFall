using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class relic : MonoBehaviour
{
    [System.Serializable]
    struct RelicData
    {
        public ItemType type;
        public int score; 
    }


    [SerializeField]
    private RelicData[] relicDatas;
    [SerializeField]
    private ItemType relicType;
    
    private int relicScore;

        
    private void Awake()
    {
        foreach (var relicData in relicDatas)
        {
            if(relicData.type == relicType)
            {
                relicScore = relicData.score;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.SetScore(relicScore);

            //if (pickupEffect != null) Instantiate(pickupEffect, transform.position, Quaternion.identity);
            //if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            SoundManager.Instance.PlaySFX(SFXType.RelicGet);
            gameObject.SetActive(false);
        }
    }
}
