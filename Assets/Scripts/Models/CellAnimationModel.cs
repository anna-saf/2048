using UnityEngine;

public class CellAnimationModel : MonoBehaviour
{
    [SerializeField] private float sizeChangeTime;
    [SerializeField] private float moveTime;

    public float SizeChangeTime { get { return sizeChangeTime; } }

    public float MoveTime { get { return moveTime; } }

    public static CellAnimationModel Instance;

    private void Awake()
    {
        Instance = this;
    }
}
