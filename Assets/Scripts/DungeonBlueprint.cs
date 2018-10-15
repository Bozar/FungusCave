using UnityEngine;

// Change the 2D array's content, which is defined in DungeonBoard.
public class DungeonBlueprint : MonoBehaviour
{
    private DungeonBoard board;

    public void PlaceWallsManually()
    {
        board.ChangeBlock(DungeonBoard.DungeonBlock.Wall, 4, 4);
        board.ChangeBlock(DungeonBoard.DungeonBlock.Wall, 5, 5);
        board.ChangeBlock(DungeonBoard.DungeonBlock.Wall, 6, 6);

        board.ChangeBlock(DungeonBoard.DungeonBlock.Wall,
            board.Width - 1, board.Height - 1);
        board.ChangeBlock(DungeonBoard.DungeonBlock.Wall,
            board.Width, board.Height - 1);

        board.ChangeBlock(DungeonBoard.DungeonBlock.Pool, 8, 8);
        board.ChangeBlock(DungeonBoard.DungeonBlock.Pool, 8, 9);
        board.ChangeBlock(DungeonBoard.DungeonBlock.Pool, 9, 9);
    }

    private void Start()
    {
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
    }
}
