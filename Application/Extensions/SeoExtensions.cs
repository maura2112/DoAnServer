using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static partial class SeoExtensions
    {
        public static UrlRecord CreateUrlRecordAsync<T>(this T entity, string alias, string seoName)
            where T : BaseEntity
        {
            var url = new UrlRecord()
            {
                EntityId = entity.Id,
                EntityName= entity.GetEntityName(),
                Slug = GenerateSlug(seoName),
                IsActive = true
            };
            return url;
        }
        public static string GenerateSlug(string input)
        {
            string slug = RemoveDiacritics(input);

            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "", RegexOptions.IgnoreCase);
            slug = slug.ToLower().Replace(" ", "-");

            slug = Regex.Replace(slug, @"-+", "-");

            slug = slug.Trim('-');

            return slug;
        }
        public static string RemoveDiacritics(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(normalizedString[i]);
                }
            }

            return stringBuilder.ToString();
        }
    }
}
