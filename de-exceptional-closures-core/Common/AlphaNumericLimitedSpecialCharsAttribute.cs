﻿using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace de_exceptional_closures_core.Common
{
    public class AlphaNumericLimitedSpecialCharsAttribute : ValidationAttribute
    {
        private readonly Regex _regex;

        public AlphaNumericLimitedSpecialCharsAttribute()
        {
            _regex = new Regex("^[\\p{L}\\p{N}\\x20\\-\\'\\,\\.\\/\\?\\!\\£\\&]*$");

            base.ErrorMessage = "Only alpha-numeric characters, spaces, hyphens, comma, full stop, question mark, exclamation mark, pound sign, ampersand, forward slash and apostrophes are permitted";
        }

        public override bool IsValid(object value)
        {
            var stringValue = value as string;

            if (string.IsNullOrWhiteSpace(stringValue)) return true;

            return _regex.IsMatch(stringValue);
        }
    }
}