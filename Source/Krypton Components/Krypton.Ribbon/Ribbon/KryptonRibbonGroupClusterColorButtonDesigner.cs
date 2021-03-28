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

using System;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Diagnostics;
using Krypton.Toolkit;

namespace Krypton.Ribbon
{
    internal class KryptonRibbonGroupClusterColorButtonDesigner : ComponentDesigner
    {
        #region Instance Fields
        private IDesignerHost _designerHost;
        private IComponentChangeService _changeService;
        private KryptonRibbonGroupClusterColorButton _ribbonColorButton;
        private DesignerVerbCollection _verbs;
        private DesignerVerb _toggleHelpersVerb;
        private DesignerVerb _moveFirstVerb;
        private DesignerVerb _movePrevVerb;
        private DesignerVerb _moveNextVerb;
        private DesignerVerb _moveLastVerb;
        private DesignerVerb _deleteButtonVerb;
        private ContextMenuStrip _cms;
        private ToolStripMenuItem _toggleHelpersMenu;
        private ToolStripMenuItem _visibleMenu;
        private ToolStripMenuItem _enabledMenu;
        private ToolStripMenuItem _checkedMenu;
        private ToolStripMenuItem _typeMenu;
        private ToolStripMenuItem _typePushMenu;
        private ToolStripMenuItem _typeCheckMenu;
        private ToolStripMenuItem _typeDropDownMenu;
        private ToolStripMenuItem _typeSplitMenu;
        private ToolStripMenuItem _moveFirstMenu;
        private ToolStripMenuItem _movePreviousMenu;
        private ToolStripMenuItem _moveNextMenu;
        private ToolStripMenuItem _moveLastMenu;
        private ToolStripMenuItem _deleteButtonMenu;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the KryptonRibbonGroupClusterColorButtonDesigner class.
        /// </summary>
        public KryptonRibbonGroupClusterColorButtonDesigner()
        {
        }
        #endregion

        #region Public
        /// <summary>
        /// Initializes the designer with the specified component.
        /// </summary>
        /// <param name="component">The IComponent to associate the designer with.</param>
        public override void Initialize(IComponent component)
        {
            Debug.Assert(component != null);

            // Validate the parameter reference
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            // Let base class do standard stuff
            base.Initialize(component);

            // Cast to correct type
            _ribbonColorButton = (KryptonRibbonGroupClusterColorButton)component;
            _ribbonColorButton.DesignTimeContextMenu += OnContextMenu;

            // Get access to the services
            _designerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
            _changeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            // We need to know when we are being removed/changed
            _changeService.ComponentChanged += OnComponentChanged;
        }

        /// <summary>
        /// Gets the design-time verbs supported by the component that is associated with the designer.
        /// </summary>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                UpdateVerbStatus();
                return _verbs;
            }
        }
        #endregion

        #region Protected
        /// <summary>
        /// Releases all resources used by the component. 
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    // Unhook from events
                    _ribbonColorButton.DesignTimeContextMenu -= OnContextMenu;
                    _changeService.ComponentChanged -= OnComponentChanged;
                }
            }
            finally
            {
                // Must let base class do standard stuff
                base.Dispose(disposing);
            }
        }
        #endregion

        #region Implementation
        private void UpdateVerbStatus()
        {
            // Create verbs first time around
            if (_verbs == null)
            {
                _verbs = new DesignerVerbCollection();
                _toggleHelpersVerb = new DesignerVerb("Toggle Helpers", OnToggleHelpers);
                _moveFirstVerb = new DesignerVerb("Move Cluster Color Button First", OnMoveFirst);
                _movePrevVerb = new DesignerVerb("Move Cluster Color Button Previous", OnMovePrevious);
                _moveNextVerb = new DesignerVerb("Move Cluster Color Button Next", OnMoveNext);
                _moveLastVerb = new DesignerVerb("Move Cluster Color Button Last", OnMoveLast);
                _deleteButtonVerb = new DesignerVerb("Delete Cluster Color Button", OnDeleteButton);
                _verbs.AddRange(new DesignerVerb[] { _toggleHelpersVerb, _moveFirstVerb, _movePrevVerb, 
                                                         _moveNextVerb, _moveLastVerb, _deleteButtonVerb });
            }

            bool moveFirst = false;
            bool movePrev = false;
            bool moveNext = false;
            bool moveLast = false;

            if (_ribbonColorButton?.Ribbon != null)
            {
                // Cast container to the correct type
                KryptonRibbonGroupCluster cluster = (KryptonRibbonGroupCluster)_ribbonColorButton.RibbonContainer;

                moveFirst = (cluster.Items.IndexOf(_ribbonColorButton) > 0);
                movePrev = (cluster.Items.IndexOf(_ribbonColorButton) > 0);
                moveNext = (cluster.Items.IndexOf(_ribbonColorButton) < (cluster.Items.Count - 1));
                moveLast = (cluster.Items.IndexOf(_ribbonColorButton) < (cluster.Items.Count - 1));
            }

            _moveFirstVerb.Enabled = moveFirst;
            _movePrevVerb.Enabled = movePrev;
            _moveNextVerb.Enabled = moveNext;
            _moveLastVerb.Enabled = moveLast;
        }

        private void OnToggleHelpers(object sender, EventArgs e)
        {
            // Invert the current toggle helper mode
            if (_ribbonColorButton?.Ribbon != null)
            {
                _ribbonColorButton.Ribbon.InDesignHelperMode = !_ribbonColorButton.Ribbon.InDesignHelperMode;
            }
        }

        private void OnMoveFirst(object sender, EventArgs e)
        {
            if (_ribbonColorButton?.Ribbon != null)
            {
                // Cast container to the correct type
                KryptonRibbonGroupCluster cluster = (KryptonRibbonGroupCluster)_ribbonColorButton.RibbonContainer;

                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupColorClusterButton MoveFirst");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(cluster)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Move position of the button
                    cluster.Items.Remove(_ribbonColorButton);
                    cluster.Items.Insert(0, _ribbonColorButton);
                    UpdateVerbStatus();

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnMovePrevious(object sender, EventArgs e)
        {
            if (_ribbonColorButton?.Ribbon != null)
            {
                // Cast container to the correct type
                KryptonRibbonGroupCluster cluster = (KryptonRibbonGroupCluster)_ribbonColorButton.RibbonContainer;

                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupColorClusterButton MovePrevious");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(cluster)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Move position of the triple
                    int index = cluster.Items.IndexOf(_ribbonColorButton) - 1;
                    index = Math.Max(index, 0);
                    cluster.Items.Remove(_ribbonColorButton);
                    cluster.Items.Insert(index, _ribbonColorButton);
                    UpdateVerbStatus();

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnMoveNext(object sender, EventArgs e)
        {
            if (_ribbonColorButton?.Ribbon != null)
            {
                // Cast container to the correct type
                KryptonRibbonGroupCluster cluster = (KryptonRibbonGroupCluster)_ribbonColorButton.RibbonContainer;

                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupColorClusterButton MoveNext");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(cluster)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Move position of the triple
                    int index = cluster.Items.IndexOf(_ribbonColorButton) + 1;
                    index = Math.Min(index, cluster.Items.Count - 1);
                    cluster.Items.Remove(_ribbonColorButton);
                    cluster.Items.Insert(index, _ribbonColorButton);
                    UpdateVerbStatus();

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnMoveLast(object sender, EventArgs e)
        {
            if (_ribbonColorButton?.Ribbon != null)
            {
                // Cast container to the correct type
                KryptonRibbonGroupCluster cluster = (KryptonRibbonGroupCluster)_ribbonColorButton.RibbonContainer;

                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupColorClusterButton MoveLast");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(cluster)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Move position of the triple
                    cluster.Items.Remove(_ribbonColorButton);
                    cluster.Items.Insert(cluster.Items.Count, _ribbonColorButton);
                    UpdateVerbStatus();

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnDeleteButton(object sender, EventArgs e)
        {
            if (_ribbonColorButton?.Ribbon != null)
            {
                // Cast container to the correct type
                KryptonRibbonGroupCluster cluster = (KryptonRibbonGroupCluster)_ribbonColorButton.RibbonContainer;

                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupColorClusterButton DeleteButton");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(cluster)["Items"];

                    // Remove the ribbon group from the ribbon tab
                    RaiseComponentChanging(null);
                    RaiseComponentChanging(propertyItems);

                    // Remove the button from the group
                    cluster.Items.Remove(_ribbonColorButton);

                    // Get designer to destroy it
                    _designerHost.DestroyComponent(_ribbonColorButton);

                    RaiseComponentChanged(propertyItems, null, null);
                    RaiseComponentChanged(null, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnVisible(object sender, EventArgs e)
        {
            if (_ribbonColorButton?.Ribbon != null)
            {
                _changeService.OnComponentChanged(_ribbonColorButton, null, _ribbonColorButton.Visible, !_ribbonColorButton.Visible);
                _ribbonColorButton.Visible = !_ribbonColorButton.Visible;
            }
        }

        private void OnEnabled(object sender, EventArgs e)
        {
            if (_ribbonColorButton?.Ribbon != null)
            {
                _changeService.OnComponentChanged(_ribbonColorButton, null, _ribbonColorButton.Enabled, !_ribbonColorButton.Enabled);
                _ribbonColorButton.Enabled = !_ribbonColorButton.Enabled;
            }
        }

        private void OnChecked(object sender, EventArgs e)
        {
            if (_ribbonColorButton?.Ribbon != null)
            {
                _changeService.OnComponentChanged(_ribbonColorButton, null, _ribbonColorButton.Checked, !_ribbonColorButton.Checked);
                _ribbonColorButton.Checked = !_ribbonColorButton.Checked;
            }
        }

        private void OnTypePush(object sender, EventArgs e)
        {
            if (_ribbonColorButton?.Ribbon != null)
            {
                _changeService.OnComponentChanged(_ribbonColorButton, null, _ribbonColorButton.ButtonType, GroupButtonType.Push);
                _ribbonColorButton.ButtonType = GroupButtonType.Push;
            }
        }

        private void OnTypeCheck(object sender, EventArgs e)
        {
            if (_ribbonColorButton?.Ribbon != null)
            {
                _changeService.OnComponentChanged(_ribbonColorButton, null, _ribbonColorButton.ButtonType, GroupButtonType.Check);
                _ribbonColorButton.ButtonType = GroupButtonType.Check;
            }
        }

        private void OnTypeDropDown(object sender, EventArgs e)
        {
            if (_ribbonColorButton?.Ribbon != null)
            {
                _changeService.OnComponentChanged(_ribbonColorButton, null, _ribbonColorButton.ButtonType, GroupButtonType.DropDown);
                _ribbonColorButton.ButtonType = GroupButtonType.DropDown;
            }
        }

        private void OnTypeSplit(object sender, EventArgs e)
        {
            if (_ribbonColorButton?.Ribbon != null)
            {
                _changeService.OnComponentChanged(_ribbonColorButton, null, _ribbonColorButton.ButtonType, GroupButtonType.Split);
                _ribbonColorButton.ButtonType = GroupButtonType.Split;
            }
        }

        private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
        {
            UpdateVerbStatus();
        }

        private void OnContextMenu(object sender, MouseEventArgs e)
        {
            if (_ribbonColorButton?.Ribbon != null)
            {
                // Create the menu strip the first time around
                if (_cms == null)
                {
                    _cms = new ContextMenuStrip();
                    _toggleHelpersMenu = new ToolStripMenuItem("Design Helpers", null, OnToggleHelpers);
                    _visibleMenu = new ToolStripMenuItem("Visible", null, OnVisible);
                    _enabledMenu = new ToolStripMenuItem("Enabled", null, OnEnabled);
                    _checkedMenu = new ToolStripMenuItem("Checked", null, OnChecked);
                    _typePushMenu = new ToolStripMenuItem("Push", null, OnTypePush);
                    _typeCheckMenu = new ToolStripMenuItem("Check", null, OnTypeCheck);
                    _typeDropDownMenu = new ToolStripMenuItem("DropDown", null, OnTypeDropDown);
                    _typeSplitMenu = new ToolStripMenuItem("Split", null, OnTypeSplit);
                    _typeMenu = new ToolStripMenuItem("Type");
                    _typeMenu.DropDownItems.AddRange(new ToolStripItem[] { _typePushMenu, _typeCheckMenu, _typeDropDownMenu, _typeSplitMenu });
                    _moveFirstMenu = new ToolStripMenuItem("Move Cluster Color Button First", Properties.Resources.MoveFirst, OnMoveFirst);
                    _movePreviousMenu = new ToolStripMenuItem("Move Cluster Color Button Previous", Properties.Resources.MovePrevious, OnMovePrevious);
                    _moveNextMenu = new ToolStripMenuItem("Move Cluster Color Button Next", Properties.Resources.MoveNext, OnMoveNext);
                    _moveLastMenu = new ToolStripMenuItem("Move Cluster Color Button Last", Properties.Resources.MoveLast, OnMoveLast);
                    _deleteButtonMenu = new ToolStripMenuItem("Delete Cluster Color Button", Properties.Resources.delete2, OnDeleteButton);
                    _cms.Items.AddRange(new ToolStripItem[] { _toggleHelpersMenu, new ToolStripSeparator(),
                                                              _visibleMenu, _enabledMenu, _checkedMenu, _typeMenu, new ToolStripSeparator(),
                                                              _moveFirstMenu, _movePreviousMenu, _moveNextMenu, _moveLastMenu, new ToolStripSeparator(),
                                                              _deleteButtonMenu });
                }

                // Update verbs to work out correct enable states
                UpdateVerbStatus();

                // Update menu items state from versb
                _toggleHelpersMenu.Checked = _ribbonColorButton.Ribbon.InDesignHelperMode;
                _visibleMenu.Checked = _ribbonColorButton.Visible;
                _enabledMenu.Checked = _ribbonColorButton.Enabled;
                _checkedMenu.Checked = _ribbonColorButton.Checked;
                _typePushMenu.Checked = (_ribbonColorButton.ButtonType == GroupButtonType.Push);
                _typeCheckMenu.Checked = (_ribbonColorButton.ButtonType == GroupButtonType.Check);
                _typeDropDownMenu.Checked = (_ribbonColorButton.ButtonType == GroupButtonType.DropDown);
                _typeSplitMenu.Checked = (_ribbonColorButton.ButtonType == GroupButtonType.Split);
                _moveFirstMenu.Enabled = _moveFirstVerb.Enabled;
                _movePreviousMenu.Enabled = _movePrevVerb.Enabled;
                _moveNextMenu.Enabled = _moveNextVerb.Enabled;
                _moveLastMenu.Enabled = _moveLastVerb.Enabled;

                // Show the context menu
                if (CommonHelper.ValidContextMenuStrip(_cms))
                {
                    Point screenPt = _ribbonColorButton.Ribbon.ViewRectangleToPoint(_ribbonColorButton.ClusterColorButtonView);
                    VisualPopupManager.Singleton.ShowContextMenuStrip(_cms, screenPt);
                }
            }
        }
        #endregion
    }
}
