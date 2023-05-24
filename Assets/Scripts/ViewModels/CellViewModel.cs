using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CellViewModel : MonoBehaviour, ICellViewModel
{
    public ReactiveProperty<string> Num { get; private set; } = new ReactiveProperty<string>();
    public ReactiveProperty<Color> Color { get; private set; } = new ReactiveProperty<Color>();
    public GameObject gameObjectOwner { get; private set; }

    private void Awake()
    {
        gameObjectOwner = gameObject;
    }

    public void ChangeColor(Color color)
    {
        Color.Value = color;
    }

    public void ChangeNum(string num)
    {
        Num.Value = num;
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


}
