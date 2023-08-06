using System.Collections;
using UnityEngine;
using System.Text;
using System;

public static class ExtensionMethods {
	// Methods
	#region Methods
	public static string RemoveDiacritics(this string input)
	{
		string stFormD = input.Normalize(NormalizationForm.FormD);
		int len = stFormD.Length;
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < len; i++)
		{
			System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[i]);
			if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
			{
				sb.Append(stFormD[i]);
			}
		}
		return (sb.ToString().Normalize(NormalizationForm.FormC));
	}

    public static string Replace(this string s, char[] separators, string newVal)
    {
        string[] temp;

        temp = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        return String.Join(newVal, temp);
    }

    public static T Check<T>(this Transform t) where T : Component
    {
        T component = t.GetComponent<T>();
        if (component == null)
        {
            Debug.LogError("Search for component type " + typeof(T) + " yielded no results", t);
        }
        return component;
    }
	#endregion
}
