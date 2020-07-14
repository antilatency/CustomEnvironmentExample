using UnityEngine;

public static class MatrixOperations{
    public static float[,] multiply(this float[,] a, float[,] b) {
        if (a.GetLength(1) != b.GetLength(0))
            throw new System.Exception("wrong matrix size");

        int M = a.GetLength(0);
        int L = a.GetLength(1);
        int N = b.GetLength(1);

        var res = new float[M, N];
        for (int m = 0; m < M; ++m) {
            for (int n = 0; n < N; ++n) {
                float s = 0;
                for (int l = 0; l < L; ++l) {
                    s += a[m, l] * b[l, n];
                }

                res[m, n] = s;
            }
        }
        return res;
    }

    public static float[] multiply(this float[,] a, float[] b) {
        if (a.GetLength(1) != b.Length)
            throw new System.Exception("wrong matrix size");

        int M = a.GetLength(0);
        int L = b.Length;

        var res = new float[M];
        for (int m = 0; m < M; ++m) {
            float s = 0;
            for (int l = 0; l < L; ++l) {
                s += a[m, l] * b[l];
            }
            res[m] = s;
        }
        return res;
    }

    public static float[,] outerProduct(this float[] a, float[] b) {
        int M = a.Length;
        int N = b.Length;

        var res = new float[M, N];
        for (int m = 0; m < M; ++m) {
            for (int n = 0; n < N; ++n) {
                res[m, n] = a[m] * b[n];
            }
        }

        return res;
    }

    public static float[,] transpose(this float[,] inp) {
        int N = inp.GetLength(0);
        int M = inp.GetLength(1);

        var res = new float[M, N];
        for (int m = 0; m < M; ++m){
            for (int n = 0; n < N; ++n) {
                res[m, n] = inp[n, m];
            }
        }

        return res;
    }

    public static float[,] inverse(this float[,] inp) {
        if(inp.GetLength(0) != 4 || inp.GetLength(1) != 4 )
            throw new System.NotImplementedException();

        var mat = new Matrix4x4();
        for (int i = 0; i < 4; ++i) {
            for (int j = 0; j < 4; ++j)
                mat[i, j] = inp[i, j];
        }

        mat = mat.inverse;
        var res = new float[4, 4];
        for (int i = 0; i < 4; ++i) {
            for (int j = 0; j < 4; ++j)
                res[i, j] = mat[i, j];
        }

        return res;
    }
}