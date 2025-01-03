﻿using System;
using System.Collections.Generic;

namespace respNewsV8.Models;

public partial class News
{
    public int NewsId { get; set; }

    public string? NewsTitle { get; set; }

    public string? NewsContetText { get; set; }

    public DateTime? NewsDate { get; set; }

    public int? NewsCategoryId { get; set; }

    public int? NewsLangId { get; set; }

    public int? NewsRating { get; set; }

    public bool? NewsStatus { get; set; }

    public bool? NewsVisibility { get; set; }

    public DateTime? NewsUpdateDate { get; set; }

    public string? NewsYoutubeLink { get; set; }

    public int? NewsOwnerId { get; set; }

    public int? NewsViewCount { get; set; }

    public int? NewsAdminId { get; set; }

    public string? NewsTags { get; set; }

    public virtual User? NewsAdmin { get; set; }

    public virtual Category? NewsCategory { get; set; }

    public virtual Language? NewsLang { get; set; }

    public virtual Owner? NewsOwner { get; set; }

    public virtual ICollection<NewsPhoto> NewsPhotos { get; set; } = new List<NewsPhoto>();

    public virtual ICollection<NewsVideo> NewsVideos { get; set; } = new List<NewsVideo>();
}
