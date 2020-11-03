using UnityEngine;
using System;
using System.Collections.Generic;

public class Locale
{
    /*public static void Create(string path)
    {
        try
        {
            if (File.Exists(path))
                File.Delete(path);

            using (FileStream fs = File.Create(path))
            {
                Byte[] content = new UTF8Encoding().GetBytes("Pozdro\nod\nDisa");
                fs.Write(content, 0, content.Length);
            }
        }
        catch (Exception exception)
        {
            Debug.LogError(exception);
        }
    }*/

    public static Dictionary<string, string> Read(string path)
    {
        TextAsset ta = Resources.Load<TextAsset>(path);

        if (!ta)
            return null;

        Dictionary<string, string> loc = new Dictionary<string, string>();
        string[] lines = ta.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        
        foreach (string l in lines)
        {
            if (l.Length > 0)
            {
                string[] s = l.Split(new char[] { '=' }, 2);
                s[1] = s[1].Replace("\\n", "\n");
                loc.Add(s[0], s[1]);
            }
        }

        return loc;
    }
}
