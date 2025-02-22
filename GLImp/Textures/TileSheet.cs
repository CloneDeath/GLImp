namespace GLImp.Textures;

public class TileSheet {
	public int HOffset;
	public int HSeparation;
	public Texture Texture;
	public int TileHeight;
	public int TileWidth;
	public int VOffset;
	public int VSeparation;

	public TileSheet(Texture tex, int tileWidth, int tileHeight) {
		Texture = tex;
		TileWidth = tileWidth;
		TileHeight = tileHeight;
		HOffset = 0;
		VOffset = 0;
		HSeparation = 0;
		VSeparation = 0;
	}

	public int HorizontalTiles => (Texture.Width - HOffset) / (TileWidth + HSeparation);

	public int VerticalTiles => (Texture.Height - VOffset) / (TileHeight + VSeparation);

	public SubImage this[int x, int y] => GetSubImage(x, y);

	public SubImage GetSubImage(int x, int y) => Texture.Subimage((TileWidth + HOffset) * x + HOffset,
		(TileHeight + VOffset) * y + VOffset, TileWidth, TileHeight);
}