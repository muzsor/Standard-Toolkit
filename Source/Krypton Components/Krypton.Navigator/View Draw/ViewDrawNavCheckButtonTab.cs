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

using Krypton.Toolkit;

namespace Krypton.Navigator
{
    /// <summary>
    /// Navigator view element for drawing a tab check button for a krypton page.
    /// </summary>
    internal class ViewDrawNavCheckButtonTab : ViewDrawNavCheckButtonBar
    {
        #region Identity
        /// <summary>
        /// Initialize a new instance of the ViewDrawNavCheckButtonTab class.
        /// </summary>
        /// <param name="navigator">Owning navigator instance.</param>
        /// <param name="page">Page this check button represents.</param>
        /// <param name="orientation">Orientation for the check button.</param>
        public ViewDrawNavCheckButtonTab(KryptonNavigator navigator,
                                         KryptonPage page,
                                         VisualOrientation orientation)
            : base(navigator, page, orientation,
                   page.StateDisabled.Tab, 
                   page.StateNormal.Tab,
                   page.StateTracking.Tab, 
                   page.StatePressed.Tab,
                   page.StateSelected.Tab,
                   page.OverrideFocus.Tab)
        {
        }

        /// <summary>
        /// Obtains the String representation of this instance.
        /// </summary>
        /// <returns>User readable name of the instance.</returns>
        public override string ToString()
        {
            // Return the class name and instance identifier
            return "ViewDrawNavCheckButtonTab:" + Id;
        }
        #endregion

        #region UpdateButtonSpecMapping
        /// <summary>
        /// Update the button spec manager mapping to reflect current settings.
        /// </summary>
        public override void UpdateButtonSpecMapping()
        {
            // Update the button spec manager for this tab to use a tab style for remapping
            ButtonSpecManager.SetRemapTarget(Navigator.Bar.TabStyle);
            ButtonSpecManager.RecreateButtons();
        }
        #endregion

        #region ButtonClickOnDown
        /// <summary>
        /// Should the item be selected on the mouse down.
        /// </summary>
        protected override bool ButtonClickOnDown => true;

        #endregion
    }
}
