﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Karamba;

namespace PTK
{
    public class PTK_2_1_Loads : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public PTK_2_1_Loads()
            : base("Loads (PTK)", "Loads",
                "Add loads here",
                CommonProps.category, CommonProps.subcat4)
        {
            Message = CommonProps.initialMessage;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddTextParameter("tag", "tag", "tag", GH_ParamAccess.item,"0");      //We should add default values here.
            pManager.AddIntegerParameter("Load Case", "LC", "Load case", GH_ParamAccess.item, 0);    //We should add default values here.
            pManager.AddPointParameter("Point Load", "pt", "Point to which load will be assigned", GH_ParamAccess.item, new Point3d() );
            pManager.AddVectorParameter("Vector Load","load vec","in [kN]. Vector which describe the diretion and value in kN", GH_ParamAccess.item, new Vector3d());
            pManager.AddVectorParameter("Vector Moment Load", "load vec", "in [kN]. Vector which describe the diretion and value in kN", GH_ParamAccess.item, new Vector3d());
            //The input was listed, but if the length of the list is wrong, a fatal error may occur, so we fixed it to an item.

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            // pManager[3].Optional = true;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Load", "L (PTK)", "Load data to be send to Assembler(PTK)", GH_ParamAccess.item);
            pManager.RegisterParam(new Karamba.Loads.Param_Load(), "Support", "supp", "Ouput support(s)");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            string Tag = "N/A";
            int lcase = 0;
            Point3d lpoint = new Point3d();
            Vector3d lfvector = new Vector3d();
            Vector3d lmvecotr = new Vector3d();
            #endregion

            #region input
            DA.GetData(0, ref Tag);
            if (!DA.GetData(1, ref lcase)) { return; }
            if (!DA.GetData(2, ref lpoint)) { return; }
            if (!DA.GetData(3, ref lfvector)) { return; }
            if (!DA.GetData(4, ref lmvecotr)) { return; }
            #endregion

            #region solve
            //Loads loads = new Loads(Tag,lpoint,lvector);
            var load = new Karamba.Loads.PointLoad(lpoint, lfvector, lmvecotr, lcase, true);
            var kload = new Karamba.Loads.GH_Load(load);

            #endregion

            #region output
            DA.SetData(0, kload);   //NotUse
            DA.SetData(1, kload);
            #endregion

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
            //You can add image files to your project resources and access them like this:
            // return Resources.IconForThisComponent;
            return PTK.Properties.Resources.ico_load;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("965bef7b-feea-46d1-abe9-f686d28b9c41"); }
        }
    }



}
