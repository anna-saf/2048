using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface ICellViewModel
{
    public ReactiveProperty<string> Num { get; }
    public ReactiveProperty<Color> Color { get; }

    public void ChangeElementNumColorSO(NumColorSO numColorSOElement);
    public void ChangeElementColorNum(Color color, string num);
}