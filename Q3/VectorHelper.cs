using System;
using System.Numerics;

namespace MapGenerator.Q3
{
    public static class VectorHelper
    {
        public static void VectorRotateOrigin(Vector3 vIn, Vector3 vRotation, Vector3 vOrigin, out Vector3 vOut )
        {
            Vector3 vTemp, vTemp2;

            VectorSubtract(vIn, vOrigin, out vTemp);
            VectorRotate(vTemp, vRotation, out vTemp2);
            VectorAdd(vTemp2, vOrigin, out vOut );
        }

        public static void VectorSubtract(Vector3 va, Vector3 vb, out Vector3 vOut )
        {
            vOut = new Vector3(va.X - vb.X, va.Y - vb.Y, va.Z - vb.Z);
        }

        public static void VectorAdd(Vector3 va, Vector3 vb, out Vector3 vOut)
        {
            vOut = new Vector3(va.X + vb.X, va.Y + vb.Y, va.Z + vb.Z);
        }

        public static void VectorRotate(Vector3 vIn, Vector3 vRotation, out Vector3 vOut)
        {
            int[,] nIndex = new int[3, 2];
            int i;

            VectorCopy(vIn, out var va );
            VectorCopy(va, out var vWork );
            nIndex[0,0] = 1; nIndex[0,1] = 2;
            nIndex[1,0] = 2; nIndex[1,1] = 0;
            nIndex[2,0] = 0; nIndex[2,1] = 1;

            for (i = 0; i < 3; i++ )
            {
                float f = vRotation.GetIndex(i);
                if (!f.Equals(0)) {
                    float dAngle = (float)(f * Math.PI / 180.0f);
                    float c = (float)Math.Cos(dAngle);
                    float s = (float)Math.Sin(dAngle);
                    var aaa = va.GetIndex(nIndex[i, 0]) * c - va.GetIndex(nIndex[i, 1]) * s;
                    var bbb = va.GetIndex(nIndex[i, 0]) * s + va.GetIndex(nIndex[i, 1]) * c;
                    vWork = vWork.SetIndex(nIndex[i, 0], aaa);
                    vWork = vWork.SetIndex(nIndex[i, 1], bbb);
                }
                VectorCopy(vWork, out va );
            }
            VectorCopy(vWork, out vOut );
        }

        public static void VectorCopy(Vector3 vIn, out Vector3 vOut)
        {
            vOut = new Vector3(vIn.X, vIn.Y, vIn.Z);
        }
    }
}
