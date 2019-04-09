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
            TSG.Point A, B, C, D, E, F, G, H, I;
            for (int i = 0; i < NumSide; i++)
            {
                double x0 = i * SizeSide;
                A = new TSG.Point(x0, -0.5 * BeamWidth, 0);
                F = new TSG.Point(x0, 0.5 * BeamWidth, 0);
                B = new TSG.Point(x0, -0.5 * BeamWidth, -BeamHeight);
                E = new TSG.Point(x0, 0.5 * BeamWidth, -BeamHeight);
                C = new TSG.Point(x0, -0.25 * BeamWidth, -BeamHeight);
                D = new TSG.Point(x0,0.25 * BeamWidth, -BeamHeight);
                I = new TSG.Point(x0, -0.25 * BeamWidth, 0);
                H = new TSG.Point(x0,0, 0);
                G = new TSG.Point(x0, 0.25 * BeamWidth, 0);



                curBeam = new Beam(A,F );
                curBeam.Profile.ProfileString = "WI400-15-20*300";
                curBeam.Insert();
                curBeam = new Beam(B,E );
                curBeam.Profile.ProfileString = "WI400-15-20*300";
                curBeam.Insert();

                curBeam = new Beam(A, B);
                curBeam.Profile.ProfileString = "WI400-15-20*300";
                curBeam.Insert();
                curBeam = new Beam(F, E);
                curBeam.Profile.ProfileString = "WI400-15-20*300";
                curBeam.Insert();

                curBeam = new Beam(C,A);
                curBeam.Profile.ProfileString = "WI400-15-20*300";
                curBeam.Insert();
                curBeam = new Beam(C,I);
                curBeam.Profile.ProfileString = "WI400-15-20*300";
                curBeam.Insert();
                curBeam = new Beam(C, H);
                curBeam.Profile.ProfileString = "WI400-15-20*300";
                curBeam.Insert();

                curBeam = new Beam(D, F);
                curBeam.Profile.ProfileString = "WI400-15-20*300";
                curBeam.Insert();
                curBeam = new Beam(D, G);
                curBeam.Profile.ProfileString = "WI400-15-20*300";
                curBeam.Insert();
                curBeam = new Beam(D, H);
                curBeam.Profile.ProfileString = "WI400-15-20*300";
                curBeam.Insert();


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
