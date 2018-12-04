namespace TwentyFirst.Data.Models
{
    public class ImageEditor
    {
        public string ImageId { get; set; }
        public virtual Image Image { get; set; }

        public string EditorId { get; set; }
        public virtual User Editor { get; set; }
    }
}
