namespace Ueco.Models;

/// <summary>
/// Engine Association is a model, which contains information about user Engine installations
/// This information is used by ueco to select correct Engine for a project
/// All EngineAssociations are stored in the config file "ueco.config.json" 
/// </summary>
public class UnrealEngineAssociation
{
    /// <summary>
    /// Name of the Engine. For example, "ueco-engine"
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Path to the Engine folder. For example, "C:\Program Files\Unreal Engine\4.27\"
    /// The folder should contain "Engine"
    /// </summary>
    public required string Path { get; set; }
    
    /// <summary>
    /// Version of the Engine
    /// </summary>
    public UnrealEngineVersion Version { get; set; }
    
    /// <summary>
    /// Use this Engine by default. If true, it will be used by default when no other Engine is selected
    /// </summary>
    public bool IsDefault { get; set; }
}