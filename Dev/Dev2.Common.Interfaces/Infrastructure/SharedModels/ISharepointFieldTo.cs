namespace Dev2.Common.Interfaces.Infrastructure.SharedModels
{
    public interface ISharepointFieldTo
    {
        string Name { get; set; }
        string InternalName { get; set; }
        SharepointFieldType Type { get; set; }
        int MinLength { get; set; }
        int MaxLength { get; set; }
        bool IsRequired { get; set; }
    }

    public enum SharepointFieldType
    {
        Boolean,
        Currency,
        DateTime,
        Integer,
        Number,
        Text,
        Note
    }
}