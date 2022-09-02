
public static class GameStatics
{
    public enum PolyominoType
    {
        Monomino,
        Domino,
        Tromino_I,
        Tromino_L,
        Tetromino_I,
        Tetromino_O,
        Tetromino_T,
        Tetromino_J,
        Tetromino_L,
        Tetromino_S,
        Tetromino_Z,
        END
    }

    public enum PolyominoRot
    {
        Rot0,
        Rot90,
        Rot180,
        Rot270
    }

    public enum GameMode
    {
        Sprint,
        Endless,
    }

    public enum GamePage
    {
        Title,
        Game
    }

    public enum GameEndType
    {
        GameOver,
        SprintFinish,
        GiveUp
    }

}
