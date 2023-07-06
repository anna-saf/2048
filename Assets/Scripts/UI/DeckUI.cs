using UnityEngine;
using UnityEngine.UI;

public class DeckUI : MonoBehaviour
{
    [SerializeField] private int paddingPartInSize; // Значение отношения размера padding к размеру доски
    [SerializeField] private int spacingPartInSize; // Значение отношения размера spacing к размеру доски

    RectTransform deckRect;
    GridLayoutGroup gameObjectGridLayoutGroup;

    float deckSize;

    private void Start()
    {
        gameObjectGridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
        gameObjectGridLayoutGroup.constraintCount = DeckModel.Instance.DeckSize;
        deckRect = GetComponent<RectTransform>();
        CorrectSize();
        CorrectGrid();
    }

    private void CorrectSize()
    {
        float gameUIwidth = deckRect.rect.width;
        float gameUIheight = deckRect.rect.height;

        if (gameUIheight > gameUIwidth)
        {
            deckRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, gameUIwidth);
        }
        else deckRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, gameUIheight);

        deckSize = deckRect.rect.width;
    }

    private void CorrectGrid()
    {
        CorrectPadding();
        CorrectSpacing();
        CorrectElementSize();
    }

    private void CorrectPadding()
    {
        int padding = (int)deckSize / paddingPartInSize;

        gameObjectGridLayoutGroup.padding.top = padding;
        gameObjectGridLayoutGroup.padding.right = padding;
        gameObjectGridLayoutGroup.padding.bottom = padding;
        gameObjectGridLayoutGroup.padding.left = padding;
    }

    private void CorrectSpacing()
    {
        int spacing = (int)deckSize / spacingPartInSize;
        gameObjectGridLayoutGroup.spacing = new Vector2(spacing, spacing);
    }

    private void CorrectElementSize()
    {
        //Так как в длину игровой доски помимо N ячеек еще включены N-1 пробелов между ячейками и 2 padding, то следующая строка оставляет только суммарную длину N ячеек
        var onlyCells = deckSize - (gameObjectGridLayoutGroup.padding.right + gameObjectGridLayoutGroup.padding.left) - gameObjectGridLayoutGroup.spacing.x * 
                        (gameObjectGridLayoutGroup.constraintCount - 1);
        var cellSize = onlyCells / gameObjectGridLayoutGroup.constraintCount;

        gameObjectGridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
    }
}
