using System;
using System.Collections.Generic;
using System.Text;
using LiteCMS.Entity;

namespace LiteCMS.Plugin
{
    public interface IArticleCollectionProvider
    {
        Dictionary<string, List<ArticleInfo>> GetAllArticleList();
    }
}
