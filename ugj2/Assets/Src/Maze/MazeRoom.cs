
public class MazeRoom
{
	public int SettingsIndex { private set; get; }

	public int Id { private set; get;}

	public MazeRoom(int id, int settingsIndex)
	{
		Id = id;
		SettingsIndex = settingsIndex;
    }

	public MazeRoomSettings Settings
	{
		get
		{
			return GameCore.mazeInstance.roomSettings[SettingsIndex];
        }
	}

}

