using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace LusieyBackgroundService.Models
{
    public sealed class Attached
    {
        private string urlProp;
        private string MediaTypeProp;
        public string url
        {
            get { return urlProp; }
            set
            {
                if (string.IsNullOrEmpty(value) == false
                    && string.IsNullOrWhiteSpace(value) == false
                    && value.Contains(".") == true)
                    urlProp = value;
                else
                    throw new ArgumentException("Url can not be empty or check if extension is included: ", nameof(urlProp) + "=>" + value);
            }
        }
        public string MediaType
        {
            get { return MediaTypeProp; }
            set
            {
                var temp = this.GetExtension(urlProp);
                if (MediaType == null)
                    MediaTypeProp = "";
                MediaTypeProp = temp;
            }
        }
        private string GetExtension(string file)
        {
            if (file.Contains(".") == false)
                return null;
            var returnVal = file.Substring(file.LastIndexOf(".") + 1);
            if (returnVal.ToLower() == "txt")
                return MediaTypeNames.Text.Plain;
            if (returnVal.ToLower() == "rtf")
                return MediaTypeNames.Text.RichText;
            if (returnVal.ToLower() == "html" || returnVal.ToLower() == "htm")
                return MediaTypeNames.Text.Html;
            if (returnVal.ToLower() == "jpg" || returnVal.ToLower() == "jpeg")
                return MediaTypeNames.Image.Jpeg;
            if (returnVal.ToLower() == "gif")
                return MediaTypeNames.Image.Gif;
            if (returnVal.ToLower() == "tiff")
                return MediaTypeNames.Image.Tiff;
            return null;
        }
        public Attached(string url)
        {
            this.url = url;
        }
    }
}
