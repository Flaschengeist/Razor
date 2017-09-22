// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Xunit;

namespace Microsoft.CodeAnalysis.Razor
{
    public class DefaultRazorIndentationFactsServiceTest
    {
        [Fact]
        public void GetDesiredIndentation_ReturnsNull_IfOwningSpanIsCode()
        {
            // Arrange
            var source = $@"
@{{
";
            var syntaxTree = GetSyntaxTree(source);
            var service = new DefaultRazorIndentationFactsService();

            // Act
            var indentation = service.GetDesiredIndentation(
                syntaxTree,
                previousLineEndIndex: 2 + Environment.NewLine.Length,
                getLineContent: line => GetLineContent(source, line),
                indentSize: 4,
                tabSize: 1);

            // Assert
            Assert.Null(indentation);
        }

        [Fact]
        public void GetDesiredIndentation_ReturnsNull_IfOwningSpanIsNone()
        {
            // Arrange
            var customDirective = DirectiveDescriptor.CreateSingleLineDirective("custom");
            var source = $@"
@custom
";
            var syntaxTree = GetSyntaxTree(source, new[] { customDirective });
            var service = new DefaultRazorIndentationFactsService();

            // Act
            var indentation = service.GetDesiredIndentation(
                syntaxTree,
                previousLineEndIndex: 7 + Environment.NewLine.Length,
                getLineContent: line => GetLineContent(source, line),
                indentSize: 4,
                tabSize: 1);

            // Assert
            Assert.Null(indentation);
        }

        [Fact]
        public void GetDesiredIndentation_ReturnsCorrectIndentation_ForMarkupWithinCodeBlock()
        {
            // Arrange
            var customDirective = DirectiveDescriptor.CreateSingleLineDirective("custom");
            var source = $@"@{{
    <div>
";
            var syntaxTree = GetSyntaxTree(source, new[] { customDirective });
            var service = new DefaultRazorIndentationFactsService();

            // Act
            var indentation = service.GetDesiredIndentation(
                syntaxTree,
                previousLineEndIndex: 11 + Environment.NewLine.Length,
                getLineContent: line => GetLineContent(source, line),
                indentSize: 4,
                tabSize: 4);

            // Assert
            Assert.Equal(4, indentation);
        }

        private static RazorSyntaxTree GetSyntaxTree(string source, IEnumerable<DirectiveDescriptor> directives = null)
        {
            directives = directives ?? Enumerable.Empty<DirectiveDescriptor>();
            var engine = RazorEngine.CreateDesignTime(builder =>
            {
                foreach (var directive in directives)
                {
                    builder.AddDirective(directive);
                }
            });

            var sourceDocument = RazorSourceDocument.Create(source, "test.cshtml");
            var codeDocument = RazorCodeDocument.Create(sourceDocument);

            engine.Process(codeDocument);

            return codeDocument.GetSyntaxTree();
        }

        private string GetLineContent(string source, int lineIndex)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            var lines = source.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            return lines[lineIndex];
        }
    }
}
