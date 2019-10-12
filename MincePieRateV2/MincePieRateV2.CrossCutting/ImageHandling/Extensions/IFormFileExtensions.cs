using Microsoft.AspNetCore.Http;
using MincePieRateV2.CrossCutting.ImageHandling.Constants;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace MincePieRateV2.CrossCutting.ImageHandling.Extensions
{
    public static class IFormFileExtensions
    {
        public const int ImageMinimumBytes = 512;

        //https://stackoverflow.com/a/51879881
        public static bool IsImage(this IFormFile postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != FileContentTypes.jpg &&
                        postedFile.ContentType.ToLower() != FileContentTypes.jpeg &&
                        postedFile.ContentType.ToLower() != FileContentTypes.pjpeg &&
                        postedFile.ContentType.ToLower() != FileContentTypes.gif &&
                        postedFile.ContentType.ToLower() != FileContentTypes.xpng &&
                        postedFile.ContentType.ToLower() != FileContentTypes.png)
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != FileExtensions.jpg
                && Path.GetExtension(postedFile.FileName).ToLower() != FileExtensions.png
                && Path.GetExtension(postedFile.FileName).ToLower() != FileExtensions.gif
                && Path.GetExtension(postedFile.FileName).ToLower() != FileExtensions.jpeg)
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return false;
                }
                //------------------------------------------
                //check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (postedFile.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                postedFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new Bitmap(postedFile.OpenReadStream()))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                postedFile.OpenReadStream().Position = 0;
            }

            return true;
        }
    }
}
