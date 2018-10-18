public class BlueprintFungus : DungeonBlueprint
{
    public void DrawBlueprint()
    {
        board.ChangeBlock(DungeonBoard.DungeonBlock.Fungus, 5, 5);
    }
}
