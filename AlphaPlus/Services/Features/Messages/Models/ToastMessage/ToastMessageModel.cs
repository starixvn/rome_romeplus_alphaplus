namespace AlphaPlus.Features.Messages.API.Models.ToastMessage
{
    using System;
    using Newtonsoft.Json;

    public class ToastMessageModel
    {
        public int ToastId { get; set; }

        public int TypeId { get; set; }

        public int SiteFunctionId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedTime { get; set; }

        public string CreatedTimeStr => CreatedTime.ToLongDateString() + " " + CreatedTime.ToLongTimeString();

        public string Title
        {
            get
            {
                var content = JsonConvert.DeserializeObject<ToastContentModel>(Content) ?? new ToastContentModel();

                return content.Title;
            }
        }

        public string Body
        {
            get
            {
                var content = JsonConvert.DeserializeObject<ToastContentModel>(Content) ?? new ToastContentModel();

                return content.Body;
            }
        }
    }
}
