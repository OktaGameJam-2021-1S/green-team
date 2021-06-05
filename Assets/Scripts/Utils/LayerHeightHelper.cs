public enum LayerHeight
{
    Street = 0,
    Sidewalk = 1,
    Building = 2,
    TopBuilding1 = 3,
    TopBuilding2 = 4,
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
            // The building height is different per building
            case LayerHeight.TopBuilding2:
                return 2f;
            case LayerHeight.TopBuilding1:
                return 1f;
            case LayerHeight.Building:
                return 0f;
            case LayerHeight.Sidewalk:
                return -1.20f;
            case LayerHeight.Street:
                return -2.30f;
        }
        return 0f;
    }

}