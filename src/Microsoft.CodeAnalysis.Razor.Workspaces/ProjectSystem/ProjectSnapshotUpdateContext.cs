﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem
{
    internal class ProjectSnapshotUpdateContext
    {
        public ProjectSnapshotUpdateContext(Project underlyingProject)
        {
            if (underlyingProject == null)
            {
                throw new ArgumentNullException(nameof(underlyingProject));
            }

            UnderlyingProject = underlyingProject;
        }

        public Project UnderlyingProject { get; }

        public ProjectExtensibilityConfiguration Configuration { get; set; }
    }
}
