using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public interface ITimeEvent
{
    void OnPlayerEnter(GameObject player);
}

public class TileComp : MonoBehaviour
{
    public Sprite[] tileSprite;
    public float randomTileTime= 5.0f;
    public float recoveryTileTime = 3.0f;
    public TileManager tileManager;
    public bool isOuterWall = false;  // ← 여기 추가
    public LayerMask playerLayer;

    public Vector2Int nodePosition;
    [SerializeField]
    TileType currentTileType = TileType.Normal;
    SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] framesBreakTile;
    [SerializeField] private float frameInterval = 0.2f; // 프레임 간 간격 (초)

    private bool bIsBreak = false;


    private bool IsPlayerOnThisTile()
    {
        Vector2 center = transform.position;
        Vector2 size = new Vector2(0.9f, 0.9f); // 타일 크기에 맞게

        Collider2D player = Physics2D.OverlapBox(center + new Vector2(0, 0.2f), size, 0f, playerLayer);

        return player != null;
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //밟았다고 치고
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerController = collision.GetComponent<PlayerController>();
            if (playerController != null && playerController.bIsJump)
            {
                Debug.Log("무적 중 → 타일 효과 무시");
                return;
            }

            if (IsWalkable())
            {
                GameObject player = collision.gameObject;

                foreach (var tileEffect in GetComponents<ISpecialTile>())
                {
                    tileEffect.Activate(player);
                }
            }
            else
            {
                Debug.Log("죽음");
                GameManager.Instance.GameOver();
            }
        }
    }


    public bool IsWalkable()
    {
        //return currentTileType != TileType.Danger && currentTileType != TileType.Destroyed;
        return currentTileType != TileType.Destroyed;
    }
    public TileType GetTileType() { return currentTileType; }

    public void SetTileType(TileType inputTileType)
    {
        currentTileType = inputTileType;
        updateTileImage();
        ClearExistingEffects();
        TileEvent();

        // 타입이 Danger 등 특수타일이면 → 플레이어 위에 있는지 확인
        if (IsSpecialTile(inputTileType) && IsPlayerOnThisTile())
        {
            Debug.Log("타일이 변한 후 위에 플레이어가 있어 효과 즉시 발동!");

            var player = Physics2D.OverlapBox(transform.position + new Vector3(0, 0.2f, 0), new Vector2(0.9f, 0.9f), 0f, playerLayer);
            DrawBox2D(transform.position + new Vector3(0, 0.2f, 0), new Vector2(0.9f, 0.9f), 0f, Color.red);

            if (player != null)
            {
                if (!IsWalkable())
                {
                    Debug.Log("죽음");
                    GameManager.Instance.GameOver();
                    return;
                }

                foreach (var tileEffect in GetComponents<ISpecialTile>())
                {
                    tileEffect.Activate(player.gameObject);
                }
            }
        }
        //if (currentTileType == TileType.Danger) StartCoroutine(DestroyTile());
    }
    void DrawBox2D(Vector2 center, Vector2 size, float angle, Color color)
    {
        Vector2 halfSize = size * 0.5f;

        // 회전
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);

        // 꼭짓점 계산 (모두 Vector2로 유지)
        Vector2 topLeft = center + (Vector2)(rot * new Vector3(-halfSize.x, halfSize.y));
        Vector2 topRight = center + (Vector2)(rot * new Vector3(halfSize.x, halfSize.y));
        Vector2 bottomRight = center + (Vector2)(rot * new Vector3(halfSize.x, -halfSize.y));
        Vector2 bottomLeft = center + (Vector2)(rot * new Vector3(-halfSize.x, -halfSize.y));

        // 라인 그리기
        Debug.DrawLine(topLeft, topRight, color, 30f);
        Debug.DrawLine(topRight, bottomRight, color, 30f);
        Debug.DrawLine(bottomRight, bottomLeft, color, 30f);
        Debug.DrawLine(bottomLeft, topLeft, color, 30f);
    }

    private bool IsSpecialTile(TileType type)
    {
        return type == TileType.Danger ||
               type == TileType.Trap ||
               type == TileType.Ice ||
               type == TileType.Fog ||
               type == TileType.Electric ||
               type == TileType.Spin ||
               type == TileType.Random ||
               type == TileType.Destroyed;
    }

    public void ClearExistingEffects()
    {
        foreach (var effect in GetComponents<ISpecialTile>())
        {
            Destroy((Component)effect);
        }
    }

    private void updateTileImage()
    {
        //if(currentTileType == TileType.Destroyed) return;
        int tileIndex = (int)currentTileType;
        if (!spriteRenderer || tileSprite.Length <= tileIndex || tileSprite[tileIndex] == null) return;

        spriteRenderer.sprite = tileSprite[tileIndex];
    }

    //셍성된 파괴된 타일은 시간이지나면 다시 원래대로 돌아옴
    public IEnumerator DestroyTile()
    {
        yield return new WaitForSeconds(1f);
        //타일 부셔지는 애니메이션 ㄱㄱ
        SoundManager.Instance.PlaySFX(SFXType.TileDestroy);
        //SetTileType(TileType.Destroyed);
        //Debug.Log(breakAnimation.Play("Broken") ? "True" : "False");
        //animator.SetTrigger("Play");
        StartBreakAnimation();

        yield return new WaitForSeconds(recoveryTileTime);
        SetTileType(TileType.Normal);
    }
    public void StartBreakAnimation()
    {
        if (!bIsBreak)
        {
            StopCoroutine(BreakAnimationRoutine());
            StartCoroutine(BreakAnimationRoutine());
        }
    }

    private IEnumerator BreakAnimationRoutine()
    {
        bIsBreak = true;

        for (int i = 0; i < framesBreakTile.Length; i++)
        {
            spriteRenderer.sprite = framesBreakTile[i];
            yield return new WaitForSeconds(frameInterval);
        }

        bIsBreak = false;

        // 예: 부서진 후 사라지게 하거나 복구
        SetTileType(TileType.Destroyed); // 또는 spriteRenderer.sprite = null;
    }
    private void TileEvent()
    {
        if (currentTileType == TileType.Normal) return;

        switch (currentTileType)
        {
            case TileType.Danger:
                gameObject.AddComponent<DangerTileEffect>();
                StartCoroutine(DestroyTile());
                break;
            case TileType.Electric:
                gameObject.AddComponent<ElectricTileEffect>();
                break;
            case TileType.Trap:
                gameObject.AddComponent<TrapTileEffect>();
                break;
            case TileType.Spin:
                gameObject.AddComponent<SpinTileEffect>();
                break;
            case TileType.Ice:
                gameObject.AddComponent<IceTileEffect>();
                break;
            case TileType.Fog:
                gameObject.AddComponent<FogTileEffect>();
                break;
            case TileType.Random:
                gameObject.AddComponent<RandomTileEffect>();
                break;
        }
    }
    public IEnumerator RevertTile()
    {
        yield return new WaitForSeconds(0f); // 타일 1초 후 복구
        SetTileType(TileType.Normal);
    }

    private bool PlayerOnTop()
    {
        Vector2 tileCenter = transform.position;
        Vector2 checkSize = new Vector2(0.9f, 0.9f); // 타일 크기에 맞춰 조정
        Collider2D hit = Physics2D.OverlapBox(tileCenter + new Vector2(0, 0.2f) , checkSize, 0f);

        return hit != null;
    }
}
