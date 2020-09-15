﻿// -------------------------------------------------------------------------------------------------
// <copyright file="DefinitionList.xaml.cs" company="Tom Luppi">
//     Copyright (c) Tom Luppi.  All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace LegendsGenerator.Editor
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using LegendsGenerator.Editor.DefinitionSelector;

    /// <summary>
    /// Interaction logic for DefinitionList.
    /// </summary>
    public partial class DefinitionList : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinitionList"/> class.
        /// </summary>
        public DefinitionList()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Fires when a definition is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The arguments.</param>
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is not InheritanceNode item)
            {
                return;
            }

            if (this.DataContext is not Context context)
            {
                throw new InvalidOperationException("DataContext must be Context.");
            }

            if (item.Definition != null)
            {
                context.SelectedDefinition = item.Definition;
            }
        }
    }
}
