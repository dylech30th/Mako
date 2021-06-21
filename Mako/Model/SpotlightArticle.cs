﻿using System;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Mako.Model
{
    [PublicAPI]
    public record SpotlightArticle
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("pure_title")]
        public string? PureTitle { get; set; }

        [JsonPropertyName("thumbnail")]
        public string? Thumbnail { get; set; }

        [JsonPropertyName("article_url")]
        public string? ArticleUrl { get; set; }

        [JsonPropertyName("publish_date")]
        public DateTimeOffset PublishDate { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("subcategory_label")]
        public string? SubcategoryLabel { get; set; }
    }
}