namespace Gu.SerializationAsserts.Newtonsoft.Json
{
    /// <summary>Specifies how comparison is performed.</summary>
    public enum JsonAssertOptions
    {
        /// <summary>Allows formatting such as indentation to differ.</summary>
        Default,

        /// <summary>Requires an exact match of the json.</summary>
        Verbatim
    }
}