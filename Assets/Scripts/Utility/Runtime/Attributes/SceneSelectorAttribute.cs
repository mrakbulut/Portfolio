using System;

namespace Portfolio.Utility
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SceneSelectorAttribute : Attribute
    {
        public bool IncludeNone { get; private set; }
        public bool Locatable { get; private set; }
        public string FolderPath { get; private set; }

        public SceneSelectorAttribute(string folderPath = "", bool includeNone = true, bool locatable = true)
        {
            IncludeNone = includeNone;
            Locatable = locatable;
            FolderPath = folderPath;
        }
    }
}
