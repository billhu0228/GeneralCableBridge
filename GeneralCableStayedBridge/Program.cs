using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSG = Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace GeneralCableStayedBridge
{
    class Program
    {
        static void Main(string[] args)
        {
            Model myModel = new Model();

            // 材料



            Bridge BPJ = new Bridge();
            BPJ.GenerateCrossTruss(ref myModel);

   

            //Beam myBeam = new Beam(new TSG.Point(0, 1000, 1000), new TSG.Point(0, 6000, 1000));
            //myBeam.Material.MaterialString = "S235JR";
            //myBeam.Profile.ProfileString = "WI400-15-20*300";            
            //myBeam.Insert();
            //myModel.CommitChanges();
        }
    }
}
