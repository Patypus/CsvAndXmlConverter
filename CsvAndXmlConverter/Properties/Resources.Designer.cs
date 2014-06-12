﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CsvAndXmlConverter.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CsvAndXmlConverter.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to find direcotry in the path &apos;{0}&apos;.
        /// </summary>
        internal static string DirectoryNotFoundMessage {
            get {
                return ResourceManager.GetString("DirectoryNotFoundMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Converted file created at {0}.
        /// </summary>
        internal static string FileCreated {
            get {
                return ResourceManager.GetString("FileCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to find file &apos;{0}&apos;.
        /// </summary>
        internal static string FileNotFoundMessage {
            get {
                return ResourceManager.GetString("FileNotFoundMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to open file: {0}.
        /// </summary>
        internal static string GenericUnableToOpenFile {
            get {
                return ResourceManager.GetString("GenericUnableToOpenFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to save to directory path {0} as permission has been denied.
        /// </summary>
        internal static string UnableToSaveFilePermissionDenied {
            get {
                return ResourceManager.GetString("UnableToSaveFilePermissionDenied", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to save as the dirctory path {0} does not exist..
        /// </summary>
        internal static string UnableToSaveToNoExistantDirectory {
            get {
                return ResourceManager.GetString("UnableToSaveToNoExistantDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This program is invoked with a single argument: a path to an existing file to change its data&apos;s format. Specify a csv file to convert it to xml or specify an xml file to convert it to a csv format. The converted file will be placed at the same location as the file specified with the same name but a different extension..
        /// </summary>
        internal static string UsageMessage {
            get {
                return ResourceManager.GetString("UsageMessage", resourceCulture);
            }
        }
    }
}
