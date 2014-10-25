using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using JCIEstimate.Models;

namespace JCIExtensions
{
    public static class MCVExtensions
    {
        public static List<SelectListItem> ToSelectList<T>(
            this IEnumerable<T> enumerable,
            Func<T, string> text,
            Func<T, string> value,
            string defaultOption)
        {
            var items = enumerable.Select(f => new SelectListItem()
            {
                Text = text(f),
                Value = value(f)
            }).ToList();
            items.Insert(0, new SelectListItem()
            {
                Text = "-- Choose --",
                Value = "-1"
            });
            return items;
        }

        public static Guid getSessionProject()
        {
            Guid sessionProject;

            sessionProject = Guid.Empty;

            if (HttpContext.Current.Session["projectUid"] != null)
            {
                sessionProject = new System.Guid(HttpContext.Current.Session["projectUid"].ToString());
            }
            else
            {
                HttpContext.Current.Response.Redirect("~/Home/Index");
            }

            return sessionProject;
        }
    }
}