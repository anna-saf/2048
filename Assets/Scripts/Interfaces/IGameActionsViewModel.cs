using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameActionsViewModel
{
    void MoveCells(SwipeData swipeData, ICellViewModel[,] cells);
}
