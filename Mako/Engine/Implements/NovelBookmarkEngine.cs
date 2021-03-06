﻿#region Copyright (c) Pixeval/Mako

// MIT License
// 
// Copyright (c) Pixeval 2021 Mako/NovelBookmarkEngine.cs
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

#endregion

using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Mako.Global.Enum;
using Mako.Model;
using Mako.Net;
using Mako.Util;

namespace Mako.Engine.Implements
{
    public class NovelBookmarkEngine : AbstractPixivFetchEngine<Novel>
    {
        private readonly PrivacyPolicy _privacyPolicy;
        private readonly TargetFilter _targetFilter;
        private readonly string _uid;

        public NovelBookmarkEngine(
            [NotNull] MakoClient makoClient,
            string uid,
            PrivacyPolicy privacyPolicy,
            TargetFilter targetFilter,
            EngineHandle? engineHandle) : base(makoClient, engineHandle)
        {
            _uid = uid;
            _privacyPolicy = privacyPolicy;
            _targetFilter = targetFilter;
        }

        public override IAsyncEnumerator<Novel> GetAsyncEnumerator(CancellationToken cancellationToken = new())
        {
            return RecursivePixivAsyncEnumerators.Novel<NovelBookmarkEngine>.WithInitialUrl(this, MakoApiKind.AppApi,
                engine => "/v1/user/bookmarks/novel"
                          + $"?user_id={engine._uid}"
                          + $"&restrict={engine._privacyPolicy.GetDescription()}"
                          + $"&filter={engine._targetFilter.GetDescription()}")!;
        }
    }
}