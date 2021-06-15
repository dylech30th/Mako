﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using Mako.Model;
using Mako.Net;
using Mako.Net.Response;
using Mako.Util;

namespace Mako.Engines
{
    internal class BookmarkEngine : AbstractPixivFetchEngine<Illustration>
    {
        private readonly string _uid;
        private readonly PrivacyPolicy _privacyPolicy;
        
        public sealed override MakoClient MakoClient { get; set; }

        public BookmarkEngine(
            MakoClient makoClient,
            string uid, 
            PrivacyPolicy privacyPolicy,
            EngineHandle? engineHandle = null) : base(engineHandle)
        {
            _uid = uid;
            _privacyPolicy = privacyPolicy;
            MakoClient = makoClient;
        }

        public override bool Validate(IList<Illustration> list, Illustration? item)
        {
            return item.Satisfies(list, MakoClient.Session);
        }

        public override IAsyncEnumerator<Illustration> GetAsyncEnumerator(CancellationToken cancellationToken = new())
        {
            return new BookmarkAsyncEnumerator(this, MakoClient)!;
        }

        private class BookmarkAsyncEnumerator : RecursivePixivAsyncEnumerator<Illustration, BookmarkResponse, BookmarkEngine>
        {
            public BookmarkAsyncEnumerator(BookmarkEngine pixivFetchEngine, [NotNull] MakoClient makoClient) 
                : base(pixivFetchEngine, MakoApiKind.AppApi, makoClient)
            {
            }
            
            protected override bool ValidateResponse(BookmarkResponse rawEntity)
            {
                return rawEntity.Illusts?.Any() ?? false;
            }

            protected override string? NextUrl()
            {
                return Entity?.NextUrl;
            }

            protected override string InitialUrl()
            {
                return $"/v1/user/bookmarks/illust?user_id={PixivFetchEngine._uid}&restrict={PixivFetchEngine._privacyPolicy.GetDescription().Name}&filter=for_ios";
            }

            protected override IEnumerator<Illustration> GetNewEnumerator()
            {
                return Entity!.Illusts!.SelectNotNull(MakoExtension.ToIllustration).GetEnumerator();
            }
        }
    }
}