﻿using System;
using Krypton.Workspace;

namespace Krypton.Docking
{
    /// <summary>
    /// Event arguments for a DockableWorkspaceCellAdding/DockableWorkspaceCellRemoving events.
    /// </summary>
    public class DockableWorkspaceCellEventArgs : EventArgs
    {
        #region Instance Fields

        #endregion
        
        #region Identity
        /// <summary>
        /// Initialize a new instance of the DockableWorkspaceCellEventArgs class.
        /// </summary>
        /// <param name="workspace">Reference to existing dockable workspace control instance.</param>
        /// <param name="element">Reference to docking workspace element that is managing the dockable workspace control.</param>
        /// <param name="cell">Reference to workspace control cell instance.</param>
        public DockableWorkspaceCellEventArgs(KryptonDockableWorkspace workspace,
                                              KryptonDockingWorkspace element,
                                              KryptonWorkspaceCell cell)
        {
            DockableWorkspaceControl = workspace;
            WorkspaceElement = element;
            CellControl = cell;
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets a reference to the KryptonDockableWorkspace that contains the cell.
        /// </summary>
        public KryptonDockableWorkspace DockableWorkspaceControl { get; }

        /// <summary>
        /// Gets a reference to the KryptonDockingWorkspace that is managing the dockable workspace.
        /// </summary>
        public KryptonDockingWorkspace WorkspaceElement { get; }

        /// <summary>
        /// Gets a reference to the KryptonWorkspaceCell control.
        /// </summary>
        public KryptonWorkspaceCell CellControl { get; }

        #endregion
    }
}
