using System;
using System.Collections.Generic;
using System.Text;

namespace Mountify.Utils;

public static class Extensions {
    public static string UpperCaseAfterSpaces(this string s) {
        var stringBuilder = new StringBuilder(s);
        for (var i = 0; i < stringBuilder.Length - 1; i++) {
            if (i == 0) {
                stringBuilder[0] = char.ToUpper(stringBuilder[0]);
            } else if (stringBuilder[i] == ' ') {
                stringBuilder[i + 1] = char.ToUpper(stringBuilder[i + 1]);
            }
        }

        return stringBuilder.ToString();
    }

    public static string LowerCaseWords(this string s, List<String> words) {
        foreach (var word in words) {
            s = s.Replace(" " + word + " ", " " + word.ToLower() + " ");
        }

        return s;
    }

    public static string LowerCaseAfter(this string s, List<Char> chars) {
        var stringBuilder = new StringBuilder(s);
        for (var i = 0; i < stringBuilder.Length - 1; i++) {
            if (chars.Contains(stringBuilder[i])) {
                stringBuilder[i + 1] = char.ToLower(stringBuilder[i + 1]);
            }
        }

        return stringBuilder.ToString();
    }
}
