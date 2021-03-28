﻿using System.ComponentModel.Design;

namespace Krypton.Toolkit
{
    internal class KryptonCommandDesigner : ComponentDesigner
    {
        #region Public Overrides
        /// <summary>
        ///  Gets the design-time action lists supported by the component associated with the designer.
        /// </summary>
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                // Create a collection of action lists
                DesignerActionListCollection actionLists = new DesignerActionListCollection
                {

                    // Add the palette specific list
                    new KryptonCommandActionList(this)
                };

                return actionLists;
            }
        }
        #endregion
    }
}
