﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Editor
{
    internal abstract class RazorTextBufferProvider
    {
        public abstract bool TryGetFromDocument(TextDocument document, out ITextBuffer textBuffer);
    }
}
