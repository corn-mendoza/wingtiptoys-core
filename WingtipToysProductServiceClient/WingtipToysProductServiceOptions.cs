using System;

namespace WingtipToys.Client
{
    public class WingtipToysProductServiceOptions
    {
        public string Scheme { get; set; } = "http";
        public string Address { get; set; }

        public string GetProductPath { get; set; }

        public string GetProductsPath { get; set; }

        public string GetAllProductsPath { get; set; }

        public string GetProductURL
        {
            get
            {
                return MakeUrl(GetProductPath);
            }
        }
        public string GetAllProductsURL
        {
            get
            {
                return MakeUrl(GetAllProductsPath);
            }
        }
        public string GetProductsURL
        {
            get
            {
                return MakeUrl(GetProductsPath);
            }
        }

        private string MakeUrl(string path)
        {
            return Scheme + "://" + Address + "/" + path;
        }

    }
}
