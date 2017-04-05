using UnityEngine;

public static class Functions{
    
    //Returns an angle that will cause the "top" of a sprite to point towards that vector direction if its "Z" rotation is set to that float
    public static float VectorToAngle(Vector2 vector)
    {
        float angle = Mathf.Atan2(vector.x, vector.y);
        angle *= Mathf.Rad2Deg * -1f;

        return angle;
    }
    
}
