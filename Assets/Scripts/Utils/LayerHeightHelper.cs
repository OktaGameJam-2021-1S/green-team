public enum LayerHeight
{
    Street = 0,
    Sidewalk = 1,
    Building = 2,
}

public static class LayerHeightHelper
{
    
    public static float GetVerticalPosition(int layer)
    {
        return GetVerticalPosition((LayerHeight) layer);
    }

    public static float GetVerticalPosition(LayerHeight layer)
    {
        switch (layer)
        {
            case LayerHeight.Building:
                return 99f;
            case LayerHeight.Sidewalk:
                return 0f;
            case LayerHeight.Street:
                return -1.5f;
        }
        return 0f;
    }

}