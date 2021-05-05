using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RaNetCore.Web.ViewModels.Interfaces;

namespace RaNetCore.Web.BaseControllers.Helpers
{
    public static class RaHelpers
    {
        public static TDetails DeserializeJson<TDetails>(JObject model) => JsonConvert.DeserializeObject<TDetails>(model.ToString());

        public static void UploadImages<TDetails>(
            TDetails viewModel, Func<string, string, string, string> uploadFunc, string folderName = "")
        {
            // Get current model type's properties
            PropertyInfo[] modelProps = typeof(TDetails).GetProperties();

            // Filter only IRaImage collection properties
            IEnumerable<PropertyInfo> imageCollectionsProperties = modelProps
                .Where(x => typeof(IEnumerable<IRaImage>)
                .IsAssignableFrom(x.PropertyType));

            foreach (PropertyInfo collectionProp in imageCollectionsProperties)
            {
                // Extract property with images to upload
                IEnumerable<IRaImage> images = ((IEnumerable<IRaImage>)collectionProp.GetValue(viewModel));

                // Current collection is empty
                if (images is null)
                    continue;

                foreach (var image in images)
                {
                    if (!String.IsNullOrEmpty(image.Base64String))
                    {
                        // Extract base64 string only 
                        int commaIndex = image.Base64String.IndexOf(",");
                        string base64Only = image.Base64String.Remove(0, commaIndex + 1);

                        // Upload and assign link
                        image.Url = uploadFunc(image.Title, folderName, base64Only);
                    }
                }

                // Update the propery value
                collectionProp.SetValue(viewModel, images);
            }
        }
    }
}
