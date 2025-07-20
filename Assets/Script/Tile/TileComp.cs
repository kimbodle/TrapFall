using UnityEngine;
using System.Collections;

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
    public bool isOuterWall = false;  // �� ���� �߰�
    public LayerMask playerLayer;

    public Vector2Int nodePosition;
    [SerializeField]
    TileType currentTileType = TileType.Normal;
    SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] framesBreakTile;
    [SerializeField] private float frameInterval = 0.2f; // ������ �� ���� (��)

    private bool bIsBreak = false;


    private bool IsPlayerOnThisTile()
    {
        Vector2 center = transform.position;
        Vector2 size = new Vector2(0.9f, 0.9f); // Ÿ�� ũ�⿡ �°�

        Collider2D player = Physics2D.OverlapBox(center, size, 0f, playerLayer);

        return player != null;
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //��Ҵٰ� ġ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerController = collision.GetComponent<PlayerController>();
            if (playerController != null && playerController.bIsJump)
            {
                Debug.Log("���� �� �� Ÿ�� ȿ�� ����");
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
                Debug.Log("����");
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

        // Ÿ���� Danger �� Ư��Ÿ���̸� �� �÷��̾� ���� �ִ��� Ȯ��
        if (IsSpecialTile(inputTileType) && IsPlayerOnThisTile())
        {
            Debug.Log("Ÿ���� ���� �� ���� �÷��̾ �־� ȿ�� ��� �ߵ�!");

            var player = Physics2D.OverlapBox(transform.position, new Vector2(0.9f, 0.9f), 0f, playerLayer);
            if (player != null)
            {
                if (!IsWalkable())
                {
                    Debug.Log("����");
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

    //�ļ��� �ı��� Ÿ���� �ð��������� �ٽ� ������� ���ƿ�
    public IEnumerator DestroyTile()
    {
        yield return new WaitForSeconds(1f);
        //Ÿ�� �μ����� �ִϸ��̼� ����
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

        // ��: �μ��� �� ������� �ϰų� ����
        SetTileType(TileType.Destroyed); // �Ǵ� spriteRenderer.sprite = null;
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
        yield return new WaitForSeconds(0f); // Ÿ�� 1�� �� ����
        SetTileType(TileType.Normal);
    }

    private bool PlayerOnTop()
    {
        Vector2 tileCenter = transform.position;
        Vector2 checkSize = new Vector2(0.9f, 0.9f); // Ÿ�� ũ�⿡ ���� ����
        Collider2D hit = Physics2D.OverlapBox(tileCenter + Vector2.up * 0.1f, checkSize, 0f);

        return hit != null;
    }
}
