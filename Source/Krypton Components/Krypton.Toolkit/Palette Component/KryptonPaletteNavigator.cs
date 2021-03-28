﻿#region BSD License
/*
 * 
 * Original BSD 3-Clause License (https://github.com/ComponentFactory/Krypton/blob/master/LICENSE)
 *  © Component Factory Pty Ltd, 2006 - 2016, All rights reserved.
 * 
 *  New BSD 3-Clause License (https://github.com/Krypton-Suite/Standard-Toolkit/blob/master/LICENSE)
 *  Modifications by Peter Wagner(aka Wagnerp) & Simon Coghlan(aka Smurf-IV), et al. 2017 - 2021. All rights reserved. 
 *  Version 6.0.0  
 *
 */
#endregion

using System.ComponentModel;
using System.Diagnostics;

namespace Krypton.Toolkit
{
    /// <summary>
    /// Storage for palette navigator states.
    /// </summary>
    public class KryptonPaletteNavigator : Storage
    {
        #region Instance Fields

        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the KryptonPaletteNavigator class.
        /// </summary>
        /// <param name="redirect">Inheritence redirection instance.</param>
        /// <param name="needPaint">Delegate for notifying paint requests.</param>
        public KryptonPaletteNavigator(PaletteRedirect redirect,
                                       NeedPaintHandler needPaint)
        {
            Debug.Assert(redirect != null);
            
            // Create the storage objects
            StateCommon = new KryptonPaletteNavigatorState(redirect, needPaint);
        }
        #endregion

        #region IsDefault
        /// <summary>
        /// Gets a value indicating if all values are default.
        /// </summary>
        [Browsable(false)]
        public override bool IsDefault => StateCommon.IsDefault;

        #endregion

        #region PopulateFromBase
        /// <summary>
        /// Populate values from the base palette.
        /// </summary>
        public void PopulateFromBase()
        {
            StateCommon.PopulateFromBase();
        }
        #endregion

        #region StateCommon
        /// <summary>
        /// Gets access to the common navigator appearance entries.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining common navigator appearance entries.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public KryptonPaletteNavigatorState StateCommon { get; }

        private bool ShouldSerializeStateCommon()
        {
            return !StateCommon.IsDefault;
        }
        #endregion
    }
}
