using UniRx;
using UnityEngine;

public class BaseCellViewModel
{
    public ReactiveProperty<string> Num { get; private set; } = new ReactiveProperty<string>();
    public ReactiveProperty<Color> Color { get; private set; } = new ReactiveProperty<Color>();

    public string num { get; private set; }
    public Color color { get; private set; }

    private void ChangeColor(Color color)
    {
        this.color = color;
    }

    private void ChangeNum(string num)
    {
        this.num = num;
    }

    public void ChangeElementNumColorSO(NumColorSO numColorSOElement)
    {
        ChangeColor(numColorSOElement.color);
        ChangeNum(numColorSOElement.value);
    }

    public void ChangeElementColorNum(Color color, string num)
    {
        ChangeColor(color);
        ChangeNum(num);
    }

    public void VisualUpdate()
    {
        Num.Value = num;
        Color.Value = color;
    }
}
