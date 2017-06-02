using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using StoreWeb.Models;

namespace StoreWeb.HtmlHelpers
{
    public static class Helpers
    {
        public static HtmlString PageLinks(PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            var builder = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tagBuilder = new TagBuilder("a");
                tagBuilder.MergeAttribute("href", pageUrl(i));
                var htmlContentBuilder = tagBuilder.InnerHtml.Append(i.ToString());

                if (i == pagingInfo.CurrentPage)
                {
                    tagBuilder.AddCssClass("selected");
                }
                StringWriter sw = new StringWriter();
                tagBuilder.WriteTo(sw, HtmlEncoder.Default);
                builder.Append(sw);
            }

            return  new HtmlString(builder.ToString());
        }

        public static Uri GetUri(HttpRequest request)
        {
            //var builder = new UriBuilder();
            //builder.Scheme = request.Scheme;
            //builder.Host = request.Host.Value;
            //builder.Path = request.Path;
            //builder.Query = request.QueryString.ToUriComponent();
            //return builder.Uri;
            return new Uri(request.Scheme + "://" + request.Host.Value + request.Path.Value+request.QueryString.Value);
        }
    }
}

