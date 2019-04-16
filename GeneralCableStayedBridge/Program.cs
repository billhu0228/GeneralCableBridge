using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSG = Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Catalogs;

namespace GeneralCableStayedBridge
{
    class Program
    {
        static void Main(string[] args)
        {
            Model myModel = new Model();
            

            // 材料 & 断面


            Bridge BPJ = new Bridge();
            BPJ.ModifyGrid(ref myModel);
            BPJ.GenerateCrossTruss(ref myModel);
            BPJ.GenerateTower(ref myModel);

            //BPJ.GenerateCable(ref myModel);


            //Bridge.ExportIFC("WorkMODEL");

            //Beam myBeam = new Beam(new TSG.Point(0, 1000, 1000), new TSG.Point(0, 6000, 1000));
            //myBeam.Material.MaterialString = "S235JR";
            //myBeam.Profile.ProfileString = "WI400-15-20*300";            
            //myBeam.Insert();
            //myModel.CommitChanges();
        }
    }
}
