﻿using System.ComponentModel;

namespace Krypton.Toolkit
{
    /// <summary>
    /// Storage for palette form states.
    /// </summary>
    public class KryptonPaletteForm : KryptonPaletteDouble3
    {
        #region Identity
        /// <summary>
        /// Initialize a new instance of the KryptonPaletteForm class.
        /// </summary>
        /// <param name="redirect">Redirector to inherit values from.</param>
        /// <param name="backStyle">Background style.</param>
        /// <param name="borderStyle">Border style.</param>
        /// <param name="needPaint">Delegate for notifying paint requests.</param>
        public KryptonPaletteForm(PaletteRedirect redirect,
                                  PaletteBackStyle backStyle,
                                  PaletteBorderStyle borderStyle,
                                  NeedPaintHandler needPaint) 
            : base(redirect, backStyle, borderStyle, needPaint)
        {
        }
        #endregion

        #region StateCommon
        /// <summary>
        /// Gets access to the common control appearance that other states can override.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining common control appearance that other states can override.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteDoubleRedirect StateCommon => _stateCommon;

        private bool ShouldSerializeStateCommon()
        {
            return !_stateCommon.IsDefault;
        }
        #endregion
    
        #region StateInactive
        /// <summary>
        /// Gets access to the inactive form appearance entries.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining inactive form appearance.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteDouble StateInactive => _stateDisabled;

        private bool ShouldSerializeStateInactive()
        {
            return !_stateDisabled.IsDefault;
        }
        #endregion

        #region StateActive
        /// <summary>
        /// Gets access to the active form appearance entries.
        /// </summary>
        [KryptonPersist]
        [Category("Visuals")]
        [Description("Overrides for defining active form appearance.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PaletteDouble StateActive => _stateNormal;

        private bool ShouldSerializeStateActive()
        {
            return !_stateNormal.IsDefault;
        }
        #endregion
    }
}
