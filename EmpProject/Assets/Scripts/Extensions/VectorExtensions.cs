﻿using UnityEngine;

public static class VectorExtensions {
    public static Vector3 WithX(this Vector3 vec, float value) {
        return new Vector3(value, vec.y, vec.z);
    }

    public static Vector3 WithY(this Vector3 vec, float value) {
        return new Vector3(vec.x, value, vec.z);
    }
    public static Vector3 WithZ(this Vector3 vec, float value) {
        return new Vector3(vec.x, vec.y, value);
    }

}

