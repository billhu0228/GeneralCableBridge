using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;

namespace GeneralCableStayedBridge
{
    public class Bridge
    {
        public int SizeMiddle
        {
            set;get;
        }
        public int SizeSide
        {
            set;get;
        }
        public int NumMiddle
        {
            set;get;
        }
        public int HalfLength
        {
            get
            {
                return SizeSide * NumSide + SizeMiddle * NumMiddle;
            }
        }
        internal void GenerateCrossTruss(ref Model myModel)
        {
            Beam curBeam;

            for (int i = 0; i < NumSide; i++)
            {
                double x0 = -HalfLength + i * SizeSide;

                curBeam = new Beam(new TSG.Point(x0, 0, 0), new TSG.Point(0, -0.5*BeamWidth,0));                
                myBeam.Profile.ProfileString = "WI400-15-20*300";            
                //myBeam.Insert();
                if (i==0)
                {
                    break;
                }
            }

            myModel.CommitChanges();
            
        }

        public int NumSide
        {
            set;get;
        }
        public List<int> NumList ;

        public int BeamHeight
        {
            set; get;
        }



        int BeamWidth;



        public Bridge()
        {
            SizeMiddle = 12000;
            SizeSide = 8000;
            NumMiddle = 34;
            NumSide = 26;
            NumList = new List<int>() { 10,11,9};
            BeamHeight = 8000;
            BeamWidth = 27900;
        }



    }
}
