﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
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
        public int NumSide
        {
            set; get;
        }
        public int BeamHeight
        {
            set; get;
        }
        public int BeamWidth
        {
            set;get;
        }
        public int CableOnTower { get; private set; }
        public int TowerAboveRoad { get; private set; }
        public int TowerBelowRoad { get; private set; }

        public List<int> NumList;




        public Bridge()
        {
            SizeMiddle = 12000;
            SizeSide = 8000;
            NumMiddle = 34;
            NumSide = 26;
            NumList = new List<int>() { 10, 11 };
            NumList.Add((NumMiddle + NumSide) / 2 - NumList.Sum());
            BeamHeight = 8000;
            BeamWidth = 27900;

            CableOnTower = 2500;
            TowerAboveRoad = 95000;
            TowerBelowRoad = 55600;

        }







        internal void ModifyGrid(ref Model myModel)
        {
            bool Success = false;
            ModelObjectEnumerator Enumerator = myModel.GetModelObjectSelector().GetAllObjects();
            while (!Success && Enumerator.MoveNext())
            {
                ModelObject ModelObject = Enumerator.Current as ModelObject;
                Type ObjectType = ModelObject.GetType();

                while (ObjectType != typeof(Grid) && ObjectType.BaseType != null)
                {
                    ObjectType = ObjectType.BaseType;
                }


                if (ObjectType == typeof(Grid))
                {
                    Success = true;
                }
                    
            }

            Grid curGr = Enumerator.Current as Grid;
            var ff = GetGridXString();
            curGr.CoordinateX= ff.Item1;
            curGr.CoordinateY = string.Format("{0:F2} 2*{1:F2}", BeamWidth * -0.5, BeamWidth * 0.5);
            curGr.LabelX = ff.Item2;
            curGr.LabelY = "R C L";
            curGr.Modify();
            myModel.CommitChanges();
        }









        /// <summary>
        /// 生成桥塔
        /// </summary>
        /// <param name="myModel"></param>
        internal void GenerateTower(ref Model myModel)
        {
            var ff = GetGridXString().Item1.Split(' ');

            for (int i = 0; i < 2; i++)
            {
                int x0 = -HalfLength + int.Parse(ff[1]) + int.Parse(ff[2]) + int.Parse(ff[3])+i* int.Parse(ff[4])*2;
                int y1 = 15000;
                int y2 = 18500;
                int y3 = (int)(BeamWidth * 0.5);
                int z1 = 0-TowerBelowRoad;
                int z2= 0 - BeamHeight - 1000;
                int z3 = TowerAboveRoad;
                int z4 = z3 + (((NumMiddle+NumSide)/2-2)+1)*CableOnTower;

                Point A1, B1, C1, D1, A, B, C, D2;

                A = new Point(x0,y1,z1);
                A1 = new Point(x0, y1, z2);
                B = new Point(x0, y2, z2);
                B1 = new Point(x0, y2, z3);
                C = new Point(x0, y3, z3);
                C1 = new Point(x0, y3, z4);
                string macro1 = string.Format("H_WLD_B12000*10000*1200*1500*11000*6000*1000*1200-{0}", y1 - y2);
                string macro2 = string.Format("H_WLD_B12000*10000*1200*1500*11000*6000*1000*1200-{0}", y2 - y1);
                CreatBeam(A, A1, macro1, Position.RotationEnum.TOP);
                CreatBeam(A.MirrorY(), A1.MirrorY(), macro2, Position.RotationEnum.TOP);
                

                macro1 = string.Format("H_WLD_B11000*6000*1000*1200*7500*6000*1000*1200-{0}", y2 - y3);
                macro2 = string.Format("H_WLD_B11000*6000*1000*1200*7500*6000*1000*1200-{0}", y3 - y2);
                CreatBeam(B, B1, macro1, Position.RotationEnum.TOP);
                CreatBeam(B.MirrorY(), B1.MirrorY(), macro2, Position.RotationEnum.TOP);


                macro1 = string.Format("H_WLD_B7500*6000*1000*1200*7500*6000*1000*1200-{0}", 0);
                
                CreatBeam(C, C1, macro1, Position.RotationEnum.TOP);
                CreatBeam(C.MirrorY(), C1.MirrorY(), macro1, Position.RotationEnum.TOP);

                CreatBeam(B, B.MirrorY(), "B_BUILT8000*9800*1000*1000", Position.DepthEnum.BEHIND);
                CreatBeam(C, C.MirrorY(), "B_BUILT7000*6300*1000*1000", Position.DepthEnum.BEHIND);


                CreatBeam(A.MoveTo(0, 0, -37500), A.MoveTo(0,0,-4500), "H_WLD_C12000*14000*1200*1000", Position.RotationEnum.TOP);
                CreatBeam(A.MirrorY().MoveTo(0, 0, -37500), A.MirrorY().MoveTo(0, 0,-4500), "H_WLD_C12000*14000*1200*1000", Position.RotationEnum.TOP);
            }
            //Column


            myModel.CommitChanges();


        }


        /// <summary>
        /// 生成主梁
        /// </summary>
        /// <param name="myModel"></param>
        internal void GenerateCrossTruss(ref Model myModel)
        {
            
            TSG.Point A, B, C, D, E, F, G, H, I;
            TSG.Point pA, pB, pC, pD, pE, pF, pG, pH, pI;
            pA = null;
            pB = null;
            pC = null;
            pD = null;
            pE = null;
            pF = null;
            pG = null;
            pH = null;
            pI = null;
            for (int i = 0; i < (NumSide+NumMiddle)*2+1; i++)
            {
                double x0 = 0; 

                if (i<=NumSide)
                {
                    x0 = -HalfLength + i * SizeSide;

                }
                else if (i<=NumMiddle*2+NumSide)
                {
                    x0 = -HalfLength + NumSide * SizeSide + (i - NumSide) * SizeMiddle;
                }
                else
                {
                    x0 = -HalfLength + NumSide * SizeSide + 2 * NumMiddle * SizeMiddle + (i - NumSide - 2 * NumMiddle) * SizeSide;
                }

                A = new TSG.Point(x0, -0.5 * BeamWidth, 0);
                F = new TSG.Point(x0, 0.5 * BeamWidth, 0);
                B = new TSG.Point(x0, -0.5 * BeamWidth, -BeamHeight);
                E = new TSG.Point(x0, 0.5 * BeamWidth, -BeamHeight);
                C = new TSG.Point(x0, -0.25 * BeamWidth, -BeamHeight);
                D = new TSG.Point(x0,0.25 * BeamWidth, -BeamHeight);
                I = new TSG.Point(x0, -0.25 * BeamWidth, 0);
                H = new TSG.Point(x0,0, 0);
                G = new TSG.Point(x0, 0.25 * BeamWidth, 0);

                CreatBeam(A, F, "H_WLD_A900*700*24*16*1261", Position.RotationEnum.TOP);
                CreatBeam(B, E, "B_WLD_F900*700*16*20", Position.RotationEnum.TOP);
                CreatBeam(A, B, "B_WLD_F900*730*36*36", Position.RotationEnum.FRONT);
                CreatBeam(F, E, "B_WLD_F900*730*36*36", Position.RotationEnum.FRONT);
                CreatBeam(C, A, "B_WLD_A700*300*20*28", Position.RotationEnum.FRONT);
                CreatBeam(C, I, "HN700*300*13*24", Position.RotationEnum.TOP);
                CreatBeam(C, H, "HN700*300*13*24", Position.RotationEnum.FRONT);
                CreatBeam(D, F, "B_WLD_A700*300*20*28", Position.RotationEnum.FRONT);
                CreatBeam(D, G, "HN700*300*13*24", Position.RotationEnum.TOP);
                CreatBeam(D, H, "HN700*300*13*24", Position.RotationEnum.FRONT);

                if (i!=0)
                {
                    CreatBeam(A, pA, "B_WLD_F900*900*36*36", Position.RotationEnum.TOP);                    
                    CreatBeam(B, pB, "B_WLD_F900*900*36*36", Position.RotationEnum.TOP);
                    CreatBeam(E, pE, "B_WLD_F900*900*36*36", Position.RotationEnum.TOP);
                    CreatBeam(F, pF, "B_WLD_F900*900*36*36", Position.RotationEnum.TOP);
                    if (x0<=0)
                    {
                        CreatBeam(A, pB, "B_WLD_A900*600*20*24", Position.RotationEnum.FRONT);
                        CreatBeam(F, pE, "B_WLD_A900*600*20*24", Position.RotationEnum.FRONT);
                    }
                    else
                    {
                        CreatBeam(B, pA, "B_WLD_A900*600*20*24", Position.RotationEnum.FRONT);
                        CreatBeam(E, pF, "B_WLD_A900*600*20*24", Position.RotationEnum.FRONT);
                    }


                    CreatPlate(A.MoveTo(0,0,450), pA.MoveTo(0, 0, 450), pH.MoveTo(0, 0, 811), H.MoveTo(0, 0, 811), 16);
                    CreatPlate(F.MoveTo(0, 0, 450), pF.MoveTo(0, 0, 450), pH.MoveTo(0, 0, 811), H.MoveTo(0, 0, 811), 16);

                }

                pA = A;
                pB = B;
                pC = C;
                pD = D;
                pE = E;
                pF = F;
                pG = G;
                pH = H;
                pI = I;
                //if (false)
                //{
                //    break;
                //}
            }
            myModel.CommitChanges();
        }




        internal void GenerateCable(ref Model myModel)
        {
            Point Ab,Fb,At,Ft;
            var ff = GetGridXString().Item1.Split(' ');
            int xt1 = -HalfLength + int.Parse(ff[1]) + int.Parse(ff[2]) + int.Parse(ff[3]);
            int xt2 = -HalfLength + int.Parse(ff[1]) + int.Parse(ff[2]) + int.Parse(ff[3]) + int.Parse(ff[4]) * 2;
            for (int i = 0; i < (NumSide + NumMiddle) * 2 + 1; i++)
            {
                int x0 = 0;

                if (i <= NumSide)
                {
                    x0 = -HalfLength + i * SizeSide;
                }
                else if (i <= NumMiddle * 2 + NumSide)
                {
                    x0 = -HalfLength + NumSide * SizeSide + (i - NumSide) * SizeMiddle;
                }
                else
                {
                    x0 = -HalfLength + NumSide * SizeSide + 2 * NumMiddle * SizeMiddle + (i - NumSide - 2 * NumMiddle) * SizeSide;
                }

                Ab = new Point(x0, -0.5 * BeamWidth, 0);
                Fb = new Point(x0, 0.5 * BeamWidth, 0);
                if (x0<=0)
                {
                    At = new Point(xt1, -0.5 * BeamWidth, TowerAboveRoad + (i + 1) * CableOnTower);
                    Ft = new Point(xt1, 0.5 * BeamWidth, TowerAboveRoad + (i + 1) * CableOnTower);
                }
                else
                {
                    At = new Point(xt2, -0.5 * BeamWidth, TowerAboveRoad + (i + 1) * CableOnTower);
                    Ft = new Point(xt2, 0.5 * BeamWidth, TowerAboveRoad + (i + 1) * CableOnTower);
                }

                CreatBeam(At, Ab, "Φ63", Position.DepthEnum.MIDDLE);
                CreatBeam(Ft, Fb, "Φ63", Position.DepthEnum.MIDDLE);

            }
            myModel.CommitChanges();









        }









        public static void ExportIFC(string outputFileName)
        {
            var componentInput = new ComponentInput();
            componentInput.AddOneInputPosition(new Point(0, 0, 0));
            var comp = new Component(componentInput)
            {
                Name = "ExportIFC",
                Number = BaseComponent.PLUGIN_OBJECT_NUMBER
            };

            // Parameters
            comp.SetAttribute("OutputFile", outputFileName);
            comp.SetAttribute("Format", 0);
            comp.SetAttribute("ExportType", 1);
            //comp.SetAttribute("AdditionalPSets", "");
            comp.SetAttribute("CreateAll", 0);  // 0 to export only selected objects

            // Advanced
            comp.SetAttribute("Assemblies", 1);
            comp.SetAttribute("Bolts", 1);
            comp.SetAttribute("Welds", 0);
            comp.SetAttribute("SurfaceTreatments", 1);

            comp.SetAttribute("BaseQuantities", 1);
            comp.SetAttribute("GridExport", 1);
            comp.SetAttribute("ReinforcingBars", 1);
            comp.SetAttribute("PourObjects", 1);

            comp.SetAttribute("LayersNameAsPart", 1);
            comp.SetAttribute("PLprofileToPlate", 0);
            comp.SetAttribute("ExcludeSnglPrtAsmb", 0);

            comp.SetAttribute("LocsFromOrganizer", 0);

            comp.Insert();
        }



        private void CreatPlate(TSG.Point a, TSG.Point b, TSG.Point c, TSG.Point d, int thick)
        {
            ContourPlate curPlate = new ContourPlate();
            
            Profile pf = new Profile();
            pf.ProfileString = string.Format("PL{0}", thick);
            var contor = new Contour();
            contor.AddContourPoint(new ContourPoint(a,null));
            contor.AddContourPoint(new ContourPoint(b, null));
            contor.AddContourPoint(new ContourPoint(c, null));
            contor.AddContourPoint(new ContourPoint(d, null));

            curPlate.Contour = contor;
            curPlate.Profile = pf;
            curPlate.Insert();

        }

        private void CreatBeam(TSG.Point a, TSG.Point f, string v, Position.DepthEnum depth)
        {
            Beam curBeam = new Beam(a, f);
            curBeam.Profile.ProfileString = v;
            curBeam.Position.Depth = depth;
            curBeam.Position.Rotation = Position.RotationEnum.TOP;
            curBeam.Insert();
        }

        private void CreatBeam(TSG.Point a, TSG.Point f, string v,Position.RotationEnum rot)
        {
            Beam curBeam = new Beam(a, f);
            curBeam.Profile.ProfileString = v;
            curBeam.Position.Depth = Position.DepthEnum.MIDDLE;
            curBeam.Position.Rotation = rot;
            curBeam.Insert();
        }

       

        Tuple<string, string> GetGridXString()
        {
            List<int> res = new List<int>();
            int x0 =-HalfLength;
            res.Add(x0);

            int i = 1;
            while (i<=NumList.Sum())
            {
                if (i <= NumSide)
                {
                    x0 += SizeSide;

                }
                else
                {
                    x0 += SizeMiddle;
                }
                if (i == NumList[0] || i ==(NumList[0] + NumList[1])||i==(NumSide+NumMiddle)/2)
                {
                    res.Add(x0);
                }
                i++;
            }
            var aa = res.ToList();
            foreach (int item in aa)
            {
                res.Add(-item);
            }
            res.Add(0);
            res.Sort();

            var tmp2 = new List<int>();
            for (int ii = 0; ii < res.Count; ii++)
            {
                if (ii==0)
                {
                    tmp2.Add(res[ii]);
                }
                else
                {
                    tmp2.Add(res[ii] - res[ii - 1]);
                }
            }
            var strres = from a in tmp2 select a.ToString();




            string s1 = string.Join(" ", strres.ToArray());
            string s2 = string.Join(" ", Enumerable.Range(0, (int)strres.LongCount() ));
            return new Tuple<string, string>(s1, s2);
        }

    }
}
