namespace Materialise.InTouch.BLL.Configs
{
    public class FullscreenBatchConfig
    {
        public virtual int FullscreenBatchSize { get; set; }
        public virtual int FullscreenMaxImagePerPostNumber { get; set; }
        public virtual int ImagePostPartDurationInSeconds { get; set; }
        public virtual int TitlePostPartDurationInSeconds { get; set; }
    }
}