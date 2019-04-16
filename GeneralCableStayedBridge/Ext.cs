using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;

namespace GeneralCableStayedBridge
{
    public static class Ext
    {
        public static Point MoveTo(this Point A,double dx,double dy,double dz)
        {
            return new Point(A.X + dx, A.Y + dy, A.Z + dz);
        }
        public static Point MirrorY(this Point Pt)
        {
            return new Point(Pt.X, -Pt.Y, Pt.Z);
        }
    }
}
