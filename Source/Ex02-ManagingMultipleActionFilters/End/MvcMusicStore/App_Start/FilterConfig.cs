﻿namespace MvcMusicStore
{    
    using System.Web.Mvc;
    using MvcMusicStore.Filters;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MyNewCustomActionFilter());
        }
    }
}